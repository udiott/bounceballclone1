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

    // 한 메인 스테이지당 서브스테이지 개수
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

    // 여기서 인덱스 하나 받아서 mainStage, subStage 계산 후 씬 로드
    public void LoadStageByIndex(int index)
    {
        index -= 1;
        if (index < 0)
        {
            Debug.LogError("잘못된 스테이지 인덱스: " + index);
            return;
        }

        // 인덱스 → mainStage, subStage 계산
        int mainStage = (index / subStageCount) + 1;       // ex: index=6, subStageCount=5 → mainStage=2
        int subStage = (index % subStageCount) + 1;        // ex: index=6, subStageCount=5 → subStage=2

        string sceneName = $"Stage{mainStage}-{subStage}";

        Debug.Log($"LoadStageByIndex 호출: 인덱스 {index} → 씬 {sceneName}");

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
        Debug.Log("클리어 데이터 리셋 완료");
        UpdateStageStates();
    }
    public void BacktoMenu()
    {
        SceneManager.LoadScene("WorldSelect");
    }
}
