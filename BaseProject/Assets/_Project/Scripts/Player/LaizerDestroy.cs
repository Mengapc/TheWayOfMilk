using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LaizerDestroy : MonoBehaviour
{
    [Header("Objetos a desligar na ordem")]
    public List<GameObject> objectsToDisable;

    [Header("Delay entre cada um")]
    public float delayBetween = 0.2f;

    // pode chamar por evento, trigger, animação, etc
    public void StartDestroySequence()
    {
        StartCoroutine(DestroySequence());
    }
    IEnumerator DestroySequence()
    {
        // desativa um por um
        for (int i = 0; i < objectsToDisable.Count; i++)
        {
            if (objectsToDisable[i] != null)
                objectsToDisable[i].SetActive(false);

            yield return new WaitForSeconds(delayBetween);
        }

        // no fim, destrói o objeto que tem esse script
        Destroy(gameObject);
    }
}
