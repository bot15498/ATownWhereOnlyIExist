using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public int maxCharactersPerLine = 52;
    public float charDelay = 0.5f;
    public GameObject dialogueStuff;
    public Text nameText;
    public Text dialogueText;
    public Image portrait;
    public bool dialogueIsActive = false;
    public bool dialogueIsTyping = false;

    [SerializeField]
    private List<DialoguePiece> currPieces;
    private int currentIdx = 0;
    private GameObject player;
    private PlayerMovement playerMovement;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            playerMovement = player.GetComponent<PlayerMovement>();
        }
    }

    public void StartDialogue(TextAsset rawFile)
    {
        playerMovement.canMove = false;
    }

    public void StartDialogue(List<DialoguePiece> dialogue)
    {
        // This called whenever a player is in range and wants to make an advancement.
        // Either start the dialogue, finish the typing, or continue to next dialogue, or close
        if(!dialogueIsActive)
        {
            currPieces = dialogue;
            playerMovement.canMove = false;
            dialogueStuff.SetActive(true);
            dialogueIsActive = true;
            currentIdx = 0;
            StartCoroutine(ScrollingText(currPieces[currentIdx], charDelay));
            currentIdx++;
        }
        else if(dialogueIsTyping)
        {
            dialogueIsTyping = false;
        }
        else if(currentIdx < currPieces.Count)
        {
            StartCoroutine(ScrollingText(currPieces[currentIdx], charDelay));
            currentIdx++;
        }
        else
        {
            dialogueIsActive = false;
            currentIdx = 0;
            dialogueStuff.SetActive(false);
            playerMovement.canMove = true;
        }
    }

    private IEnumerator ScrollingText(DialoguePiece dialoguePiece, float delay)
    {
        nameText.text = dialoguePiece.characterName;
        dialogueIsTyping = true;
        dialogueText.text = "";
        int currChar = 0;
        while(dialogueIsTyping && currChar < dialoguePiece.text.Length)
        {
            dialogueText.text += dialoguePiece.text[currChar];
            currChar++;
            yield return new WaitForSecondsRealtime(delay);
        }
        dialogueText.text = dialoguePiece.text;
        dialogueIsTyping = false;
        yield return null;
    }


    public List<DialoguePiece> ParseText(TextAsset rawFile)
    {
        List<DialoguePiece> dialogue = new List<DialoguePiece>();
        string[] lines = Regex.Split(rawFile.text, "\r\n|\n|\r");
        foreach(string line in lines)
        {
            string trimmedLine = line.Trim();
            trimmedLine = Regex.Replace(trimmedLine, @"#.*", "");
            if(trimmedLine.Trim() == string.Empty)
            {
                continue;
            }

            // separate name and text to say. Currently used | to separate name and text, and == to set sprite portrait
            // TODO: add error checking for when some idiot writes the file wrong.
            // TODO: choices in dialogue.
            string[] pieces = trimmedLine.Split('|');
            DialoguePiece dp = new DialoguePiece();
            dp.text = FixLongLines(pieces[1]);
            if (pieces[0].Contains("=="))
            {
                string[] nameAndPortrait = Regex.Split(pieces[0], "==");
                dp.characterName = nameAndPortrait[0];
                dp.portraitSpriteName = nameAndPortrait[1];
            }
            else
            {
                dp.characterName = pieces[0];
                dp.portraitSpriteName = "";
            }
            dialogue.Add(dp);
        }

        return dialogue;
    }

    private string FixLongLines(string originalLine)
    {
        string fixedLine = "";
        int currChars = 0;
        foreach(string word in originalLine.Split(' '))
        {
            currChars += word.Length + 1;
            if (currChars > maxCharactersPerLine)
            {
                fixedLine += '\n';
                currChars -= maxCharactersPerLine;
            }
            fixedLine += word + ' ';
        }
        return fixedLine;
    }
}
