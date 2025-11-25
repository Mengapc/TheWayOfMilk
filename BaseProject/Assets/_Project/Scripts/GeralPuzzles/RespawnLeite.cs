using Unity.VisualScripting;
using UnityEngine;

public class RespawnLeite : MonoBehaviour
{
    [Header("Componetes do Respanw")]
    [SerializeField] private Transform pontoRespanw;
    [SerializeField] private GameObject leitePrefab;
    [SerializeField] private GameObject DissolvePrefab;
    [SerializeField] private GameObject leiteAtivo;

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            leiteAtivo = collision.gameObject;
        }

    }

    private void Start()
    {
        Respanw();
    }

    public void LateUpdate()
    {
        if (leiteAtivo == null)
        {
            Respanw();
        }
    }

    public void Respanw()
    {
        Instantiate(DissolvePrefab, pontoRespanw.position, pontoRespanw.rotation);
        leiteAtivo = Instantiate(leitePrefab, pontoRespanw.position, pontoRespanw.rotation);
    }
}