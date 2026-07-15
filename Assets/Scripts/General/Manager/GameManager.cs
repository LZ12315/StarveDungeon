using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("玩家数据")]
    public Vector3 playerPosUpdate;
    public Vector3 playerPosLate;
    public float playerMaxHealth = 0;
    public float playerCurrentHealth = 0;
    public Transform playerTransform;

    [Header("移动基地数据")]
    public Transform fortTranform;
    public float fortFollowDistance;

    [Header("游戏数据")]
    public bool inWave;
    public bool inPrepare;
    public float currentWave;
    public GameObject nexWaveDetectPrefab;
    public List<GameObject> alivedEnemies = new List<GameObject>();

    [Header("数据监听")]
    public VoidEventSO afterSceneLoadedEvent;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        //isGame = true;
        //notGame = false;
        inWave = true;
    }

    private void OnEnable()
    {
        afterSceneLoadedEvent.OnEventRaised += Initialize;
    }

    private void OnDisable()
    {
        afterSceneLoadedEvent.OnEventRaised -= Initialize;
    }

    private void Initialize()
    {
        alivedEnemies.Clear();
        currentWave = 0;
    }

    public void CreatNextWaveDetect()
    {
        Instantiate(nexWaveDetectPrefab, playerPosUpdate, Quaternion.identity);
    }
}
