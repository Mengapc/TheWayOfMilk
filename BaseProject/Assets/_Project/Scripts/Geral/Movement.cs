using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{

    [SerializeField] private InputAction control;
    [SerializeField] private float speed = 5f;
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 gravity;

    private Vector3 direction;
    private Vector3 moveDirection;
    private Vector3 velocity;
    private CharacterController characterController;


    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        Move();
    }

    public void DirectionMovoment(InputAction.CallbackContext context)
    {
        direction = new Vector3(context.ReadValue<Vector2>().x, 0, context.ReadValue<Vector2>().y);
    }


    private void Move()
    {

        if (direction != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            player.rotation = Quaternion.RotateTowards(player.rotation, toRotation, 720 * Time.deltaTime);
        }

        velocity = direction * speed;
        moveDirection = velocity;
        moveDirection.y += gravity.y * Time.deltaTime;
        characterController.Move(moveDirection * Time.deltaTime);

    }
}
