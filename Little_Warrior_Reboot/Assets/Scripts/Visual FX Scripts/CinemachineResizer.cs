using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineResizer : MonoBehaviour
{
    CinemachineVirtualCamera _cineCam;
    [SerializeField] float _newSize;
    [SerializeField] bool _movesCameraForward;
    [SerializeField] bool _movesCameraBackward;
    [SerializeField] float _newScreenX;

    float _currSmoothVelo;
    float _screenPosSmoothVelo;
    // Start is called before the first frame update
    void Start()
    {
        _cineCam = FindObjectOfType<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CinemachineResizer[] resizers = FindObjectsOfType<CinemachineResizer>();

            foreach(CinemachineResizer resizer in resizers)
            {
                resizer.StopCameraCoroutines();
            }

            StartCoroutine(ResizeCineCamOrthoSizeCo());

        }
    }

    public void StopCameraCoroutines()
    {
        StopCoroutine(ResizeCineCamOrthoSizeCo());
    }

    //To do: replace current resizeorthosizeco with a version that sets to a specific size instead of a general increase/decrease
    IEnumerator ResizeCineCamOrthoSizeCo()
    {

        float startScreenX = _cineCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX;
        float startScreenSize = _cineCam.m_Lens.OrthographicSize;
        float timePercentage;

        if (_newSize < _cineCam.m_Lens.OrthographicSize)
        {
            Debug.Log("DECREASING CAMERA SIZE");
            timePercentage = 1;
            //while (_cineCam.m_Lens.OrthographicSize > _newSize || (_movesCameraBackward && _cineCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX < _newScreenX))
            while(timePercentage > 0)
            {
                //_cineCam.m_Lens.OrthographicSize = Mathf.SmoothDamp(_cineCam.m_Lens.OrthographicSize, _newSize, ref _currSmoothVelo, 2f);
                _cineCam.m_Lens.OrthographicSize = Mathf.Lerp(_newSize, startScreenSize, timePercentage);

                if (_movesCameraBackward)
                {
                    _cineCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX = Mathf.SmoothDamp(_cineCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX, _newScreenX, ref _screenPosSmoothVelo, 2f);
                }
                timePercentage -= GamePause.deltaTime / 2f;
                yield return null;
            }
        }
        else
        {
            Debug.Log("INCREASING CAMERA SIZE");
            timePercentage = 0;
            //while(_cineCam.m_Lens.OrthographicSize < _newSize || (_movesCameraForward && _cineCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX > _newScreenX))
            while(timePercentage < 1)
            {
                //_cineCam.m_Lens.OrthographicSize = Mathf.SmoothDamp(_cineCam.m_Lens.OrthographicSize, _newSize, ref _currSmoothVelo, 2f);
                _cineCam.m_Lens.OrthographicSize = Mathf.Lerp(startScreenSize, _newSize, timePercentage);

                if (_movesCameraForward)
                {
                    _cineCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX = Mathf.SmoothDamp(_cineCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX, _newScreenX, ref _screenPosSmoothVelo, 2f);
                }

                timePercentage += GamePause.deltaTime / 2f;

                Debug.Log(timePercentage);

                yield return null;
            }
        }

        _cineCam.m_Lens.OrthographicSize = Mathf.Round(_cineCam.m_Lens.OrthographicSize);
    }

    IEnumerator ResizeCamCo()
    {
        yield return null;
    }

}
