using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IReward
{
    public void SetSubject(IRewardSubject rewardSubject);

    public void ClearNowSubject();

    public void RewardApply();
}
