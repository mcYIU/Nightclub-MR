using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;
using System.Text;

/// <summary>
/// DialogueManager handles the display, progression, and audio of dialogues and monologues in the game
/// It manages dialogue state, UI, audio playback, and transitions between dialogue and interaction/gameplay
/// </summary>

public class DialogueManager : MonoBehaviour
{
    [Serializable]
    public class DialogueState
    {
        public DialogueData dialogue;
        public Queue<string> queue = new Queue<string>();
        public Canvas canvas;
        public TextMeshProUGUI text;
        public float displayTime;
        public const float defaultSentenceTime = 5.0f;
    }

    private static DialogueManager instance;
    public static DialogueManager Instance => instance;
    public static bool isTalking = false;

    [Header("Components")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Canvas monologueCanvas;
    [SerializeField] private float monologueTransitionInterval;
    [HideInInspector] public DialogueState state;

    private GameObject currentMonologueCharacter;
    private DialogueTrigger dialogueTrigger;
    private MonologueTrigger monologueTrigger;

    private void Awake()
    {
        InitializeSingleton();
    }

    private void InitializeSingleton()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void OverrideSetAudio(AudioClip clip)
    {
        // Static method to set the audio clip for dialogue
        Instance.SetAudio(clip);
    }

    public static void StartDialogue(DialogueData dialogue, Canvas canvas, DialogueTrigger trigger = null)
    {
        // Static method to start a dialogue sequence
        Instance.InitializeDialogue(dialogue, canvas, trigger);
    }

    private void InitializeDialogue(DialogueData dialogue, Canvas canvas, DialogueTrigger trigger = null)
    {
        // Set up dialogue state and start either voice over or character speech
        isTalking = true;
        dialogueTrigger = trigger;
        state.dialogue = dialogue;
        state.canvas = canvas;
        if (CanVoiceOverPlay())
        {
            StartVoiceOver();
        }
        else
        {
            StartCharacterSpeech();
        }
    }

    private bool CanVoiceOverPlay()
    {
        // Check if a voice over can be played for the current dialogue
        return state.dialogue.voiceOverText.Length > 0
            && state.dialogue.voiceOverAudio != null
            && !state.dialogue.isVoiceOverPlayed;
    }

    private void StartVoiceOver()
    {
        // Start the voice over sequence for the dialogue
        LoadDialogueState(state.dialogue.voiceOverText, state.dialogue.voiceOverAudio);
    }

    private void StartCharacterSpeech()
    {
        // Start the character speech sequence for the dialogue
        LoadDialogueState(state.dialogue.characterText, state.dialogue.characterAudio);
        state.dialogue.isVoiceOverPlayed = true;
    }

    public static void StartMonologue(MonologueContent monologue, MonologueTrigger trigger)
    {
        // Static method to start a monologue sequence
        Instance.InitializeMonologue(monologue.sentences, monologue.audioClip, monologue.character, trigger);
    }

    private void InitializeMonologue(string[] sentences, AudioClip clip, GameObject character, MonologueTrigger trigger)
    {
        // Set up monologue state, canvas, and character, then start the monologue
        if (!monologueTrigger) monologueTrigger = trigger;
        if (monologueCanvas) state.canvas = monologueCanvas;
        else
        {
            var cameraCanvas = GameObject.FindGameObjectWithTag("CameraCanvas");
            state.canvas = (cameraCanvas.TryGetComponent(out Canvas canvas))? canvas : null;
        }
        LoadDialogueState(sentences, clip);
        SetupCharacter(character);
    }

    private void SetupCharacter(GameObject character)
    {
        // Activate or deactivate the character GameObject for the monologue
        if (!character.activeSelf)
        {
            character.SetActive(true);
            currentMonologueCharacter = character;
        }
        else
        {
            character.SetActive(false);
        }
    }

    private void LoadDialogueState(string[] sentences, AudioClip clip)
    {
        // Set up the dialogue state with sentences and audio, then enqueue sentences
        state.displayTime = clip.length / sentences.Length;
        EnqueueSentences(sentences);
        SetAudio(clip);
    }

    private void SetAudio(AudioClip audio = null)
    {
        // Play or stop the audio for the dialogue/monologue
        if (audio != null)
        {
            audioSource.clip = audio;
            audioSource.Play();
        }
        else
        {
            audioSource.Stop();
        }
    }

    private void EnqueueSentences(string[] sentences)
    {
        // Queue up all sentences for the dialogue/monologue
        if (state.queue.Count > 0) state.queue.Clear();
        foreach (string sentence in sentences)
        {
            state.queue.Enqueue(sentence);
        }
        NextSentence();
    }

    private void NextSentence()
    {
        // Display the next sentence or handle the end of the dialogue
        if (state.queue.Count == 0)
        {
            HandleDialogueEnd();
        }
        else
        {
            DisplayNextSentence();
        }
    }

    private void DisplayNextSentence()
    {
        // Show the next sentence in the UI
        string sentence = state.queue.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    private IEnumerator TypeSentence(string sentence)
    {
        // Animate the display of the sentence, then wait and show the next
        yield return TypeDialogue(sentence);
        yield return new WaitForSeconds(state.displayTime);
        NextSentence();
    }

    private IEnumerator TypeDialogue(string text)
    {
        // Display the dialogue text in the UI, formatting as needed
        state.text = state.canvas.GetComponentInChildren<TextMeshProUGUI>();
        state.canvas.enabled = true;
        StringBuilder builder = new StringBuilder();
        foreach (char c in text)
        {
            // Move to next row if the character "." is met
            if (c == '.') builder.Append('\n');
            else builder.Append(c);
        }
        state.text.text = builder.ToString();
        yield return null;
    }

    private void HandleDialogueEnd()
    {
        // Handle what happens when the dialogue/monologue ends
        EndDialogue();
        // Handle characters' monologues in the 2nd scene
        if (monologueTrigger != null)
        {
            StartCoroutine(HandleMonologueTransition());
            return;
        }
        // Handle character speech after voiceover is played
        if (CanVoiceOverPlay())
        {
            state.dialogue.isVoiceOverPlayed = true;
            InitializeDialogue(state.dialogue, state.canvas, dialogueTrigger);
        }
        // Enable interaction when dialogue is completed 
        else
        {
            EnableInteraction();
        }
    }

    public static void EndDialogue()
    {
        // Static method to end the current dialogue and clean up
        Instance.CleanDialogue();
        Instance.SetAudio(null);
    }

    private void CleanDialogue()
    {
        // Stop all dialogue coroutines and clear the UI
        StopAllCoroutines();
        CleanRegularDialogue();
    }

    private void CleanRegularDialogue()
    {
        // Clear the dialogue queue and hide the dialogue UI
        state.queue.Clear();
        if (state.canvas)
        {
            state.canvas.enabled = false;
            state.text.text = string.Empty;
        }
    }

    private void EnableInteraction()
    {
        // Enable interactions after dialogue ends
        if (dialogueTrigger != null)
        {
            dialogueTrigger.EnableInteraction();
        }
        else
        {
            isTalking = false;
        }
    }

    private IEnumerator HandleMonologueTransition()
    {
        // Fade out, transition to the next monologue, then fade in
        FaderController.FadeOut();
        yield return new WaitForSeconds(monologueTransitionInterval);
        SetupCharacter(currentMonologueCharacter);
        monologueTrigger.MonologueIndex++;
        FaderController.FadeIn();
    }
}