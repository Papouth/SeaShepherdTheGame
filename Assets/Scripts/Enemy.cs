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

	private float _currentHP;
	private bool _playerInArea = false;
	private bool _regen = false;
	private float _hpToRegenInFight;

	private PlayerExp _playerExp;
	private UIManager _ui;
	private GameManager _gm;
	private QuestManager _qm;
	#endregion
	
	#region Properties
	#endregion
	
	#region Built-in Methods
    void Start()
    {
        _ui = UIManager.instance;
		_gm = GameManager.instance;
		_qm = QuestManager.instance;
		_playerExp = GameObject.Find("Player").GetComponent<PlayerExp>();
		
		_currentHP = enemyMaxHP;
		_hpToRegenInFight = enemyMaxHP * inFightRegenRate;
		GetComponent<SphereCollider>().radius = fightAreaRadius;
		_ui.SetEnemyHealthBar(transform.GetChild(1).gameObject, enemyMaxHP);
    }
	
	void OnTriggerEnter(Collider other){
		if (other.transform.gameObject.layer == LayerMask.NameToLayer("Player")){
			_playerInArea = true;
			StartCoroutine(InFightRegen());
		}
	}

	void OnTriggerExit(Collider other){
		if (other.transform.gameObject.layer == LayerMask.NameToLayer("Player")){
			_playerInArea = false;
			if (_currentHP != enemyMaxHP && !_regen){
				_regen = true;
				StartCoroutine(ResetEnemy());
			}
		}
	}
	#endregion
	
	#region Custom Methods
	//Retire des HP a l'ennemi si le joueur se trouve dans la zone de combat
	public void TakeDamage(Vector3 hitPos){
		if (_playerInArea){
			_currentHP--;
			_ui.ShowPlayerDamage(hitPos, 1);
			_ui.UpdateEnemyHealthBar(transform.GetChild(1).gameObject, _currentHP);
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
			for (int i = 0; i < 60; i++){
				if (_currentHP + _hpToRegenInFight / 60 >= enemyMaxHP){
					_currentHP = enemyMaxHP;
					_ui.UpdateEnemyHealthBar(transform.GetChild(1).gameObject, _currentHP);
				}
				else{
					_currentHP += _hpToRegenInFight / 60;
					_ui.UpdateEnemyHealthBar(transform.GetChild(1).gameObject, _currentHP);
				}
				yield return new WaitForSeconds(1f / 60f);
			}
			StartCoroutine(InFightRegen());
		}
	}

	//Quand le joueur sort de la zone de combat, l'ennemi regagne tous ses HP en 1s
	IEnumerator ResetEnemy(){
		for (int i = 0; i < 60; i++){
			float HpToRegen = enemyMaxHP / 60;
			if (_currentHP + HpToRegen >= enemyMaxHP){
				_currentHP = enemyMaxHP;
				_ui.UpdateEnemyHealthBar(transform.GetChild(1).gameObject, _currentHP);
				break;
			}
			_currentHP += HpToRegen;
			_ui.UpdateEnemyHealthBar(transform.GetChild(1).gameObject, _currentHP);
			yield return new WaitForSeconds(1f / 60f);
		}
		_regen = false;
	}
	#endregion
}
