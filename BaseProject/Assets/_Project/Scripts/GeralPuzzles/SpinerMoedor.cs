using UnityEngine;

public class SpinerMoedor : MonoBehaviour
{
    [Header("Configura��es de Rota��o")]
    [Tooltip("Velocidade de rota��o em graus por segundo.")]
    [SerializeField] private float rotationSpeed = 90f;
    [Tooltip("Dire��o da rota��o (true para hor�rio, false para anti-hor�rio).")]
    [SerializeField] private bool rotateClockwise = true;

    private void Update()
    {
        Rotate();
    }

    private void Rotate()
    {
        float rotationDirection = rotateClockwise ? 1f : -1f;
        transform.Rotate(Vector3.up, rotationSpeed * rotationDirection * Time.deltaTime);
    }
}
