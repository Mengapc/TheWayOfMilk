using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{


    [Header("Configura��es de Movimento.")]
    [SerializeField] public bool canMove = true;
    [Tooltip("Velocidade de movimento do jogador.")]
    [SerializeField] private float speed = 5f;
    [Tooltip("Transform do jogador para rota��o.")]
    [SerializeField] public Transform player;
    [Tooltip("Dire��o do movimento do jogador.")]
    [SerializeField] private Direction direction;
    [Tooltip("Velocidade da rota��o em graus por segundo.")]
    [SerializeField] private float rotationSpeed = 720f; // Adicionado para ser mais flex�vel

    [Space]
    [Header("Configura��es de F�sica.")]
    [Tooltip("Valor da gravidade aplicada ao jogador.")]
    [SerializeField] public float gravityValue = -9.81f;

    private Vector3 inputDirection;
    private Vector3 playerVelocity;
    private CharacterController characterController;
    private ObjectGrabbing objectGrabbing;


    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        objectGrabbing = GetComponent<ObjectGrabbing>();
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
        // CORRE��O 1 e 2:
        // Checamos se h� input (sqrMagnitude > 0.01f) para evitar o erro de LookRotation
        // e para o personagem s� girar quando estiver se movendo.
        if (inputDirection.sqrMagnitude > 0.01f)
        {
            // Calcula a rota��o alvo (para onde queremos olhar)
            Quaternion toRotation = Quaternion.LookRotation(inputDirection, Vector3.up);

            // Gira suavemente do 'player.rotation' atual para o 'toRotation' alvo
            // Usei a vari�vel 'rotationSpeed' que criei no topo
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
