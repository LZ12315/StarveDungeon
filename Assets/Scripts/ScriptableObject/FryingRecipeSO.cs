using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Recipe/FryingRecipeSO")]
public class FryingRecipeSO : ScriptableObject
{
    public FoodSO input;
    public FoodSO output;
    public float fryingTime;
}
