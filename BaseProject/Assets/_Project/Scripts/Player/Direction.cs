using UnityEngine;

public class Direction : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] public Vector3 directionVector;
    [SerializeField] private float distance;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Vector3 ofSet;

    private void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    private void Update()
    {
        Plane playerPlane = new Plane(Vector3.up, player.position + ofSet);
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (playerPlane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            directionVector = hitPoint - player.position;
            directionVector.y = 0;
            distance = directionVector.magnitude;
            directionVector.Normalize();
            Debug.DrawRay(player.position, directionVector * distance, Color.blue);
        }
    }
    private void OnDrawGizmosSelected()
    {
        // Garante que o player foi definido antes de tentar desenhar
        if (player == null)
        {
            return;
        }

        // Define a cor do gizmo (ex: ciano semi-transparente)
        Gizmos.color = new Color(0, 1, 1, 0.5f);

        // Pega a posição do player, que é o centro do plano
        Vector3 planeCenter = player.position;

        // Define o tamanho da representação visual do plano (ex: 20x20 unidades)
        // Usamos '0' no eixo Y porque o plano é infinitamente fino.
        Vector3 planeSize = new Vector3(20, 0, 20);

        // Desenha um "cubo" de arame no local e tamanho definidos
        Gizmos.DrawWireCube(planeCenter + ofSet, planeSize);
    }

}
