using DefineHelper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    readonly string ADRESS = "https://docs.google.com/spreadsheets/d/1WZn3noPOkuyTiz1BF3oWcgOcrIWrZM7o0z9znJI1ooo";
    readonly string jsonName = "JsonData.json";
    readonly string playerName = "PlayerData.json";
    public string path = string.Empty;
    //
    [Header("Goolgole Sheet")]
    [SerializeField] string[] RANGE;
    [SerializeField] long[] SHEET_ID;

    [Header("Data List")]
    [ArrayElementTitle("lvStr")]
    public List<StatData> _StatData;
    [ArrayElementTitle("npcID")]
    public List<TalkData> _TalkData;

    [Header("Scene Type")]
    SceneType _currentScene;
    bool SuccessCheck = false;

    [Header("Loading Manager")]
    public LoadingManager _loadingmanager;

    private void Awake()
    {
        // SoundManager 인스턴스가 이미 있는지 확인, 이 상태로 설정
        if (Instance == null)
            Instance = this;

        /*// 인스턴스가 이미 있는 경우 오브젝트 제거
        else if (Instance != this)
            Destroy(gameObject);*/

        // 이렇게 하면 다음 scene으로 넘어가도 오브젝트가 사라지지 않습니다.
        DontDestroyOnLoad(gameObject);
#if UNITY_EDITOR
        path = Application.dataPath + "/8.Json";
#else
    path = Application.persistentDataPath;
#endif
        _currentScene = SceneType.LobbyScene;
        SetSoundCheck();
        SetControlCheck();
    }

    private void Start()
    {
        StartCoroutine(LoadData());
    }


    #region [Google Spread Sheet Methods]
    public static string GetTSVAdress(string adress, string range, long sheetID)
    {
        return $"{adress}/export?format=tsv&range={range}&gid={sheetID}";
    }

    private IEnumerator LoadData()
    {
        for (int datatype = 0; datatype < RANGE.Length; datatype++)
        {
            UnityWebRequest www = UnityWebRequest.Get(GetTSVAdress(ADRESS, RANGE[datatype], SHEET_ID[datatype]));
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                SuccessCheck = true;
                LoadDataSetting(datatype, www);
                Debug.Log(www.downloadHandler.text);
            }
            else
            {
                SuccessCheck = false;
                break;
            }
        }

        if (SuccessCheck)
        {
            DataSaveConvertToJson();
            LobbyManager.Singleton.DataLoadSuccess();
        }
        else
        {
            if(File.Exists(Path.Combine(path, jsonName)))
            {
                JsonLoadConvertToData();
                LobbyManager.Singleton.DataLoadBack();
            }
            else
            LobbyManager.Singleton.DataLoadFailure();
        }
    }//처음 데이터 로딩할때 필요한 코루틴

    private void LoadDataSetting(int datatype, UnityWebRequest www)//구글스프레드시트에서 값을 가져오기
    {
        switch (datatype)
        {
            case (int)DataType.StatData:
                _StatData = GetDatas<StatData>(www.downloadHandler.text);
                break;
            case (int)DataType.TalkData:
                _TalkData = GetDatas<TalkData>(www.downloadHandler.text);
                break;
        }
    }

    private void DataSaveConvertToJson()//구글 스프레드시트에서 가져온 Data Json으로 가져오기
    {
        JsonData json = new JsonData(_StatData, _TalkData);
        string jsondata = JsonUtility.ToJson(json, true);
        File.WriteAllText(Path.Combine(path, jsonName), jsondata);
    }

    private void JsonLoadConvertToData()//구글 스프레드시트에서 가져온 Json을 읽기
    {
        JsonData json = new JsonData();
        string jsondata = File.ReadAllText(Path.Combine(path, jsonName));
        json = JsonUtility.FromJson<JsonData>(jsondata);
        _StatData = json._statData;
        _TalkData = json._talkData;
    }


    public void NewPlayerDataSave(string name, StatData stat, MapType type, Vector3 position)
    {
        PlayerData playerdata = new PlayerData(name, stat, type, position);
        string jsondata = JsonUtility.ToJson(playerdata, true);
        File.WriteAllText(Path.Combine(path, playerName), jsondata);
    }

    public void PlayerDataSave(Stat stat, Inventory inven, MapType type, Vector3 position)
    {
        PlayerData playerdata = new PlayerData(stat, inven, type, position);
        string jsondata = JsonUtility.ToJson(playerdata, true);
        File.WriteAllText(Path.Combine(path, playerName), jsondata);
    }

    public PlayerData PlayerDataLoad()
    {
        PlayerData data = new PlayerData();
        string jsondata = File.ReadAllText(Path.Combine(path, playerName));
        data = JsonUtility.FromJson<PlayerData>(jsondata);
        return data;
    }

    public Dictionary<int, string[]> LoadTalkData()
    {
        Dictionary<int, string[]> talkData = new Dictionary<int, string[]>();
        List<TalkData> _talkLoad = DataManager.Instance._TalkData;
        for (int i = 0; i < _talkLoad.Count; i++)
        {
            talkData.Add(_talkLoad[i].npcID + _talkLoad[i].step, _talkLoad[i].setStrArr());
        }
        return talkData;
    }

    public bool PlayerDataCheck()
    {
        return File.Exists(Path.Combine(path, playerName));
    }
    #endregion [Google Spread Sheet Methods]

    #region [Datas Mehods]

    List<T> GetDatas<T>(string data)
    {
        List<T> returnList = new List<T>();
        string[] splitedData = data.Split('\n');

        foreach (string element in splitedData)
        {
            string[] datas = element.Split('\t');
            returnList.Add(GetData<T>(datas));
        }

        return returnList;
    }

    T GetData<T>(string[] datas)
    {
        object data = Activator.CreateInstance(typeof(T));

        // 클래스에 있는 변수들을 순서대로 저장한 배열
        FieldInfo[] fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        for (int i = 0; i < datas.Length; i++)
        {
            try
            {
                // string > parse
                Type type = fields[i].FieldType;

                if (string.IsNullOrEmpty(datas[i])) continue;

                // 변수에 맞는 자료형으로 파싱해서 넣는다
                if (type == typeof(int))
                    fields[i].SetValue(data, int.Parse(datas[i]));

                else if (type == typeof(float))
                    fields[i].SetValue(data, float.Parse(datas[i]));

                else if (type == typeof(bool))
                    fields[i].SetValue(data, bool.Parse(datas[i]));

                else if (type == typeof(string))
                    fields[i].SetValue(data, datas[i]);

                // enum
                else
                    fields[i].SetValue(data, Enum.Parse(type, datas[i]));
            }

            catch (Exception e)
            {
                Debug.LogError($"SpreadSheet Error : {e.Message}");
            }
        }

        return (T)data;
    }

    #endregion [Datas Mehods]

    #region [Scene Load Methods]

    public void LoadScene(SceneType type)
    {
        _currentScene = type;
        switch(_currentScene)
        {
            case SceneType.LobbyScene:
                DestroyObj();
                break;
            case SceneType.IngameScene:
                break;
        }
        SceneManager.LoadScene(_currentScene.ToString());
        AudioManager.Instance.SetMusic(_currentScene);
    }

    public SceneType SceneState()
    {
        return _currentScene;
    }

    #endregion [Scene Load Methods]

    #region [PlayerPrefs Methods]

    public void SetSoundCheck()
    {
        if (!PlayerPrefs.HasKey("bgmMute"))
            PlayerPrefs.SetInt("bgmMute", 0); // bool 0 = false 1 = true
        if (!PlayerPrefs.HasKey("sfxMute"))
            PlayerPrefs.SetInt("sfxMute", 0); // bool 0 = false 1 = true

        if (!PlayerPrefs.HasKey("bgmVolume"))
            PlayerPrefs.SetFloat("bgmVolume", 1f);
        if (!PlayerPrefs.HasKey("sfxVolume"))
            PlayerPrefs.SetFloat("sfxVolume", 1f);
    }

    public void SetControlCheck()
    {
        if (!PlayerPrefs.HasKey("Sensitivity"))
            PlayerPrefs.SetFloat("Sensitivity", 100f);
        if (!PlayerPrefs.HasKey("distanceMax"))
            PlayerPrefs.SetFloat("distanceMax", 10f);
    }


    #endregion [PlayerPrefs Methods]

    #region [DonDestroy Methods]

    public void DestroyObj()
    {
        Destroy(gameObject);
    }

    #endregion [DonDestroy Methods]
}
