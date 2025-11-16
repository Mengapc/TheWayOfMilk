using UnityEngine;
using System.Collections.Generic;
public class DestroyOnVoid : MonoBehaviour
{
    [Tooltip("Lista de tags que serão checadas")]
    public List<string> targetTags = new List<string>();

    [Tooltip("Altura mínima para destruir os objetos")]
    public float voidY = -40f;

    void Update()
    {
        foreach (string tag in targetTags)
        {
            // pega todos objetos com essa tag
            GameObject[] objs = GameObject.FindGameObjectsWithTag(tag);

            foreach (GameObject obj in objs)
            {
                if (obj.transform.position.y < voidY)
                {
                    Destroy(obj);
                }
            }
        }
    }
}
