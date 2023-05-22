using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
	#region Variables
	[SerializeField] private List<QuestScriptable> smallQuestList = new List<QuestScriptable>();
	[SerializeField] private List<QuestScriptable> mediumQuestList = new List<QuestScriptable>();
	[SerializeField] private List<QuestScriptable> greatQuestList = new List<QuestScriptable>();
	[SerializeField] private Transform questListGO;
	
	private Quest activeQuest;

	private GameManager _gm;
	private Player _player;
	#endregion
	
	#region Properties
	#endregion
	
	#region Built-in Methods
	void Start(){
		_gm = GameManager.instance;
		_player = GameObject.Find("Player").GetComponent<Player>();

		GameManager.QuestAdvancement += QuestAdvancement;

		AddNewQuest();
	}
	#endregion
	
	#region Custom Methods
	//Ajoute une nouvelle quete en fonction du bateau
	public void AddNewQuest(){
		List<QuestScriptable> randomQuestList = ChooseRandomQuestList();
		int randomIndex = Random.Range(0, randomQuestList.Count);
		print(randomIndex);
		QuestScriptable randomQuest = randomQuestList[randomIndex];

		GameObject newQuestGO = new GameObject();
		newQuestGO.transform.SetParent(questListGO);
		newQuestGO.AddComponent<Quest>();
		Quest newQuest = newQuestGO.GetComponent<Quest>();
		newQuest.QuestDetails = randomQuest;
		newQuestGO.name = newQuest.QuestDetails.QuestName;
		Quest.EndOfQuest += EndQuest;
		
		activeQuest = newQuest;
	}

	//Choisi une liste random par rapport au bateau
	private List<QuestScriptable> ChooseRandomQuestList(){
		List<QuestScriptable> questListToChoose = new List<QuestScriptable>();
		if (_gm.ShipState == 0){
			return smallQuestList;
		}
		else if (_gm.ShipState == 1){
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

	//Lorsqu'un event qui peut faire avancer une quete a lieu, verifie s'il y a une quete correspondante a cet event
	private void QuestAdvancement(QuestScriptable.QType questType){
		if (activeQuest.QuestDetails.QuestType == questType){
			activeQuest.QuestAdvancement();
		}
	}

	//Quete finie, donne exp et ajoute une nouvelle quete ou pas
	public void EndQuest(){
		_player.AddExp(activeQuest.QuestDetails.Exp);
		Destroy(activeQuest.gameObject);
		AddNewQuest();
	}
	#endregion
}
