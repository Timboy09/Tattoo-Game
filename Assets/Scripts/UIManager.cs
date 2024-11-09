using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{
    [HideInInspector] public GameManager gameManager;

    [Header("Main Menu")]
    [SerializeField]
    public GameObject mainMenuPanel;

    [SerializeField]
    private Button playBtn;

    [SerializeField]
    private Button creditsBtn;

    [SerializeField]
    private Button closeCreditsBtn;

    [SerializeField]
    private GameObject creditsPanel;

    [SerializeField]
    private Button quitBtn;

    [SerializeField]
    private Button yesQuitBtn;

    [SerializeField]
    private Button noQuitBtn;

    [SerializeField]
    private GameObject quitPanel;


    [Header("Dialogue")]
    public GameObject dialoguePanel;

    [Header("Game")]
    public GameObject gamePanel;

    [SerializeField]
    private GameObject tattooWarningPanel;

    public GameObject tattooCompletionPanel;

    public GameObject optionsPanel;

    public List<Button> flairOptionBtns;

    public TextMeshProUGUI tattooCompletionText;

    private bool optionSelectedOnce = false;

    private void Start()
    {
        gameManager = GameManager.instance; 

        #region Quit Game 

        HandleAlphaThreshold();

        quitBtn.onClick.AddListener(() =>
        {
            quitPanel.SetActive(true);
        });

        yesQuitBtn.onClick.AddListener(() =>
        {
            Application.Quit();
        });

        noQuitBtn.onClick.AddListener(() =>
        {
            quitPanel.gameObject.SetActive(false);
        });

        #endregion

        playBtn.onClick.AddListener(() =>
        {
            mainMenuPanel.SetActive(false);
            dialoguePanel.SetActive(true);
        });

        #region Credits

        creditsBtn.onClick.AddListener(() =>
        {
            creditsPanel.SetActive(true);
        });

        closeCreditsBtn.onClick.AddListener(() =>
        {
            creditsPanel.SetActive(false);
        });

        #endregion

        HandleFlairOptions();
    }

    private void HandleFlairOptions()
    {
        flairOptionBtns[0].onClick.AddListener(() =>
        {
            gameManager.reyaSelectedFlair.gameObject.SetActive(true);
            string btntext = flairOptionBtns[0].GetComponentInChildren<TextMeshProUGUI>().text;
            if (btntext == "Confirm?")
            {
                //Clicked second time, free this option
                Debug.Log("Continue Story here");
            }
            else
            {
                //Clicked Once, give players chace to see all Options
                flairOptionBtns[0].GetComponentInChildren<TextMeshProUGUI>().text = "Confirm?";
                gameManager.reyaSelectedFlair.sprite = gameManager.reyaOptionsSprs[0];
            }
        });

        flairOptionBtns[1].onClick.AddListener(() =>
        {
            gameManager.reyaSelectedFlair.gameObject.SetActive(true);
            string btntext = flairOptionBtns[1].GetComponentInChildren<TextMeshProUGUI>().text;
            if (btntext == "Confirm?")
            {
                //Clicked second time, free this option
                Debug.Log("Continue Story here");
            }
            else
            {
                //Clicked Once, give players chace to see all Options
                flairOptionBtns[1].GetComponentInChildren<TextMeshProUGUI>().text = "Confirm?";
                gameManager.reyaSelectedFlair.sprite = gameManager.reyaOptionsSprs[1];
            }
        });

        flairOptionBtns[2].onClick.AddListener(() =>
        {
            gameManager.reyaSelectedFlair.gameObject.SetActive(true);
            string btntext = flairOptionBtns[2].GetComponentInChildren<TextMeshProUGUI>().text;
            if (btntext == "Confirm?")
            {
                //Clicked second time, free this option
                Debug.Log("Continue Story here");
            }
            else
            {
                //Clicked Once, give players chace to see all Options
                flairOptionBtns[2].GetComponentInChildren<TextMeshProUGUI>().text = "Confirm?";
                gameManager.reyaSelectedFlair.sprite = gameManager.reyaOptionsSprs[2];
            }
        });
    }

    private void HandleAlphaThreshold()
    {
        playBtn.GetComponent<Image>().alphaHitTestMinimumThreshold = 1f;
        quitBtn.GetComponent<Image>().alphaHitTestMinimumThreshold = 1f;
        creditsBtn.GetComponent<Image>().alphaHitTestMinimumThreshold = 1f;
    }

    public void TriggerTattooWarning()
    {
        StopAllCoroutines();
        StartCoroutine(TattooWarningCO());
    }

    private IEnumerator TattooWarningCO()
    {
        tattooWarningPanel.SetActive(true);
        yield return new WaitForSeconds(1);
        tattooWarningPanel.SetActive(false);
    }
}
