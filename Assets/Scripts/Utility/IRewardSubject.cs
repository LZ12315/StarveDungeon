using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRewardSubject
{
    public void ComeNewReward(IReward reward);

    public void UseReward();

    public Vector3 ReturnRewardPoint();

    public IReward ReturnNowReward();

    public void ClearNowReward();

    public Transform ReturnSubjectTransform();
}
