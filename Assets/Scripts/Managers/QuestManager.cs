using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
	#region Variables
	[SerializeField] private List<Quest> smallQuestList = new List<Quest>();
	[SerializeField] private List<Quest> mediumQuestList = new List<Quest>();
	[SerializeField] private List<Quest> greatQuestList = new List<Quest>();
	
	private Quest _activeQuest;
	private int _questProgressCount = 0;
	private int _questClearedInARow = 0;
	private bool _shipMustEvolve = false;

	private GameManager _gm;
	private PlayerExp _player;

	public static QuestManager instance;
	#endregion
	
	#region Properties
	#endregion

	//Replacer les quetes correctement dans l'inspecteur
	
	#region Built-in Methods
	void Awake(){
		if (instance != null){
			Destroy(gameObject);
			return;
		}
		instance = this;
	}

	void Start(){
		_gm = GameManager.instance;
		_player = GameObject.Find("Player").GetComponent<PlayerExp>();

		GameManager.QuestAdvancement += QuestAdvancement;
		PlayerExp.ShipLevelUp += ShipMustEvolve;

		AddNewQuest();
	}
	#endregion
	
	#region Custom Methods
	//Ajoute une nouvelle quete en fonction du bateau si le joueur n'a pas deja realise 3 quetes
	public void AddNewQuest(){
		if (!_shipMustEvolve){
			if (_questClearedInARow < 3){
				List<Quest> randomQuestList = ChooseRandomQuestList();
				int randomIndex = Random.Range(0, randomQuestList.Count);
				Quest randomQuest = randomQuestList[randomIndex];
				
				_activeQuest = randomQuest;
			}
			else{
				print("retour base");
			}
		}
	}

	//Choisi une liste random par rapport au bateau
	private List<Quest> ChooseRandomQuestList(){
		List<Quest> questListToChoose = new List<Quest>();
		if (_gm.ShipState == 1){
			return smallQuestList;
		}
		else if (_gm.ShipState == 2){
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
	//Changer l'endroit d'ou provient l'event (ex : a la fin de la collecte d'un tas de dechets)
	private void QuestAdvancement(Quest.QType questType){
		if (_activeQuest){
			if (_activeQuest.QuestType == questType){
				_questProgressCount++;
				if (_questProgressCount >= _activeQuest.TimeForCompletion){
					EndQuest();
				}
			}
		}
	}

	//Quete finie, donne exp et ajoute une nouvelle quete ou pas
	public void EndQuest(){
		_player.AddExp(_activeQuest.Exp);
		_questClearedInARow++;
		_activeQuest = null;
		AddNewQuest();
	}

	//Le joueur vient de level up, il doit ameliorer son bateau
	private void ShipMustEvolve(){
		_shipMustEvolve = true;
		print("return to base, ship evolution unlocked");
	}

	//Le joueur vient d'ameliorer son bateau, il peut recevoir de nouvelles quetes
	public void EndShipEvolution(){
		_shipMustEvolve = false;
		AddNewQuest();
	}

	public void CheckForEnemyExp(int expAmount){
		if (_activeQuest){
			if (_activeQuest.QuestType != Quest.QType.StopPoaching){
				_player.AddExp(expAmount);
			}
		}
	}
	#endregion
}
