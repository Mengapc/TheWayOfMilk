using System.Collections.Generic;
using UnityEngine;

public class SpanwBalls : MonoBehaviour
{
    [Header("Configurações de Spawn")]
    [Tooltip("Prefab da bola a ser instanciada. Arraste o prefab do GameObject aqui.")]
    [SerializeField] private GameObject ballPrefab;
    [Tooltip("Área onde as bolas serão spawnadas.")]
    [SerializeField] private Collider areaSpanw;
    [Tooltip("Número total de bolas a serem spawnadas.")]
    [SerializeField] public int maxBalls = 10;
    [Tooltip("Intervalo de tempo entre cada spawn de bola.")]
    [SerializeField] private float spawnInterval = 1f;
    [Tooltip("Script de scaleManager da balança.")]
    [SerializeField] private ScaleManager scaleManager;

    [Header("Propriedades da Bola Sala 2")]
    [SerializeField] private Color color;
    [SerializeField] private Material baseMaterial;
    [SerializeField] private List<int> possibleWeights;
    [SerializeField] private List<Color> possibleColors;



    private float timer;
    [HideInInspector]
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
        if (spawnedBalls >= maxBalls && scaleManager.isFinalized) return;
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnBall();
        }
    }

    private void SpawnBall()
    {
        if (spawnedBalls >= maxBalls) return;

        // 1. Calcula a posição de spawn aleatória.
        Vector3 spawnPosition = new Vector3(
            Random.Range(areaSpanw.bounds.min.x, areaSpanw.bounds.max.x),
            Random.Range(areaSpanw.bounds.min.y, areaSpanw.bounds.max.y),
            Random.Range(areaSpanw.bounds.min.z, areaSpanw.bounds.max.z)
        );

        // 2. (FORMA CORRETA) Instancia a nova bola PRIMEIRO.
        GameObject newBall = Instantiate(ballPrefab, spawnPosition, Quaternion.identity);

        // 3. Modifica a bola recém-criada.
        InitiateBall(newBall);

        spawnedBalls++;
        timer = 0f;
    }

    public void ForceSpawnNewBall()
    {
        if (spawnedBalls < maxBalls)
        {
            SpawnBall();
        }
    }

    private GameObject InitiateBall(GameObject ball)
    {
        // Esta lógica de inicialização está perfeita e não precisa mudar.
        if (possibleColors.Count > 0 && possibleWeights.Count > 0)
        {
            if (possibleColors.Count != possibleWeights.Count)
            {
                Debug.LogWarning("As listas de cores e pesos devem ter o mesmo tamanho.");
                return null;
            }
            BallController ballScript = ball.GetComponent<BallController>();
            if (ballScript == null)
            {
                Debug.LogError("A bolo não possui um BallController.");
                return null;
            }

            int randomIndex = Random.Range(0, possibleWeights.Count);
            int weight = possibleWeights[randomIndex];
            ballScript.weight = weight;
            color = possibleColors[randomIndex];

            
            if (ball.GetComponent<Renderer>() != null)
            {
                ball.GetComponent<Renderer>().material.color = color;
            }
        }
        return ball;
    }
}

