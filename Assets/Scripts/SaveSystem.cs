using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    private const string dayKey = "Save_Day";
    private const string ratingKey = "Save_Rating";
    private const string successStreakKey = "Save_SuccessStreak";
    private const string failStreakKey = "Save_FailStreak";
    private const string ordersTodayKey = "Save_OrdersToday";
    private const string successTodayKey = "Save_SuccessToday";

    public static void Save(SaveData data)
    {
        PlayerPrefs.SetInt(dayKey, data.Day);
        PlayerPrefs.SetFloat(ratingKey, data.Rating);
        PlayerPrefs.SetInt(successStreakKey, data.SuccessStreak);
        PlayerPrefs.SetInt(failStreakKey, data.FailStreak);
        PlayerPrefs.SetInt(ordersTodayKey, data.OrdersToday);
        PlayerPrefs.SetInt(successTodayKey, data.SuccessToday);

        PlayerPrefs.Save();

        Debug.Log("💾 PlayerPrefs 저장 완료");
    }

    public static SaveData Load()
    {
        SaveData data = new SaveData();

        data.Day = PlayerPrefs.GetInt(dayKey, 1);
        data.Rating = PlayerPrefs.GetFloat(ratingKey, 0f);
        data.SuccessStreak = PlayerPrefs.GetInt(successStreakKey, 0);
        data.FailStreak = PlayerPrefs.GetInt(failStreakKey, 0);
        data.OrdersToday = PlayerPrefs.GetInt(ordersTodayKey, 0);
        data.SuccessToday = PlayerPrefs.GetInt(successTodayKey, 0);

        Debug.Log("📂 PlayerPrefs 불러오기 완료");

        return data;
    }

    public static void DeleteSave()
    {
        PlayerPrefs.DeleteKey(dayKey);
        PlayerPrefs.DeleteKey(ratingKey);
        PlayerPrefs.DeleteKey(successStreakKey);
        PlayerPrefs.DeleteKey(failStreakKey);
        PlayerPrefs.DeleteKey(ordersTodayKey);
        PlayerPrefs.DeleteKey(successTodayKey);

        PlayerPrefs.Save();

        Debug.Log("🗑 PlayerPrefs 저장 데이터 삭제 완료");
    }
}
