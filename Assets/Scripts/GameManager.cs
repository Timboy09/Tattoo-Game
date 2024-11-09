using FreeDraw;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int currentChapter = 0;
    public UIManager uiManager;
    public DialogueManager dialogueManager;
    public List<Shapes> tattooShapes;

    [HideInInspector] public int tattooCompletionIndex;
    [HideInInspector] public int tattooCompletionTotal;

    [SerializeField]
    private GameObject gameObj;

    [SerializeField]
    private GameObject drawAreaObj;

    [SerializeField]
    private GameObject tattooPrefab;

    [Header("Reya Tattoo")]
    [SerializeField] private Sprite reyaTattooBg;
    [SerializeField] private List<Sprite> reyaTattooSprs;
    public List<Sprite> reyaOptionsSprs;
    public SpriteRenderer reyaSelectedFlair;

    [Header("Qi Tattoo")]
    [SerializeField]
    private Sprite qiTattooBg;

    [Header("Nayr Tattoo")]
    [SerializeField]
    private Sprite nayrTattooBg;

    [Header("Urza Tattoo")]
    [SerializeField]
    private Sprite urzaTattooBg;

    [Header("Nury Tattoo")]
    [SerializeField]
    private Sprite nuryTattooBg;

    [HideInInspector] public const int chapterZero = 0;
    [HideInInspector] public const int chapterOne = 1;
    [HideInInspector] public const int chapterTwo = 2;
    [HideInInspector] public const int chapterThree = 3;
    [HideInInspector] public const int chapterFour = 4;
    [HideInInspector] public const int chapterFive = 5;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void DrawTattooShapes(Shapes _shape)
    {
        tattooCompletionIndex++;
        uiManager.tattooCompletionText.text = tattooCompletionIndex + "/" + tattooCompletionTotal;

        //TODO: Hard Coding this part, later think of a better solution
        switch (_shape)
        {
            case Shapes.TRIANGLE:
                switch (currentChapter)
                {
                    case chapterZero:
                        GameObject tattoo = Instantiate(tattooPrefab, gameObj.transform);
                        tattoo.GetComponent<SpriteRenderer>().sprite = reyaTattooSprs[0];
                        tattoo.GetComponent<SpriteRenderer>().sortingLayerName = "Tattoo";
                        tattoo.GetComponent<SpriteRenderer>().sortingOrder = 0;
                    break;

                    case chapterOne:

                    break;
                }
                break;

            case Shapes.CIRCLE:
                switch (currentChapter)
                {
                    case chapterZero:
                        GameObject tattoo = Instantiate(tattooPrefab, gameObj.transform);
                        tattoo.GetComponent<SpriteRenderer>().sprite = reyaTattooSprs[1];
                        tattoo.GetComponent<SpriteRenderer>().sortingLayerName = "Tattoo";
                        tattoo.GetComponent<SpriteRenderer>().sortingOrder = 0;
                        break;

                    case chapterOne:

                        break;
                }
                break;

            case Shapes.QUAD:
                switch (currentChapter)
                {
                    case chapterZero:
                        GameObject tattoo = Instantiate(tattooPrefab, gameObj.transform);
                        tattoo.GetComponent<SpriteRenderer>().sprite = reyaTattooSprs[2];
                        tattoo.GetComponent<SpriteRenderer>().sortingLayerName = "Tattoo";
                        tattoo.GetComponent<SpriteRenderer>().sortingOrder = 0;
                        break;

                    case chapterOne:

                        break;
                }
                break;
        }        
    }

    public void StartFlairGame(string[] _btnText)
    {
        uiManager.dialoguePanel.SetActive(false);
        uiManager.optionsPanel.SetActive(true);
        uiManager.gamePanel.SetActive(true);

        gameObj.SetActive(true);

        for (int i = 0; i < uiManager.flairOptionBtns.Count; i++)
        {
            uiManager.flairOptionBtns[i].GetComponentInChildren<TextMeshProUGUI>().text = _btnText[i];
        }
    }

    public IEnumerator EndFlairgame()
    {
        uiManager.optionsPanel.SetActive(false);
        yield return new WaitForSeconds(1f);
        uiManager.dialoguePanel.SetActive(true);        
        uiManager.gamePanel.SetActive(false);

        gameObj.SetActive(false);
        dialogueManager.StartChapterZero();
    }

    public void StartTattooGame()
    {
        drawAreaObj.GetComponent<Drawable>().enabled = true;
        drawAreaObj.GetComponent<RecognitionManager>().enabled = true;

        uiManager.dialoguePanel.SetActive(false);
        uiManager.tattooCompletionPanel.SetActive(true);
        uiManager.gamePanel.SetActive(true);
        uiManager.tattooCompletionText.text = tattooCompletionIndex + "/" + tattooCompletionTotal;

        reyaSelectedFlair.gameObject.SetActive(false);
        gameObj.SetActive(true);
        tattooCompletionTotal = tattooShapes.Count;
        tattooCompletionIndex = 0;

        switch (currentChapter)
        {
            case chapterZero:
                gameObj.GetComponent<SpriteRenderer>().sprite = reyaTattooBg;
                break;

            case chapterOne:
                gameObj.GetComponent<SpriteRenderer>().sprite = qiTattooBg;
                break;

            case chapterTwo:
                gameObj.GetComponent<SpriteRenderer>().sprite = nayrTattooBg;
                break;

            case chapterThree:
                gameObj.GetComponent<SpriteRenderer>().sprite = urzaTattooBg;
                break;

            case chapterFour:
                gameObj.GetComponent<SpriteRenderer>().sprite = nuryTattooBg;
                break;
        }
    }

    public IEnumerator EndTattooGame()
    {
        drawAreaObj.GetComponent<Drawable>().enabled = false;
        drawAreaObj.GetComponent<RecognitionManager>().enabled = false;

        yield return new WaitForSeconds(1f);
        uiManager.dialoguePanel.SetActive(true);
        uiManager.tattooCompletionPanel.SetActive(false);
        uiManager.gamePanel.SetActive(false);

        gameObj.SetActive(false);
        dialogueManager.StartChapterZero();
    }
}
