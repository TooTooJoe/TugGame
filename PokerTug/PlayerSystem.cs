using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;


/// <summary>
/// Json To Array 多筆Json轉陣列的Helper
/// </summary>
public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}

public class PlayerSystem : MonoBehaviour
{
    [SerializeField] MainSystem mainSystem;
    [SerializeField] RoundSystem roundSystem;
    [SerializeField] UISystem uISystem;

    [Serializable]
    public class GameData
    {
        public string name;
        public string winner;
        public int endinground;
    }
    public GameData gameData;
    RoundData roundData;
    GameData[] gameRanking;
    public string json;

    // Start is called before the first frame update

    void Start()
    {
        uISystem.OnFinalResult += RefreshPlayerDataOnline;
        StartCoroutine(Download());
    }
    /// <summary>
    /// On InputField Edit End to Change Name.
    /// </summary>
    /// <param name="s"></param>
    public void WritePlayerName(string s)
    {
        gameData.name = s;
        gameData.winner = "";
        gameData.endinground = 0;
    }
    /// <summary>
    /// 刷新Player Data
    /// </summary>
    void RefreshPlayerDataOnline()
    {
        StartCoroutine(Download());
    }
    /// <summary>
    /// UpLoad後 Reset PlayerData.
    /// </summary>
    void ResetPlayerData()
    {
        gameData.winner = "";
        gameData.endinground = 0;
    }

    /// <summary>
    /// 非自設定的Json要調整for JsonHelper使用
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// If this is a Json array from the server and you did not create it by hand:    
    /// You may have to Add {"Items": in front of the received string then add } at the end of it.
    string FixJson(string value)
    {
        value = "{\"Items\":" + value + "}";
        return value;
    }


    /// <summary>
    /// 上傳遊戲結果
    /// </summary>
    /// <returns></returns>
    IEnumerator Upload()
    {
        WWWForm form = new WWWForm();
        form.AddField("method", "write");
        form.AddField("name", gameData.name);
        form.AddField("winner", gameData.winner);
        form.AddField("endinground", gameData.endinground);
        using UnityWebRequest www = UnityWebRequest.Post("https://script.google.com/macros/s/AKfycbzlvuWn3c-gnkKnwmgksDRsc2Zfcmmuo99ltD7Ga-zn6JPH5eS-uYuqbAcpWN7Q7oQ/exec", form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
#if UNITY_EDITOR
            Debug.LogError(www.error);
#endif
        }
        else
        {
            ResetPlayerData();
#if UNITY_EDITOR
            Debug.Log("Form upload complete!");
#endif
        }
    }

    /// <summary>
    /// 下載遊戲結果
    /// </summary>
    /// <returns></returns>
    IEnumerator Download()
    {
        WWWForm form = new WWWForm();
        form.AddField("method", "read");
        using UnityWebRequest www = UnityWebRequest.Post("https://script.google.com/macros/s/AKfycbzlvuWn3c-gnkKnwmgksDRsc2Zfcmmuo99ltD7Ga-zn6JPH5eS-uYuqbAcpWN7Q7oQ/exec", form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
#if UNITY_EDITOR
            Debug.LogError(www.error);
#endif
        }
        else
        {
            json = www.downloadHandler.text;
            string fixJson = FixJson(json);
            gameRanking = JsonHelper.FromJson<GameData>(fixJson);
            Debug.Log(fixJson);
            uISystem.ShowPlayersHistory(gameRanking);
#if UNITY_EDITOR
            Debug.Log("Form download complete!");
#endif
        }
    }
}

