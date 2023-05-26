using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Island : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI actualIsland;
    [SerializeField] private AudioClip seagul;
    private AudioSource source;


    private void Start()
    {
        source = GetComponent<AudioSource>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            actualIsland.text = gameObject.name;
            source.PlayOneShot(seagul);
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
            source.Stop();
        }
    }
}