using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("Debugging")]
    [SerializeField] private bool _disableDataPersistence = false;
    [SerializeField] private bool _initializeDataIfNull = false;
    [SerializeField] private bool _overrideSelectedProfileId = false;
    [SerializeField] private string _testSelectedProfileId = "test";

    [Header("File Storage Config")]
    [SerializeField] private string _fileName;
    [SerializeField] private bool _useEncryption;

    [Header("Auto Save Config")]
    [SerializeField] private float _autoSaveTimeSeconds = 60f;

    private GameData _gameData;
    private List<IDataPersistence> _dataPersistenceObjects;
    private FileDataHandler _dataHandler;

    private string _selectedProfileId = "";

    private Coroutine _autoSaveCoroutine;

    public static DataPersistenceManager instance { get; private set; }

    private void Awake() 
    {
        if (instance != null) 
        {
            Debug.Log("Found more than one Data Persistence Manager in the scene. Destroying the newest one.");
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);

        if (_disableDataPersistence) 
        {
            Debug.LogWarning("Data Persistence is currently disabled!");
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

            _autoSaveCoroutine = StartCoroutine(_AutoSaveCo());
        }
    }

    
    public void ChangeSelectedProfileId(string newProfileId) 
    {
        // update the profile to use for saving and loading
        this._selectedProfileId = newProfileId;
        // load the game, which will use that profile, updating our game data accordingly
        LoadGame();
    }

    public void DeleteProfileData(string profileId)
    {
        //To do: add delete method to dataHandler script
        _dataHandler.Delete(profileId);

        _InitializeSelectedProfileId();
        LoadGame();
    }

    public void NewGame() 
    {
        this._gameData = new GameData();
    }

    public void LoadGame()
    {
        // return right away if data persistence is disabled
        if (_disableDataPersistence) 
        {
            return;
        }

        // load any saved data from a file using the data handler
        this._gameData = _dataHandler.Load(_selectedProfileId);

        // start a new game if the data is null and we're configured to initialize data for debugging purposes
        if (this._gameData == null && _initializeDataIfNull) 
        {
            NewGame();
        }

        // if no data can be loaded, don't continue
        if (this._gameData == null) 
        {
            Debug.Log("No data was found. A New Game needs to be started before data can be loaded.");
            return;
        }

        // push the loaded data to all other scripts that need it
        foreach (IDataPersistence dataPersistenceObj in _dataPersistenceObjects) 
        {
            dataPersistenceObj.LoadData(_gameData);
        }
    }

    public void SaveGame()
    {
        // return right away if data persistence is disabled
        if (_disableDataPersistence) 
        {
            return;
        }

        // if we don't have any data to save, log a warning here
        if (this._gameData == null) 
        {
            Debug.LogWarning("No data was found. A New Game needs to be started before data can be saved.");
            return;
        }

        // pass the data to other scripts so they can update it
        foreach (IDataPersistence dataPersistenceObj in _dataPersistenceObjects) 
        {
            dataPersistenceObj.SaveData(_gameData);
        }

        // timestamp the data so we know when it was last saved
        _gameData.lastUpdated = System.DateTime.Now.ToBinary();

        // save that data to a file using the data handler
        _dataHandler.Save(_gameData, _selectedProfileId);
    }

    void _InitializeSelectedProfileId()
    {
        this._selectedProfileId = _dataHandler.GetMostRecentlyUpdatedProfileId();

        if (_overrideSelectedProfileId)
        {
            this._selectedProfileId = _testSelectedProfileId;
            Debug.LogWarning("SELECTED ID HAS BEEN OVERRIDDEN!");
        }
    }

    private void OnApplicationQuit() 
    {
        if (SceneManager.GetActiveScene().name != "Main_Menu")
        {
            SaveGame();
        }
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects() 
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>()
            .OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    public string GetProfileLastSavedScene()
    {
        return _dataHandler.GetProfileLastSavedScene(_selectedProfileId);
    }

    public bool GetDisabledDataPersistence()
    {
        return _disableDataPersistence;
    }

    public bool HasGameData() 
    {
        return _gameData != null;
    }

    public Dictionary<string, GameData> GetAllProfilesGameData() 
    {
        return _dataHandler.LoadAllProfiles();
    }

    IEnumerator _AutoSaveCo()
    {
        while (true)
        {
            yield return new WaitForSeconds(_autoSaveTimeSeconds);
            SaveGame();
            Debug.Log("Auto Saved!");
        }
    }
}
