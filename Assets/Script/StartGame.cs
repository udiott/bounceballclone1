using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class StartGame : MonoBehaviour
{
    public Text startText;         // "Press Any Key" ���� �ؽ�Ʈ

    private bool isBlinking = true;

    void Start()
    {
        // �ؽ�Ʈ �����̴� �ڷ�ƾ ����
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
        yield return new WaitForSeconds(0.15f); // 0.15�� ���� ��ٷ� �Է� �Ҹ�
        SceneManager.LoadScene("WorldSelect");
    }


    IEnumerator BlinkText()
    {
        while (isBlinking)
        {
            startText.enabled = !startText.enabled; // �״� ����
            yield return new WaitForSeconds(0.4f);  // 0.5�� ����
        }
    }
}
