using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public void OnConfirmAction(PlayerInteract player);

    public void OnSpecialAction(PlayerInteract player);

    public void OnSelected();

    public void NotSelected();
}
