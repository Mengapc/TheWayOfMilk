using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class BallController : MonoBehaviour
{
    [Header("Configurações Gerais da Bola")]
    private Vector3 initialPosition; // Não precisa ser serializado se for pego no Awake
    private Quaternion initialRotation; // Não precisa ser serializado se for pego no Awake
    private Rigidbody rb;

    [Header("Propriedades da Bola")]
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

    // MUDANÇA 3: O método GenerationNewBall() foi removido completamente.
    // A responsabilidade de criar bolas agora é 100% do SpanwBalls.

    public void DestroyBall() => Destroy(gameObject);
}
