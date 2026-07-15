using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Recipe/BurnedRecipeSO")]
public class BurnedRecipeSO : ScriptableObject
{
    public FoodSO input;
    public FoodSO output;
    public float fryingTime;
}
