using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private TurretCounter turretCounter;
    [SerializeField] private Image barImage;

    private float progressPercent;
    private void Update()
    {
    }

    public void SetPercent(float percent)
    {
        barImage.fillAmount = percent;
    }
}
