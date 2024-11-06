using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Main Menu")]
    [SerializeField]
    private GameObject mainMenuPanel;

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


    [Header("Game")]
    [SerializeField]
    private GameObject gamePanel;

    private void Start()
    {
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
            gamePanel.SetActive(true);
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
    }

    private void HandleAlphaThreshold()
    {
        playBtn.GetComponent<Image>().alphaHitTestMinimumThreshold = 1f;
        quitBtn.GetComponent<Image>().alphaHitTestMinimumThreshold = 1f;
        creditsBtn.GetComponent<Image>().alphaHitTestMinimumThreshold = 1f;
    }
}
