using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    [SerializeField] private float speed = 5f;
    [SerializeField] public Transform player; // O transform do modelo do jogador para rotação

    [Header("Configurações de Física")]
    [SerializeField] private float gravityValue = -9.81f;

    private Vector3 inputDirection;
    private Vector3 playerVelocity; // Agora esta variável vai acumular a gravidade
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
        // --- 1. Checagem de Chão ---
        // characterController.isGrounded nos diz se o personagem está tocando o chão
        bool isGrounded = characterController.isGrounded;
        if (isGrounded && playerVelocity.y < 0)
        {
            // Se estiver no chão, zera a velocidade vertical para não acumular gravidade
            playerVelocity.y = -2f;
        }

        // Se o movimento estiver desativado, o jogador não se move
        if (!podeMover)
        {
            // Aplica apenas gravidade quando o movimento está desativado
            characterController.Move(playerVelocity * Time.deltaTime);
            return;
        }

        // --- 2. Movimento e Rotação ---
        Move();
        Rotate();

        // --- 3. Aplicação da Gravidade ---
        // A gravidade é adicionada à velocidade vertical a cada frame, fazendo o jogador acelerar para baixo
        playerVelocity.y += gravityValue * Time.deltaTime;
        // Aplica o movimento vertical (gravidade) ao personagem
        characterController.Move(playerVelocity * Time.deltaTime);
    }

    // Função pública para ser chamada por outros scripts (ex: elevador)
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

        // A direção do input é sempre lida
        Vector2 input = context.ReadValue<Vector2>();
        inputDirection = new Vector3(input.x, 0, input.y);
    }

    private void Rotate()
    {
        // A rotação só acontece se o jogador NÃO estiver carregando o arremesso.
        if (objectGrabbing != null && objectGrabbing.estaCarregandoArremesso)
        {
            return; // Interrompe a função de rotação
        }

        if (player != null && inputDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(inputDirection, Vector3.up);
            player.rotation = Quaternion.RotateTowards(player.rotation, toRotation, 720 * Time.deltaTime);
        }
    }

    private void Move()
    {
        // O movimento horizontal é aplicado separadamente da gravidade
        Vector3 move = inputDirection * speed;
        characterController.Move(move * Time.deltaTime);
    }
}

