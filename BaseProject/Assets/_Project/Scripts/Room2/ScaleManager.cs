using UnityEngine;

public class ScaleManager : MonoBehaviour
{
    [Header("Configura��o de inclina��o da balan�a")]
    [SerializeField] private Transform armScale;
    [Tooltip("Transform da placa esquerda")]
    [SerializeField] private Transform leftPlate;
    [Tooltip("Transform da placa direita")]
    [SerializeField] private Transform rightPlate;
    [Tooltip("Transform do ponto de inclina��o da placa esquerda")]
    [SerializeField] private Transform pointLeftPlate;
    [Tooltip("Transform do ponto de inclina��o da placa direita")]
    [SerializeField] private Transform pointRightPlate;
    [Tooltip("�ngulo m�ximo de inclina��o")]
    [Range(0f, 8f)]
    [SerializeField] private float maxAngle = 6f;
    [Tooltip("Velocidade de inclina��o")]
    [SerializeField] private float ajustSpeed = 2f;
    [Space(5)]
    [Header("Configura��o de peso")]
    [Tooltip("Script da placa esquerda")]
    [SerializeField] private PlateScale leftPlateScript;
    [Tooltip("Script da placa direita")]
    [SerializeField] private PlateScale rightPlateScript;
    [Tooltip("Peso total na placa esquerda")]
    [SerializeField] private float leftWeight = 0f;
    [Tooltip("Peso total na placa direita")]
    [SerializeField] private float rightWeight = 0f;




    private void Start()
    {
        // Inicializa as placas na posi��o neutra
        leftPlate.localRotation = Quaternion.Euler(0, 0, 0);
        rightPlate.localRotation = Quaternion.Euler(0, 0, 0);
    }

    private void Update()
    {
        AjustPositionPlate();
        AjustPlate();
    }

    private void AjustPositionPlate()
    {
        leftPlate.transform.position = pointLeftPlate.position;
        rightPlate.transform.position = pointRightPlate.position;
    }
    private void AjustPlate()
    {
        if (leftWeight > rightWeight)
        {
            // Inclina a balan�a para a esquerda
            Quaternion targetRotation = Quaternion.Euler(0, 0, maxAngle);
            armScale.localRotation = Quaternion.Slerp(armScale.localRotation, targetRotation, Time.deltaTime * ajustSpeed);
        }
        else if (rightWeight > leftWeight)
        {
            // Inclina a balan�a para a direita
            Quaternion targetRotation = Quaternion.Euler(0, 0, -maxAngle);
            armScale.localRotation = Quaternion.Slerp(armScale.localRotation, targetRotation, Time.deltaTime * ajustSpeed);
        }
        else
        {
            // Retorna a balan�a para a posi��o neutra
            Quaternion targetRotation = Quaternion.Euler(0, 0, 0);
            armScale.localRotation = Quaternion.Slerp(armScale.localRotation, targetRotation, Time.deltaTime * ajustSpeed);
        }

    }
    



}
