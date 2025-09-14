using UnityEngine;
using UnityEngine.SceneManagement;


public class GoalManager : MonoBehaviour
{
    public static GoalManager Instance;

    private int goalCount;

    public GameObject Clear;

    public GameObject clearMenu;

    public GameObject Menu;

    public GameObject resumebutton;
    void Awake()
    {
        // 싱글턴 패턴 — 어디서든 GoalManager.Instance로 접근 가능
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        // 태그로 Goal 개수 세기
        goalCount = GameObject.FindGameObjectsWithTag("Goal").Length;
        Debug.Log("이번 스테이지 Goal 개수: " + goalCount);
    }

    public void CollectGoal()
    {
        goalCount--;
        Debug.Log("남은 Goal 개수: " + goalCount);

        if (goalCount <= 0)
        {
            Debug.Log("게임 클리어!");
            Menu.SetActive(false);
            clearMenu.SetActive(true);
            resumebutton.SetActive(false);
            Time.timeScale = 0f;

            // 현재 씬 이름 기준으로 클리어 정보 저장
            string sceneName = SceneManager.GetActiveScene().name;
            string stageKey = sceneName + "Clear";
            PlayerPrefs.SetInt(stageKey, 1);
            PlayerPrefs.Save();

            // ClearMenu 참조
            ClearMenu clearMenuScript = clearMenu.GetComponent<ClearMenu>();

            // 🔎 스테이지 이름 분석해서 마지막 서브스테이지면 nextStage 버튼 끄기
            // 씬 이름은 "Stage1-5" 이런 형태라고 가정
            if (sceneName.Contains("-"))
            {
                string[] parts = sceneName.Split('-');
                if (parts.Length == 2)
                {
                    int subStage;
                    if (int.TryParse(parts[1], out subStage))
                    {
                        if (subStage >= 5) // 마지막 스테이지
                        {
                            clearMenuScript.nextStageButton.SetActive(false);
                            return;
                           
                        }
                    }
                }
            }

            // 기본적으로는 켜줌
            clearMenuScript.nextStageButton.SetActive(true);
        }
    }

}
