using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallController : MonoBehaviour
{
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    // M�todo que retorna a bola ao seu estado e posi��o originais
    public void ResetPosition()
    {
        // Zera qualquer movimento que a bola tenha
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Garante que a f�sica seja reativada caso a bola estivesse dentro de um tubo
        rb.isKinematic = false;

        // Move a bola de volta para o ponto de partida
        transform.position = initialPosition;
        transform.rotation = initialRotation;
    }
}

