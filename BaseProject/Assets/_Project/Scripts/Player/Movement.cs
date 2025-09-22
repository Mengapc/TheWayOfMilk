using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [Header("Configura��es de Movimento")]
    [SerializeField] private float speed = 5f;
    [SerializeField] public Transform player; // O transform do modelo do jogador para rota��o

    [Header("Configura��es de F�sica")]
    [SerializeField] private float gravityValue = -9.81f;

    private Vector3 inputDirection;
    private Vector3 playerVelocity; // Agora esta vari�vel vai acumular a gravidade
    private CharacterController characterController;
    private ObjectGrabbing objectGrabbing;

    // Controla se o jogador pode se mover
    private bool podeMover = true;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        objectGrabbing = GetComponent<ObjectGrabbing>();
    }

    void Update()
    {
        // --- 1. Checagem de Ch�o ---
        // characterController.isGrounded nos diz se o personagem est� tocando o ch�o
        bool isGrounded = characterController.isGrounded;
        if (isGrounded && playerVelocity.y < 0)
        {
            // Se estiver no ch�o, zera a velocidade vertical para n�o acumular gravidade
            playerVelocity.y = -2f;
        }

        // Se o movimento estiver desativado, o jogador n�o se move
        if (!podeMover)
        {
            // Aplica apenas gravidade quando o movimento est� desativado
            characterController.Move(playerVelocity * Time.deltaTime);
            return;
        }

        // --- 2. Movimento e Rota��o ---
        Move();
        Rotate();

        // --- 3. Aplica��o da Gravidade ---
        // A gravidade � adicionada � velocidade vertical a cada frame, fazendo o jogador acelerar para baixo
        playerVelocity.y += gravityValue * Time.deltaTime;
        // Aplica o movimento vertical (gravidade) ao personagem
        characterController.Move(playerVelocity * Time.deltaTime);
    }

    // Fun��o p�blica para ser chamada por outros scripts (ex: elevador)
    public void ToggleMovement(bool enable)
    {
        podeMover = enable;
        if (!enable)
        {
            inputDirection = Vector3.zero; // Para o movimento horizontal imediatamente
        }
    }

    public void DirectionMovoment(InputAction.CallbackContext context)
    {
        if (!podeMover)
        {
            inputDirection = Vector3.zero;
            return;
        }
        ;

        // A dire��o do input � sempre lida
        Vector2 input = context.ReadValue<Vector2>();
        inputDirection = new Vector3(input.x, 0, input.y);
    }

    private void Rotate()
    {
        // A rota��o s� acontece se o jogador N�O estiver carregando o arremesso.
        if (objectGrabbing != null && objectGrabbing.estaCarregandoArremesso)
        {
            return; // Interrompe a fun��o de rota��o
        }

        if (player != null && inputDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(inputDirection, Vector3.up);
            player.rotation = Quaternion.RotateTowards(player.rotation, toRotation, 720 * Time.deltaTime);
        }
    }

    private void Move()
    {
        // O movimento horizontal � aplicado separadamente da gravidade
        Vector3 move = inputDirection * speed;
        characterController.Move(move * Time.deltaTime);
    }
}

