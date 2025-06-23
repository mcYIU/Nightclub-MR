using UnityEngine;

/// <summary>
/// CustomerDialogueTrigger is used to trigger customer-specific dialogue after completing a quest
/// It interacts with the DialogueManager to start the appropriate dialogue sequence
/// </summary>

public class CustomerDialogueTrigger : MonoBehaviour
{
    [Header("Dialogue Components")]
    [SerializeField] private DialogueData data;
    [SerializeField] private Canvas dialogueCanvas;

    public void Talk()
    {
        DialogueManager.StartDialogue(data, dialogueCanvas);
    }
}
