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

    [Header("Ω±¿¯Ω” ’")]
    public List<ItemSO> rewardsThisWave;
    public List<ItemSO> rewardsToDisplay;
    private int currentWaveCount;

    [Header("Ω±¿¯—°‘Ò")]
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
        RewardCanvas.SetActive(true);
        SelectRandomRewards(3);
    }

    private void SelectRandomRewards(int count)
    {
        if(currentWaveCount <= 3)
        {
            for (int i = 0; i < count; i++)
            {
                int index = UnityEngine.Random.Range(1, 10);
                if(index <= 7)
                {
                    while (true)
                    {
                        ItemSO newReward = rewardsThisWave[UnityEngine.Random.Range(0, rewardsThisWave.Count)];
                        if(newReward.rewardRank == RewardRank.Common)
                        {
                            rewardsToDisplay.Add(rewardsThisWave[UnityEngine.Random.Range(0, rewardsThisWave.Count)]);
                            break;
                        }
                    }
                }
                else if(index <= 10)
                {
                    while (true)
                    {
                        ItemSO newReward = rewardsThisWave[UnityEngine.Random.Range(0, rewardsThisWave.Count)];
                        if (newReward.rewardRank == RewardRank.Rare)
                        {
                            rewardsToDisplay.Add(rewardsThisWave[UnityEngine.Random.Range(0, rewardsThisWave.Count)]);
                            break;
                        }
                    }
                }
            }
        }
        else if(currentWaveCount <= 6)
        {
            for (int i = 0; i < count; i++)
            {
                int index = UnityEngine.Random.Range(1, 10);
                if (index <= 6)
                {
                    while (true)
                    {
                        ItemSO newReward = rewardsThisWave[UnityEngine.Random.Range(0, rewardsThisWave.Count)];
                        if (newReward.rewardRank == RewardRank.Common)
                        {
                            rewardsToDisplay.Add(rewardsThisWave[UnityEngine.Random.Range(0, rewardsThisWave.Count)]);
                            break;
                        }
                    }
                }
                else if (index <= 9)
                {
                    while (true)
                    {
                        ItemSO newReward = rewardsThisWave[UnityEngine.Random.Range(0, rewardsThisWave.Count)];
                        if (newReward.rewardRank == RewardRank.Rare)
                        {
                            rewardsToDisplay.Add(rewardsThisWave[UnityEngine.Random.Range(0, rewardsThisWave.Count)]);
                            break;
                        }
                    }
                }
                else if (index <= 10)
                {
                    while (true)
                    {
                        ItemSO newReward = rewardsThisWave[UnityEngine.Random.Range(0, rewardsThisWave.Count)];
                        if (newReward.rewardRank == RewardRank.Epic)
                        {
                            rewardsToDisplay.Add(rewardsThisWave[UnityEngine.Random.Range(0, rewardsThisWave.Count)]);
                            break;
                        }
                    }
                }
            }
        }
        else if(currentWaveCount <= 9)
        {
            for (int i = 0; i < count; i++)
            {
                int index = UnityEngine.Random.Range(1, 10);
                if (index <= 3)
                {
                    while (true)
                    {
                        ItemSO newReward = rewardsThisWave[UnityEngine.Random.Range(0, rewardsThisWave.Count)];
                        if (newReward.rewardRank == RewardRank.Common)
                        {
                            rewardsToDisplay.Add(rewardsThisWave[UnityEngine.Random.Range(0, rewardsThisWave.Count)]);
                            break;
                        }
                    }
                }
                else if (index <= 8)
                {
                    while (true)
                    {
                        ItemSO newReward = rewardsThisWave[UnityEngine.Random.Range(0, rewardsThisWave.Count)];
                        if (newReward.rewardRank == RewardRank.Rare)
                        {
                            rewardsToDisplay.Add(rewardsThisWave[UnityEngine.Random.Range(0, rewardsThisWave.Count)]);
                            break;
                        }
                    }
                }
                else if (index <= 10)
                {
                    while (true)
                    {
                        ItemSO newReward = rewardsThisWave[UnityEngine.Random.Range(0, rewardsThisWave.Count)];
                        if (newReward.rewardRank == RewardRank.Epic)
                        {
                            rewardsToDisplay.Add(rewardsThisWave[UnityEngine.Random.Range(0, rewardsThisWave.Count)]);
                            break;
                        }
                    }
                }
            }
        }
        else if(currentWaveCount <= 11)
        {
            for (int i = 0; i < count; i++)
            {
                int index = UnityEngine.Random.Range(1, 10);
                if (index <= 5)
                {
                    while (true)
                    {
                        ItemSO newReward = rewardsThisWave[UnityEngine.Random.Range(0, rewardsThisWave.Count)];
                        if (newReward.rewardRank == RewardRank.Rare)
                        {
                            rewardsToDisplay.Add(rewardsThisWave[UnityEngine.Random.Range(0, rewardsThisWave.Count)]);
                            break;
                        }
                    }
                }
                else if (index <= 10)
                {
                    while (true)
                    {
                        ItemSO newReward = rewardsThisWave[UnityEngine.Random.Range(0, rewardsThisWave.Count)];
                        if (newReward.rewardRank == RewardRank.Epic)
                        {
                            rewardsToDisplay.Add(rewardsThisWave[UnityEngine.Random.Range(0, rewardsThisWave.Count)]);
                            break;
                        }
                    }
                }
            }
        } 
        DisplayRewardPreview();
    }

    private void DisplayRewardPreview()
    {
        for(int i = 0;i < rewardsToDisplay.Count;i++)
        {
            UIGameObject obj = UIGameObjects[i];
            obj.image.sprite = rewardsToDisplay[i].sprite;
            //obj.textMeshPro.text = rewardsToDisplay[i].name;
            UIGameObjects[i] = obj;
        }
    }

    public void SelectReward1()
    {
        GameObject newReward = Instantiate(rewardsToDisplay[0].prefab);
        IReward reward = newReward.GetComponent<IReward>();
        reward.SetSubject(GameManager.instance.playerTransform.GetComponent<IRewardSubject>());
        RewardCanvas.SetActive(false);
        Time.timeScale = 1.0f;
        rewardsThisWave.Clear();
        rewardsToDisplay.Clear();
    }

    public void SelectReward2()
    {
        GameObject newReward = Instantiate(rewardsToDisplay[1].prefab);
        IReward reward = newReward.GetComponent<IReward>();
        reward.SetSubject(GameManager.instance.playerTransform.GetComponent<IRewardSubject>());
        RewardCanvas.SetActive(false);
        Time.timeScale = 1.0f;
        rewardsThisWave.Clear();
        rewardsToDisplay.Clear();
    }

    public void SelectReward3()
    {
        GameObject newReward = Instantiate(rewardsToDisplay[2].prefab);
        IReward reward = newReward.GetComponent<IReward>();
        reward.SetSubject(GameManager.instance.playerTransform.GetComponent<IRewardSubject>());
        RewardCanvas.SetActive(false);
        Time.timeScale = 1.0f;
        rewardsThisWave.Clear();
        rewardsToDisplay.Clear();
    }
}
