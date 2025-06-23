using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager_D1 : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private float typeSpeed;

    [SerializeField] GameObject player;
    [SerializeField] float maxRotationAngle;
    float rotationY;

    [SerializeField] float dialogueTime;
    private Queue<string> dialogueQueue;
    bool isTyped = false;
    bool dialogueShown = false;
    float dialogueTimer = 0f;

    private void Start()
    {
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

        if (dialogueShown)
        {
            dialogueTimer += Time.deltaTime;
            Debug.Log(dialogueTimer);
            if (dialogueTimer >= dialogueTime)
            {
                Debug.Log("Next");
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
            //Debug.Log(dialogueQueue.Count);
        }

        NextSentence();
    }

    public void NextSentence()
    {
        dialogueShown = false;
        if (dialogueQueue.Count == 0)
        {
            EndDialogue();
            //return;
        }
        else
        {
            Debug.Log(dialogueQueue.Count);
            string sentence = dialogueQueue.Dequeue();
            StopAllCoroutines();
            StartCoroutine(Type(sentence));
        }
    }

    IEnumerator Type(string sentence)
    {
        dialogueText.text = "";
        canvas.enabled = true;
        isTyped = true;

        foreach (char c in sentence.ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }

        isTyped = false;
        dialogueShown = true;
    }

    void EndDialogue()
    {
        StopAllCoroutines();
        dialogueQueue.Clear();

        canvas.enabled = false;
        dialogueText.text = "";

        dialogueTimer = 0f;
        isTyped = false;
        dialogueShown = false;
    }
}