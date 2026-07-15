using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/RapartEventSO")]
public class RapartEventSO : ScriptableObject
{
    public UnityAction<Rapart> OnEventRaised;

    public void RaiseEvent(Rapart rapart)
    {
        OnEventRaised?.Invoke(rapart);
    }
}
