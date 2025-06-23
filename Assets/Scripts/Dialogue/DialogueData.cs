using UnityEngine;

/// <summary>
/// DialogueData is a ScriptableObject that stores the text and audio for both voice over and character speech in a dialogue sequence.
/// It is used by the DialogueManager and DialogueTrigger to manage dialogue content and state.
/// </summary>

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue System/Dialogue Data")]
public class DialogueData : ScriptableObject
{
    [Header("Voice Over")]
    public string[] voiceOverText;
    public AudioClip voiceOverAudio;

    [Header("Character Speech")]
    public string[] characterText;
    public AudioClip characterAudio;

    [HideInInspector] public bool isVoiceOverPlayed;

    public void Reset()
    {
        isVoiceOverPlayed = false;
    }
}
