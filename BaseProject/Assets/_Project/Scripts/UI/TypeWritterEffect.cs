using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

[RequireComponent(typeof(TMP_Text))]

public class TypeWritterEffect : MonoBehaviour
{
    private TMP_Text _textBox;

    private int _currentVisibleCharacterIndex;
    private Coroutine _typewriterCoroutine;
    private bool _readyForNewText = true;

    private WaitForSeconds _simpleDelay;
    private WaitForSeconds _interpunctuationDelay;
    private WaitForSeconds _textboxFullEventDelay;
    [SerializeField][Range(0.1f, 0.5f)] private float sendDoneDelay = 0.25f; 

    [Header("TypeWritter Settings")]
    [SerializeField] private float charactersPerSecond = 20f;
    [SerializeField] private float interpunctuationDelay = 0.5f;

    public static event Action CompleteTextRevealed;
    public static event Action<char> CharacterRevealed;

    private void Awake()
    {
        _textBox = GetComponent<TMP_Text>();
        _simpleDelay = new WaitForSeconds(1f / charactersPerSecond);
        _interpunctuationDelay = new WaitForSeconds(interpunctuationDelay);
        _textboxFullEventDelay = new WaitForSeconds(sendDoneDelay);
    }

    private void OnEnable()
    {
        TMPro_EventManager.TEXT_CHANGED_EVENT.Add(PrepareForNewText);
    }

    private void OnDisable()
    {
        TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(PrepareForNewText);
    }

    public void PrepareForNewText(Object obj)
    {
        if (obj != _textBox)
            return;

        if (!_readyForNewText)
            return;

        _readyForNewText = false;

        if (_typewriterCoroutine != null)
        {
            StopCoroutine(_typewriterCoroutine);
        }

        //_textBox.text = string.Empty;
        _textBox.maxVisibleCharacters = 0;
        _currentVisibleCharacterIndex = 0;

        _typewriterCoroutine = StartCoroutine(routine:Typewriter());
    }

    private IEnumerator Typewriter()
    {
        // Força o TMP a recalcular as informações do texto (essencial!)
        _textBox.ForceMeshUpdate();

        TMP_TextInfo textInfo = _textBox.textInfo;
        int totalCharacters = textInfo.characterCount;

        // O loop agora vai de 0 até (totalCharacters - 1)
        while (_currentVisibleCharacterIndex < totalCharacters)
        {
            // Pega o caractere atual
            char character = textInfo.characterInfo[_currentVisibleCharacterIndex].character;

            // Revela o caractere. 
            // (Usar "=" é mais seguro que "++" se a corotina for interrompida)
            _textBox.maxVisibleCharacters = _currentVisibleCharacterIndex + 1;

            // Dispara o evento PARA o caractere atual
            CharacterRevealed?.Invoke(character);

            // Checa a pausa (removi a vírgula duplicada)
            if (character == '?' || character == '.' || character == ',' || character == ':' ||
                character == ';' || character == '!' || character == '-')
            {
                yield return _interpunctuationDelay;
            }
            else
            {
                yield return _simpleDelay;
            }

            // Avança para o próximo caractere
            _currentVisibleCharacterIndex++;
        }

        // --- O LOOP TERMINOU ---
        // Todos os caracteres foram revelados.

        // Espera o delay final antes de "terminar"
        yield return _textboxFullEventDelay;

        // Dispara o evento de "texto completo"
        CompleteTextRevealed?.Invoke();

        // Libera para um novo texto
        _readyForNewText = true;
    }
}
