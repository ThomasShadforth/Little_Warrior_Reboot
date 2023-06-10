using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, IDataPersistence
{
    public static GameManager instance;

    [SerializeField] string _currentScene;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name != "Main_Menu")
        {
            StartCoroutine(_SetCurrentLevelCo());
        }
    }

    public void SaveData(GameData data)
    {
        data.lastSavedScene = SceneManager.GetActiveScene().name;
    }

    public void LoadData(GameData data)
    {

    }
    

    public void LoadLevel(string levelToLoad)
    {
        StartCoroutine(LoadLevelCo(levelToLoad));
    }

    public string GetCurrentScene()
    {
        return _currentScene;
    }

    public string GetSceneManagerScene()
    {
        return SceneManager.GetActiveScene().name;
    }

    IEnumerator _SetCurrentLevelCo()
    {
        yield return new WaitForSeconds(.5f);
        _currentScene = SceneManager.GetActiveScene().name;
    }

    IEnumerator LoadLevelCo(string levelToLoad)
    {
        if(UIScreenFade.instance != null)
        {
            UIScreenFade.instance.FadeToBlack();
        }

        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadSceneAsync(levelToLoad);
    }
}
