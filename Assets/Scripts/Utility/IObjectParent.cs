using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public interface IObjectParent
{
    public void ObjectInteract(IObjectParent player);

    public void ComeNewObject(Object newFood);

    public Vector3 ReturnHoldPlace1();

    public Vector3 ReturnHoldPlace2();

    public Object ReturnNowObject1();

    public Object ReturnNowObject2();

    public void ClearNowObject();
}
