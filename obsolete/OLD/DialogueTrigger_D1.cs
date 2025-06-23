using UnityEngine;

public class DialogueTrigger_D1 : MonoBehaviour
{
    public Dialogue dialogue;
    public DialogueManager_D1 dialogueManager_d1;

    private void OnTriggerEnter(Collider other)
    {
        dialogueManager_d1.StartDialogue(dialogue);
    }
}
