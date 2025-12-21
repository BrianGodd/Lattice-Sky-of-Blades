using UnityEngine;
using System;
using System.Collections.Generic;

[System.Serializable]
public class LevelCompletionData
{
    public string levelName;
    public bool isCompleted;
    public float completionTime;
    public DateTime completionDate;
    public int attempts;
    public bool isReward;
    
    public LevelCompletionData(string name)
    {
        levelName = name;
        isCompleted = false;
        completionTime = 0f;
        completionDate = DateTime.MinValue;
        attempts = 0;
        isReward = false;
    }
}

public class LevelCompletionManager : MonoBehaviour
{
    public static LevelCompletionManager Instance { get; private set; }
    
    public event Action<string, float> OnLevelCompleted;
    
    private Dictionary<string, LevelCompletionData> _levelData = new Dictionary<string, LevelCompletionData>();
    private const string SAVE_KEY_PREFIX = "LevelCompletion_";
    
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        LoadAllData();
    }
    
    /// <summary>
    /// Mark a level as completed with the given completion time
    /// </summary>
    public void CompleteLevel(string levelName, float completionTime)
    {
        if (!_levelData.ContainsKey(levelName))
        {
            _levelData[levelName] = new LevelCompletionData(levelName);
        }
        
        var data = _levelData[levelName];
        data.isCompleted = true;
        data.completionTime = completionTime;
        data.completionDate = DateTime.Now;
        
        SaveLevelData(levelName);
        OnLevelCompleted?.Invoke(levelName, completionTime);
        
        Debug.Log($"Level '{levelName}' completed in {completionTime:F2} seconds");
    }

    public void RewardLevel(string levelName)
    {
        if (!_levelData.ContainsKey(levelName))
        {
            _levelData[levelName] = new LevelCompletionData(levelName);
        }
        
        var data = _levelData[levelName];
        data.isReward = true;
        
        SaveLevelData(levelName);
        //OnLevelRewarded?.Invoke(levelName);
        
        Debug.Log($"Level '{levelName}' Rewarded");
    }
    
    /// <summary>
    /// Record a level attempt
    /// </summary>
    public void RecordAttempt(string levelName)
    {
        if (!_levelData.ContainsKey(levelName))
        {
            _levelData[levelName] = new LevelCompletionData(levelName);
        }
        
        _levelData[levelName].attempts++;
        SaveLevelData(levelName);
    }
    
    /// <summary>
    /// Check if a level has been completed
    /// </summary>
    public bool IsLevelCompleted(string levelName)
    {
        return _levelData.ContainsKey(levelName) && _levelData[levelName].isCompleted;
    }

    public bool IsLevelRewarded(string levelName)
    {
        return _levelData.ContainsKey(levelName) && _levelData[levelName].isReward;
    }
    
    /// <summary>
    /// Get completion time for a level
    /// </summary>
    public float GetCompletionTime(string levelName)
    {
        if (_levelData.ContainsKey(levelName))
        {
            return _levelData[levelName].completionTime;
        }
        return 0f;
    }
    
    /// <summary>
    /// Get number of attempts for a level
    /// </summary>
    public int GetAttempts(string levelName)
    {
        if (_levelData.ContainsKey(levelName))
        {
            return _levelData[levelName].attempts;
        }
        return 0;
    }
    
    /// <summary>
    /// Get completion date for a level
    /// </summary>
    public DateTime GetCompletionDate(string levelName)
    {
        if (_levelData.ContainsKey(levelName))
        {
            return _levelData[levelName].completionDate;
        }
        return DateTime.MinValue;
    }
    
    /// <summary>
    /// Get all completed levels
    /// </summary>
    public List<string> GetCompletedLevels()
    {
        var completedLevels = new List<string>();
        foreach (var kvp in _levelData)
        {
            if (kvp.Value.isCompleted)
            {
                completedLevels.Add(kvp.Key);
            }
        }
        return completedLevels;
    }
    
    /// <summary>
    /// Get total number of completed levels
    /// </summary>
    public int GetCompletedLevelCount()
    {
        int count = 0;
        foreach (var data in _levelData.Values)
        {
            if (data.isCompleted) count++;
        }
        return count;
    }
    
    /// <summary>
    /// Reset a specific level's completion data
    /// </summary>
    public void ResetLevel(string levelName)
    {
        if (_levelData.ContainsKey(levelName))
        {
            _levelData[levelName] = new LevelCompletionData(levelName);
            SaveLevelData(levelName);
            Debug.Log($"Reset data for level '{levelName}'");
        }
    }
    
    /// <summary>
    /// Reset all level completion data
    /// </summary>
    public void ResetAllLevels()
    {
        foreach (var levelName in new List<string>(_levelData.Keys))
        {
            PlayerPrefs.DeleteKey(SAVE_KEY_PREFIX + levelName);
        }
        _levelData.Clear();
        PlayerPrefs.Save();
        Debug.Log("All level data reset");
    }
    
    private void SaveLevelData(string levelName)
    {
        if (_levelData.ContainsKey(levelName))
        {
            var json = JsonUtility.ToJson(_levelData[levelName]);
            PlayerPrefs.SetString(SAVE_KEY_PREFIX + levelName, json);
            PlayerPrefs.Save();
        }
    }
    
    private void LoadLevelData(string levelName)
    {
        string key = SAVE_KEY_PREFIX + levelName;
        if (PlayerPrefs.HasKey(key))
        {
            var json = PlayerPrefs.GetString(key);
            var data = JsonUtility.FromJson<LevelCompletionData>(json);
            _levelData[levelName] = data;
        }
    }
    
    private void LoadAllData()
    {
        // Note: PlayerPrefs doesn't have a way to enumerate all keys
        // You might want to maintain a list of level names elsewhere
        // For now, this will load data when levels are accessed
    }
    
    /// <summary>
    /// Print all completion data for debugging
    /// </summary>
    public void PrintAllData()
    {
        Debug.Log("=== Level Completion Data ===");
        foreach (var kvp in _levelData)
        {
            var data = kvp.Value;
            Debug.Log($"Level: {data.levelName}");
            Debug.Log($"  Completed: {data.isCompleted}");
            Debug.Log($"  Time: {data.completionTime:F2}s");
            Debug.Log($"  Attempts: {data.attempts}");
            Debug.Log($"  Date: {data.completionDate}");
        }
    }
}
