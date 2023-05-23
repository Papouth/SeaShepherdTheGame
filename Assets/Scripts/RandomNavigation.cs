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
