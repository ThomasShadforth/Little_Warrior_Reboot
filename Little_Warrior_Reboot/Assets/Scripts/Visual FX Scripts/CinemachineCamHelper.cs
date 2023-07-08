using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public static class CinemachineCamHelper
{
    public static CinemachineVirtualCamera GetCineCam(ICinemachineCamera activeCamera)
    {
        CinemachineVirtualCamera returnedCam = null;

        if (activeCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>())
        {
            returnedCam = activeCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();
        } else if (activeCamera.VirtualCameraGameObject.GetComponent<CinemachineStateDrivenCamera>())
        {
            returnedCam = GetCamFromStateDrivenCam(activeCamera.VirtualCameraGameObject.GetComponent<CinemachineStateDrivenCamera>());
        }

        return returnedCam;
    }

    static CinemachineVirtualCamera GetCamFromStateDrivenCam(CinemachineStateDrivenCamera stateCam)
    {
        CinemachineVirtualCamera cam = stateCam.LiveChild.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();
        return cam;
    }
}
