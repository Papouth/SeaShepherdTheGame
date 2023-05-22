using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    #region Variables
    private PlayerInputManager playerInput;
    #endregion


    private void Start()
    {
        playerInput = GetComponent<PlayerInputManager>();
    }

    private void Update()
    {
        Honking();
    }

    private void Honking()
    {
        // Bruit du klaxon
        playerInput.CanHonk = false;
    }
}