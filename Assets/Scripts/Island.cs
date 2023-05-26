using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Island : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI actualIsland;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            actualIsland.text = gameObject.name;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            actualIsland.text = gameObject.name;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            actualIsland.text = "";
        }
    }
}