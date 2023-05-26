using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandomNavigation : MonoBehaviour
{
	#region Variables
	[SerializeField] private float randomXRange;
	[SerializeField] private float randomZRange;
	[SerializeField] private float movementSpeed = 1f;
    [SerializeField] private float turnSmoothVelocity = .1f;
	[SerializeField] private float timeBetweenEachMovement;
	[SerializeField] private float raycastDistanceForRandomDestination;
	[SerializeField] private float raycastDistanceForObstacleChecking;

	private Vector3 _spawnPos;
	private Vector3 _randomDestination = Vector3.zero;
	private Vector3 _randomDestinationStep = Vector3.zero;
	private bool _hasRandomDest = false;
	private bool _isMoving = false;
	private bool _hasStepDestination = false;
	private float _distanceRemaining;
	private bool _depthReached = false;

	private Rigidbody _rb;
	#endregion
	
	#region Properties
	#endregion
	
	#region Built-in Methods
    void Start()
    {
		_rb = GetComponent<Rigidbody>();

		_spawnPos = transform.position;
		GetRandomDestination();
    }

    void Update()
    {
        if (_hasRandomDest){
			_hasRandomDest = false;
			_isMoving = true;
			if (gameObject.layer == LayerMask.NameToLayer("Enemy")){
				GetComponent<Enemy>().IsMoving = true;
			}
		}
		if (_isMoving){
			if (gameObject.layer == LayerMask.NameToLayer("Fish")){
				if (_depthReached){
					MovementRotation();
					CheckDestinationDistance();
					CheckObstacles();
				}
			}
			else{
				MovementRotation();
				CheckDestinationDistance();
				CheckObstacles();
			}
		}
    }

	void FixedUpdate(){
		if (_isMoving){
			Vector3 dir = transform.forward;
			if (gameObject.layer == LayerMask.NameToLayer("Fish")){
				if (!_depthReached){
					float depth = GetComponent<FishToRescue>().DepthWhenRelease;
					if (transform.position.y - depth <= .1f){
						_depthReached = true;
						_spawnPos = transform.position;
						GetRandomDestination();
					}
					else{
						dir = new Vector3(0, -Vector3.up.y, 0);
					}
				}
			}
			_rb.velocity = dir * movementSpeed;
		}
	}
	#endregion
	
	#region Custom Methods
	//Prend des coordonnees random dans le carre defini dans l'inspecteur
	//Si les coordonnees menent sur terre, cherche d'autres coordonnees
	private void GetRandomDestination(){
		float randomX = Random.Range(-randomXRange, randomXRange);
		float randomZ = Random.Range(-randomZRange, randomZRange);
		_randomDestination = new Vector3(_spawnPos.x + randomX, _spawnPos.y, _spawnPos.z + randomZ);
		RaycastHit hit;
		if (Physics.Raycast(_randomDestination + Vector3.up * 2, -Vector3.up, out hit, raycastDistanceForRandomDestination)){
			if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Island")){
				GetRandomDestination();
				return;
			}
		}
		_hasRandomDest = true;
	}

	//Rotation lors du mouvement
	private void MovementRotation(){
		if (_isMoving){
			Quaternion baseRotation = transform.rotation;
			if (_hasStepDestination){
				transform.LookAt(_randomDestinationStep);
			}
			else{
				transform.LookAt(_randomDestination);
			}
			Quaternion destRotation = transform.rotation;
			transform.rotation = baseRotation;
			transform.rotation = Quaternion.Slerp(baseRotation, destRotation, Time.deltaTime * turnSmoothVelocity);
		}
	}

	//Verifie si on arrive a destination
	private void CheckDestinationDistance(){
		if (_hasStepDestination){
			_distanceRemaining = Vector3.Distance(transform.position, _randomDestinationStep);
		}
		else{
			_distanceRemaining = Vector3.Distance(transform.position, _randomDestination);
		}
		if (_distanceRemaining <= 1.25f && _hasStepDestination){
			_hasStepDestination = false;
		}
		else if (_distanceRemaining <= 1f && !_hasStepDestination){
			_isMoving = false;
			StartCoroutine(DestinationReached());
			if (gameObject.layer == LayerMask.NameToLayer("Enemy")){
				GetComponent<Enemy>().IsMoving = false;
			}
			StartCoroutine(TimeBetweenEachRandomDestination());
		}
	}

	//Contourne les obstacles (a voir si on utilise selon les spawns)
	private void CheckObstacles(){
		RaycastHit hit;
		Debug.DrawRay(transform.position, transform.forward * raycastDistanceForObstacleChecking, Color.green, 10f);
		if (Physics.Raycast(transform.position, transform.forward * raycastDistanceForObstacleChecking, out hit, 2f) && _distanceRemaining > 2){
			if (hit.transform.gameObject.layer != gameObject.layer){
				if (CheckRandomDestinationDir()){
					FindDirForAvoindingObstacles(raycastDistanceForObstacleChecking, false, false);
					_hasStepDestination = true;
				}
			}
		}
	}

	//Check s'il y a besoin de contourner l'obstacle pour arriver a la destination random
	private bool CheckRandomDestinationDir(){
		bool isGoodDir = true;
		if (transform.forward.x >= 0){
			if (transform.position.x > _randomDestination.x){
				isGoodDir = false;
			}
		}
		else if (transform.forward.x < 0){
			if (transform.position.x <= _randomDestination.x){
				isGoodDir = false;
			}
		}
		if (transform.forward.z >= 0){
			if (transform.position.z > _randomDestination.z){
				isGoodDir = false;
			}
		}
		else if (transform.forward.z < 0){
			if (transform.position.z <= _randomDestination.z){
				isGoodDir = false;
			}
		}
		return isGoodDir;
	}

	//Cherche un point a cote de lui qui servira a eviter l'obstacle
	private void FindDirForAvoindingObstacles(float raycastOffset, bool toRight, bool toLeft){
		RaycastHit hit;
		bool canGoRight = true;
		bool canGoLeft = true;
		if (Physics.Raycast(transform.position + Vector3.up / 2, transform.forward + transform.right * raycastOffset, out hit, 2f)){
			canGoRight = false;
		}
		if (Physics.Raycast(transform.position + Vector3.up / 2, transform.forward + -transform.right * raycastOffset, out hit, 2f)){
			canGoLeft = false;
		}
		if (canGoLeft && !canGoRight){
			_randomDestinationStep = transform.position +transform.forward - transform.right * raycastOffset * 1.5f;
		}
		else if (!canGoLeft && canGoRight){
			_randomDestinationStep = transform.position + transform.forward + transform.right * raycastOffset * 1.5f;
		}
		else if (canGoLeft && canGoRight){
			if (toRight && toLeft){
				FindDirForAvoindingObstacles(raycastOffset * .9f, canGoRight, canGoLeft);
			}
			else{ //Droite par defaut
				_randomDestinationStep = transform.position + transform.forward + transform.right * raycastOffset * 1.5f;
			}
		}
		else{
			if (!toLeft && !toRight){
				FindDirForAvoindingObstacles(raycastOffset * 1.5f, canGoRight, canGoLeft);
			}
			else{ //Tjrs la droite par defaut
				_randomDestinationStep = transform.position + (transform.forward + transform.right * raycastOffset / 9 * 10) * 1.5f;
			}
		}
	}

	//Decceleration lorsque le bateau arrive a destination
	IEnumerator DestinationReached(){
		Vector3 deceleration = _rb.velocity / 120;
		for (int i = 0; i < 120; i++){
			if (i != 119){
				_rb.velocity -= deceleration;
			}
			else{
				_rb.velocity = Vector3.zero;
			}
			yield return new WaitForEndOfFrame();
		}
	}

	//Pause avant recherche d'autres coordonnees random
	IEnumerator TimeBetweenEachRandomDestination(){
		_rb.constraints = RigidbodyConstraints.FreezeRotation;
		yield return new WaitForSeconds(timeBetweenEachMovement);
		_rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
		GetRandomDestination();
	}
	#endregion
}
