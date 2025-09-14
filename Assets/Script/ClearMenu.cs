using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ClearMenu : MonoBehaviour
{
    public circlemove player;

    public string nextSceneName;

    public GameObject nextStageButton;

    public GameObject clearMenu;

    public GameObject Menu;

    public GameObject toutext;

    public string stageName;


    void Start()
    { 
        // 게임 시작 시 tout 실행
        StartCoroutine(tout());
    }

    public void ShowClearMenu()
    {
        Menu.SetActive(false);
        Time.timeScale = 0f;
        clearMenu.SetActive(true);
        string stageKey = SceneManager.GetActiveScene().name + "Clear";

        if (PlayerPrefs.GetInt(stageKey, 0)==0)
            nextStageButton.SetActive(false);

    }

    public void RetryStage()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }

    public void NextStage()
    {
        SceneManager.LoadScene(nextSceneName);
        Time.timeScale = 1f;
    }

    public void GoToStageSelect()
    {
        SceneManager.LoadScene(stageName);
        Time.timeScale = 1f;
    }
    public void Resume()
    {
        clearMenu.SetActive(false);
        Time.timeScale = 1f;
        Menu.SetActive(true);
    }
    IEnumerator tout()
    {
        if (toutext != null)
        {
            toutext.SetActive(true);   // 시작 시 보이게
            yield return new WaitForSeconds(3f);
            toutext.SetActive(false);  // 5초 후 꺼짐
        }
    }
}