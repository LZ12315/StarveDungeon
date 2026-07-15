using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillValueNumber : MonoBehaviour
{
    public Image TargetImage;
    // Update is called once per frame
    void Update()
    {
    }

    public void OnHealthChange(float percentage)
    {
        TargetImage.fillAmount = percentage;
    }
}
