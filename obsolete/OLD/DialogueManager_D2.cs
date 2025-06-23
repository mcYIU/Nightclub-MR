using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager_D2 : MonoBehaviour
{
    [SerializeField] private Animator fade;
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
        dialogueQueue = new Queue<string>();
    }

    private void Update()
    {
        if (isTyped || dialogueShown)
        {
            if (dialogueText.text != "" || isTyped)
            {
                if
                    ((player.transform.rotation.y - rotationY <= -maxRotationAngle)
                    || (player.transform.rotation.y - rotationY >= maxRotationAngle))
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
        fade.SetBool("isTalking", true);
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

        fade.SetBool("isTalking", false);
        dialogueText.text = "";

        dialogueTimer = 0f;
        isTyped = false;
        dialogueShown = false;
    }
}