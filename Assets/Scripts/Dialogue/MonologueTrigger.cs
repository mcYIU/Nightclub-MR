using System.Collections;
using TMPro;
using UnityEngine;

[System.Serializable]
public struct MonologueContent
{
    public string[] sentences;
    public AudioClip audioClip;
    public GameObject character;
}

/// <summary>
/// MonologueTrigger manages the triggering and progression of character monologues in the game.
/// It handles displaying monologue text, playing audio, and transitioning to the next scene after the monologue ends.
/// </summary>

public class MonologueTrigger : MonoBehaviour
{
    private int monologueIndex = 0;
    private const float startTime = 2.0f;

    [Header("Character Monologue")]
    public MonologueContent[] monologues;

    [Header("End Text")]
    [SerializeField] private TextMeshProUGUI TMP;
    [SerializeField] private float endDuration;

    public int MonologueIndex
    {
        get => monologueIndex;
        set
        {
            if (monologueIndex == value) return;

            monologueIndex = value;
            StartMonologue();
        }
    }

    private void Start()
    {
        // Hide the end text at the start and begin the first monologue after a delay
        if (TMP != null) TMP.enabled = false;
        Invoke(nameof(StartMonologue), startTime);
    }

    private void StartMonologue()
    {
        // Start the current monologue
        if (MonologueIndex == monologues.Length)
        {
            StartCoroutine(DisplayEndText());
        }
        // Display the end text if all monologues are finished
        else
        {
            DialogueManager.StartMonologue(monologues[monologueIndex], this);
        }
    }

    private IEnumerator DisplayEndText()
    {
        // Show the end text for a set duration, then change to the next scene
        TMP.enabled = true;
        yield return new WaitForSeconds(endDuration);
        GameManager.ChangeToNextScene();
    }
}
