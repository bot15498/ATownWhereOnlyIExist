using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PromptDialogue : MonoBehaviour
{
    // use this if there is a lot of text and you want to write it out on a txt file.
    public TextAsset toSay;
    // Use this if it's just something quick. Having a text asset above will negate the effect of this.
    public string name = "";
    public string portraitName;
    public List<string> thingsToSay = new List<string>();

    private DialogueManager dm;
    private bool canStartTalk = false;
    [SerializeField]
    private List<DialoguePiece> dps = new List<DialoguePiece>();
    private float lastA = 0f;

    void Start()
    {
        dm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<DialogueManager>();
        // TODO if loading takes a while, might want to move loading text to be async with loading the level.
        if(toSay != null)
        {
            dps = DialogueManager.ParseText(toSay);
        }
        else
        {
            foreach(string dialogue in thingsToSay)
            {
                dps.Add(new DialoguePiece(name, dialogue, portraitName));
            }
        }
    }

    void Update()
    {
        float currA = Input.GetAxisRaw("AButt");
        if (canStartTalk && currA > 0.1f && currA != lastA)
        {
            dm.StartDialogue(dps);
        }
        lastA = currA;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            canStartTalk = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            canStartTalk = false;
        }
    }
}
