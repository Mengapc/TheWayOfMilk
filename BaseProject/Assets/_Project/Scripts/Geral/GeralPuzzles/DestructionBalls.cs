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
        ballController = other.GetComponent<BallController>();
        if (other.CompareTag("Ball"))
        {
            
            /* Arrumar para o primeira puzzle
            if (ballController.roomIdentifier == RoomIdentifier.Sala1)
            {
                Instantiate(ballController.ballPrefab, ballController.spawnPoint.position, ballController.spawnPoint.rotation);
            }
            */
            if (ballController.roomIdentifier == RoomIdentifier.Sala2)
            {
                spanwBalls.spawnedBalls--;
            }
            Destroy(ballController);
            Destroy(other);
        }
    }
}