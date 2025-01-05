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

    [SerializeField]
    private List<DialogueSO> chapter_one;

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

    [HideInInspector] public string[] btnText = new string[3];

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

        #region CHOICES BUTTONS
        optionABtn.onClick.AddListener(() =>
        {
            List<DialogueSO> chapter = null;

            switch (gameManager.currentChapter)
            {
                case 0:
                    chapter = chapter_zero;                 
                    break;

                case 1:
                    chapter = chapter_one;
                    break;
            }

            if (!chapter[currentDialogueIndex].forceBranchIndex)
            {
                branchEndIndex = chapter[currentDialogueIndex].branchEndIndex;
            }
            else
            {
                branchEndIndex = chapter[currentDialogueIndex + optionAIndex].branchEndIndex;
                Debug.Log("Forced Branch to: " + branchEndIndex);
            }
            currentDialogueIndex = chapter[currentDialogueIndex].branchAStartIndex;
            StartDialogue(chapter[currentDialogueIndex]);
            isBranch = true;

        });

        optionBBtn.onClick.AddListener(() =>
        {
            List<DialogueSO> chapter = null;

            switch (gameManager.currentChapter)
            {
                case 0:
                    chapter = chapter_zero;
                    isBranch = true;
                    break;

                case 1:
                    chapter = chapter_one;
                    break;
            }

            if (!chapter[currentDialogueIndex].forceBranchIndex)
            {
                branchEndIndex = chapter[currentDialogueIndex].branchEndIndex;
            }
            else
            {
                branchEndIndex = chapter[currentDialogueIndex + optionBIndex].branchEndIndex;
                Debug.Log("Forced Branch to: " + branchEndIndex);
            }
            currentDialogueIndex = chapter[currentDialogueIndex].branchBStartIndex;
            StartDialogue(chapter[currentDialogueIndex]);
        });

        optionCBtn.onClick.AddListener(() =>
        {
            List<DialogueSO> chapter = null;

            switch (gameManager.currentChapter)
            {
                case 0:
                    chapter = chapter_zero;
                    break;

                case 1:
                    chapter = chapter_one;
                    break;
            }

            if (!chapter[currentDialogueIndex].forceBranchIndex)
            {
                branchEndIndex = chapter[currentDialogueIndex].branchEndIndex;
            }
            else
            {
                branchEndIndex = chapter[currentDialogueIndex + optionCIndex].branchEndIndex;
                Debug.Log("Forced Branch to: " + branchEndIndex);
            }
            currentDialogueIndex = chapter[currentDialogueIndex].branchCStartIndex;
            StartDialogue(chapter[currentDialogueIndex]);
            isBranch = true;
        });

        #endregion

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
        else
        {
            if(currentDialogueIndex != chapter_zero.Count)
            {
                //TO DO: REMOVE HARD CODED 2ND CHAPTER. DONE 1 NIGHT BEFORE JURY
                print("Load Chapter One");
                currentDialogueIndex = -1;
                gameManager.currentChapter = 1;
                StartChapterOne();
            }
        }
    }

    public void StartChapterOne()
    {
        if (chapter_one.Count != 0 && currentDialogueIndex != chapter_one.Count - 1)
        {            
            if (!isBranch)
            {
                currentDialogueIndex++;
                if (currentDialogueIndex != -1 && chapter_one[currentDialogueIndex].forceBranchIndex && chapter_one[currentDialogueIndex].character != Character.CHOICE)
                {
                    currentDialogueIndex = chapter_one[currentDialogueIndex - 1].branchEndIndex;
                }
            }
            else
            {
                currentDialogueIndex = branchEndIndex;
                isBranch = false;
                branchEndIndex = -1;
            }
            
            StartDialogue(chapter_one[currentDialogueIndex]);
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
                btnText[0] = _dialogue.dialogues[0];
                btnText[1] = _dialogue.dialogues[1];
                btnText[2] = _dialogue.dialogues[2];
                gameManager.StartFlairGame(btnText);
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
        List<DialogueSO> chapter = null;

        switch (gameManager.currentChapter)
        {
            case 0:
                chapter = chapter_zero;
                break;

            case 1:
                chapter = chapter_one;
                break;
        }
        optionABtn.GetComponentInChildren<TextMeshProUGUI>().text = chapter[currentDialogueIndex].dialogues[optionAIndex];
        optionBBtn.GetComponentInChildren<TextMeshProUGUI>().text = chapter[currentDialogueIndex].dialogues[optionBIndex];
        if (chapter[currentDialogueIndex].branchCStartIndex == 0)
        {
            optionCBtn.gameObject.SetActive(false);
        }
        else
        {
            optionCBtn.gameObject.SetActive(true);
            optionCBtn.GetComponentInChildren<TextMeshProUGUI>().text = chapter[currentDialogueIndex].dialogues[optionCIndex];
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

        switch (gameManager.currentChapter)
        {
            case 0:
                StartChapterZero();
                break;

            case 1:
                StartChapterOne();
                break;
        }
    }

    private IEnumerator TypeSentence(string _senence)
    {
        //continueBtn.gameObject.SetActive(false);
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
