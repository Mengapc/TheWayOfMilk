using UnityEngine;
using System.Collections.Generic;



public enum RoomIdentifier
{
    Sala1,
    Sala2,
    Sala3
}
[RequireComponent(typeof(Rigidbody))]
public class BallController : MonoBehaviour
{
    [Header("Configurações Gerais da Bola")]
    [SerializeField] private Vector3 initialPosition;
    [SerializeField] private Quaternion initialRotation;
    [SerializeField] private Rigidbody rb;
    [SerializeField] public RoomIdentifier roomIdentifier;

    [Header("Propriedades da Bola Sala 1")]
    [SerializeField] public GameObject ballPrefab;
    [SerializeField] public Transform spawnPoint;

    [Header("Propriedades da Bola Sala 2")]
    [SerializeField] public bool resetavel = true;
    [SerializeField] public int weight = 1;
    [SerializeField] private Color color;
    [SerializeField] private Material baseMaterial;
    [SerializeField] private List<int> possibleWeights;
    [SerializeField] private List<Color> possibleColors;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }
    void Start()
    {
        InitiateBall();
    }
    private void InitiateBall()
    {
        // Esta lógica de inicialização está perfeita e não precisa mudar.
        if (possibleColors.Count > 0 && possibleWeights.Count > 0)
        {
            if (possibleColors.Count != possibleWeights.Count)
            {
                Debug.LogWarning("As listas de cores e pesos devem ter o mesmo tamanho.");
                return;
            }

            int randomIndex = Random.Range(0, possibleWeights.Count);
            weight = possibleWeights[randomIndex];
            rb.mass = weight;
            color = possibleColors[randomIndex];

            Renderer renderer = GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = color;
            }
        }
    }
    public void ResetPosition()
    {
        rb.linearVelocity = Vector3.zero; // Corrigido de linearVelocity
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = false;

        transform.position = initialPosition;
        transform.rotation = initialRotation;
    }

    public void Destroy()
    {
        Destroy(this.gameObject);
    }

    public void InstatiateBall()
    {
        if (roomIdentifier == RoomIdentifier.Sala1 || roomIdentifier == RoomIdentifier.Sala3)
        {
            Instantiate(ballPrefab, spawnPoint.position, Quaternion.identity);
        }
    }

    public void OnDestroy()
    {
        InstatiateBall();
    }
}
