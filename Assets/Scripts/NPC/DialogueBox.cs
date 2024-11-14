using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    private Image panelImage;


    private void Awake()
    {
        playerLayer = LayerMask.GetMask("Player");
        panelImage = dialoguePanel.GetComponent<Image>();
    }

    private void Update()
    {
        if (IsPlayerInRange() && Input.GetKeyDown(KeyCode.E))
        {
            if (!dialoguePanel.activeSelf)
            {
                ActivePanel();
            }

            if (!isTyping)
            {
                
                ManageDialogues();
            }
        }
        else if (!IsPlayerClose() && dialoguePanel.activeSelf)
        {
            dialoguePanel.SetActive(false);
        }
    }

    /// <summary>
    /// Starts a dialogue
    /// </summary>
    /// <param name="dialogue"></param>
    public void StartDialogue(string dialogue)
    {
        currentDialogue = dialogue;
        StartCoroutine(TypeDialogue());
        AudioManager.Instance.DialoguePlay();

    }

    /// <summary>
    /// Does the typing dialogue by each letter until finished
    /// </summary>
    /// <returns></returns>
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
        AudioManager.Instance.DialogueStop();

        isTyping = false;

    }

    /// <summary>
    /// Checks if player is in interactuable range
    /// </summary>
    /// <returns></returns>
    private bool IsPlayerInRange()
    {
        return Physics2D.OverlapCircle(transform.position, 0.5f, playerLayer);
    }

    /// <summary>
    /// Checks if the player is close enough to see the dialogues
    /// </summary>
    /// <returns></returns>
    private bool IsPlayerClose()
    {
        return Physics2D.OverlapCircle(transform.position, 5f, playerLayer);
    }



    /// <summary>
    /// Activates the dialogue panel and starts dialogue
    /// </summary>
    private void ActivePanel()
    {
        dialoguePanel.SetActive(true);
        
        StartDialogue(texts[dialoguePosition]);
        dialoguePosition++;
        return;
    }

    /// <summary>
    /// Manages the dialogue array
    /// </summary>
    private void ManageDialogues()
    {
        if (texts.Length <= dialoguePosition)
        {
            dialoguePosition = 0;
            dialoguePanel.SetActive(false);
            return;
        }

        dialogueText.text = "";
        StartDialogue(texts[dialoguePosition]);
        dialoguePosition++;
    }

}
