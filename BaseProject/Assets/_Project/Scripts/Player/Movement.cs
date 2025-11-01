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
    [SerializeField] private float rotationSpeed = 720f; // Adicionado para ser mais flexível

    [Space]
    [Header("Configurações de Física.")]
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
        // CORREÇÃO 1 e 2:
        // Checamos se há input (sqrMagnitude > 0.01f) para evitar o erro de LookRotation
        // e para o personagem só girar quando estiver se movendo.
        if (inputDirection.sqrMagnitude > 0.01f)
        {
            // Calcula a rotação alvo (para onde queremos olhar)
            Quaternion toRotation = Quaternion.LookRotation(inputDirection, Vector3.up);

            // Gira suavemente do 'player.rotation' atual para o 'toRotation' alvo
            // Usei a variável 'rotationSpeed' que criei no topo
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
