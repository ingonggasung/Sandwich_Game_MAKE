using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string saveFile = Path.Combine(Application.persistentDataPath, "save.json");

    // 저장
    public static void Save(SaveData data)
    {
        string json = JsonUtility.ToJson(data, true); // 보기 좋게 포맷팅
        File.WriteAllText(saveFile, json);
        Debug.Log($"💾 저장 완료 → {saveFile}");
    }

    // 불러오기
    public static SaveData Load()
    {
        if (File.Exists(saveFile))
        {
            string json = File.ReadAllText(saveFile);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            Debug.Log($"📂 불러오기 완료 → {saveFile}");
            return data;
        }
        else
        {
            Debug.LogWarning("⚠ 저장 파일 없음 → 기본 데이터 반환");
            return new SaveData { Day = 1, Money = 0, SuccessStreak = 0 };
        }
    }

    // 저장 파일 삭제 (New Game용)
    public static void DeleteSave()
    {
        if (File.Exists(saveFile))
        {
            File.Delete(saveFile);
            Debug.Log("🗑 저장 파일 삭제 완료");
        }
    }
}
