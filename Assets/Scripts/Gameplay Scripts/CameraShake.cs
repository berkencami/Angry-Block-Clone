using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
   
    private Transform cameraContainer;
    private float rotateAmount = 4f;
    private float shakeAmount;

    private Vector3 startLocalPos;
    void Start()
    {
        cameraContainer = GameObject.Find("CameraContainer").transform;
    }


    void Update()
    {
        if (shakeAmount > 0.01f)
        {
            Vector3 locaclPosition = startLocalPos;
            locaclPosition.x += shakeAmount * Random.Range(3, 5);
            locaclPosition.y += shakeAmount * Random.Range(3, 5);
            transform.localPosition = locaclPosition;
            shakeAmount = 0.9f * shakeAmount;
        }
    }


    public void Shake()
    {
        shakeAmount = Mathf.Min(0.1f, shakeAmount + 0.01f);
    }

    public void MediumShake()
    {
        shakeAmount = Mathf.Min(0.15f, shakeAmount + 0.015f);
    }


    public void RotateCameraToSide()
    {
        StartCoroutine(RotateCameraToSideRoutine());

    }

    public void RotateCameraToFront()
    {
        StartCoroutine(RotateCameraToFrontRoutine());
    }

    IEnumerator RotateCameraToSideRoutine()
    {
        int frames = 20;
        float increment = rotateAmount / (float)frames;
        for (int i = 0; i < frames; i++)
        {
            cameraContainer.RotateAround(Vector3.zero,Vector3.up,increment);
            yield return null;
        }
        yield break;
    }


    IEnumerator RotateCameraToFrontRoutine()
    {
        int frames = 20;
        float increment = rotateAmount / (float)frames;
        for (int i = 0; i < frames; i++)
        {
            cameraContainer.RotateAround(Vector3.zero, Vector3.up, -increment);
            yield return null;
        }

        cameraContainer.localEulerAngles = new Vector3(0, 0, 0);
        yield break;
    }
}
