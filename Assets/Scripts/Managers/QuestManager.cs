using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
	#region Variables
	[SerializeField] private List<QuestScriptable> smallQuestList = new List<QuestScriptable>();
	[SerializeField] private List<QuestScriptable> mediumQuestList = new List<QuestScriptable>();
	[SerializeField] private List<QuestScriptable> greatQuestList = new List<QuestScriptable>();

	private List<Quest> activeQuestList = new List<Quest>();

	private GameManager _gm;
	#endregion
	
	#region Properties
	#endregion
	
	#region Built-in Methods
	void Start(){
		_gm = GameManager.instance;
	}
	#endregion
	
	#region Custom Methods
	//Ajoute une nouvelle quete en fonction du bateau
	public void AddNewQuest(){
		List<QuestScriptable> randomQuestList = ChooseRandomQuestList();
	}

	private List<QuestScriptable> ChooseRandomQuestList(){
		List<QuestScriptable> questListToChoose = new List<QuestScriptable>();
		if (_gm.ShipState == "small"){
			return smallQuestList;
		}
		else if (_gm.ShipState == "medium"){
			int randomQuestList = Random.Range(0, 2);
			if (randomQuestList == 0){
				return smallQuestList;
			}
			else{
				return mediumQuestList;
			}
		}
		else{
			int randomQuestList = Random.Range(0, 3);
			if (randomQuestList == 0){
				return smallQuestList;
			}
			else if (randomQuestList == 1){
				return  mediumQuestList;
			}
			else{
				return greatQuestList;
			}
		}
	}
	#endregion
}
