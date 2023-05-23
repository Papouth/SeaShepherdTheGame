using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerDamageUI : MonoBehaviour
{
	#region Variables
	[SerializeField] private float timeBeforeDestroy = 1f;
	#endregion
	
	#region Properties
	#endregion
	
	#region Built-in Methods
    void Start()
    {
        transform.LookAt(Camera.main.transform);
		Vector3 onlyXRotation = new Vector3(transform.eulerAngles.x, 0, 0);
		transform.eulerAngles = -onlyXRotation;
		StartCoroutine(TimerBeforeDestroy());
		StartCoroutine(MovementAnim());
    }

    void Update()
    {
        
    }
	#endregion
	
	#region Custom Methods
	public void SetPlayerDamageText(float damageAmount){
		transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = damageAmount.ToString();
	}

	IEnumerator TimerBeforeDestroy(){
		yield return new WaitForSeconds(timeBeforeDestroy);
		Destroy(gameObject);
	}

	IEnumerator MovementAnim(){
		float randomX = Random.Range(-.5f, .5f) / 60 * timeBeforeDestroy;
		float randomY = Random.Range(0f, 1f) / 60 * timeBeforeDestroy;
		for (int i = 0; i < 60 * timeBeforeDestroy; i++){
			transform.Translate(new Vector3(randomX, randomY, 0));
			yield return (timeBeforeDestroy / 60);
		}
	}
	#endregion
}
