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
	[SerializeField] private float depthWhenRelease;

	private bool _playerInArea = false;
	private float _currentHP;
	private bool _canHitThis = true;
	private float _wastesDisappearanceRate;
	private GameObject _healthBar;
	private Transform _wasteSelected;

	private GameManager _gm;
	private UIManager _ui;
	#endregion
	
	#region Properties
	public float DepthWhenRelease => depthWhenRelease;
	#endregion
	
	#region Built-in Methods
    void Start()
    {
		_gm = GameManager.instance;
		_ui = UIManager.instance;
		
		_healthBar = transform.GetChild(2).gameObject;
		_healthBar.GetComponent<Canvas>().worldCamera = Camera.main;
		_currentHP = fishMaxHP;
		_wastesDisappearanceRate = 1f / wastesNumber;
		GetComponent<SphereCollider>().radius = inRangeRadius;
		_ui.SetHealthBar(_healthBar, fishMaxHP);
		_ui.HealthBarRotation(_healthBar);

		SpawnWastes();
    }

	void OnTriggerEnter(Collider other){
		if (other.transform.gameObject.layer == LayerMask.NameToLayer("Player")){
			_playerInArea = true;
			if (_canHitThis){
				_healthBar.SetActive(true);
			}
		}
	}

	void OnTriggerExit(Collider other){
		if (other.transform.gameObject.layer == LayerMask.NameToLayer("Player")){
			_playerInArea = false;
			if (_canHitThis){
				_healthBar.SetActive(false);
			}
		}
	}
	#endregion
	
	#region Custom Methods
	//Spawn les dechets qui vont disparaitre au fur et a mesure des clicks
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
			newWaste.gameObject.layer = LayerMask.NameToLayer("Fish");
		}
	}

	//Enleve de la "vie" aux dechets puis apres un certain taux de pv restant detruit un dechet
	public void TakeDamage(Vector3 hitPos, float damageAmount){
		if (_playerInArea && _canHitThis && CheckHitDistance(hitPos)){
			_currentHP -= damageAmount;
			_ui.ShowPlayerDamage(hitPos, damageAmount);
			_ui.UpdateHealthBar(_healthBar, _currentHP);
			if (_currentHP / fishMaxHP <= (transform.GetChild(1).childCount - 1) * _wastesDisappearanceRate){
				WasteToDestroy(hitPos);
			}
			if (_currentHP <= 0){
				Release();
			}
		}
	}

	private bool CheckHitDistance(Vector3 hitPos){
		bool canHit = false;
		foreach (Transform waste in transform.GetChild(1)){
			if (Vector3.Distance(hitPos, waste.position) < Vector3.Distance(hitPos, gameObject.transform.position) && !canHit){
				canHit = true;
			}
		}
		return canHit;
	}

	private void WasteToDestroy(Vector3 hit){
		foreach (Transform waste in transform.GetChild(1)){
			if (_wasteSelected == null){
				_wasteSelected = waste;
			}
			else if (Vector3.Distance(hit, waste.position) < Vector3.Distance(hit, _wasteSelected.position)){
				_wasteSelected = waste;
			}
		}
		Destroy(_wasteSelected.gameObject);
	}

	//Le poisson est libere
	public void Release(){
		_canHitThis = false;
		GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
		_gm.QuestProgress(Quest.QType.FishRelease);
		GetComponent<RandomNavigation>().enabled = true;
		_healthBar.SetActive(false);
	}
	#endregion
}
