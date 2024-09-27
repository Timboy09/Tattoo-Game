using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene : MonoBehaviour
{
    void Start()
    {
        Debug.Log("LoadSceneA");
    }

    public void LoadA(string scenename)
    {
        Debug.Log("sceneName to load: " + scenename);
        SceneManager.LoadScene(scenename);
    }
}