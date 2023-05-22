using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Variables
    [Header("Player Movement")]
    public float moveSpeed = 3f;
    private float currentSpeed;
    public Vector3 directionInput;
    private Vector3 movement;
    [SerializeField] private float turnSmoothTime = 0.1f;
    [SerializeField] private float turnSmoothVelocity = 0.1f;

    [Header("Flotaison")]
    [Tooltip("Hauteur de flotaison")]
    [SerializeField] private float height = 0.1f;
    [Tooltip("Rapidité d'une période de flotaison")]
    [SerializeField] private float timer = 1;

    private Vector3 initialPos;
    private float offset;
    private float playerY;

    [Header("Player Component")]
    public Camera cam;
    private PlayerInputManager playerInput;
    public Animator animator;
    private Rigidbody rb;
    #endregion

    private void Awake()
    {
        playerInput = GetComponent<PlayerInputManager>();
        //animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();


        initialPos = transform.position;

        offset = 1 - (Random.value * 2);
    }

    private void Start()
    {
        currentSpeed = moveSpeed;
    }

    private void Update()
    {
        Locomotion();

        SetAnimator();

        FloatingEffect();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(movement.x * 10, playerY, movement.z * 10);
    }

    /// <summary>
    /// Gere le deplacement du personnage avec le character controller
    /// </summary>
    public void Locomotion()
    {
        if (!playerInput) return;

        directionInput.Set(playerInput.MoveInput.x, 0, playerInput.MoveInput.y);

        // Joueur regarde dans la direction où il se déplace
        if (directionInput.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(directionInput.x, directionInput.z) * Mathf.Rad2Deg +
                cam.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle,
                ref turnSmoothVelocity, turnSmoothTime);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            directionInput = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        }

        movement = directionInput.normalized * (currentSpeed * Time.deltaTime);
    }

    private void SetAnimator()
    {
        // On peut imaginer de la fumée qui sort de la cheminée par exemple
        //animator.SetFloat("Movement", directionInput.magnitude);
    }


    /// <summary>
    /// Permet de simuler un effet de flotaison sur les objets
    /// </summary>
    private void FloatingEffect()
    {
        playerY = initialPos.y * Mathf.Sin((Time.time + offset) * timer) * height;
    }
}