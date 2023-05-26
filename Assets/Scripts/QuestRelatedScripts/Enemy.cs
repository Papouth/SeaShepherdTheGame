using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	#region Variables
	[SerializeField] private float enemyMaxHP;
	[SerializeField] private float fightAreaRadius;
	[SerializeField] private int expOnDeath;
	[SerializeField] private float inFightRegenRate = .05f;
	[SerializeField] private GameObject GOForRespawn;

	private float _currentHP;
	private bool _playerInArea = false;
	private bool _regen = false;
	private float _hpToRegenInFight;
	private bool _isMoving = false;

	private PlayerExp _playerExp;
	private UIManager _ui;
	private GameManager _gm;
	private QuestManager _qm;
	#endregion
	
	#region Properties
	public bool IsMoving{
		get{return _isMoving;}
		set{_isMoving = value;}
	}
	#endregion
	
	#region Built-in Methods
    void Start()
    {
        _ui = UIManager.instance;
		_gm = GameManager.instance;
		_qm = QuestManager.instance;
		_playerExp = GameObject.Find("Player").GetComponent<PlayerExp>();
		
		transform.GetChild(1).gameObject.GetComponent<Canvas>().worldCamera = Camera.main;
		_currentHP = enemyMaxHP;
		_hpToRegenInFight = enemyMaxHP * inFightRegenRate;
		GetComponent<SphereCollider>().radius = fightAreaRadius;
		_ui.SetHealthBar(transform.GetChild(1).gameObject, enemyMaxHP);
    }

	void Update(){
		if (_isMoving){
		_ui.HealthBarRotation(transform.GetChild(1).gameObject);
		}
	}
	
	void OnTriggerEnter(Collider other){
		if (other.transform.gameObject.layer == LayerMask.NameToLayer("Player") && _currentHP != enemyMaxHP){
			_playerInArea = true;
			StartCoroutine(InFightRegen());
		}
	}

	void OnTriggerStay(Collider other){
		if (other.transform.gameObject.layer == LayerMask.NameToLayer("Player") && !_playerInArea){
			_playerInArea = true;
			StartCoroutine(InFightRegen());
		}
	}

	void OnTriggerExit(Collider other){
		if (other.transform.gameObject.layer == LayerMask.NameToLayer("Player")){
			if (Vector3.Distance(other.transform.position, transform.position) > fightAreaRadius){
				_playerInArea = false;
			}
			if (_currentHP != enemyMaxHP && !_regen){
				_regen = true;
				//StartCoroutine(ResetEnemy());
			}
		}
	}

	void OnCollisionEnter(Collision other){
		if (other.transform.gameObject.layer == LayerMask.NameToLayer("Player")){
			GetComponent<Rigidbody>().velocity = Vector3.zero;
			GetComponent<Rigidbody>().isKinematic = true; 
		}
	}

	void OnCollisionExit(Collision other){
		if (other.transform.gameObject.layer == LayerMask.NameToLayer("Player")){
			GetComponent<Rigidbody>().isKinematic = false;
		}
	}
	#endregion
	
	#region Custom Methods
	//Retire des HP a l'ennemi si le joueur se trouve dans la zone de combat
	public void TakeDamage(Vector3 hitPos, float damageAmount){
		if (_playerInArea){
			_currentHP -= damageAmount;
			_ui.ShowPlayerDamage(hitPos, damageAmount);
			_ui.UpdateHealthBar(transform.GetChild(1).gameObject, _currentHP);
			if (_currentHP <= 0){
				Death();
			}
		}
	}
	
	//Si plus de HP, l'ennemi disparait
	private void Death(){
		_qm.CheckForEnemyExp(expOnDeath);
		_gm.QuestProgress(Quest.QType.StopPoaching);
		Destroy(gameObject);
	}

	//Anim de regen passive en combat
	IEnumerator InFightRegen(){
		if (_playerInArea){
			if (_currentHP + _hpToRegenInFight / 60 >= enemyMaxHP){
				_currentHP = enemyMaxHP;
				_ui.UpdateHealthBar(transform.GetChild(1).gameObject, _currentHP);
			}
			else{
				_currentHP += _hpToRegenInFight / 60;
				_ui.UpdateHealthBar(transform.GetChild(1).gameObject, _currentHP);
			}
			yield return new WaitForSeconds(1f / 60f);
			StartCoroutine(InFightRegen());
		}
	}

	//Quand le joueur sort de la zone de combat, l'ennemi regagne tous ses HP en 1s
	//Desactive car bug et on a plus de temps
	IEnumerator ResetEnemy(){
		if(!_playerInArea){
			float HpToRegen = enemyMaxHP / 60;
			for (int i = 0; i < 60; i++){
				if (_playerInArea){
					break;
				}
				else if (_currentHP + HpToRegen >= enemyMaxHP){
					_currentHP = enemyMaxHP;
					_ui.UpdateHealthBar(transform.GetChild(1).gameObject, _currentHP);
					break;
				}
				_currentHP += HpToRegen;
				_ui.UpdateHealthBar(transform.GetChild(1).gameObject, _currentHP);
				yield return new WaitForSeconds(1f / 60f);
			}
			_regen = false;
		}
	}
	#endregion
}
