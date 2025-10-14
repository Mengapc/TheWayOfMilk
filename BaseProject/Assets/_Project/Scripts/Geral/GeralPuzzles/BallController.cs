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
    [SerializeField] private Rigidbody rb;
    [SerializeField] public RoomIdentifier roomIdentifier;
    [SerializeField] public GameObject ballPrefab;
    [SerializeField] public bool resetavel = true;
    [SerializeField] public int weight = 1;
}
