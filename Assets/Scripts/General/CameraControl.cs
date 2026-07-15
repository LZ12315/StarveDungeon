using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public static CameraControl Instance;

    public VoidEventSO cameraShakeEvent;
    public VoidEventSO afterSceneLoadedEvent;
    public CinemachineImpulseSource impulseSource;

    [Header("…„œÒÕ∑“∆∂Ø")]
    private Vector3 originPos;
    public float duration = 2;
    public float speed = 1;
    public float Distance = 12;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        originPos = transform.position;
    }


    public void SetTargetPlace(Vector3 movePos)
    {   
        StartCoroutine(CameraMove(movePos));
    }


    private IEnumerator CameraMove(Vector3 movePos)
    {
        Vector3 startPos= transform.position;
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime * speed;
            float t = time / 2f;
            transform.position = Vector3.Lerp(startPos, originPos + movePos.normalized * Distance, t);
            yield return null;
        }
    }

    private void OnEnable()
    {
        //cameraShakeEvent.OnEventRaised += OnCameraShake;
        //afterSceneLoadedEvent.OnEventRaised += OnAfterSceneLoaded;
    }

    private void OnDisable()
    {
        //cameraShakeEvent.OnEventRaised -= OnCameraShake;
        //afterSceneLoadedEvent.OnEventRaised -= OnAfterSceneLoaded;
    }

    private void OnCameraShake()
    {
        impulseSource.GenerateImpulse();
    }

    private void OnAfterSceneLoaded()
    {
    }
}
