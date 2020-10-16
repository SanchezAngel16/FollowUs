using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    Vector3 cameraInitialPosition;
    public float shakeMagnitude = 0.05f, shakeTime = 0.3f;
    public Camera mainCamera;

    public void shakeIt()
    {
        cameraInitialPosition = mainCamera.transform.position;
        InvokeRepeating("startCameraShaking", 0f, 0.005f);
        Invoke("stopCameraShaking", shakeTime);
    }

    void startCameraShaking()
    {
        float cameraShakingOffsetX = Random.value * shakeMagnitude * 2 - shakeMagnitude;
        float cameraShakingOffsetY = Random.value * shakeMagnitude * 2 - shakeMagnitude;
        Vector3 cameraIntermadiatePosition = mainCamera.transform.position;
        cameraIntermadiatePosition.x += cameraShakingOffsetX;
        cameraIntermadiatePosition.y += cameraShakingOffsetY;
        mainCamera.transform.position = cameraIntermadiatePosition;
    }

    void stopCameraShaking()
    {
        CancelInvoke("startCameraShaking");
        mainCamera.transform.position = cameraInitialPosition;
    }
}
