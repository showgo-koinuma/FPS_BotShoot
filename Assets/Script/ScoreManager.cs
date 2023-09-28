using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class ScoreManager : MonoBehaviour
{
    Ranking _ranking;
    static ScoreManager _instance;
    public static ScoreManager Instance => _instance;

    [Serializable]
    class Ranking
    {
        public List<Data> _ranking;
    }

    [Serializable]
    struct Data
    {
        public string _name;
        public int _score;
    }

    private void Awake()
    {
        if (!Instance) _instance = this;
        _ranking = new Ranking();
        Data data = new Data();
        data._name = "anonymous";
        data._score = 0;
        _ranking._ranking = new List<Data> { data, data, data, data, data};
        LoadRanking();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        if (scene.name == "Lobby") // ロビーがロードされたら_scoreTextにスコアを表示する
        {
            GameObject rankingObj = GameObject.Find("RankingText");
            string rankingScoreString = "";
            for (int i = 0; i < _ranking._ranking.Count; i++)
            {
                rankingScoreString += $"{_ranking._ranking[i]._score}\r\n";
            }
            rankingObj.GetComponent<TextMeshProUGUI>().text = rankingScoreString;
            rankingObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = rankingScoreString;
            Debug.Log(rankingScoreString);
        }
    }

    /// <summary>新しいスコアをセットする</summary>
    public void SetScore(int newScore)
    {
        Data data = new Data();
        data._name = "anonymous";
        data._score = newScore;
        _ranking._ranking.Add(data);
        _ranking._ranking.Sort((a, b) => b._score - a._score);
        if (_ranking._ranking.Count > 5)
        {
            Debug.Log(_ranking._ranking[5]._score + "を削除した");
            _ranking._ranking.Remove(_ranking._ranking[5]);
        }
    }

    public void SaveRanking()
    {
        string jsonData = JsonUtility.ToJson(_ranking);
        StreamWriter writer = new StreamWriter(Application.persistentDataPath + "/.savedata.json");
        writer.Write(jsonData);
        writer.Flush();
        writer.Close();
        Debug.Log(jsonData);
    }

    void LoadRanking()
    {
        if (File.Exists(Application.persistentDataPath + "/.savedata.json"))
        {
            StreamReader streamReader = new StreamReader(Application.persistentDataPath + "/.savedata.json");
            string jsonData = streamReader.ReadToEnd();
            streamReader.Close();
            Ranking checkData = JsonUtility.FromJson<Ranking>(jsonData);
            if (checkData._ranking != null)
            {
                _ranking = checkData;
            }
        }
    }

    void ResetRanking()
    {
        //Ranking._ranking = new Dictionary<string, int> { { "anonymous", 0 }, { "anonymous", 0 }, { "anonymous", 0 }, { "anonymous", 0 }, { "anonymous", 0 } };
    }
}
