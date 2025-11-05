using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager instance;

    [SerializeField] private AudioSource soundFXObject;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null)
        {
            instance = this;
        }
    }

    public void PlaySoundFXClip(AudioClip audioClip, Transform spawntransform, float volume)
    {
        // Spawn GameObject
        AudioSource audioSource = Instantiate(soundFXObject, spawntransform.position, Quaternion.identity);

        // Assign AudioClip
        audioSource.clip = audioClip;

        // Assing Volume
        audioSource.volume = volume;

        // Play Sound
        audioSource.Play();

        // Get lenght of soundFX clip
        float clipLength = audioSource.clip.length;

        // Destroy the clip
        Destroy(audioSource.gameObject, clipLength);

    }

    public void PlayRandomSoundFXClip(AudioClip[] audioClip, Transform spawntransform, float volume)
    {
        // assign random index
        int rand = Random.Range(0, audioClip.Length);

        // Spawn GameObject
        AudioSource audioSource = Instantiate(soundFXObject, spawntransform.position, Quaternion.identity);

        // Assign AudioClip
        audioSource.clip = audioClip[rand];

        // Assing Volume
        audioSource.volume = volume;

        // Play Sound
        audioSource.Play();

        // Get lenght of soundFX clip
        float clipLength = audioSource.clip.length;

        // Destroy the clip
        Destroy(audioSource.gameObject, clipLength);
    }
}
