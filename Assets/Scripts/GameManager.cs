using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int currentChapter = 0;
    public UIManager uiManager;
    public List<Shapes> tattooShapes;

    [HideInInspector] public int tattooCompletionIndex;
    [HideInInspector] public int tattooCompletionTotal;

    [SerializeField]
    private GameObject gameObj;

    [SerializeField]
    private GameObject tattooPrefab;

    [Header("Reya Tattoo")]
    [SerializeField] private Sprite reyaTattooBg;
    [SerializeField] private List<Sprite> reyaTattooSprs;
    [SerializeField] private List<Sprite> reyaOptionsSprs;

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

    public void StartTattooGame()
    {
        uiManager.dialoguePanel.SetActive(false);
        gameObj.SetActive(true);
        uiManager.gamePanel.SetActive(true);
        tattooCompletionTotal = tattooShapes.Count;
        tattooCompletionIndex = 0;
        uiManager.tattooCompletionText.text = tattooCompletionIndex + "/" + tattooCompletionTotal;

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
}
