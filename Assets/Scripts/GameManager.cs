using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public UIManager uiManager;

    [HideInInspector]
    public List<Shapes> tattooShapes;

    [SerializeField]
    private GameObject gameObj;

    [SerializeField]
    private Sprite reyaTattooBg;

    [SerializeField]
    private Sprite qiTattooBg;

    [SerializeField]
    private Sprite nayrTattooBg;

    [SerializeField]
    private Sprite urzaTattooBg;

    [SerializeField]
    private Sprite nuryTattooBg;

    public int currentChapter = 0;

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

    public void StartTattooGame()
    {
        uiManager.dialoguePanel.SetActive(false);
        gameObj.SetActive(true);

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
