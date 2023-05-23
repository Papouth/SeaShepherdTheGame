using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerExp : MonoBehaviour
{
	#region Variables
	[SerializeField] private int expToLvlUp;
    //Liste de degats de chaque bateau (du bateau de depart a la derniere evolution)
    [SerializeField] private List<float> damagePerShipUpgrade = new List<float>();

	public static event Action ShipLevelUp;

	private int _exp = 0;
	private int _level = 1;
	#endregion
	
	#region Properties
	public int Level => _level;
	#endregion
	
	#region Built-in Methods
	void Start(){
		SetDamageAmount();
	}
	#endregion
	
	#region Custom Methods
	public void AddExp(int amount){
		_exp += amount;
		if (_exp >= expToLvlUp){
			LevelUp();
		}
	}

	private void LevelUp(){
		_level++;
		_exp -= expToLvlUp;
		ShipLevelUp?.Invoke();
		if (_level <= 3){
			SetDamageAmount();
		}
	}

	private void SetDamageAmount(){
		GetComponent<PlayerInteraction>().DamageAmount = damagePerShipUpgrade[_level - 1];
	}
	#endregion
}
