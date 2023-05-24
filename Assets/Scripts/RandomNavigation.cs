using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandomNavigation : MonoBehaviour
{
	#region Variables
	[SerializeField] private float randomXRange;
	[SerializeField] private float randomZRange;
    [SerializeField] private float turnSmoothVelocity = .1f;
	[SerializeField] private float timeBetweenEachMovement;

	private Vector3 _spawnPos;
	private Vector3 _randomDestination = Vector3.zero;
	private Vector3 _randomDirection;
	private bool _hasRandomDest = false;
	private bool _isMoving = false;
	private float _distanceRemaining;

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
			MovementRotation();
			CheckDestinationDistance();
		}
    }

	void FixedUpdate(){
		if (_isMoving){
			_rb.velocity = transform.forward;
		}
	}
	#endregion
	
	#region Custom Methods
	private void GetRandomDestination(){
		float randomX = Random.Range(-randomXRange, randomXRange);
		float randomZ = Random.Range(-randomZRange, randomZRange);
		_randomDestination = new Vector3(_spawnPos.x + randomX, _spawnPos.y, _spawnPos.z + randomZ);
		_hasRandomDest = true;
		Debug.DrawLine(_randomDestination, _randomDestination + Vector3.up, Color.red, 10f);
	}

	private void MovementRotation(){
		if (_isMoving){
			Quaternion baseRotation = transform.rotation;
			transform.LookAt(_randomDestination);
			Quaternion destRotation = transform.rotation;
			transform.rotation = baseRotation;
			transform.rotation = Quaternion.Slerp(baseRotation, destRotation, Time.deltaTime * turnSmoothVelocity);
		}
	}

	private void CheckDestinationDistance(){
		_distanceRemaining = Vector3.Distance(transform.position, _randomDestination);
		if (_distanceRemaining <= .5f){
			_isMoving = false;
			StartCoroutine(DestinationReached());
			if (gameObject.layer == LayerMask.NameToLayer("Enemy")){
				GetComponent<Enemy>().IsMoving = false;
			}
			StartCoroutine(TimeBetweenEachRandomDestination());
		}
	}

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

	IEnumerator TimeBetweenEachRandomDestination(){
		yield return new WaitForSeconds(timeBetweenEachMovement);
		GetRandomDestination();
	}
	#endregion
}
