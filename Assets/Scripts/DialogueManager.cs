using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    private Queue<string> dialogues;

    [SerializeField]
    private TextMeshProUGUI charNameText;

    [SerializeField]
    private TextMeshProUGUI dialoguesText;

    [SerializeField]
    private Button continueBtn;

    private void Start()
    {
        dialogues = new Queue<string>();
        continueBtn.onClick.AddListener(() =>
        {
            DisplayNextSentence();
        });
    }

    public void StartDialogue(Dialogue _dialogue)
    {
        dialogues.Clear();
        charNameText.text = _dialogue.charName;
        foreach (var sentence in dialogues)
        {
            dialogues.Enqueue(sentence);
        }
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if(dialogues.Count == 0)
        {
            EndDialogue();
            return;
        }
        string currentDialogue = dialogues.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(currentDialogue));
    }

    public void EndDialogue()
    {
        //End Conversation : Load Tattoo Game / Next Chapter
        Debug.Log("End of Conversation");
    }

    private IEnumerator TypeSentence(string _senence)
    {
        dialoguesText.text = "";
        foreach (char letter in _senence.ToCharArray())
        {
            dialoguesText.text += letter;
            yield return null;
        }
    }
}

public enum Character
{
    MIN,
    PLAYER,
    REYA,
    QI,
    NAYR,
    URZA,
    NURY,
    MAX
}
