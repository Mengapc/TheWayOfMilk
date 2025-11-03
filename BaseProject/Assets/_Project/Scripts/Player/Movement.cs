using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{


    [Header("Configurações de Movimento.")]
    [SerializeField] public bool canMove = true;
    [Tooltip("Velocidade de movimento do jogador.")]
    [SerializeField] private float speed = 5f;
    [Tooltip("Transform do jogador para rotação.")]
    [SerializeField] public Transform player;
    [Tooltip("Direção do movimento do jogador.")]
    [SerializeField] private Direction direction;
    [Tooltip("Velocidade da rotação em graus por segundo.")]
    [SerializeField] private float rotationSpeed = 720f; 

    [Space]
    [Header("Configurações de Física.")]
    [Tooltip("Valor da gravidade aplicada ao jogador.")]
    [SerializeField] public float gravityValue = -9.81f;

    [Space]
    [Header("Referências de Animação")]
    [Tooltip("Referência ao script que controla o Animator.")]
    [SerializeField] private PlayerAnimationController animController;

    private Vector3 inputDirection;
    private Vector3 playerVelocity;
    private CharacterController characterController;
    private ObjectGrabbing objectGrabbing;


    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        objectGrabbing = GetComponent<ObjectGrabbing>();

        if (animController == null)
        {
            animController = GetComponent<PlayerAnimationController>();
        }
    }

    void Update()
    {
        bool isGrounded = characterController.isGrounded;
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }

        Move();
        Rotate();


        float currentSpeed = inputDirection.sqrMagnitude;
        animController?.SetMoveSpeed(currentSpeed);


        playerVelocity.y += gravityValue * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);
    }



    public void DirectionMovoment(InputAction.CallbackContext context)
    {


        Vector2 input = context.ReadValue<Vector2>();
        inputDirection = new Vector3(input.x, 0, input.y);
    }

    private void Rotate()
    {
        if (inputDirection.sqrMagnitude > 0.01f)
        {
            Quaternion toRotation = Quaternion.LookRotation(inputDirection, Vector3.up);

            player.rotation = Quaternion.RotateTowards(player.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void Move()
    {
        if (canMove == false)
        {
            inputDirection = Vector3.zero;
        }
        Vector3 move = inputDirection * speed;
        characterController.Move(move * Time.deltaTime);
    }
}

