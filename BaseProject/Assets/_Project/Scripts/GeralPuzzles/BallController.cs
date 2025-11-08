using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class BallController : MonoBehaviour
{
    [SerializeField] private GameObject LeiteQuebrado;
    [SerializeField] private Rigidbody Rigidbody;
    [SerializeField] private float gravidade = -3f;

    public void InstantiateEfect()
    {
        Instantiate(LeiteQuebrado, transform.position, transform.rotation);
    }

    private void Update()
    {
        if (Rigidbody.isKinematic != true)
        {
            Rigidbody.AddForce(new Vector3(0, gravidade, 0), ForceMode.Acceleration);
        }
    }
}