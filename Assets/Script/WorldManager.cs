using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class WorldManager : MonoBehaviour
{
    [System.Serializable]
    public class WorldInfo
    {
        public Button worldButton;       // 월드 선택 버튼
        public GameObject clearTextObj;  // "클리어" 표시
        public int worldIndex;           // 월드 번호 (1,2,3,...)
    }

    public List<WorldInfo> worlds;

    void Start()
    {
        UpdateWorldStates();
    }

    void UpdateWorldStates()
    {
        for (int i = 0; i < worlds.Count; i++)
        {
            var info = worlds[i];
            bool unlocked = false;

            if (i == 0)
            {
                // 첫 번째 월드는 기본 해금
                info.worldButton.interactable = true;
            }
            else
            {
                // 이전 월드의 마지막 스테이지 클리어 여부 확인
                int prevWorld = worlds[i - 1].worldIndex;
                string lastStageKey = $"Stage{prevWorld}-5Clear";
                unlocked = PlayerPrefs.GetInt(lastStageKey, 0) == 1;

                info.worldButton.interactable = unlocked;
                worlds[i - 1].clearTextObj.SetActive(unlocked);
            }
        }

        // 🔥 마지막 월드 CLEAR 표시 강제 체크
        var lastWorld = worlds[worlds.Count - 1];
        string lastStageKeyFinal = $"Stage{lastWorld.worldIndex}-5Clear";
        bool lastClear = PlayerPrefs.GetInt(lastStageKeyFinal, 0) == 1;
        lastWorld.clearTextObj.SetActive(lastClear);
    }


    public void LoadStageSelectByIndex(int index)
    {
        string sceneName = $"StageSelect{index}";
        Debug.Log($"로드할 씬: {sceneName}");

        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1f;
    }
}
