using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI dayText;
    [SerializeField] private TextMeshProUGUI infoText;
    [SerializeField] private TextMeshProUGUI ratingText;

    [Header("Rules")]
    [SerializeField] private int successesPerDay = 5;

    public int Day { get; private set; } = 1;
    public int SuccessStreak { get; private set; } = 0;
    public int FailStreak { get; private set; } = 0;
    public float Rating { get; private set; } = 0f;

    public int OrdersToday { get; private set; } = 0;
    public int SuccessToday { get; private set; } = 0;

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
            //ResetData();
        }
    }

    private void Start()
    {
        RefreshUI();
    }

    public void MakeSuccess()
    {
        SuccessStreak++;
        FailStreak = 0;
        OrdersToday++;
        SuccessToday++;
        IncreaseRating();

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

        SaveProgress();
        CheckAndEndDay();

        LogInfo("Fail!");
        RefreshUI();
    }

    private void IncreaseRating()
    {
        if (SuccessStreak >= 9) Rating += 1f;
        else if (SuccessStreak >= 4) Rating += 0.3f;
        else Rating += 0.1f;
    }

    private void DecreaseRating()
    {
        if (FailStreak >= 8) Rating -= 1.2f;
        else if (FailStreak >= 4) Rating -= 0.5f;
        else Rating -= 0.2f;

        if (Rating < 0) Rating = 0f;
    }

    private void CheckAndEndDay()
    {
        bool canEndDay = (OrdersToday >= 10 && SuccessToday >= 5);
        Debug.Log($"[CheckAndEndDay] OrdersToday={OrdersToday}, SuccessToday={SuccessToday}, canEndDay={canEndDay}");

        if (canEndDay)
        {
            EndDayAndSave();
        }
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
        this.Day = data.Day;
        this.Rating = data.Rating;
        this.SuccessStreak = data.SuccessStreak;
        this.FailStreak = data.FailStreak;
        this.OrdersToday = data.OrdersToday;
        this.SuccessToday = data.SuccessToday;
    }

    //public void ResetData()
    //{
    //    SaveSystem.DeleteSave();
    //    Day = 1;
    //    Rating = 0f;
    //    SuccessStreak = 0;
    //    FailStreak = 0;
    //    OrdersToday = 0;
    //    SuccessToday = 0;

    //    SaveData();
    //    Debug.Log("🔄 새 게임 시작 → 데이터 초기화");
    //}
}
