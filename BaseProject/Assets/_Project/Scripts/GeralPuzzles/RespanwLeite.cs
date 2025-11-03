using Unity.VisualScripting;
using UnityEngine;

public class RespanwLeite : MonoBehaviour
{
    [Header("Componetes do Respanw")]
    [SerializeField] private Transform pontoRespanw;
    [SerializeField] private GameObject leitePrefab;
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
        leiteAtivo = Instantiate(leitePrefab, pontoRespanw.position, pontoRespanw.rotation);
    }
}
