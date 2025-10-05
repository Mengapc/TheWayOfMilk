using UnityEngine;

public class DestructionBalls : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Collider que detecta a colis�o com as bolas para destru�-las.")]
    [SerializeField] private Collider destructCollider;
    [Tooltip("Refer�ncia ao script SpanwBalls para controlar o n�mero de bolas spawnadas(sala 2).")]
    [SerializeField] private SpanwBalls spanwBalls;
    [Tooltip("Refer�ncia ao script BallController para acessar propriedades da bola.")]
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
