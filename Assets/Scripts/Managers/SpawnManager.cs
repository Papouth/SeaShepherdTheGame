using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
	#region Variables
	[SerializeField] private GameObject field;
	#endregion
	
	#region Properties
	#endregion
	
	#region Built-in Methods
    void Start()
    {
        print(field.GetComponent<Renderer>().bounds.size);
    }

    void Update()
    {
        
    }
	#endregion
	
	#region Custom Methods
	#endregion
}
