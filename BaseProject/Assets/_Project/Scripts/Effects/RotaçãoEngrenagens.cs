using UnityEngine;

public class RotaçãoEngrenagens : MonoBehaviour
{
    [Header("Velocidade de rotação")]
    public float speed = 50f;

    [Header("Eixo de rotação")]
    public Vector3 axis = Vector3.forward;

    void Update()
    {
        transform.Rotate(axis * speed * Time.deltaTime);
    }
}
