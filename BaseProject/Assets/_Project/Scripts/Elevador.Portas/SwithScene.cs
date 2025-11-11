using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class SwithScene : MonoBehaviour
{
    [SerializeField] private string sceneName;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (sceneName != null)
            {
                SceneLoader.LoadScene(sceneName);
            }
            else
            {
                Debug.LogWarning("Nenhuma cena atribuída ao SwithScene.");
            }
        }
    }
    
}
