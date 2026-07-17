using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardUIManager : MonoBehaviour
{
    public static RewardUIManager instance;
    public GameObject RewardCanvas;

    [Header("Reward Source")]
    public List<ItemSO> rewardsThisWave;
    public List<ItemSO> rewardsToDisplay;
    private int currentWaveCount;

    [Header("Reward Selection")]
    [SerializeField] public List<UIGameObject> UIGameObjects = new List<UIGameObject>();
    [Serializable]public struct UIGameObject
    {
        [SerializeField] public GameObject UIParent;
        [SerializeField] public Image image;
        [SerializeField] public TextMeshProUGUI textMeshPro;
        [SerializeField] public Button button;
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void RewardTime(List<ItemSO> rewards,int wave)
    {
        Time.timeScale = 0;
        rewardsThisWave = rewards;
        currentWaveCount = wave;
        rewardsToDisplay.Clear();

        if (rewardsThisWave == null || rewardsThisWave.Count == 0)
        {
            Time.timeScale = 1.0f;
            return;
        }

        RewardCanvas.SetActive(true);
        SelectRandomRewards(3);
    }

    private void SelectRandomRewards(int count)
    {
        for (int i = 0; i < count; i++)
        {
            ItemSO reward = GetRandomReward(RollRewardRank());
            if (reward != null)
            {
                rewardsToDisplay.Add(reward);
            }
        }

        DisplayRewardPreview();
    }

    private RewardRank RollRewardRank()
    {
        int index = UnityEngine.Random.Range(1, 11);

        if (currentWaveCount <= 3)
        {
            return index <= 7 ? RewardRank.Common : RewardRank.Rare;
        }

        if (currentWaveCount <= 6)
        {
            if (index <= 6) return RewardRank.Common;
            return index <= 9 ? RewardRank.Rare : RewardRank.Epic;
        }

        if (currentWaveCount <= 9)
        {
            if (index <= 3) return RewardRank.Common;
            return index <= 8 ? RewardRank.Rare : RewardRank.Epic;
        }

        return index <= 5 ? RewardRank.Rare : RewardRank.Epic;
    }

    private ItemSO GetRandomReward(RewardRank rewardRank)
    {
        if (rewardsThisWave == null || rewardsThisWave.Count == 0)
        {
            return null;
        }

        List<ItemSO> matchingRewards = new List<ItemSO>();
        foreach (ItemSO reward in rewardsThisWave)
        {
            if (reward.rewardRank == rewardRank)
            {
                matchingRewards.Add(reward);
            }
        }

        List<ItemSO> rewardPool = matchingRewards.Count > 0 ? matchingRewards : rewardsThisWave;
        return rewardPool[UnityEngine.Random.Range(0, rewardPool.Count)];
    }

    private void DisplayRewardPreview()
    {
        int displayCount = Mathf.Min(rewardsToDisplay.Count, UIGameObjects.Count);
        for(int i = 0;i < displayCount;i++)
        {
            UIGameObject obj = UIGameObjects[i];
            obj.image.sprite = rewardsToDisplay[i].sprite;
            //obj.textMeshPro.text = rewardsToDisplay[i].name;
            UIGameObjects[i] = obj;
        }
    }

    public void SelectReward1()
    {
        SelectReward(0);
    }

    public void SelectReward2()
    {
        SelectReward(1);
    }

    public void SelectReward3()
    {
        SelectReward(2);
    }

    private void SelectReward(int index)
    {
        if (index < 0 || index >= rewardsToDisplay.Count)
        {
            return;
        }

        GameObject newReward = Instantiate(rewardsToDisplay[index].prefab);
        IReward reward = newReward.GetComponent<IReward>();
        reward.SetSubject(GameManager.instance.playerTransform.GetComponent<IRewardSubject>());
        RewardCanvas.SetActive(false);
        Time.timeScale = 1.0f;
        rewardsThisWave.Clear();
        rewardsToDisplay.Clear();
    }
}
