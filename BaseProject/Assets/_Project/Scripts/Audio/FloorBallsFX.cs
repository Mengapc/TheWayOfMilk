using Unity.VisualScripting;
using UnityEngine;

public class FloorBallsFX : MonoBehaviour
{
    [Header("Áudio")]
    [Tooltip("Som que toca ao destruir o galão.")]
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private float soundVolume = 1f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            if (hitSound != null && SoundFXManager.instance != null)
            {
                // 'transform' fará o som sair do jogador
                SoundFXManager.instance.PlaySoundFXClip(hitSound, transform, soundVolume);
            }
            other.GetComponent<BallController>().InstantiateEfect();
            Destroy(other.gameObject);
        }
    }
}