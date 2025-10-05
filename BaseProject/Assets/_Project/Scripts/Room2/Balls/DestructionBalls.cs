using UnityEngine;

public class DestructionBalls : MonoBehaviour
{
    [SerializeField] private Collider destructCollider;
    [SerializeField] private GameObject destructGameObject;
    [SerializeField] private SpanwBalls spanwBalls;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            destructGameObject = other.gameObject;
            Destroy(other.gameObject);
            spanwBalls.spawnedBalls--;
        }
    }
}
