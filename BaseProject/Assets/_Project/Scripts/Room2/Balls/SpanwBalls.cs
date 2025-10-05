using UnityEngine;

public class SpanwBalls : MonoBehaviour
{
    [Header("Configurações de Spawn")]
    [Tooltip("Prefab da bola a ser instanciada. Arraste o prefab do GameObject aqui.")]
    [SerializeField] private GameObject ballPrefab;
    [Tooltip("Área onde as bolas serão spawnadas.")]
    [SerializeField] private Collider areaSpanw;
    [Tooltip("Número total de bolas a serem spawnadas.")]
    [SerializeField] public int numberOfBalls = 10;
    [Tooltip("Intervalo de tempo entre cada spawn de bola.")]
    [SerializeField] private float spawnInterval = 1f;
    [SerializeField] private ScaleManager scaleManager;

    private float timer;
    public int spawnedBalls = 0;
    private Vector3 spawnPosition;


    private void Start()
    {
        if (ballPrefab == null)
        {
            Debug.LogError("O Prefab da Bola não foi atribuído no Inspector!", this.gameObject);
        }
    }

    private void Update()
    {
        if (spawnedBalls >= numberOfBalls && scaleManager.isFinalized) return;
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnBall();
        }
    }

    private void SpawnBall()
    {
        if (spawnedBalls >= numberOfBalls) return;

        spawnPosition = new Vector3(
            Random.Range(areaSpanw.bounds.min.x, areaSpanw.bounds.max.x),
            Random.Range(areaSpanw.bounds.min.y, areaSpanw.bounds.max.y),
            Random.Range(areaSpanw.bounds.min.z, areaSpanw.bounds.max.z)
        );

        GameObject newBall = Instantiate(ballPrefab, spawnPosition, Quaternion.identity);

        spawnedBalls++;
        timer = 0f;
    }

    public void ForceSpawnNewBall()
    {
        if (spawnedBalls < numberOfBalls)
        {
            SpawnBall();
        }
    }
}

