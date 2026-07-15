using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RapidProduction : MonoBehaviour,ITurretCounterFunction
{
    public VoidEventSO nextWaveSO;
    public int rapidProductNumber;
    private int rapidProductCounter;

    private void Awake()
    {
        rapidProductCounter = rapidProductNumber;
    }

    private void OnEnable()
    {
        nextWaveSO.OnEventRaised += Initialize;
    }

    private void OnDisable()
    {
        nextWaveSO.OnEventRaised -= Initialize;
    }

    public void Initialize()
    {
        rapidProductCounter = rapidProductNumber;
    }

    public void OnFunction(TurretCounter thisCounter)
    {
        StartCoroutine(rapidProduct(thisCounter));
    }

    private IEnumerator rapidProduct(TurretCounter thisCounter)
    {
        float originDuration = thisCounter.cookDuration; 
        if (rapidProductCounter > 0)
        {
            thisCounter.cookDuration = 0f;
            rapidProductCounter--;
        }
        yield return new WaitForSeconds(0.1f);
        thisCounter.cookDuration = originDuration;
    }
}
