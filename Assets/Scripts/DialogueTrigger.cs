using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{

    public Dialogue dialogue;

    public List<Dialogue> multiDialogue;

    public int currentDialogueCount = -1;

    public int maxDialogueCount;

    void Start()
    {
        currentDialogueCount++;
    }

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(multiDialogue, currentDialogueCount);
    }

}
