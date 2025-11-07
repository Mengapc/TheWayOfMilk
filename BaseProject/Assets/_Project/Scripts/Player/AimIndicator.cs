using UnityEngine;

public class AimIndicator : MonoBehaviour
{
    [Header("Referências")]
    [Tooltip("Arraste para cá o GameObject do Player que tem o script 'ObjectGrabbing'.")]
    [SerializeField] private ObjectGrabbing objectGrabbing;

    [Tooltip("Arraste para cá o GameObject que tem o script 'Direction'.")]
    [SerializeField] private Direction directionScript;

    [Tooltip("O 'Transform' da seta (o próprio objeto).")]
    [SerializeField] private Transform arrowTransform;

    private void Awake()
    {
        // Tenta pegar a referência automaticamente se não for definida
        if (arrowTransform == null)
        {
            arrowTransform = transform;
        }

        // Começa desativado
        arrowTransform.gameObject.SetActive(false);
    }

    private void Update()
    {
        // Se não tiver as referências, não faz nada
        if (objectGrabbing == null || directionScript == null)
        {
            return;
        }

        // Verifica se o jogador está a carregar o arremesso
        if (objectGrabbing.IsCharging)
        {
            // Ativa a seta
            arrowTransform.gameObject.SetActive(true);

            // Garante que a seta não tenha uma direção nula
            if (directionScript.directionVector != Vector3.zero)
            {
                // Faz a seta "olhar" (apontar o seu eixo Z) para a direção da mira
                arrowTransform.rotation = Quaternion.LookRotation(directionScript.directionVector);
            }
        }
        else
        {
            // Desativa a seta se não estiver a carregar
            arrowTransform.gameObject.SetActive(false);
        }
    }
}