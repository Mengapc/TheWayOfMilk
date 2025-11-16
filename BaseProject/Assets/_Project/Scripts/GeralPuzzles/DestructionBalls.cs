using Unity.VisualScripting;
using UnityEngine;

public class DestructionBalls : MonoBehaviour
{
    [Header("Áudio")]
    [Tooltip("Som que toca ao destruir o galão.")]
    [SerializeField] private AudioClip explosionSound;
    [SerializeField] private float soundVolume = 0.5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            if (explosionSound != null && SoundFXManager.instance != null)
            {
                // 'transform' fará o som sair do jogador
                SoundFXManager.instance.PlaySoundFXClip(explosionSound, transform, soundVolume);
            }
            other.GetComponent<BallController>().InstantiateEfect();
            Destroy(other.gameObject);
        }
    }
}