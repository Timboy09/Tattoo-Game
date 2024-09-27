using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI nametext;
    public TextMeshProUGUI dialogueText;
    public Animator animator;
    private Queue<string> sentences;
    public DialogueTrigger dialogueTrigger;
    // Use this for initialization
    void Start()
    {
        sentences = new Queue<string>();
    }
    public void StartDialogue(List<Dialogue> multiDialogue, int _index)
    {
        animator.SetBool("IsOpen", true);
        nametext.text = multiDialogue[_index].name;
        sentences.Clear();
        foreach (string sentence in multiDialogue[_index].sentences)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
    }
    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            if (dialogueTrigger.currentDialogueCount >= dialogueTrigger.maxDialogueCount - 1)
            {
                EndDialogue();
                dialogueTrigger.currentDialogueCount = 0;
                //Change the Scene;
                SceneManager.LoadScene("Game");
                return;
            }
            else
            {
                dialogueTrigger.currentDialogueCount++;
                StartDialogue(dialogueTrigger.multiDialogue, dialogueTrigger.currentDialogueCount);
                return;
            }
        }
        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }
    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }
    void EndDialogue()
    {
        animator.SetBool("IsOpen", false);
    }
}