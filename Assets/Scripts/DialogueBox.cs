using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueBox : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private float typingSpeed = 0.05f;
    private bool isTyping = false;
    private string currentDialogue;
    [SerializeField] private string[] texts;
    private int dialoguePosition = 0;
    private void Start()
    {
        
    }

    public void StartDialogue(string dialogue)
    {
        currentDialogue = dialogue;
        StartCoroutine(TypeDialogue());
        
    }

    private IEnumerator TypeDialogue()
    {
        dialogueText.text = "";
        isTyping = true;

        foreach (char letter in currentDialogue)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        yield return new WaitForSeconds(2f);

        isTyping = false;

        

    }


    private void Update()
    {
        if (!isTyping && texts.Length > dialoguePosition)
        {
            dialogueText.text = "";
            StartDialogue(texts[dialoguePosition]);
            dialoguePosition++;
        }
    }
}
