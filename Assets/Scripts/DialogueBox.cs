using System.Collections;
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
    private LayerMask playerLayer;
    [SerializeField] private GameObject keyLabel;
    [SerializeField] private GameObject dialoguePanel;


    private void Awake()
    {
        playerLayer = LayerMask.GetMask("Player");

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

        string[] words = currentDialogue.Split(' ');

        foreach (string word in words)
        {
            if (word.StartsWith("<") && word.EndsWith(">"))
            {
                dialogueText.text += word + "  ";
            }
            else
            {
                foreach (char letter in word)
                {
                    dialogueText.text += letter;
                    yield return new WaitForSeconds(typingSpeed);
                }
                dialogueText.text += " ";
            }
        }

        isTyping = false;

    }

    private bool IsPlayerInRange()
    {
        return Physics2D.OverlapCircle(transform.position, 0.5f, playerLayer);
    }


    private void Update()
    {
        if (IsPlayerInRange() && Input.GetKeyDown(KeyCode.E))
        {
            if (!dialoguePanel.activeSelf)
            {
                dialoguePanel.SetActive(true);
                StartDialogue(texts[dialoguePosition]);
                dialoguePosition++;
                return;
            }

            if (!isTyping && texts.Length > dialoguePosition)
            {
                dialogueText.text = "";
                StartDialogue(texts[dialoguePosition]);
                dialoguePosition++;
            }
        }



    }
}
