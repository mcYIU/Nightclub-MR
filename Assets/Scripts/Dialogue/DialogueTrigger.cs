using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// DialogueTrigger manages the activation and flow of dialogues when the player interacts with certain objects or areas.
/// It handles UI display, dialogue progression, and interaction enabling/disabling.
/// </summary>

public class DialogueTrigger : MonoBehaviour
{
    [Serializable]
    public struct DialogueUIElements
    {
        public Image noticeUI;
        public Canvas dialogueCanvas;
    }

    [Header("Dialogue")]
    public DialogueData[] data;
    [Header("Dialogue UI")]
    public DialogueUIElements UIElements;
    [Header("Interaction Manager")]
    [SerializeField] InteractionManager interactionManager;

    [HideInInspector] private bool isDialogueTriggered = false;

    private void Start()
    {
        // Initialize dialogue UI and reset dialogue data at the start
        InitializeDialogue();
    }

    private void InitializeDialogue()
    {
        // Hide the dialogue canvas and reset all dialogue data
        UIElements.dialogueCanvas.enabled = false;
        ResetDialogueData();
    }

    private void OnTriggerStay(Collider other)
    {
        // When the player stays in the trigger, check if dialogue should start
        if (!IsValidTrigger(other)) return;
        HandleTriggerEnter();
    }

    private void OnTriggerExit(Collider other)
    {
        // When the player exits the trigger, check if dialogue should end
        if (!IsValidTrigger(other)) return;
        HandleTriggerExit();
    }

    private bool IsValidTrigger(Collider other)
    {
        // Check if the collider is the player and if dialogue can be triggered
        return other.CompareTag("Player") &&
               GameManager.IsStarted &&
               !GameManager.IsCompleted &&
               !DialogueManager.isTalking;
    }

    private void HandleTriggerEnter()
    {
        // Start dialogue if it hasn't been triggered yet
        if (!isDialogueTriggered)
        {
            StartDialogue(interactionManager.LevelIndex);
            isDialogueTriggered = true;
        }
    }

    private void HandleTriggerExit()
    {
        // End dialogue and clean up if player leaves the interaction area
        if (!IsWithinInteractionLimit() && isDialogueTriggered)
        {
            EndDialogue();
            interactionManager.CleanInteraction();
            DialogueManager.isTalking = false;
            isDialogueTriggered = false;
        }
    }

    private bool IsWithinInteractionLimit()
    {
        // Check if the current interaction level is within the allowed range
        if (interactionManager == null) return false;
        return interactionManager.LevelIndex < interactionManager.interactionLayers.Length;
    }

    public void StartDialogue(int index = 0)
    {
        // Start the dialogue for the given index if valid
        if (!IsValidDialogueIndex(index)) return;
        InitiateDialogue(index);
        SetupDialogueUI(false);
    }

    public void EnableInteraction()
    {
        // Enable interactions after dialogue ends
        if (interactionManager != null && IsWithinInteractionLimit())
        {
            interactionManager.EnableInteraction();
        }
        else
        {
            DialogueManager.isTalking = false;
        }
    }

    private void ResetDialogueData()
    {
       // Reset the state of all dialogue data objects
       foreach(var _data in data)
        {
            _data.Reset();
        }
    }

    private bool IsValidDialogueIndex(int index)
    {
        // Check if the dialogue index is within the valid range
        return index >= 0 && index < data.Length;
    }

    private void SetupDialogueUI(bool isNoticeVisible)
    {
        // Show or hide the notice UI element
        if (UIElements.noticeUI != null)
        {
            UIElements.noticeUI.enabled = isNoticeVisible;
        }
    }

    private void InitiateDialogue(int index)
    {
        // Start the dialogue using the DialogueManager
        DialogueManager.StartDialogue(data[index], UIElements.dialogueCanvas, this);
    }

    public void EndDialogue()
    {
        // End the current dialogue and hide the UI
        SetupDialogueUI(false);
        DialogueManager.EndDialogue();
    }
}