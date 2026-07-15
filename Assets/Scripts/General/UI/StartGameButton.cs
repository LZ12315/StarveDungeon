using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StartGameButton : MenuButton
{
    public VoidEventSO newGameEvent;

    public override void OnPointerClick(PointerEventData eventData)
    {
        newGameEvent.RaiseEvent();
    }
}
