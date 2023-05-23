using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandomNavigation : MonoBehaviour
{
	#region Variables
	[SerializeField] private float randomXRange;
	[SerializeField] private float randomZRange;
    [SerializeField] private float turnSmoothTime = 0.1f;
    [SerializeField] private float turnSmoothVelocity = 0.1f;

	private Vector3 _spawnPos;
	private Vector3 _randomDestination = Vector3.zero;
	private Vector3 _randomDirection;
	private bool _hasRandomDest = false;
	private bool _isMoving = false;

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
		}
		if (_isMoving){
			MovementRotation();
			CheckDestinationDistance();
		}
    }

	void FixedUpdate(){
		if (_isMoving){
			_rb.velocity = _randomDirection;
		}
	}
	#endregion
	
	#region Custom Methods
	private void GetRandomDestination(){
		float randomX = Random.Range(-randomXRange, randomXRange);
		float randomZ = Random.Range(-randomZRange, randomZRange);
		_randomDestination = new Vector3(_spawnPos.x + randomX, _spawnPos.y, _spawnPos.z + randomZ);
		_hasRandomDest = true;
	}

	private void MovementRotation(){
		if (_hasRandomDest){
            float targetAngle = Mathf.Atan2(_randomDestination.x, _randomDestination.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0, angle, 0);
            _randomDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
			Debug.DrawRay(_randomDestination + Vector3.up, _randomDestination, Color.red, 10f);
        }
	}

	private void CheckDestinationDistance(){
		float distanceRemaining = Vector3.Distance(transform.position, _randomDestination);
		print(_randomDestination);
		print(distanceRemaining);
		if (distanceRemaining <= .5f){
			_isMoving = false;
			_rb.velocity = Vector3.zero;
		}
	}
	#endregion
}
