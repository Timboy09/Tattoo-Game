using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [SerializeField]
    private List<DialogueSO> chapter_zero;

    private int currentDialogueIndex = -1;
    private int branchEndIndex = -1;

    private Queue<string> dialogues;

    [SerializeField]
    private GameObject charNameBox;

    [SerializeField]
    private TextMeshProUGUI charNameText;

    [SerializeField]
    private TextMeshProUGUI dialoguesText;

    [SerializeField]
    private Image charImg;

    [Header("Buttons")]
    [SerializeField]
    private Button continueBtn;

    [SerializeField]
    private GameObject optionsPanel;

    [SerializeField]
    private Button optionABtn;

    [SerializeField]
    private Button optionBBtn;

    [SerializeField]
    private Button optionCBtn;

    [Header("Character Display Options")]
    [SerializeField]
    private Color playerColor;

    [SerializeField]
    private Color reyaColor;

    [SerializeField]
    private Color qiColor;

    [SerializeField]
    private Color nayrColor;

    [SerializeField]
    private Color urzaColor;

    [SerializeField]
    private Color nuryColor;

    [SerializeField]
    private Sprite reyaSpr;

    [SerializeField]
    private Sprite qiSpr;

    [SerializeField]
    private Sprite nayrSpr;

    [SerializeField]
    private Sprite urzaSpr;

    [SerializeField]
    private Sprite nurySpr;

    private const string reyaName = "Reya";
    private const string qiName = "Qi";
    private const string nayrName = "Nayr";
    private const string urzaName = "Urza";
    private const string nuryName = "Nury";
    private const string playerName = "Player"; //THIS IS TEMP, NEED INPUT FROM PLAYER

    private const int optionAIndex = 1;
    private const int optionBIndex = 2;
    private const int optionCIndex = 3;

    private bool isBranch = false;

    private void Start()
    {
        dialogues = new Queue<string>();
        continueBtn.onClick.AddListener(() =>
        {
            DisplayNextSentence();
        });

        optionABtn.onClick.AddListener(() =>
        {
            branchEndIndex = chapter_zero[currentDialogueIndex].branchEndIndex;
            currentDialogueIndex = chapter_zero[currentDialogueIndex].branchAStartIndex;
            StartDialogue(chapter_zero[currentDialogueIndex]);
            isBranch = true;
        });

        optionBBtn.onClick.AddListener(() =>
        {
            branchEndIndex = chapter_zero[currentDialogueIndex].branchEndIndex;
            currentDialogueIndex = chapter_zero[currentDialogueIndex].branchBStartIndex;
            StartDialogue(chapter_zero[currentDialogueIndex]);
            isBranch = true;
        });

        optionCBtn.onClick.AddListener(() =>
        {
            branchEndIndex = chapter_zero[currentDialogueIndex].branchEndIndex;
            currentDialogueIndex = chapter_zero[currentDialogueIndex].branchCStartIndex;
            StartDialogue(chapter_zero[currentDialogueIndex]);
            isBranch = true;
        });

        StartChapterZero();
    }

    public void StartChapterZero()
    {
        //THIS IS TEMPORARY AND NEEDS TO CHANGE CHAPTERS AUTOMATICALLY

        if (chapter_zero.Count != 0 && currentDialogueIndex != chapter_zero.Count - 1)
        {
            if (!isBranch)
            {
                currentDialogueIndex++;
            }
            else
            {
                currentDialogueIndex = branchEndIndex;
                isBranch = false;
                branchEndIndex = -1;
            }
            StartDialogue(chapter_zero[currentDialogueIndex]);
        }
    }

    public void StartDialogue(DialogueSO _dialogue)
    {
        dialogues.Clear();

        optionsPanel.SetActive(false);
        continueBtn.gameObject.SetActive(true);
        charImg.gameObject.SetActive(true);

        #region Handle Character Box Rendering         

        switch (_dialogue.character)
        {
            case Character.SCENE:
                charNameText.text = "";
                charImg.sprite = null;
                break;

            case Character.PLAYER:
                charNameText.text = playerName;
                charNameText.color = playerColor;
                charImg.sprite = null;
                break;

            case Character.REYA:
                charNameText.text = reyaName;
                charNameText.color = reyaColor;
                charImg.sprite = reyaSpr;
                break;

            case Character.QI:
                charNameText.text = qiName;
                charNameText.color = qiColor;
                charImg.sprite = qiSpr;
                break;

            case Character.NAYR:
                charNameText.text = nayrName;
                charNameText.color = nayrColor;
                charImg.sprite = nayrSpr;
                break;

            case Character.URZA:
                charNameText.text = urzaName;
                charNameText.color = urzaColor;
                charImg.sprite = urzaSpr;
                break;

            case Character.NURY:
                charNameText.text = nuryName;
                charNameText.color = nuryColor;
                charImg.sprite = nurySpr;
                break;

            case Character.CHOICE:
                charNameText.text = "";
                charImg.sprite = null;
                optionsPanel.SetActive(true);
                continueBtn.gameObject.SetActive(false);
                HandleOptions();
                break;
        }

        charImg.preserveAspect = true;

        if (charNameText.text == "")
        {
            charNameBox.gameObject.SetActive(false);
        }
        else
        {
            charNameBox.gameObject.SetActive(true);
        }

        if(charImg.sprite == null)
        {
            charImg.gameObject.SetActive(false);
        }

        #endregion

        foreach (var sentence in _dialogue.dialogues)
        {
            dialogues.Enqueue(sentence);
        }
        DisplayNextSentence();
    }

    public void HandleOptions()
    {
        optionABtn.GetComponentInChildren<TextMeshProUGUI>().text = chapter_zero[currentDialogueIndex].dialogues[optionAIndex];
        optionBBtn.GetComponentInChildren<TextMeshProUGUI>().text = chapter_zero[currentDialogueIndex].dialogues[optionBIndex];
        optionCBtn.GetComponentInChildren<TextMeshProUGUI>().text = chapter_zero[currentDialogueIndex].dialogues[optionCIndex];
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
        StartChapterZero();
    }

    private IEnumerator TypeSentence(string _senence)
    {
        dialoguesText.text = "";
        foreach (char letter in _senence.ToCharArray())
        {
            dialoguesText.text += letter;
            yield return new WaitForSeconds(0.02f);    
        }
    }
}

public enum Character
{
    MIN,
    SCENE,
    PLAYER,
    REYA,
    QI,
    NAYR,
    URZA,
    NURY,
    CHOICE,
    MAX
}
