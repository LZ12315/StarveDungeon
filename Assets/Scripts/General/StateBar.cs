using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateBar : MonoBehaviour
{
    public Image bloodFlow;
    public Image pointer;

    void FixedUpdate()
    {
        pointer.rectTransform.rotation = Quaternion.Euler(0, 0, WaveManager.instance.waveTimer / WaveManager.instance.waveDuration * -360);
    }

    public void OnHealthChange(float percentage)
    {
        bloodFlow.fillAmount = percentage;
    }
}
