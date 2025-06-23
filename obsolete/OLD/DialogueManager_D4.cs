using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager_D4 : MonoBehaviour
{
    [Header("Reading Time")]
    //[SerializeField] private float typeSpeed;
    public bool isTyped = false;
    [SerializeField] private float dialogueTime;

    float dialogueTimer = 0f;

    private Queue<string> dialogueQueue;
    private GameObject dialogueCanvas;
    private TextMeshProUGUI dialogueText;

    bool isDialogueShown = false;

    private void Start()
    {
        dialogueQueue = new Queue<string>();
    }

    private void Update()
    {
        if (isDialogueShown)
        {
            dialogueTimer += Time.deltaTime;
            if (dialogueTimer >= dialogueTime)
            {
                NextSentence();
                dialogueTimer = 0f;
            }
        }
    }

    public void StartDialogue(Dialogue dialogue, GameObject canvas)
    {
        dialogueCanvas = canvas;

        dialogueQueue.Clear();
        foreach (string sentence in dialogue.sentences)
        {
            dialogueQueue.Enqueue(sentence);
        }

        NextSentence();
    }

    public void NextSentence()
    {
        isDialogueShown = false;
        if (dialogueQueue.Count == 0)
        {
            EndDialogue();
        }
        else
        {
            string sentence = dialogueQueue.Dequeue();
            StopAllCoroutines();
            StartCoroutine(Type(sentence));
        }
    }

    IEnumerator Type(string sentence)
    {
        if (dialogueCanvas != null)
        {
            dialogueText = dialogueCanvas.GetComponentInChildren<TextMeshProUGUI>();
            dialogueText.text = "";
            isTyped = true;

            dialogueCanvas.SetActive(true);
            dialogueText.text += sentence;
        }

        isDialogueShown = true;

        yield return new WaitForSeconds(0f);

        //isTyped = true;

        /*foreach (char c in sentence.ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(0.1f);
        }
        */
        //isTyped = false;
    }

    public void EndDialogue()
    {
        StopAllCoroutines();
        dialogueQueue.Clear();

        if (dialogueCanvas != null)
        {
            dialogueCanvas.SetActive(false);
            dialogueText.text = "";
        }

        dialogueTimer = 0f;
        isTyped = false;
        isDialogueShown = false;
    }
}