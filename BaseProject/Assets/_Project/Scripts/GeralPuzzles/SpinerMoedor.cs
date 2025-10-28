using UnityEngine;

public class SpinerMoedor : MonoBehaviour
{
    [Header("Configurações de Rotação")]
    [Tooltip("Velocidade de rotação em graus por segundo.")]
    [SerializeField] private float rotationSpeed = 90f;
    [Tooltip("Direção da rotação (true para horário, false para anti-horário).")]
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
