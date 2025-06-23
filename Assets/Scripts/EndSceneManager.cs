using System;
using UnityEngine;

/// <summary>
/// EndSceneManager manages a npc's dialogue at the start of the last (3rd) scene.
/// </summary>

public class EndSceneManager : MonoBehaviour
{
    [Serializable]
    private struct NPC
    {
        public GameObject gameObject;
        public DialogueTrigger dialogueTrigger;
    }

    [SerializeField] private NPC npc;
    [SerializeField] private float dialogueDelay;

    void Start()
    {
        // Schedules the NPC dialogue to begin after a delay
        Invoke(nameof(StartNPCDialogue), dialogueDelay);
    }

    private void StartNPCDialogue()
    {
        npc.gameObject.TryGetComponent<DialogueTrigger>(out DialogueTrigger _NPC_Trigger);
        _NPC_Trigger.StartDialogue();
    }


}
