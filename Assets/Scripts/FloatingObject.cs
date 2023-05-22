using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    #region Variables
    [Tooltip("Hauteur de flotaison")]
    [SerializeField] private float height = 0.1f;
    [Tooltip("Rapidité d'une période de flotaison")]
    [SerializeField] private float timer = 1;

    private Vector3 initialPos;
    private float offset;
    #endregion


    private void Awake()
    {
        initialPos = transform.position;

        offset = 1 - (Random.value * 2);
    }

    private void Update()
    {
        FloatingEffect();
    }

    /// <summary>
    /// Permet de simuler un effet de flotaison sur les objets
    /// </summary>
    private void FloatingEffect()
    {
        transform.position = initialPos - Vector3.up * Mathf.Sin((Time.time + offset) * timer) * height;
    }
}