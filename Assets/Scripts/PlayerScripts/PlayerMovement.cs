using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerMovement : MonoBehaviour
{
    #region Variables
    [Header("Player Movement")]
    public float moveSpeed = 3f;
    private float currentSpeed;
    public Vector3 directionInput;
    private Vector3 movement;

    [Header("Player Click Movement")]
    private Vector3 newPos;
    private Coroutine coroutine;
    [SerializeField] private LayerMask groundLayer;

    [Header("Flotaison")]
    [Tooltip("Hauteur de flotaison")]
    [SerializeField] private float height = 0.1f;
    [Tooltip("Rapidité d'une période de flotaison")]
    [SerializeField] private float timer = 1;

    private Vector3 initialPos;
    private float offset;
    private float playerY;

    [Header("MiniMap")]
    private bool camOnMap;
    private int hightCap = 10;
    private int lowCap = 5;

    [Header("Player Component")]
    private Camera cam;
    [SerializeField] private CinemachineVirtualCamera playerCam;
    [SerializeField] private CinemachineVirtualCamera mapCam;
    private PlayerInputManager playerInput;
    private Rigidbody rb;
    #endregion

    #region Built-in Methods
    private void Awake()
    {
        cam = Camera.main;
        playerInput = GetComponent<PlayerInputManager>();
        rb = GetComponent<Rigidbody>();

        initialPos = transform.position;

        offset = 1 - (Random.value * 2);
    }

    private void Start()
    {
        currentSpeed = moveSpeed;
        newPos = transform.position;
    }

    private void Update()
    {
        RightClickMovment();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(movement.x * 10, playerY, movement.z * 10);

        if (rb.velocity == Vector3.zero) FloatingEffect();
    }
    #endregion


    #region Customs Methods
    private void RightClickMovment()
    {
        if (playerInput.CanRightClick)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(playerInput.MousePos);
            if (Physics.Raycast(ray, out hit, 200f, groundLayer))
            {
                if (coroutine != null) StopCoroutine(coroutine);
                coroutine = StartCoroutine(PlayerMoveTowards(hit.point));
            }
        }
    }

    private IEnumerator PlayerMoveTowards(Vector3 target)
    {
        float playerDistanceToFloor = transform.position.y - target.y;
        target.y += playerDistanceToFloor;

        while (Vector3.Distance(transform.position, target) > 0.2f)
        {
            // smooth la rotation
            var targetRotation = Quaternion.LookRotation(target - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.85f * Time.deltaTime);

            rb.AddRelativeForce(Vector3.forward * currentSpeed * 20, ForceMode.Force);

            if (Vector3.Distance(transform.position, target) <= 5f)
            {
                playerInput.CanRightClick = false;
                if (coroutine != null) StopCoroutine(coroutine);
            }

            yield return null;
        }
    }

    public void SwitchCameraMap()
    {
        if (!camOnMap)
        {
            // On met la camera sur la mini map
            playerCam.Priority = lowCap;
            mapCam.Priority = hightCap;
            camOnMap = true;

            // On executera le code d'ajout d'UI sur la mini map ici
        }
        else if (camOnMap)
        {
            // On met la camera sur le joueur
            playerCam.Priority = hightCap;
            mapCam.Priority = lowCap;
            camOnMap = false;
        }
    }

    /// <summary>
    /// Permet de simuler un effet de flotaison sur les objets
    /// </summary>
    private void FloatingEffect()
    {
        playerY = initialPos.y * Mathf.Sin((Time.time + offset) * timer) * height;
    }
    #endregion
}