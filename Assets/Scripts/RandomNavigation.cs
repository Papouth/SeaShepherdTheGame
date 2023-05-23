using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandomNavigation : MonoBehaviour
{
	#region Variables
	[SerializeField] private float randomXRange;
	[SerializeField] private float randomZRange;

	private Vector3 _spawnPos;
	#endregion
	
	#region Properties
	#endregion
	
	#region Built-in Methods
    void Start()
    {
		
		_spawnPos = transform.position;
		GetRandomDestination();
    }

    void Update()
    {
        
    }
	#endregion
	
	#region Custom Methods
	private void GetRandomDestination(){
		float randomX = Random.Range(-randomXRange, randomXRange);
		float randomZ = Random.Range(-randomZRange, randomZRange);
		Vector3 randomDestination = new Vector3(_spawnPos.x + randomX, _spawnPos.y, _spawnPos.z + randomZ);
	}
	#endregion
}
