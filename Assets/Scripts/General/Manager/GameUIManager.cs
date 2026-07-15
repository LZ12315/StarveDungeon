using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager instance;
    public StateBar playerStateBar;

    [Header("ÊÂ¼þ¼àÌý")]
    public CharacterEventSO PlayerHealthEvent;
    public RapartEventSO RapartHealthEvent;
    public GameObject pausePanel;

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        //PlayerHealthEvent.OnEventRaised += OnHealthEventCharacter;
        RapartHealthEvent.OnEventRaised += RapartHealthChange;
    }
    private void OnDisable()
    {
        //PlayerHealthEvent.OnEventRaised -= OnHealthEventCharacter;
        RapartHealthEvent.OnEventRaised -= RapartHealthChange;
    }

    //private void OnHealthEventCharacter(Character character)
    //{
    //    var percentage = character.currentHealth / character.maxHealth;
    //    fillValueNumber.OnHealthChange(percentage);
    //}

    public void RapartHealthChange(Rapart rapart)
    {
        var percentage = rapart.currentHealth / rapart.maxHealth;
        playerStateBar.OnHealthChange(percentage);
    }

    public void GamePause()
    {
        Time.timeScale = 0;
        pausePanel.SetActive(true);
    }

    public void GameResume()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }

}
