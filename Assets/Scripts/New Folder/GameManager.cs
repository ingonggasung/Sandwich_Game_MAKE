using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI dayText;
    [SerializeField] private TextMeshProUGUI infoText;
    [SerializeField] private TextMeshProUGUI moneyText;

    [Header("Rules")]
    [SerializeField] private int successesPerDay = 5;
    [SerializeField] private int moneyPerClick = 500;

    public int Day { get; private set; } = 1;
    public int SuccessStreak { get; private set; } = 0;
    public int Money { get; private set; } = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        string gameMode = PlayerPrefs.GetString("GameMode", "NewGame");

        if (gameMode == "Continue")
        {
            LoadData();
        }
        else if (gameMode == "NewGame")
        {
            ResetData();
        }
    }

    private void Start()
    {
        RefreshUI();
    }

    public void OnCubeSuccess()
    {
        SuccessStreak++;
        Money += moneyPerClick;

        SaveProgress(); // 성공 시 최신 저장

        LogInfo($"Success {SuccessStreak}/{successesPerDay}");

        if (SuccessStreak >= successesPerDay)
        {
            SuccessStreak = 0;
            Day++;
            LogInfo($"Day Up! → Day {Day}");

            SaveData(); // 하루 끝날 때 전체 저장
        }

        RefreshUI();
    }

    public void OnCubeFail()
    {
        LogInfo("Fail!");
        SaveProgress(); // 실패도 저장 (선택)
        RefreshUI();
    }

    private void RefreshUI()
    {
        if (dayText != null) dayText.text = $"Day {Day}";
        if (moneyText != null) moneyText.text = $"${Money}";
    }

    private void LogInfo(string msg)
    {
        if (infoText != null) infoText.text = msg;
        Debug.Log(msg);
    }

    // 진행 상황 저장 (Money + SuccessStreak)
    public void SaveProgress()
    {
        SaveData data = new SaveData
        {
            Day = this.Day,
            Money = this.Money,
            SuccessStreak = this.SuccessStreak
        };
        SaveSystem.Save(data);
    }

    // 하루가 끝날 때 전체 저장
    public void SaveData()
    {
        SaveData data = new SaveData
        {
            Day = this.Day,
            Money = this.Money,
            SuccessStreak = this.SuccessStreak
        };
        SaveSystem.Save(data);
    }

    // 불러오기
    public void LoadData()
    {
        SaveData data = SaveSystem.Load();
        this.Day = data.Day;
        this.Money = data.Money;
        this.SuccessStreak = data.SuccessStreak;
    }

    // 새 게임 시작 (기존 저장 삭제)
    public void ResetData()
    {
        SaveSystem.DeleteSave();
        Day = 1;
        Money = 0;
        SuccessStreak = 0;
        SaveData(); // 초기 데이터 저장
        Debug.Log("🔄 새 게임 시작 → 데이터 초기화");
    }
}
