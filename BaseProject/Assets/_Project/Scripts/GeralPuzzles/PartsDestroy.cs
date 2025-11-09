using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class PartsDestroy : MonoBehaviour
{
    [SerializeField] private float minDelayBeforeDestroy = 2f;
    [SerializeField] private float maxDelayBeforeDestroy = 5f;

    [SerializeField] private List<GameObject> partsToDestroy;
    void Start()
    {
        foreach (var part in partsToDestroy)
        {
            float delay = Random.Range(minDelayBeforeDestroy, maxDelayBeforeDestroy);
            Destroy(part, delay);
        }
        Destroy(gameObject, maxDelayBeforeDestroy + 1f);
    }
}