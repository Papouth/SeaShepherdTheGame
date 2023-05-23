using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	#region Variables
	public static UIManager instance;
	#endregion
	
	#region Properties
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
	public void SetEnemyHealthBar(GameObject enemyHealthBar, int maxHP){
		enemyHealthBar.transform.LookAt(Camera.main.transform);
		Vector3 onlyXRotation = new Vector3(enemyHealthBar.transform.eulerAngles.x, 0, 0);
		enemyHealthBar.transform.eulerAngles = -onlyXRotation;
		Slider slider = enemyHealthBar.transform.GetChild(0).gameObject.GetComponent<Slider>();
		slider.maxValue = maxHP;
		slider.value = maxHP;
	}

	public void UpdateEnemyHealthBar(GameObject enemyHealthBar, int currentHP){
		Slider slider = enemyHealthBar.transform.GetChild(0).gameObject.GetComponent<Slider>();
		slider.value = currentHP;
	}
	#endregion
}
