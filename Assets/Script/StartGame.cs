using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class StartGame : MonoBehaviour
{
    public Text startText;         // "Press Any Key" 같은 텍스트

    private bool isBlinking = true;

    void Start()
    {
        // 텍스트 깜빡이는 코루틴 시작
        if (startText != null)
        {
            StartCoroutine(BlinkText());
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isBlinking = false;
            StartCoroutine(LoadNextScene());
        }
    }
    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(0.15f); // 0.15초 정도 기다려 입력 소모
        SceneManager.LoadScene("WorldSelect");
    }


    IEnumerator BlinkText()
    {
        while (isBlinking)
        {
            startText.enabled = !startText.enabled; // 켰다 껐다
            yield return new WaitForSeconds(0.4f);  // 0.5초 간격
        }
    }
}
