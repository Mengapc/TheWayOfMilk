using UnityEngine;
using UnityEngine.InputSystem;

public class MovementScale : MonoBehaviour
{
    public float PlayerRotationInfluence { get; private set; }

    [Header("Referências")]
    [SerializeField] private PainelScaleCollider painel;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private ScaleManager scaleManager;

    [Header("Controle")]
    private Vector2 inputVector;
    private bool isPlayerControlling = false;

    [Header("Configuração de Rotação do Jogador")]
    [SerializeField] private float rotationSpeed = 20f;
    [SerializeField] private float maxRotationFromPlayer = 30f;

    [Header("Configuração de Movimento (Posição)")]
    [SerializeField] private Transform scale;
    [SerializeField] private Transform pointMove;
    [SerializeField] private Vector3 offsetPosition;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float maxDistance;
    private Vector3 initialPosition;


    private void Start()
    {
        inputVector = Vector3.zero;
        scale.position = pointMove.position;
        initialPosition = scale.position;
    }

    private void Update()
    {
        if (isPlayerControlling)
        {
            MoveScale();
            CalculatePlayerRotation();
        }
        else
        {
            PlayerRotationInfluence = Mathf.Lerp(PlayerRotationInfluence, 0f, Time.deltaTime * rotationSpeed);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        inputVector = context.ReadValue<Vector2>();
    }

    public void SwithControl(InputAction.CallbackContext context)
    {
        if (playerInput == null)
        {
            Debug.LogError("A referência do PlayerInput não foi atribuída no Inspector do MovementScale!");
            return;
        }

        if (context.performed && painel.IsColliding)
        {
            isPlayerControlling = !isPlayerControlling;

            if (isPlayerControlling)
            {
                playerInput.SwitchCurrentActionMap("Scale");
            }
            else
            {
                playerInput.SwitchCurrentActionMap("Player");
            }
        }
    }

    private void MoveScale()
    {
        Vector3 targetPosition = pointMove.position + offsetPosition + new Vector3(inputVector.x, 0f, 0f);
        if (Vector3.Distance(initialPosition, targetPosition) < maxDistance)
        {
            pointMove.position = Vector3.Lerp(pointMove.position, targetPosition, Time.deltaTime * moveSpeed);
            scale.position = pointMove.position;
        }
    }

    private void CalculatePlayerRotation()
    {
        float rotationAmount = inputVector.y * rotationSpeed * Time.deltaTime;
        PlayerRotationInfluence += rotationAmount;
        PlayerRotationInfluence = Mathf.Clamp(PlayerRotationInfluence, -maxRotationFromPlayer, maxRotationFromPlayer);
    }
}

