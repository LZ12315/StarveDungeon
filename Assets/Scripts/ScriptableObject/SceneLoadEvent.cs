using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = ("Event/SceneLoadEventSO"))]
public class SceneLoadEvent : ScriptableObject
{
    public UnityAction<GameSceneSO,Vector3,bool> LoadRequestEvent;

    public void RaiseLoadRequestEvent(GameSceneSO locationToLoad, Vector3 positionToGo, bool fadeScreen)
    {
        LoadRequestEvent?.Invoke(locationToLoad, positionToGo, fadeScreen);
    }
}
