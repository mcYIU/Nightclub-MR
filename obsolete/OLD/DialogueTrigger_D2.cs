using UnityEngine;

public class DialogueTrigger_D2 : MonoBehaviour
{
    public Dialogue dialogue;
    public DialogueManager_D2 dialogueManager_d2;

    private void OnTriggerEnter(Collider other)
    {
        dialogueManager_d2.StartDialogue(dialogue);
    }
}
