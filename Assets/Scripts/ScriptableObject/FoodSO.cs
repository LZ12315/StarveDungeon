using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Object/FoodSO")]
public class FoodSO : ScriptableObject
{
    public GameObject prefab;
    public Sprite formalSprite;
    public Sprite cookedSprite;
    public string objectName;
}
