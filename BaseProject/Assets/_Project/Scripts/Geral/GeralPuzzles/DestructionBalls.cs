using UnityEngine;

public class DestructionBalls : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Collider que detecta a colisão com as bolas para destruí-las.")]
    [SerializeField] private Collider destructCollider;
    [Tooltip("Referência ao script SpanwBalls para controlar o número de bolas spawnadas(sala 2).")]
    [SerializeField] private SpanwBalls spanwBalls;
    [Tooltip("Referência ao script BallController para acessar propriedades da bola.")]
    [SerializeField] private BallController ballController;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            ballController = other.GetComponent<BallController>();
            ballController.Destroy();

            if (ballController.roomIdentifier == RoomIdentifier.Sala1)
            {
                Instantiate(ballController.ballPrefab, ballController.spawnPoint.position, Quaternion.identity);
            }
            if (ballController.roomIdentifier == RoomIdentifier.Sala2)
            {
                spanwBalls.spawnedBalls--;
            }
        }
    }
}
