using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIScreenFade : MonoBehaviour
{
    [SerializeField] Image _fadeImage;
    [SerializeField] float _fadeSpeed;

    public static UIScreenFade instance;

    bool _fadingToBlack;
    bool _fadingFromBlack;

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
        if(mode != LoadSceneMode.Additive)
        {
            if(_fadeImage.color.a == 1)
            {
                FadeFromBlack();
            }
        }
    }


    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_fadingToBlack)
        {
            _fadeImage.color = new Color(_fadeImage.color.r, _fadeImage.color.g, _fadeImage.color.b, Mathf.MoveTowards(_fadeImage.color.a, 1, _fadeSpeed * GamePause.deltaTime));

            if(_fadeImage.color.a == 1)
            {
                _fadingToBlack = false; 
            }

        }

        if (_fadingFromBlack)
        {
            _fadeImage.color = new Color(_fadeImage.color.r, _fadeImage.color.g, _fadeImage.color.b, Mathf.MoveTowards(_fadeImage.color.a, 0, _fadeSpeed * GamePause.deltaTime));

            if (_fadeImage.color.a == 0)
            {
                _fadingFromBlack = false;
            }
        }
    }

    public void FadeToBlack()
    {
        _fadingToBlack = true;
        _fadingFromBlack = false;
    }

    public void FadeFromBlack()
    {
        _fadingFromBlack = true;
        _fadingToBlack = false;
    }
}
