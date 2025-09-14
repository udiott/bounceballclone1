using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class StageSelectManager : MonoBehaviour
{
    [System.Serializable]
    public class StageInfo
    {
        public Button stageButton;
        public GameObject clearTextObj;
        public int mainStage;
        public int subStage;

        public string GetClearKey()
        {
            return $"Stage{mainStage}-{subStage}Clear";
        }

        public string GetSceneName()
        {
            return $"Stage{mainStage}-{subStage}";
        }
    }

    public List<StageInfo> stages;

    // �� ���� ���������� ���꽺������ ����
    public int subStageCount = 5;

    void Start()
    {
        UpdateStageStates();
    }

    void UpdateStageStates()
    {
        for (int i = 0; i < stages.Count; i++)
        {
            var info = stages[i];
            int clearValue = PlayerPrefs.GetInt(info.GetClearKey(), 0);

            info.clearTextObj.SetActive(clearValue == 1);

            if (i == 0)
            {
                info.stageButton.interactable = true;
            }
            else
            {
                var prevInfo = stages[i - 1];
                int prevClearValue = PlayerPrefs.GetInt(prevInfo.GetClearKey(), 0);
                info.stageButton.interactable = (prevClearValue == 1);
            }
        }
    }

    // ���⼭ �ε��� �ϳ� �޾Ƽ� mainStage, subStage ��� �� �� �ε�
    public void LoadStageByIndex(int index)
    {
        index -= 1;
        if (index < 0)
        {
            Debug.LogError("�߸��� �������� �ε���: " + index);
            return;
        }

        // �ε��� �� mainStage, subStage ���
        int mainStage = (index / subStageCount) + 1;       // ex: index=6, subStageCount=5 �� mainStage=2
        int subStage = (index % subStageCount) + 1;        // ex: index=6, subStageCount=5 �� subStage=2

        string sceneName = $"Stage{mainStage}-{subStage}";

        Debug.Log($"LoadStageByIndex ȣ��: �ε��� {index} �� �� {sceneName}");

        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1f;
    }



    public void QuitGame()
    {
        Application.Quit();
    }

    public void ResetClearData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("Ŭ���� ������ ���� �Ϸ�");
        UpdateStageStates();
    }
    public void BacktoMenu()
    {
        SceneManager.LoadScene("WorldSelect");
    }
}
