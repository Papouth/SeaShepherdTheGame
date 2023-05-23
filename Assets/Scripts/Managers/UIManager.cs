using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	#region Variables
	[SerializeField] private Transform playerDamageList;
	[SerializeField] private GameObject playerDamageGO;

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
	public void SetEnemyHealthBar(GameObject enemyHealthBar, float maxHP){
		enemyHealthBar.transform.LookAt(Camera.main.transform);
		Vector3 onlyXRotation = new Vector3(enemyHealthBar.transform.eulerAngles.x, 0, 0);
		enemyHealthBar.transform.eulerAngles = -onlyXRotation;
		Slider slider = enemyHealthBar.transform.GetChild(0).gameObject.GetComponent<Slider>();
		slider.maxValue = maxHP;
		slider.value = maxHP;
	}

	public void UpdateEnemyHealthBar(GameObject enemyHealthBar, float currentHP){
		Slider slider = enemyHealthBar.transform.GetChild(0).gameObject.GetComponent<Slider>();
		slider.value = currentHP;
	}

	public void ShowPlayerDamage(Vector3 hitPos, float damageAmount){
		GameObject playerDamage = Instantiate(playerDamageGO, hitPos, Quaternion.identity);
		playerDamage.transform.SetParent(playerDamageList);
		playerDamage.GetComponent<PlayerDamageUI>().SetPlayerDamageText(damageAmount);
	}
	#endregion
}
