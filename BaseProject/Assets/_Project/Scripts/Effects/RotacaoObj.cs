using UnityEngine;

public class RotacaoObj : MonoBehaviour
{
    [Header("Velocidade de rotação")]
    public float speed = 50f;

    [Header("Eixo de rotação")]
    public Vector3 axis = Vector3.up;

    void Update()
    {
        transform.Rotate(axis * speed * Time.deltaTime);
    }
}
