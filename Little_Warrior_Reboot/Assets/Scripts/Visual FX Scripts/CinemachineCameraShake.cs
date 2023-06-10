using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public static class CinemachineCameraShake
{
    public static IEnumerator StartCamShakeCo(float camAmpGain, float shakeDuration, CinemachineVirtualCamera cineCam)
    {
        CinemachineBasicMultiChannelPerlin perlinNoise = cineCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        perlinNoise.m_AmplitudeGain = camAmpGain;
        yield return new WaitForSeconds(shakeDuration);
        perlinNoise.m_AmplitudeGain = 0.0f;
    }
}
