using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class BallController : MonoBehaviour
{
    [SerializeField] private GameObject LeiteQuebrado;
    [SerializeField] private Rigidbody Rigidbody;
    [SerializeField] private float gravidade = -3f;

    [Space]
    [Header("Áudio")]
    [Tooltip("Som que toca ao destruir o galão.")]
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private float soundVolume = 1f;

    [Tooltip("Garante que o som de impacto toque apenas uma vez.")]
    private bool hasPlayedHitSound = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Chao") && !hasPlayedHitSound)
        {
            if (hitSound != null && SoundFXManager.instance != null)
            {
                // Marca imediatamente que o som vai tocar
                hasPlayedHitSound = true;

                // 'transform' fará o som sair do objeto
                SoundFXManager.instance.PlaySoundFXClip(hitSound, transform, soundVolume);
            }
        }
    }

    public void ResetHitSound()
    {
        hasPlayedHitSound = false;
    }

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