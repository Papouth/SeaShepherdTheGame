using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
	#region Variables
	private int shipState;

	private Player _player;

	public static GameManager instance;
	public static event Action<QuestScriptable.QType> QuestAdvancement;
	#endregion
	
	#region Properties
	public int ShipState => shipState;
	#endregion
	
	#region Built-in Methods
	void Awake(){
		if (instance != null){
			Destroy(gameObject);
			return;
		}
		instance = this;
	}

	void Start(){
		_player = GameObject.Find("Player").GetComponent<Player>();
		StartCoroutine(Test());
	}
	#endregion
	
	#region Custom Methods
	IEnumerator Test(){
		yield return new WaitForSeconds(1f);
		QuestAdvancement?.Invoke(QuestScriptable.QType.Wastes);
	}
	#endregion
}
