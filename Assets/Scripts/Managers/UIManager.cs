using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
	#region Variables
	[SerializeField] private Transform playerDamageList;
	[SerializeField] private GameObject playerDamageGO;
	[SerializeField] private TMP_Text questText;

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
	public void SetHealthBar(GameObject healthBar, float maxHP){
		Slider slider = healthBar.transform.GetChild(0).gameObject.GetComponent<Slider>();
		slider.maxValue = maxHP;
		slider.value = maxHP;
	}

	public void HealthBarRotation(GameObject healthBar){
		healthBar.transform.LookAt(Camera.main.transform);
		Vector3 onlyXRotation = new Vector3(healthBar.transform.eulerAngles.x, 0, 0);
		healthBar.transform.eulerAngles = -onlyXRotation;
	}

	public void UpdateHealthBar(GameObject healthBar, float currentHP){
		Slider slider = healthBar.transform.GetChild(0).gameObject.GetComponent<Slider>();
		slider.value = currentHP;
	}

	public void ShowPlayerDamage(Vector3 hitPos, float damageAmount){
		GameObject playerDamage = Instantiate(playerDamageGO, hitPos, Quaternion.identity);
		playerDamage.transform.SetParent(playerDamageList);
		playerDamage.GetComponent<PlayerDamageUI>().SetPlayerDamageText(damageAmount);
	}

	public void ChangeQuestText(string text){
		questText.text = text;
	}
	#endregion
}
