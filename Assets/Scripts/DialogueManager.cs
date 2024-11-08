using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    private GameManager gameManager;

    [SerializeField]
    private List<DialogueSO> chapter_zero;

    [HideInInspector] public int currentDialogueIndex = -1;
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
        gameManager = GameManager.instance;
        dialogues = new Queue<string>();
        continueBtn.onClick.AddListener(() =>
        {
            DisplayNextSentence();
        });

        optionABtn.onClick.AddListener(() =>
        {
            if (!chapter_zero[currentDialogueIndex].forceBranchIndex)
            {
                branchEndIndex = chapter_zero[currentDialogueIndex].branchEndIndex;
            }
            else
            {
                branchEndIndex = chapter_zero[currentDialogueIndex + optionAIndex].branchEndIndex;
                Debug.Log("Forced Branch to: " + branchEndIndex);
            }
            currentDialogueIndex = chapter_zero[currentDialogueIndex].branchAStartIndex;
            StartDialogue(chapter_zero[currentDialogueIndex]);
            isBranch = true;
        });

        optionBBtn.onClick.AddListener(() =>
        {
            if (!chapter_zero[currentDialogueIndex].forceBranchIndex)
            {
                branchEndIndex = chapter_zero[currentDialogueIndex].branchEndIndex;
            }
            else
            {
                branchEndIndex = chapter_zero[currentDialogueIndex + optionBIndex].branchEndIndex;
                Debug.Log("Forced Branch to: " + branchEndIndex);
            }
            currentDialogueIndex = chapter_zero[currentDialogueIndex].branchBStartIndex;
            StartDialogue(chapter_zero[currentDialogueIndex]);
            isBranch = true;
        });

        optionCBtn.onClick.AddListener(() =>
        {
            if (!chapter_zero[currentDialogueIndex].forceBranchIndex)
            {
                branchEndIndex = chapter_zero[currentDialogueIndex].branchEndIndex;
            }
            else
            {
                branchEndIndex = chapter_zero[currentDialogueIndex + optionCIndex].branchEndIndex;
                Debug.Log("Forced Branch to: " + branchEndIndex);
            }
            currentDialogueIndex = chapter_zero[currentDialogueIndex].branchCStartIndex;
            StartDialogue(chapter_zero[currentDialogueIndex]);
            isBranch = true;
        });

        switch (gameManager.currentChapter)
        {
            case 0:
                StartChapterZero();
                break;
        }
    }

    public void StartChapterZero()
    {
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

    private bool forceCloseContinueBtn = false;

    public void StartDialogue(DialogueSO _dialogue)
    {
        forceCloseContinueBtn = false;
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
                optionsPanel.SetActive(true);
                forceCloseContinueBtn = true;
                HandleOptions();
                break;

            case Character.CLUE:
                forceCloseContinueBtn = true;
                ClueInteractions();
                break;

            case Character.GAME:
                gameManager.StartTattooGame();
                break;

            case Character.FLAIR:
                gameManager.StartFlairGame();
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

    #region Clue Interactions

    private string[] triangleClues = { "APEX" };
    private string[] circleClues = { "ORBIT" };
    private string[] quadClues = { "FRAME" };

    private List<Shapes> tattooShapes;

    private string LastClickedWord;

    private bool startClueDetection = false;

    [Header("Tattoo Clues")]
    [SerializeField]
    private GameObject hintPanel;

    [SerializeField]
    private GameObject shapeBubble;

    [SerializeField]
    private Image clickedShape;

    [SerializeField]
    private Sprite triangleSpr;

    [SerializeField]
    private Sprite circleSpr;

    [SerializeField]
    private Sprite quadSpr;

    public void ClueInteractions()
    {
        hintPanel.SetActive(true);
        startClueDetection = true;
        tattooShapes = new List<Shapes>();
        gameManager.tattooShapes = new List<Shapes>();
        for (int i = 0; i < chapter_zero[currentDialogueIndex].tattooShapes.Count; i++)
        {
            tattooShapes.Add(chapter_zero[currentDialogueIndex].tattooShapes[i]);
            gameManager.tattooShapes.Add(chapter_zero[currentDialogueIndex].tattooShapes[i]);
        }
    }

    private void Update()
    {
        if(tattooShapes != null && tattooShapes.Count == 0 && startClueDetection)
        {
            startClueDetection = false;
            continueBtn.gameObject.SetActive(true);
            hintPanel.SetActive(false);
        }

        if (Input.GetMouseButtonDown(0) && startClueDetection && tattooShapes.Count > 0)
        {
            var wordIndex = TMP_TextUtilities.FindIntersectingWord(dialoguesText, Input.mousePosition, null);

            if (wordIndex != -1)
            {
                LastClickedWord = dialoguesText.textInfo.wordInfo[wordIndex].GetWord();

                foreach (var clue in triangleClues)
                {
                    if(LastClickedWord == clue)
                    {
                        var targetIndex = tattooShapes.IndexOf(Shapes.TRIANGLE);
                        if (targetIndex != -1)
                        {
                            tattooShapes.RemoveAt(targetIndex);
                            shapeBubble.SetActive(true);
                            clickedShape.sprite = triangleSpr;
                        }
                        
                    }
                }

                foreach (var clue in circleClues)
                {
                    if (LastClickedWord == clue)
                    {
                        var targetIndex = tattooShapes.IndexOf(Shapes.CIRCLE);
                        if(targetIndex != -1)
                        {
                            tattooShapes.RemoveAt(targetIndex);
                            shapeBubble.SetActive(true);
                            clickedShape.sprite = circleSpr;
                        }                        
                    }
                }

                foreach (var clue in quadClues)
                {
                    if (LastClickedWord == clue)
                    {
                        var targetIndex = tattooShapes.IndexOf(Shapes.QUAD);
                        if(targetIndex != -1)
                        {
                            tattooShapes.RemoveAt(targetIndex);
                            shapeBubble.SetActive(true);
                            clickedShape.sprite = quadSpr;
                        }                        
                    }
                }
            }
        }
    }

    #endregion

    public void HandleOptions()
    {
        optionABtn.GetComponentInChildren<TextMeshProUGUI>().text = chapter_zero[currentDialogueIndex].dialogues[optionAIndex];
        optionBBtn.GetComponentInChildren<TextMeshProUGUI>().text = chapter_zero[currentDialogueIndex].dialogues[optionBIndex];
        if(chapter_zero[currentDialogueIndex].branchCStartIndex == 0)
        {
            optionCBtn.gameObject.SetActive(false);
        }
        else
        {
            optionCBtn.gameObject.SetActive(true);
            optionCBtn.GetComponentInChildren<TextMeshProUGUI>().text = chapter_zero[currentDialogueIndex].dialogues[optionCIndex];
        }
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
        shapeBubble.SetActive(false);
        StartChapterZero();
    }

    private IEnumerator TypeSentence(string _senence)
    {
        continueBtn.gameObject.SetActive(false);
        dialoguesText.text = "";
        foreach (char letter in _senence.ToCharArray())
        {
            dialoguesText.text += letter;
            yield return new WaitForSeconds(0.02f);    
        }
        if (!forceCloseContinueBtn)
        {
            continueBtn.gameObject.SetActive(true);
        }
    }
}
