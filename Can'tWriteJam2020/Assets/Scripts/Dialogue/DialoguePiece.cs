using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialoguePiece 
{
    public string characterName;
    public string text;
    public string portraitSpriteName;
    // TODO add transition information here

    public DialoguePiece(string name, string text, string portraitSpriteName)
    {
        this.characterName = name;
        this.text = text;
        this.portraitSpriteName = portraitSpriteName;
    }

    public DialoguePiece()
    {
        this.characterName = "";
        this.text = "";
        this.portraitSpriteName = "";
    }
}
