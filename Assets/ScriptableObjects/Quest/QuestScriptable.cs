using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SeaShepherd/QuestScriptable", order = 1, fileName = "QuestName")]
public class QuestScriptable : ScriptableObject
{
	#region Variables
	[SerializeField] private string questName;
	[SerializeField] private QType questType;
	[SerializeField] private int timeForCompletion = 1;
	[SerializeField] private string fishToRelease;

	private enum QType{
		FishRelease,
		Wastes
	}
	#endregion
	
	#region Properties
	#endregion
	
	#region Built-in Methods
	#endregion
	
	#region Custom Methods
	#endregion
}
