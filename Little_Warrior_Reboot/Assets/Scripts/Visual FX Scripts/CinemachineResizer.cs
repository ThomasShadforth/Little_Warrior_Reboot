using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineResizer : MonoBehaviour
{
    CinemachineVirtualCamera _cineCam;
    [SerializeField] float _resizeAmount;
    [SerializeField] float _resizeTime;
    float _currSmoothVelo;

    // Start is called before the first frame update
    void Start()
    {
        _cineCam = FindObjectOfType<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator ResizeOrthoSizeCo()
    {
        yield return new WaitForSeconds(5f);
        float targetOrthoSize = _cineCam.m_Lens.OrthographicSize + _resizeAmount;

        while(_cineCam.m_Lens.OrthographicSize < targetOrthoSize)
        {

            //cineCam.m_Lens.OrthographicSize = Mathf.Lerp(cineCam.m_Lens.OrthographicSize, targetOrthoSize, timePercentage);
            _cineCam.m_Lens.OrthographicSize = Mathf.SmoothDamp(_cineCam.m_Lens.OrthographicSize, targetOrthoSize, ref _currSmoothVelo, 300f * GamePause.deltaTime);
            yield return null;
        }
    }
}
