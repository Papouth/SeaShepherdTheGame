using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerExp : MonoBehaviour
{
	#region Variables
	[SerializeField] private int expToLvlUp;

	public static event Action ShipLevelUp;

	private int _exp = 0;
	private int _level = 1;
	#endregion
	
	#region Properties
	public int Level => _level;
	#endregion
	
	#region Built-in Methods
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
	}
	#endregion
}
