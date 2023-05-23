using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	#region Variables
	[SerializeField] private int enemyMaxHP;
	[SerializeField] private float fightAreaRadius;
	[SerializeField] private int expOnDeath;

	private int _currentHP;
	private bool _playerInArea = false;
	private PlayerExp _playerExp;

	private UIManager _ui;
	#endregion
	
	#region Properties
	#endregion
	
	#region Built-in Methods
    void Start()
    {
        _ui = UIManager.instance;
		_playerExp = GameObject.Find("Player").GetComponent<PlayerExp>();
		
		_currentHP = enemyMaxHP;
		GetComponent<SphereCollider>().radius = fightAreaRadius;
		_ui.SetEnemyHealthBar(transform.GetChild(1).gameObject, enemyMaxHP);
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
	//Check si le joueur est dans la zone de combat
	public void CheckFightArea(){
		if (_playerInArea){
			TakeDamage();
		}
	}

	//Retire 1HP a l'ennemi
	private void TakeDamage(){
		_currentHP--;
		_ui.UpdateEnemyHealthBar(transform.GetChild(1).gameObject, _currentHP);
		if (_currentHP <= 0){
			Death();
		}
	}
	
	//Si plus de HP, l'ennemi disparait
	private void Death(){
		_playerExp.AddExp(expOnDeath);
		Destroy(gameObject);
	}
	#endregion
}
