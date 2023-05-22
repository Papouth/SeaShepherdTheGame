using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    #region Variables
    private Vector2 moveInput;
    private bool canSelect;
    private bool canHonk;
    #endregion


    #region Bool Functions
    public Vector2 MoveInput => moveInput;

    public bool CanSelect
    {
        get { return canSelect; }
        set { canSelect = value; }
    }

    public bool CanHonk
    {
        get { return canHonk; }
        set { canHonk = value; }
    }
    #endregion


    #region Functions
    public void OnMovement(InputValue value)
    {
        // On r�cup�re la valeur du mouvement qu'on stock dans un Vector2
        moveInput = value.Get<Vector2>();
    }

    public void OnSelect()
    {
        canSelect = true;
        Invoke("SelectTimer", 0.1f);
    }

    private void SelectTimer()
    {
        canSelect = false;
    }

    public void OnHonk()
    {
        canHonk = true;
    }
    #endregion
}