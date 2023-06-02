using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waste : MonoBehaviour
{
	#region Variables
	[SerializeField] private float wasteMaxHP;
	[SerializeField] private float inRangeRadius;
	[SerializeField] private float maxSpawnRange;
	[SerializeField] private int wastesNumber;
	[SerializeField] private List<GameObject> wastesGOList = new List<GameObject>();

	private bool _playerInArea = false;
	private float _wastesDisappearanceRate;
	private float _currentHP;
	private GameObject _healthBar;
	private Transform _wasteSelected;
	private GameObject canvasMap;

	private GameManager _gm;
	private UIManager _ui;
	private PlayerMovement playerMove;
	#endregion
	
	#region Properties
	#endregion
	
	#region Built-in Methods
    void Start()
    {
        _gm = GameManager.instance;
		_ui = UIManager.instance;
		playerMove = FindObjectOfType<PlayerMovement>();


		_healthBar = transform.GetChild(1).gameObject;
		_healthBar.GetComponent<Canvas>().worldCamera = Camera.main;

		// UI worldspace Mini Map
		canvasMap = transform.GetChild(2).gameObject;
		canvasMap.GetComponent<Canvas>().worldCamera = Camera.main;
		canvasMap.SetActive(false);

		_currentHP = wasteMaxHP;
		_ui.SetHealthBar(_healthBar, wasteMaxHP);
		_ui.HealthBarRotation(_healthBar);
		
		GetComponent<SphereCollider>().radius = inRangeRadius;
		SpawnWastes();
	}

	void OnTriggerEnter(Collider other){
		if (other.transform.gameObject.layer == LayerMask.NameToLayer("Player")){
			_playerInArea = true;
			_healthBar.SetActive(true);
		}
	}

	void OnTriggerExit(Collider other){
		if (other.transform.gameObject.layer == LayerMask.NameToLayer("Player")){
			_playerInArea = false;
			_healthBar.SetActive(false);
		}
	}

    private void Update()
    {
		if (playerMove.camOnMap)
		{
			// La camera est sur la map on affiche donc l'icone du bateau
			canvasMap.SetActive(true);
		}
		else if (!playerMove.camOnMap)
		{
			// La camera est sur le joueur on cache donc l'icone du bateau
			canvasMap.SetActive(false);
		}
	}
    #endregion

    #region Custom Methods
    //Spawn les dechets qui disparaitront avec les clicks
    private void SpawnWastes(){
		for (int i = 0; i < wastesNumber; i++){
			float randomX = Random.Range(-maxSpawnRange, maxSpawnRange);
			float randomZ = Random.Range(-maxSpawnRange, maxSpawnRange);
			Vector3 newPos = new Vector3(randomX, 0, randomZ);
			Transform newWaste = Instantiate(wastesGOList[Random.Range(0, wastesGOList.Count)].transform, transform.position + newPos, Quaternion.identity);
			newWaste.SetParent(transform.GetChild(0));
		}
		_wastesDisappearanceRate = 1f / transform.GetChild(0).childCount;
	}
	
	//Les dechets perdent de la "vie" puis a un certain taux de HP disparaissent
	public void TakeDamage(Vector3 hitPos, float damageAmount){
		if (_playerInArea){
			_currentHP -= damageAmount;
			_ui.ShowPlayerDamage(hitPos, damageAmount);
			_ui.UpdateHealthBar(_healthBar, _currentHP);
			if (_currentHP / wasteMaxHP <= (transform.GetChild(0).childCount - 1) * _wastesDisappearanceRate){
				WasteToDestroy(hitPos);
			}
			if (_currentHP <= 0){
				WastesDisappearance();
			}
		}
	}

	private void WasteToDestroy(Vector3 hit){
		foreach (Transform waste in transform.GetChild(0)){
			if (_wasteSelected == null){
				_wasteSelected = waste;
			}
			else if (Vector3.Distance(hit, waste.position) < Vector3.Distance(hit, _wasteSelected.position)){
				_wasteSelected = waste;
			}
		}
		Destroy(_wasteSelected.gameObject);
	}

	//Tous les dechets ont ete ramasse
	private void WastesDisappearance(){
		_gm.QuestProgress(Quest.QType.Wastes);
		Destroy(gameObject);
	}
	#endregion
}
