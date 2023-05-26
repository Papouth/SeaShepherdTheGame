using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    #region Variables
    private PlayerInputManager playerInput;

    private bool _onClick = false;
    private float _damageAmount;
    private AudioSource source;
    [SerializeField] AudioClip honkClip;
    #endregion

    #region Properties
    public float DamageAmount{
        get{return _damageAmount;}
        set{_damageAmount = value;}
    }
    #endregion

    #region Built-in Methods
    private void Start()
    {
        playerInput = GetComponent<PlayerInputManager>();
        source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (playerInput.CanHonk) Honking();

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
        source.PlayOneShot(honkClip);
        playerInput.CanHonk = false;
    }

    //Lors d'un click, check ce que le joueur a clique puis agit en consequence
    private void OnPlayerSelect(){
        if (!_onClick){
            _onClick = true;
            RaycastHit hit;
            Vector3 mousePosOnClick = new Vector3(playerInput.MousePos.x, playerInput.MousePos.y, Camera.main.nearClipPlane);
            Ray ray = Camera.main.ScreenPointToRay(mousePosOnClick);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity)){
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Enemy")){
                    hit.transform.gameObject.GetComponent<Enemy>().TakeDamage(hit.point, _damageAmount);
                }
                else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Fish")){
                    hit.transform.gameObject.GetComponent<FishToRescue>().TakeDamage(hit.point, _damageAmount);
                }
                else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Waste")){
                    hit.transform.parent.parent.gameObject.GetComponent<Waste>().TakeDamage(hit.point, _damageAmount);
                }
            }
        }
    }
    #endregion
}