using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI dayText;
    [SerializeField] private TextMeshProUGUI infoText;
    [SerializeField] private TextMeshProUGUI ratingText;

    [Header("결과 패널 UI")]
    public GameObject blockerPanel; // 전체 입력 막는 BlockerPanel(Inspector 등록)
    public GameObject resultPanel; // 결과 패널(Inspector 등록)
    public TextMeshProUGUI earnedScoreText; // 얻은 점수(Inspector 등록)
    public TextMeshProUGUI lostScoreText;   // 잃은 점수(Inspector 등록)
    public TextMeshProUGUI successCountText; // 성공 횟수(Inspector 등록)
    public TextMeshProUGUI failCountText;    // 실패 횟수(Inspector 등록)
    public Button startNextDayButton;        // 스타트 버튼(Inspector 등록)

    [Header("Rules")]
    [SerializeField] private int successesPerDay = 5;

    public int Day { get; private set; } = 1;
    public int SuccessStreak { get; private set; } = 0;
    public int FailStreak { get; private set; } = 0;
    public float Rating { get; private set; } = 0f;

    public int OrdersToday { get; private set; } = 0;
    public int SuccessToday { get; private set; } = 0;

    // 얻은 점수/잃은 점수 누적 변수 (하루 기준)
    private float earnedScore = 0f;
    private float lostScore = 0f;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        string gameMode = PlayerPrefs.GetString("GameMode", "NewGame");

        if (gameMode == "Continue")
        {
            LoadData();
            RefreshUI();
        }
        else if (gameMode == "NewGame")
        {
            ResetData();
        }

        // 결과 패널 처음엔 꺼둠
        if (blockerPanel != null) blockerPanel.SetActive(false);
        if (resultPanel != null) resultPanel.SetActive(false);
    }

    private void Start()
    {
        if (ReceipeManager.Instance != null)
        {
            ReceipeManager.Instance.UnlockRecipesByDay(this.Day);
        }
        else
        {
            Debug.LogWarning("Start에서 ReceipeManager.Instance가 null입니다. 씬에 ReceipeManager가 있는지 확인하세요.");
            RefreshUI();
        }
    }

    public void MakeSuccess()
    {
        SuccessStreak++;
        FailStreak = 0;
        OrdersToday++;
        SuccessToday++;
        IncreaseRating();

        // 점수 기록 (점수 계산 방식은 필요시 조정)
        earnedScore += CalculateEarnedScore();

        SaveProgress();
        CheckAndEndDay();

        LogInfo($"Success {SuccessStreak}/{successesPerDay}");
        RefreshUI();
    }

    public void MakeFail()
    {
        FailStreak++;
        SuccessStreak = 0;
        OrdersToday++;
        DecreaseRating();

        // 잃은 점수 기록
        lostScore += CalculateLostScore();

        SaveProgress();
        CheckAndEndDay();

        LogInfo("Fail!");
        RefreshUI();
    }

    private float CalculateEarnedScore()
    {
        // 성공 시 얻을 점수 계산(예시)
        if (SuccessStreak >= 9) return 1f;
        else if (SuccessStreak >= 4) return 0.3f;
        else return 0.1f;
    }

    private float CalculateLostScore()
    {
        // 실패 시 잃을 점수 계산(예시)
        if (FailStreak >= 8) return 1.2f;
        else if (FailStreak >= 4) return 0.5f;
        else return 0.2f;
    }

    private void IncreaseRating()
    {
        Rating += CalculateEarnedScore();
    }

    private void DecreaseRating()
    {
        Rating -= CalculateLostScore();

        if (Rating < 0) Rating = 0f;
    }

    private void CheckAndEndDay()
    {
        //bool canEndDay = (OrdersToday >= 10 && SuccessToday >= 5);
        bool canEndDay = (SuccessToday >= 5);
        Debug.Log($"[CheckAndEndDay] OrdersToday={OrdersToday}, SuccessToday={SuccessToday}, canEndDay={canEndDay}");

        if (canEndDay)
        {
            ShowResultPanel();
        }
    }

    //🔽 [여기서 패널 표시]
    private void ShowResultPanel()
    {
        if (blockerPanel != null) blockerPanel.SetActive(true);
        if (resultPanel != null)
        {
            resultPanel.SetActive(true);

            if (earnedScoreText != null)
                earnedScoreText.text = $"얻은 점수: {earnedScore:F2}";
            if (lostScoreText != null)
                lostScoreText.text = $"잃은 점수: {lostScore:F2}";
            if (successCountText != null)
                successCountText.text = $"성공 횟수: {SuccessToday}";
            if (failCountText != null)
                failCountText.text = $"실패 횟수: {OrdersToday - SuccessToday}";

            if (startNextDayButton != null)
            {
                startNextDayButton.onClick.RemoveAllListeners();
                startNextDayButton.onClick.AddListener(() => StartNextDay());
            }
        }
    }

    // 스타트 버튼이 다음날로 넘기는 함수
    public void StartNextDay()
    {
        if (blockerPanel != null) blockerPanel.SetActive(false);
        if (resultPanel != null)
            resultPanel.SetActive(false);

        // [1] 당일 종료/저장
        EndDayAndSave();

        // [2] 일차별 레시피 해금 - 여기서 호출!
        ReceipeManager.Instance.UnlockRecipesByDay(Day);

        // 얻은 점수/잃은 점수도 초기화
        earnedScore = 0f;
        lostScore = 0f;

        RefreshUI();
    }


    private void EndDayAndSave()
    {
        OrdersToday = 0;
        SuccessToday = 0;
        SuccessStreak = 0;
        FailStreak = 0;
        Day++;

        SaveData();
        ReceipeManager.Instance.SaveUnlockedRecipes();

        RefreshUI();
        LogInfo($"Day Up! → Day {Day}");
    }

    private void RefreshUI()
    {
        if (dayText != null) dayText.text = $"Day {Day}";
        if (ratingText != null) ratingText.text = $"Rating: {Rating:F2}";
    }

    private void LogInfo(string msg)
    {
        if (infoText != null) infoText.text = msg;
        Debug.Log(msg);
    }

    public void SaveProgress()
    {
        SaveData data = new SaveData
        {
            Day = this.Day,
            Rating = this.Rating,
            SuccessStreak = this.SuccessStreak,
            FailStreak = this.FailStreak,
            OrdersToday = this.OrdersToday,
            SuccessToday = this.SuccessToday
        };
        SaveSystem.Save(data);
    }

    public void SaveData()
    {
        SaveData data = new SaveData
        {
            Day = this.Day,
            Rating = this.Rating,
            SuccessStreak = this.SuccessStreak,
            FailStreak = this.FailStreak,
            OrdersToday = this.OrdersToday,
            SuccessToday = this.SuccessToday
        };
        SaveSystem.Save(data);
    }

    public void LoadData()
    {
        SaveData data = SaveSystem.Load();
        if (data == null)
        {
            Debug.LogWarning("세이브 데이터가 없어서 NewGame처럼 시작합니다.");
            ResetData();
            return;
        }
        this.Day = data.Day;
        this.Rating = data.Rating;
        this.SuccessStreak = data.SuccessStreak;
        this.FailStreak = data.FailStreak;
        this.OrdersToday = data.OrdersToday;                
        this.SuccessToday = data.SuccessToday;
        if (ReceipeManager.Instance != null)
        {
            ReceipeManager.Instance.UnlockRecipesByDay(this.Day);
        }
        else
        {
            Debug.LogWarning("ReceipeManager.Instance가 아직 null입니다. Start에서 다시 맞춰줍니다.");
        }
    }

    public void ResetData()
    {
        SaveSystem.DeleteSave();
        Day = 1;
        Rating = 0f;
        SuccessStreak = 0;
        FailStreak = 0;
        OrdersToday = 0;
        SuccessToday = 0;
    
        SaveData();
        Debug.Log("🔄 새 게임 시작 → 데이터 초기화");
    }
}