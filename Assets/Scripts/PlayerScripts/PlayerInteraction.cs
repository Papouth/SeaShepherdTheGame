using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    #region Variables
    private PlayerInputManager playerInput;

    private bool _onClick = false;
    #endregion

    #region Properties
    #endregion

    #region Built-in Methods
    private void Start()
    {
        playerInput = GetComponent<PlayerInputManager>();
    }

    private void Update()
    {
        Honking();
        if (playerInput.CanSelect){
            OnPlayerSelect();
        }
        else if (_onClick){
            _onClick = false;
        }
    }
    #endregion

    #region Custom Methods
    private void Honking()
    {
        // Bruit du klaxon
        playerInput.CanHonk = false;
    }

    private void OnPlayerSelect(){
        if (!_onClick){
            _onClick = true;
            RaycastHit hit;
            Vector3 mousePosOnClick = new Vector3(playerInput.MousePos.x, playerInput.MousePos.y, Camera.main.nearClipPlane);
            Ray ray = Camera.main.ScreenPointToRay(mousePosOnClick);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity)){
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.position = hit.point;
            }
        }
    }
    #endregion
}