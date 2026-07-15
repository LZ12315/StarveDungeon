using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Object/ItemSO")]
public class ItemSO : ScriptableObject
{
    public GameObject prefab;
    public Sprite sprite;
    public string objectName;
    public RewardRank rewardRank;
}
