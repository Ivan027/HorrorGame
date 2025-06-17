using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(AudioSource))]
public class DialogueDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _dialogueText;
    [SerializeField] private GameObject _dialoguePanel;
    [SerializeField] private TextButton[] _answerButtons = new TextButton[5];
    [SerializeField] private float _clickCooldown = 0.3f;
    
    [Header("Typewriter Effect Settings")]
    [SerializeField] private float _delayBtwnChars = 0.05f;
    
    private DialogueController _dialogueController;
    private PlayerMovement _playerMovement;
    private AudioSource _audioSource;

    private Coroutine _typewriterCoroutine;
    private bool _canClick = true;
    
    [Inject]
    public void Construct(DialogueController dialogueController, PlayerMovement playerMovement)
    {
        _dialogueController = dialogueController;
        _playerMovement = playerMovement;
    }

    #region Unity Lifecycle Methods

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        HandleDialogueEnd();
    }

    private void OnEnable()
    {
        _dialogueController.OnDialogueStarted += HandleDialogueStart;
        _dialogueController.OnDialogueEnded += HandleDialogueEnd;
        _dialogueController.OnPhraseUpdated += DisplayPhrase;
        _dialogueController.OnAnswerPresented += DisplayAnswers;
    }

    private void OnDisable()
    {
        _dialogueController.OnDialogueStarted -= HandleDialogueStart;
        _dialogueController.OnDialogueEnded -= HandleDialogueEnd;
        _dialogueController.OnPhraseUpdated -= DisplayPhrase;
        _dialogueController.OnAnswerPresented -= DisplayAnswers;
    }

    private void Update()
    {
        if (!_dialogueController.IsDialogueActive) return;

        if (_canClick && Input.GetMouseButtonDown(0))
        {
            _canClick = false;
            _dialogueController.ContinueDialogue();
            StartCoroutine(ResetClickCooldown());
        }
    }

    #endregion

    #region Handles

    private void HandleDialogueStart(Dialogue dialogue)
    {
        Debug.Log("Диалог начался");
        _playerMovement.FreezePlayer();
        ShowCursor();
        _dialoguePanel.SetActive(true);
    }

    private void HandleDialogueEnd()
    {
        Debug.Log("Диалог завершён");
        HideCursor();
        _playerMovement.UnfreezePlayer();

        foreach (TextButton button in _answerButtons)
        {
            button.SetText("Answer");
            button.gameObject.SetActive(false);
        }

        _dialoguePanel.SetActive(false);
    }

    #endregion

    #region Display Methods

    private void DisplayPhrase(string text, Speaker speaker, AudioClip voice)
    {
        if (_typewriterCoroutine != null) StopCoroutine(_typewriterCoroutine);
        
        _typewriterCoroutine = StartCoroutine(PlayTypewriter(text, speaker, voice));
    }

    private void DisplayAnswers(List<Answer> answers)
    {
        HideAnswerButtons();

        if (answers.Count > _answerButtons.Length)
            Debug.LogWarning("Недостаточно кнопок для отображения всех вариантов ответа.");

        int count = Mathf.Min(_answerButtons.Length, answers.Count);

        for (int i = 0; i < count; i++)
        {
            TextButton button = _answerButtons[i];
            Answer answer = answers[i];

            button.gameObject.SetActive(true);
            button.SetText(answer.AnswerText);
            button.ClearOnClickListeners();
            button.AddOnClickListener(() =>
            {
                _dialogueController.ContinueDialogue(answers.IndexOf(answer));
                HideAnswerButtons();
            });
        }
    }

    #endregion

    #region Coroutines

    private IEnumerator PlayTypewriter(string text, Speaker speaker, AudioClip voice)
    {
        _audioSource.clip = voice;
        _audioSource.Play();
        
        string speakerName = $"<b><color=#{speaker.HexNameColor}>{speaker.SpeakerName}</color></b>: ";

        for (int i = 0; i < text.Length; i++)
        {
            _dialogueText.text = speakerName + text.Substring(0, i + 1);
            yield return new WaitForSeconds(_delayBtwnChars);
        }

        _typewriterCoroutine = null;
    }

    private IEnumerator ResetClickCooldown()
    {
        yield return new WaitForSeconds(_clickCooldown);
        _canClick = true;
    }

    #endregion

    #region Show/Hide Methods

    public void HideAnswerButtons()
    {
        foreach (TextButton button in _answerButtons)
        {
            button.ClearOnClickListeners();
            button.gameObject.SetActive(false);
        }
    }

    public void ShowCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void HideCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    #endregion
}