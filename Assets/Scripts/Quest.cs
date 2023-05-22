using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Quest : MonoBehaviour
{
	#region Variables
	[SerializeField] private QuestScriptable quest;

	public static event Action EndOfQuest;

	private int questAdvancementCount = 0;
	#endregion
	
	#region Properties
	public QuestScriptable QuestDetails{
		get {return quest;}
		set {quest = value;}
	}
	#endregion
	
	#region Built-in Methods
	#endregion
	
	#region Custom Methods
	//Avancement de la quete, puis verifie si elle est terminee
	public void QuestAdvancement(){
		questAdvancementCount++;
		if (questAdvancementCount == quest.TimeForCompletion){
			EndOfQuest?.Invoke();
		}
	}
	#endregion
}
