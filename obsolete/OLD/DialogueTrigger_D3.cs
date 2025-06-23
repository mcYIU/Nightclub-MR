using Oculus.Interaction;
using UnityEngine;

public class DialogueTrigger_D3 : MonoBehaviour
{
    public Dialogue dialogue;
    public GameObject dialogueNoticeUI;
    public GameObject dialogueCanvas;

    public GameObject[] interactables;

    DialogueManager_D3 dialogueManager_d3;
    DialogueManager_D4 dialogueManager_d4;

    bool isTrigger = false;

    private void Start()
    {
        dialogueManager_d3 = FindAnyObjectByType<DialogueManager_D3>();
        dialogueManager_d4 = FindAnyObjectByType<DialogueManager_D4>(); 

        dialogueCanvas.SetActive(false);

        for (int i = 0; i < interactables.Length; i++)
        {
            if (interactables[i] != null)
                interactables[i].SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isTrigger)
        {
            if(interactables.Length > 0)
            {
                for (int i = 0; i < interactables.Length; i++)
                {
                    if (other.gameObject.GetComponentInChildren<Grabbable>() == null)
                    {
                        StartDialogue();

                        interactables[i].SetActive(true);
                    }
                }
            }
            else
            {
                StartDialogue();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        EndDialogue();
        isTrigger = false;
        dialogueNoticeUI.SetActive(true);
    }

    private void StartDialogue()
    {
        dialogueNoticeUI.SetActive(false);
        //dialogueManager_d3.StartDialogue(dialogue);
        dialogueManager_d4.StartDialogue(dialogue, dialogueCanvas);
        isTrigger = true;
    }
    
    private void EndDialogue()
    {
        dialogueNoticeUI.SetActive(true);
        //dialogueManager_d3.EndDialogue();
        dialogueManager_d4.EndDialogue(); 
    }
}
