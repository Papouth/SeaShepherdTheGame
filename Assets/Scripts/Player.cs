using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	#region Variables
	[SerializeField] private int expToLvlUp;

	private int exp = 0;
	private int level = 1;
	#endregion
	
	#region Properties
	#endregion
	
	#region Built-in Methods
	#endregion
	
	#region Custom Methods
	public void AddExp(int amount){
		exp += amount;
		print(exp.ToString() + "/" + expToLvlUp.ToString());
		if (exp >= expToLvlUp){
			LevelUp();
		}
	}

	private void LevelUp(){
		level++;
		exp -= expToLvlUp;
	}
	#endregion
}
