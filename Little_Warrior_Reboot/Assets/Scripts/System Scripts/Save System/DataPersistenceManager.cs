using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("Debugging:")]
    [SerializeField] private bool _initializeDataIfNull = false;
    [SerializeField] private bool _disableDataPersistence = false;
    [SerializeField] private bool _overrideSelectedProfileId = false;
    [SerializeField] private string _testOverrideId = "TestBuild";

    [Header("File Storage Config:")]
    [SerializeField] private string _fileName;
    [SerializeField] private bool _useEncryption;

    [Header("Auto Save Config:")]
    [SerializeField] private float _autoSaveTimeSeconds = 60f;

    private GameData _gameData;
    private List<IDataPersistence> _dataPersistenceObjects;
    private FileDataHandler _dataHandler;

    private string _selectedProfileId = "test";

    private Coroutine _autoSaveCoroutine;

    public static DataPersistenceManager instance { get; private set; }

    bool _isQuitting;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        if (_disableDataPersistence)
        {
            Debug.LogWarning("Data Persistence is disabled!");
        }

        this._dataHandler = new FileDataHandler(Application.persistentDataPath, _fileName, _useEncryption);

        _InitializeSelectedProfileId();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
       
            SceneManager.sceneLoaded -= OnSceneLoaded;
        
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        this._dataPersistenceObjects = FindAllDataPersistenceObjects();

        LoadGame();

        if(SceneManager.GetActiveScene().name != "Main_Menu")
        {
            if(_autoSaveCoroutine != null)
            {
                StopCoroutine(_autoSaveCoroutine);
            }

            _autoSaveCoroutine = StartCoroutine(_AutoSave());
        }
    }

    public void ChangeSelectedProfileID(string newProfileId)
    {
        this._selectedProfileId = newProfileId;
        LoadGame();
    }

    public void DeleteProfileData(string profileId)
    {
        _dataHandler.Delete(profileId);

        _InitializeSelectedProfileId();

        LoadGame();
    }

    void _InitializeSelectedProfileId()
    {
        this._selectedProfileId = _dataHandler.GetMostRecentProfile();

        if (_overrideSelectedProfileId)
        {
            this._selectedProfileId = _testOverrideId;
        }
    }

    public void NewGame()
    {
        this._gameData = new GameData();
    }

    public void SaveGame()
    {

        if (_disableDataPersistence)
        {
            return;
        }

        if(this._gameData == null)
        {
            Debug.LogWarning("No data was found. A new game needs to be tarted before data can be saved");
            return;
        }

        foreach(IDataPersistence dataPersistenceObj in _dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(_gameData);
        }

        this._gameData.lastSceneSaved = SceneManager.GetActiveScene().name;

        _gameData.lastSaved = System.DateTime.Now.ToBinary();

        _dataHandler.Save(_gameData, _selectedProfileId);
    }

    public void LoadGame()
    {
        if (_disableDataPersistence)
        {
            return;
        }

        this._gameData = _dataHandler.Load(_selectedProfileId);

        if(this._gameData == null && _initializeDataIfNull)
        {
            NewGame();
        }

        if(this._gameData == null)
        {
            return;
        }

        foreach(IDataPersistence dataPersistenceObj in _dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(_gameData);
        }

        //Debug.Log("Loaded");
    }

    public string GetLastScene()
    {
        return _dataHandler.GetLastSavedScene(_selectedProfileId);
    }

    public string GetSelectedID()
    {
        return _selectedProfileId;
    }

    List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>(true).OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    public bool HasGameData()
    {
        return _gameData != null;
    }

    public Dictionary<string, GameData> GetAllProfilesGameData()
    {
        return _dataHandler.LoadAllProfiles();
    }

    private IEnumerator _AutoSave()
    {
        while (true)
        {
            yield return new WaitForSeconds(_autoSaveTimeSeconds);
            SaveGame();

        }
    }

    private void OnApplicationQuit()
    {
        if (!_isQuitting)
        {
            Debug.Log(_isQuitting);
            _isQuitting = true;
            if (SceneManager.GetActiveScene().name != "Main_Menu")
            {
                SaveGame();

            }

            
        }
    }
}
