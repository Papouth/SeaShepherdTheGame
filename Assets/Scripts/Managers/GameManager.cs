using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	#region Variables
	private string shipState = "small";

	public static GameManager instance;
	#endregion
	
	#region Properties
	public string ShipState => shipState;
	#endregion
	
	#region Built-in Methods
	void Awake(){
		if (instance != null){
			Destroy(gameObject);
			return;
		}
		instance = this;
	}
	#endregion
	
	#region Custom Methods
	#endregion
}
