using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [Header("Configurações de Movimento.")]
    [SerializeField] private float speed = 5f;
    [SerializeField] public Transform player;
    [SerializeField] private Elevator elevator;

    [Space]
    [Header("Configurações de Física.")]
    [SerializeField] private float gravityValue = -9.81f;

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
        if (objectGrabbing != null && objectGrabbing.estaCarregandoArremesso)
        {
            return;
        }

        if (player != null && inputDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(inputDirection, Vector3.up);
            player.rotation = Quaternion.RotateTowards(player.rotation, toRotation, 720 * Time.deltaTime);
        }
    }

    private void Move()
    {
        Vector3 move = inputDirection * speed;
        characterController.Move(move * Time.deltaTime);
    }
}
