using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
	#region Variables
	private int _shipState;

	private PlayerExp _player;

	public static GameManager instance;
	public static event Action<Quest.QType> QuestAdvancement;
	public static event Action<GameObject, int> Respawn;
	#endregion
	
	#region Properties
	public int ShipState => _shipState;
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
		_player = GameObject.Find("Player").GetComponent<PlayerExp>();
		GetShipState();
	}
	#endregion
	
	#region Custom Methods
	public void GetShipState(){
		_shipState = _player.Level;
	}

	//Lorqu'on a fait une action qui peut faire avancer une quete
	public void QuestProgress(Quest.QType questType){
		QuestAdvancement?.Invoke(questType);
	}

	public void RespawnDead(GameObject go, int difficulty){
		Respawn?.Invoke(go, difficulty);
	}

	// QuestProgress a replacer aux endroits adequats
	IEnumerator Test(){
		yield return new WaitForSeconds(1f);
		QuestProgress(Quest.QType.StopPoaching);
	}
	#endregion
}