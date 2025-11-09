using System.Collections;
using UnityEngine;

public class AmbientSoundPlayer : MonoBehaviour
{
    [Header("Configuração de Áudio")]
    [Tooltip("Array de clipes de áudio que podem ser tocados aleatoriamente.")]
    [SerializeField] private AudioClip[] audioClips;

    [Tooltip("Volume do som a ser tocado.")]
    [Range(0f, 1f)]
    [SerializeField] private float volume = 0.7f;

    [Header("Configuração de Tempo (Random)")]
    [Tooltip("Tempo MÍNIMO em segundos antes de tocar o próximo som.")]
    [SerializeField] private float minWaitTime = 5.0f;

    [Tooltip("Tempo MÁXIMO em segundos antes de tocar o próximo som.")]
    [SerializeField] private float maxWaitTime = 10.0f;

    [Tooltip("Se marcado, começa a tocar automaticamente quando o jogo inicia.")]
    [SerializeField] private bool playOnStart = true;

    private void Start()
    {
        // Inicia a rotina de áudio assim que o jogo começa, se playOnStart for verdadeiro
        if (playOnStart)
        {
            StartCoroutine(PlayAmbientSoundsRoutine());
        }
    }

    private IEnumerator PlayAmbientSoundsRoutine()
    {
        // Loop infinito, enquanto o objeto estiver ativo
        while (true)
        {
            // 1. Calcula um tempo de espera aleatório dentro do intervalo definido
            float waitTime = Random.Range(minWaitTime, maxWaitTime);

            // 2. Espera por esse tempo
            yield return new WaitForSeconds(waitTime);

            // 3. Verifica se temos áudios e se o SoundFXManager existe
            if (audioClips == null || audioClips.Length == 0)
            {
                Debug.LogWarning("AmbientSoundPlayer: Nenhum AudioClip foi assignado no Inspector.", this.gameObject);
                continue; // Pula esta iteração do loop, mas continua tentando
            }

            if (SoundFXManager.instance == null)
            {
                Debug.LogWarning("AmbientSoundPlayer: SoundFXManager.instance não encontrado!", this.gameObject);
                yield break; // Para a corrotina se o manager não existir
            }

            // 4. Chama a função do seu SoundFXManager para tocar um som aleatório do array
            // O som será tocado na posição deste GameObject (usando 'transform')
            SoundFXManager.instance.PlayRandomSoundFXClip(audioClips, transform, volume);
        }
    }
}