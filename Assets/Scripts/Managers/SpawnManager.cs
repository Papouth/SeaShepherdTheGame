using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
	#region Variables
	[SerializeField] private GameObject field;
	[SerializeField] private float searchPositionRaycastLength = 3f;

	[Header ("Objects to spawn")]
	[SerializeField] private List<float> surfaceRateForEachDifficulty = new List<float>();

	[Header ("Low Difficulty")]
	[SerializeField] private List<GameObject> lowDifficultyObjectList = new List<GameObject>();
	[SerializeField] private List<float> lowDifficultySpawnRate = new List<float>();
	[SerializeField] private int lowDifficultyNumberToSpawn;
	
	[Header ("Mid Difficulty")]
	[SerializeField] private List<GameObject> midDifficultyObjectList = new List<GameObject>();
	[SerializeField] private List<float> midDifficultySpawnRate = new List<float>();
	[SerializeField] private int midDifficultyNumberToSpawn;

	[Header ("Great Difficulty")]
	[SerializeField] private List<GameObject> greatDifficultyObjectList = new List<GameObject>();
	[SerializeField] private List<float> greatDifficultySpawnRate = new List<float>();
	[SerializeField] private int greatDifficultyNumberToSpawn;

	private Vector3 fieldHalfSize;
	private Vector3 objectSpawnPos;
	#endregion
	
	#region Properties
	#endregion
	
	#region Built-in Methods
    void Start()
    {
        fieldHalfSize = field.GetComponent<Renderer>().bounds.size / 2;
		SpawnObjects();
	}
	#endregion
	
	#region Custom Methods
	private void SpawnObjects(){
		for (int i = 0; i < lowDifficultyObjectList.Count; i++){
			float numberToSpawn = Mathf.Floor(lowDifficultySpawnRate[i] * lowDifficultyNumberToSpawn);
			for (int j = 0; j < numberToSpawn; j++){
				GetSpawnPosition(0);
				GameObject newObject = Instantiate(lowDifficultyObjectList[i], objectSpawnPos, Quaternion.identity);
				newObject.transform.SetParent(field.transform.GetChild(0));
			}
		}
		for (int i = 0; i < midDifficultyObjectList.Count; i++){
			float numberToSpawn = Mathf.Floor(midDifficultySpawnRate[i] * midDifficultyNumberToSpawn);
			for (int j = 0; j < numberToSpawn; j++){
				GetSpawnPosition(1);
				GameObject newObject = Instantiate(midDifficultyObjectList[i], objectSpawnPos, Quaternion.identity);
				newObject.transform.SetParent(field.transform.GetChild(1));
			}
		}
		for (int i = 0; i < greatDifficultyObjectList.Count; i++){
			float numberToSpawn = Mathf.Floor(greatDifficultySpawnRate[i] * greatDifficultyNumberToSpawn);
			for (int j = 0; j < numberToSpawn; j++){
				GetSpawnPosition(2);
				GameObject newObject = Instantiate(greatDifficultyObjectList[i], objectSpawnPos, Quaternion.identity);
				newObject.transform.SetParent(field.transform.GetChild(2));
			}
		}
	}

	private void GetSpawnPosition(int difficulty){
		float maxX = fieldHalfSize.x * surfaceRateForEachDifficulty[difficulty];
		float maxZ = fieldHalfSize.z * surfaceRateForEachDifficulty[difficulty];
		float minX = 0;
		float minZ = 0;
		float randomX = Random.Range(-maxX, maxX);
		float randomZ = Random.Range(-maxZ, maxZ);
		if (difficulty != 0){
			minX = fieldHalfSize.x * surfaceRateForEachDifficulty[difficulty - 1];
			minZ = fieldHalfSize.z * surfaceRateForEachDifficulty[difficulty - 1];
			if (randomX < minX  && randomX > -minX|| randomZ < minZ && randomZ > -minZ){
				GetSpawnPosition(difficulty);
				return;
			}
		}
		Vector3 newPos = new Vector3(randomX, 0, randomZ);
		RaycastHit hit;
		Debug.DrawRay(newPos + field.transform.position, Vector3.up, Color.red, 10f);
		if (Physics.Raycast(newPos + field.transform.position + Vector3.up, -Vector3.up, out hit, searchPositionRaycastLength)){
			print(hit.transform.gameObject.name);
			if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Island")){
				GetSpawnPosition(difficulty);
				return;
			}
		}

		objectSpawnPos = newPos + field.transform.position;
		objectSpawnPos.y = 0;
	}
	#endregion
}
