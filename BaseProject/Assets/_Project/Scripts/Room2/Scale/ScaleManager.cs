using UnityEngine;
using System.Collections.Generic;
using System.Collections;

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
    [Tooltip("Script da placa esquerda para acessar o peso.")]
    [SerializeField] private PlateScale leftPlateScript;
    [Tooltip("Script da placa direita para acessar o peso.")]
    [SerializeField] private PlateScale rightPlateScript;
    [Tooltip("Peso total na placa esquerda.")]
    [SerializeField] private float leftWeight = 0f;
    [Tooltip("Peso total na placa direita.")]
    [SerializeField] private float rightWeight = 0f;
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
        if (leftWeight == rightWeight && leftWeight > 0 && rightWeight > 0)
        {
            StartCoroutine(FinalizePuzzleWithDelay(finalizedTime));
        }
        else
        {
            StopAllCoroutines();
        }

    }

    private void AjustPositionPlate()
    {
        leftPlate.transform.position = pointLeftPlate.position;
        rightPlate.transform.position = pointRightPlate.position;
    }

    private void AjustWeight()
    {
        leftWeight = leftPlateScript.WeightBall;
        rightWeight = rightPlateScript.WeightBall;
    }

    private void CombineRotationsAndAjustPlate()
    {
        float targetAngleFromWeight = 0f;
        if (leftWeight > rightWeight)
        {
            targetAngleFromWeight = maxAngleFromWeight;
        }
        else if (rightWeight > leftWeight)
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

