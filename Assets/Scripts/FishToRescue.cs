using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishToRescue : MonoBehaviour
{
	#region Variables
	[SerializeField] private float fishMaxHP;
	[SerializeField] private float inRangeRadius;
	[SerializeField] private int wastesNumber;
	[SerializeField] private List<GameObject> wastesGOList = new List<GameObject>();
	[SerializeField] private float distanceBetweenFishAndWastes = 1f;

	private bool _playerInArea = false;
	private float _currentHP;
	private bool _canHitThis = true;
	private float _wastesDisappearanceRate;

	private GameManager _gm;
	private UIManager _ui;
	#endregion
	
	#region Properties
	#endregion
	
	#region Built-in Methods
    void Start()
    {
		_gm = GameManager.instance;
		_ui = UIManager.instance;
		
		_currentHP = fishMaxHP;
		_wastesDisappearanceRate = 1f / wastesNumber;
		GetComponent<SphereCollider>().radius = inRangeRadius;
		SpawnWastes();
    }

    void Update()
    {
        
    }

	void OnTriggerEnter(Collider other){
		if (other.transform.gameObject.layer == LayerMask.NameToLayer("Player")){
			_playerInArea = true;
		}
	}

	void OnTriggerExit(Collider other){
		if (other.transform.gameObject.layer == LayerMask.NameToLayer("Player")){
			_playerInArea = false;
		}
	}
	#endregion
	
	#region Custom Methods
	private void SpawnWastes(){
		for (int i = 0; i < wastesNumber; i++){
			float randomX = 0;
			float randomZ = 0;
			Vector3 newPos = Vector3.zero;
			while (Vector3.Distance(transform.position, transform.position + newPos) < distanceBetweenFishAndWastes){
				randomX = Random.Range(-distanceBetweenFishAndWastes * 2, distanceBetweenFishAndWastes * 2);
				randomZ = Random.Range(-distanceBetweenFishAndWastes * 2, distanceBetweenFishAndWastes * 2);
				newPos = new Vector3(randomX, 0, randomZ);
			}
			Transform newWaste = Instantiate(wastesGOList[Random.Range(0, wastesGOList.Count)].transform, transform.position + newPos, Quaternion.identity);
			newWaste.SetParent(transform.GetChild(1));
		}
	}

	public void TakeDamage(Vector3 hitPos, float damageAmount){
		if (_playerInArea && _canHitThis){
			_currentHP -= damageAmount;
			_ui.ShowPlayerDamage(hitPos, damageAmount);
			if (_currentHP / fishMaxHP <= (transform.GetChild(1).childCount - 1) * _wastesDisappearanceRate){
				Destroy(transform.GetChild(1).GetChild(transform.GetChild(1).childCount - 1).gameObject);
			}
			if (_currentHP <= 0){
				Release();
			}
		}
	}

	public void Release(){
		_canHitThis = false;
		GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
		_gm.QuestProgress(Quest.QType.FishRelease);
		GetComponent<RandomNavigation>().enabled = true;
	}
	#endregion
}
