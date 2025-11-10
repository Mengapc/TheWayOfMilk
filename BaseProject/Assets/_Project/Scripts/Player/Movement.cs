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
    [Header("Áudio de Passos")]
    [Tooltip("Array de sons de passos (tocados aleatoriamente).")]
    [SerializeField] private AudioClip[] footstepSounds; 
    [Tooltip("O intervalo (em segundos) entre cada som de passo.")] 
    [SerializeField] private float footstepInterval = 0.5f; 
    [Tooltip("Volume dos passos.")] 
    [Range(0f, 1f)] 
    [SerializeField] private float footstepVolume = 0.8f;

    [Space]
	[Header("Referências de Animação")]
	[Tooltip("Referência ao script que controla o Animator.")]
	[SerializeField] private PlayerAnimationController animController;

	private Vector3 inputDirection;
	private Vector3 playerVelocity;
	private CharacterController characterController;
	private Elevator currentElevator = null;
    private float footstepTimer;

    // --- ADIÇÃO ---
    // Permite que outros scripts (como o ObjectGrabbing) anulem a rotação de movimento
    public bool overrideRotation = false;
	// --- FIM DA ADIÇÃO ---


	private void Awake()
	{
		characterController = GetComponent<CharacterController>();

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
		Rotate(); // A rotação normal só será executada se 'overrideRotation' for falso


		// --- LÓGICA DE ANIMAÇÃO SIMPLIFICADA ---
		// Usamos sqrMagnitude por ser mais otimizado que Magnitude
		float currentSpeed = inputDirection.sqrMagnitude;
		animController?.SetMoveSpeed(currentSpeed);
        // --- FIM DA LÓGICA DE ANIMAÇÃO ---

        HandleFootsteps(currentSpeed, isGrounded);

        playerVelocity.y += gravityValue * Time.deltaTime;
		characterController.Move(playerVelocity * Time.deltaTime);
	}

    private void HandleFootsteps(float currentSpeedSqr, bool isGrounded)
    {
        // 1. O timer só conta para baixo se for maior que zero
        if (footstepTimer > 0)
        {
            footstepTimer -= Time.deltaTime;
        }

        // 2. Verifica se PODE tocar o som
        // Condições: Movendo, no chão, e o timer zerou
        if (currentSpeedSqr > 0.01f && isGrounded && footstepTimer <= 0)
        {
            // 3. Verifica se temos os recursos de áudio
            if (SoundFXManager.instance != null && footstepSounds != null && footstepSounds.Length > 0)
            {
                // Toca o som (usando 'transform' para a posição do player)
                SoundFXManager.instance.PlayRandomSoundFXClip(footstepSounds, transform, footstepVolume);

                // Reseta o timer
                footstepTimer = footstepInterval;
            }
        }
    }

    public void DirectionMovoment(InputAction.CallbackContext context)
	{


		Vector2 input = context.ReadValue<Vector2>();
		inputDirection = new Vector3(input.x, 0, input.y);
	}

	private void Rotate()
	{
		// --- ADIÇÃO ---
		// Se a rotação estiver a ser controlada por outro script (ex: mira), não faz nada.
		if (overrideRotation) return;
		// --- FIM DA ADIÇÃO ---

		if (inputDirection.sqrMagnitude > 0.01f)
		{
			// Calcula a rotação alvo (para onde queremos olhar)
			Quaternion toRotation = Quaternion.LookRotation(inputDirection, Vector3.up);

			player.rotation = Quaternion.RotateTowards(player.rotation, toRotation, rotationSpeed * Time.deltaTime);
		}
	}

	// --- NOVA FUNÇÃO PÚBLICA ---
	// Gira o jogador para uma direção específica (usado pela mira)
	public void RotateTowards(Vector3 targetDirection)
	{
		if (targetDirection.sqrMagnitude > 0.01f)
		{
			Quaternion toRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
			player.rotation = Quaternion.RotateTowards(player.rotation, toRotation, rotationSpeed * Time.deltaTime);
		}
	}
	// --- FIM DA NOVA FUNÇÃO ---

	private void Move()
	{
		if (canMove == false)
		{
			inputDirection = Vector3.zero;
		}
		Vector3 move = inputDirection * speed;
		characterController.Move(move * Time.deltaTime);
	}

	public void SetCanMove()
	{
		canMove = !canMove;
    }

    // --- Lógica do Elevador (existente) ---
    public void OnElevatorInput(InputAction.CallbackContext context)
	{        // Se o botão foi pressionado (performed) e estamos dentro de um elevador
		if (context.performed && currentElevator != null)
		{
			// Ativa o elevador específico em que estamos
			currentElevator.ElevatorActivation();
		}
	}

	// Esta função é chamada pelo ElavatorCollider quando entramos no trigger
	public void SetCurrentElevator(Elevator elevator)
	{
		this.currentElevator = elevator;
	}

	// Esta função é chamada pelo ElavatorCollider quando saímos do trigger
	public void ClearCurrentElevator()
	{
		this.currentElevator = null;
	}
}