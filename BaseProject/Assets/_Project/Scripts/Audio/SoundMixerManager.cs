using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    // O parâmetro 'level' aqui vem direto do Slider (valor entre -80 e 0)
    public void SetMasterVolume(float level)
    {
        // Passa o valor de decibéis diretamente para o mixer
        audioMixer.SetFloat("masterVolume", level);
    }

    public void SetSoundFXVolume(float level)
    {
        // Passa o valor de decibéis diretamente para o mixer
        audioMixer.SetFloat("soundFXVolume", level);
    }

    public void SetMusicVolume(float level)
    {
        // Passa o valor de decibéis diretamente para o mixer
        audioMixer.SetFloat("musicVolume", level);
    }
}