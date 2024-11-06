using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogues")]
public class DialogueSO : ScriptableObject
{
    public Character character;

    public string charName;

    [TextArea(3,10)]
    public string[] dialogues;
}
