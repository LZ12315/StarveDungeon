using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter, IInteractable, IObjectParent
{
    [Header("≈Î‚øœ‡πÿ")]
    public CuttingRecipeSO[] cuttingRecipeSOArray;
    public CuttingRecipeSO nowCuttingRecipe;
    public bool isCutting;

    protected override void Awake()
    {
        base.Awake();
    }

    public void OnConfirmAction(PlayerInteract player)
    {
        CounterInteract(player);
    }

    public void OnSpecialAction(PlayerInteract player)
    {
        if(!isCutting)
        {
            StartCoroutine(Cutting());
        }
    }

    public override void ObjectInteract(IObjectParent player)
    {
        this.player = player;

        if (nowObject != null && player.ReturnNowObject1() == null)
        {
            TakeAwayObject();
        }
        else if (nowObject == null && player.ReturnNowObject1() != null && HasCuttingInput(player.ReturnNowObject1().ReturnObjectSO()))
        {
            GetObject();
        }
        else if (nowObject != null && player.ReturnNowObject1() != null)
        {
            Debug.Log("You have something on hand");
        }
    }

    public IEnumerator Cutting()
    {
        if (nowObject != null && player.ReturnNowObject1() == null && HasCuttingInput(nowObject.ReturnObjectSO()))
        {
            nowObjectSO = GetCuttingOutput(nowObject.ReturnObjectSO());
            isCutting = true;
            
            yield return new WaitForSeconds(nowCuttingRecipe.cutTime);

            nowObject.CookingDestroy();
            Create();
            isCutting = false;
        }
        else if (nowObject != null && player.ReturnNowObject1() != null)
        {
            Debug.Log("You have something on hand");
            yield return null;
        }
    }

    private void Create()
    {
        GameObject newCuttingOutputPrefab = Instantiate(nowObjectSO.prefab, transform.position, Quaternion.identity);
        nowObject = newCuttingOutputPrefab.GetComponent<Object>();
        nowObject.SetParent(this);
        nowObject.transform.position = transform.position;
    }

    private bool HasCuttingInput(FoodSO inputObjectSO)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
        {
            if (cuttingRecipeSO.input == inputObjectSO)
            {
                nowCuttingRecipe = cuttingRecipeSO;
                return true;
            }
        }
        return false;
    }

    private FoodSO GetCuttingOutput(FoodSO inputObjectSO)
    {
        foreach(CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
        {
            if (cuttingRecipeSO.input == inputObjectSO)
                return cuttingRecipeSO.output;
        }
        return null;
    }
}
