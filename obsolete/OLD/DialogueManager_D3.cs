using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager_D3 : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private Canvas canvas;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI nameTag;
    [SerializeField] private GameObject player;
    [SerializeField] private float maxRotationAngle;
    private float rotationY;

    [Header("Reading Time")]
    [SerializeField] private float typeSpeed;
    [SerializeField] private float dialogueTime;
    float dialogueTimer = 0f;

    private Queue<string> dialogueQueue;

    bool isTyped = false;
    bool isDialogueShown = false;

    private void Start()
    {
        nameTag.text = "";
        dialogueText.text = "";
        canvas.enabled = false;
        dialogueQueue = new Queue<string>();
    }

    private void Update()
    {
        if (dialogueText.text != "" || isTyped)
        {
            if
                ((player.transform.rotation.y - rotationY < -maxRotationAngle)
                || (player.transform.rotation.y - rotationY > maxRotationAngle))
            {
                EndDialogue();
            }
        }

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

    public void StartDialogue(Dialogue dialogue)
    {
        dialogueQueue.Clear();
        rotationY = player.transform.rotation.y;

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
        dialogueText.text = "";
        canvas.enabled = true;

        yield return new WaitForSeconds(0.0f);
        dialogueText.text += sentence;
        //isTyped = true;

        /*foreach (char c in sentence.ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }
        */
        //isTyped = false;
        isDialogueShown = true;
    }

    public void EndDialogue()
    {
        StopAllCoroutines();
        dialogueQueue.Clear();

        canvas.enabled = false;
        dialogueText.text = "";

        dialogueTimer = 0f;
        isTyped = false;
        isDialogueShown = false;
    }
}