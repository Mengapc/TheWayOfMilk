using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.InputSystem;

public class ScaleManager : MonoBehaviour
{
    [Header("Refer�ncias")]
    [SerializeField] public Transform armScale;
    [SerializeField] private MovementScale movementController;
    [SerializeField] private float finalizedTime = 5f;
    public bool isFinalized = false;
    [SerializeField] private DoorController door;

    [Header("Configura��o de Inclina��o")]
    [Tooltip("Transform das placas da balan�a.")]
    [SerializeField] private Transform leftPlate;
    [Tooltip("Transform do ponto de ancoragem da placa esquerda.")]
    [SerializeField] private Transform rightPlate;
    [Tooltip("Transform do ponto de ancoragem da placa direita.")]
    [SerializeField] private Transform pointLeftPlate;
    [Tooltip("Transform do ponto de ancoragem da placa direita.")]
    [SerializeField] private Transform pointRightPlate;
    [Tooltip("�ngulo m�ximo que a balan�a pode inclinar com base no peso.")]
    [Range(0f, 45f)]
    [SerializeField] private float maxAngleFromWeight = 45f;
    [Tooltip("Velocidade de ajuste da rota��o da balan�a.")]
    [SerializeField] private float ajustSpeed = 3f;

    [Header("Configura��o de Peso")]
    [Tooltip("Peso minimo para finalizar")]
    [SerializeField] private float minWisghtFinilize = 3;
    [Tooltip("Script da placa esquerda para acessar o peso.")]
    [SerializeField] private PlateScale leftPlateScript;
    [Tooltip("Script da placa direita para acessar o peso.")]
    [SerializeField] private PlateScale rightPlateScript;
    [Tooltip("Peso total na placa esquerda.")]
    [SerializeField] private float leftWeight = 0f;
    [Tooltip("Peso total na placa direita.")]
    [SerializeField] private float rightWeight = 0f;
    [Tooltip("Script de gera��o de bolas.")] 
    [SerializeField] private SpanwBalls spanwBalls;
    [Tooltip("Lista de objetos atualmente na balan�a (para debug).")]
    [SerializeField] public List<GameObject> objectsOnPlate;

    private void Update()
    {
        if (!isFinalized)
        {
            UpdateScaleState();
            AjustPositionPlate();
            AjustWeight();
            CombineRotationsAndAjustPlate();
        }
        else
        {
            // Se a balan�a estiver finalizada, garantir que fique nivelada.
            leftWeight = rightWeight;
        }
    }

    private IEnumerator FinalizePuzzleWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        OpenDoor();
        isFinalized = true;
        Debug.Log("A balan�a est� equilibrada!");

    }

    private void UpdateScaleState()
    {
        if (leftWeight == rightWeight && leftWeight > minWisghtFinilize && rightWeight > minWisghtFinilize)
        {
            StartCoroutine(FinalizePuzzleWithDelay(finalizedTime));
        }
        else
        {
            StopAllCoroutines();
        }

    }

    public void ResetPuzzle(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            // *** CORRE��O APLICADA AQUI ***
            // Verificamos se as refer�ncias existem antes de us�-las.
            // Isso evita o erro 'NullReferenceException'.

            if (leftPlateScript != null)
            {
                leftPlateScript.weightBall = 0;
            }
            if (rightPlateScript != null)
            {
                rightPlateScript.weightBall = 0;
            }

            leftWeight = 0f;
            rightWeight = 0f;

            // Esta � a linha que estava causando o erro.
            // A verifica��o impede que o erro ocorra e avisa sobre o problema.
            if (spanwBalls != null)
            {
                spanwBalls.spawnedBalls = 0;
            }
            else
            {
                Debug.LogError("A refer�ncia para 'SpanwBalls' n�o foi atribu�da no Inspector do ScaleManager!", this.gameObject);
            }

            foreach (GameObject ball in objectsOnPlate)
            {
                // � uma boa pr�tica verificar se o objeto n�o � nulo antes de destru�-lo.
                if (ball != null)
                {
                    Destroy(ball);
                }
            }
            objectsOnPlate.Clear();
        }
    }

    private void AjustPositionPlate()
    {
        leftPlate.transform.position = pointLeftPlate.position;
        rightPlate.transform.position = pointRightPlate.position;
    }

    private void AjustWeight()
    {
        leftWeight = leftPlateScript.weightBall;
        rightWeight = rightPlateScript.weightBall;
    }

    private void CombineRotationsAndAjustPlate()
    {
        float targetAngleFromWeight = 0f;
        if (leftWeight < rightWeight)
        {
            targetAngleFromWeight = maxAngleFromWeight;
        }
        else if (rightWeight < leftWeight)
        {
            targetAngleFromWeight = -maxAngleFromWeight;
        }

        float playerInfluenceAngle = 0f;
        if (movementController != null && movementController.enabled)
        {
            playerInfluenceAngle = movementController.PlayerRotationInfluence;
        }

        Quaternion finalTargetRotation = Quaternion.Euler(0, playerInfluenceAngle, targetAngleFromWeight);
        armScale.localRotation = Quaternion.Slerp(armScale.localRotation, finalTargetRotation, Time.deltaTime * ajustSpeed);
    }

    private void OpenDoor()
    {
        door.OpenDoor();
    }
}

