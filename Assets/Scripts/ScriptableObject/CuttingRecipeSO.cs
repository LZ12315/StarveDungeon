using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Recipe/CuttingRecipeSO")]
public class CuttingRecipeSO : ScriptableObject
{
    public FoodSO input;
    public FoodSO output;
    public float cutTime;
}
