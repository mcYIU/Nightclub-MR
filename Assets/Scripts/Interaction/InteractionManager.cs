using System;
using TMPro;
using UnityEngine;

/// <summary>
/// InteractionManager controls the flow of player interactions in the game, managing layers of interactable objects and progression.
/// It handles enabling/disabling interactions, displaying notices, and triggering dialogue or game state changes.
/// </summary>

public class InteractionManager : MonoBehaviour
{
    public InteractionLayer[] interactionLayers;
    public NoticeSystem noticeSystem;
    [SerializeField, Header("Dialogue Trigger")] private DialogueTrigger dialogueTrigger;

    private int levelIndex = 0;

    [Serializable]
    public struct InteractionLayer
    {
        public Interactable[] interactables;
        public string noticeText;
    }

    [Serializable]
    public class NoticeSystem
    {
        public TextMeshProUGUI interactionNotice;
        [HideInInspector] public bool isNoticed;

        public void Initialize()
        {
            interactionNotice.text = string.Empty;
            isNoticed = false;
        }
    }

    public int LevelIndex
    {
        get => levelIndex;
        set
        {
            if (levelIndex == value) return;

            levelIndex = value;
            HandleLevelChange();
        }
    }

    private void Start()
    {
        // Initialize all interactions and the notice system at the start of the scene
        ResetInteraction();
        noticeSystem.Initialize();
    }

    public void ChangeLevelIndex(int index)
    {
        // Change the current interaction level and trigger level change handling
        LevelIndex = index;
    }

    private void HandleLevelChange()
    {
        // Called when the interaction level changes; cleans up previous interactions, triggers dialogue, and checks for game completion
        CleanInteraction();
        noticeSystem.isNoticed = false;
        dialogueTrigger.StartDialogue(LevelIndex);
        if (LevelIndex == interactionLayers.Length)
        {
            GameManager.CheckGameState();
        }
    }

    private void DisplayNotice()
    {
        // Show the notice UI and enable UI for interactables at the current level
        if (LevelIndex < interactionLayers.Length)
        {
            noticeSystem.interactionNotice.enabled = true;
            noticeSystem.interactionNotice.text = interactionLayers[LevelIndex].noticeText;
            noticeSystem.isNoticed = true;
            foreach (var interactable in interactionLayers[LevelIndex].interactables)
            {
                interactable.SetUI(true);
            }
        }
    }

    public void ResetInteraction()
    {
        // Disable all interactions and set their levels for all layers
        for (int i = 0; i < interactionLayers.Length; i++)
        {
            foreach (var interactable in interactionLayers[i].interactables)
            {
                interactable.SetInteraction(false);
                interactable.SetInteractionLevel(i);
            }
        }
    }

    public void EnableInteraction()
    {
        // Enable interactions and display notice for the current level
        if (LevelIndex < interactionLayers.Length)
        {
            DisplayNotice();
            EnableInteractables(LevelIndex);
        }
    }

    private void EnableInteractables(int levelIndex)
    {
        // Enable all interactables for the specified level
        foreach (var interactable in interactionLayers[levelIndex].interactables)
        {
            interactable.SetInteraction(true);
        }
    }

    public void CleanInteraction()
    {
        // Clean up the notice UI and hide all interactable UIs
        if (noticeSystem.isNoticed)
        {
            noticeSystem.Initialize();
            for (int i = 0; i < interactionLayers.Length; i++)
            {
                foreach (var interactable in interactionLayers[i].interactables)
                {
                    interactable.SetUI(false);
                }
            }
        } 
        StopAllCoroutines();
    }
}