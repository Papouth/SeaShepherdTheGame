using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SeaShepherd/QuestScriptable", order = 1, fileName = "QuestName")]
public class QuestScriptable : ScriptableObject
{
	#region Variables
	[SerializeField] private string questName;
	[SerializeField] private QType questType;
	[SerializeField] private int exp;
	[SerializeField] private int timeForCompletion = 1;
	[SerializeField] private string fishToRelease;

	public enum QType{
		FishRelease,
		Wastes,
		StopPoaching
	}
	#endregion
	
	#region Properties
	public string QuestName => questName;
	public QType QuestType => questType;
	public int Exp => exp;
	public int TimeForCompletion => timeForCompletion;
	public string FishToRelease => fishToRelease;
	#endregion
	
	#region Built-in Methods
	#endregion
	
	#region Custom Methods
	#endregion
}
