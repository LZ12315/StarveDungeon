using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextWaveDetect : MonoBehaviour, IInteractable
{
    public GameObject selectedSprite;

    public void NotSelected()
    {
        selectedSprite.SetActive(false);
    }

    public void OnConfirmAction(PlayerInteract player)
    {
        GameManager.instance.inWave = true;
        GameManager.instance.inPrepare = false;
        //GameManager.instance.isGame = true;
        //GameManager.instance.notGame = false;
        //WaveManager.instance.OnBeginNextWave();
        Destroy(gameObject);
    }

    public void OnSelected()
    {
        selectedSprite.SetActive(true);
    }

    public void OnSpecialAction(PlayerInteract player)
    {
    }
}
