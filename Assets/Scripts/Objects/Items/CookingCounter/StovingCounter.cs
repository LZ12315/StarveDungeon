using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class StovingCounter : BaseCounter,IInteractable
{
    [Header("Cooking")]
    public FryingRecipeSO[] FryingRecipeSOArray;
    public BurnedRecipeSO[] BurnedRecipeSOArray;
    public FryingRecipeSO nowFyingRecipeSO;
    public BurnedRecipeSO nowBurnedRecipeSO;
    public StovingState nowState;
    public float fryingTimer;
    public bool isFry;
    public bool isBurned;

    [Header("Effects")]
    public GameObject fryingEffect;

    protected override void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
        nowState = StovingState.Idle;
    }

    protected override void Update()
    {
        base.Update();
        Frying();
        FryingCounter();
    }

    private void FixedUpdate()
    {
    }

    public void OnConfirmAction(PlayerInteract player)
    {
        CounterInteract(player);
        //ObjectInteract(player);
    }

    public void OnSpecialAction(PlayerInteract player)
    {
        if(!isFry)
        {
            StartFrying();
        }
    }

    public override void ObjectInteract(IObjectParent player)
    {
        this.player = player;

        if (nowObject != null && player.ReturnNowObject1() == null)
        {
            TakeAwayObject();
            fryingEffect.SetActive(false);
        }
        else if (nowObject == null && player.ReturnNowObject1() != null && ConfirmFryingRecipe(player.ReturnNowObject1().ReturnObjectSO()))
        {
            GetObject();
        }
        else if (nowObject != null && player.ReturnNowObject1() != null)
        {
            Debug.Log("You have something on hand");
        }
    }

    private void StartFrying()
    {
        if (nowObject != null && player.ReturnNowObject1() == null && ConfirmFryingRecipe(nowObject.ReturnObjectSO()))
        {
            isFry = true;
            nowState = StovingState.Frying;
        }
        else if (nowObject != null && player.ReturnNowObject1() != null)
        {
            Debug.Log("You have something on hand");
        }
    }

    private void Frying()
    {
        switch (nowState)
        {
            case StovingState.Idle:
                break;
            case StovingState.Frying:
                ConfirmFryingRecipe(nowObject.ReturnObjectSO());
                fryingEffect.SetActive(true);
                isFry = true;

                if (fryingTimer > nowFyingRecipeSO.fryingTime)
                {
                    nowObjectSO = GetFryingOutput(nowObject.ReturnObjectSO());
                    nowObject.CookingDestroy();
                    Create();
                    if (ConfirmBurnedRecipe(nowObject.ReturnObjectSO()))
                    {
                        nowState = StovingState.Fried;
                    }
                    else
                    {
                        nowState = StovingState.Burned;
                    }
                }
                break;
            case StovingState.Fried:
                if (nowBurnedRecipeSO != null && fryingTimer >= nowFyingRecipeSO.fryingTime + nowBurnedRecipeSO.fryingTime)
                {
                    nowObjectSO = GetBurnedOutput(nowObject.ReturnObjectSO());
                    nowObject.CookingDestroy();
                    Create();
                    nowState = StovingState.Burned;
                }
                break;
            case StovingState.Burned:
                isFry = false;
                fryingTimer = 0;
                fryingEffect.SetActive(false);
                break;
        }
    }

    private void FryingCounter()
    {
        if (isFry)
        {
            fryingTimer += Time.deltaTime;
        }
        else
        {
            fryingTimer = 0;
            //fryingEffect.SetActive(false);
            nowState = StovingState.Idle;
        }
    }

    private void Create()
    {
        GameObject newCuttingOutputPrefab = Instantiate(nowObjectSO.prefab, transform.position, Quaternion.identity);
        nowObject = newCuttingOutputPrefab.GetComponent<Object>();
        nowObject.SetParent(this);
        nowObject.transform.position = transform.position;
    }

    private bool ConfirmFryingRecipe(FoodSO inputObjectSO)
    {
        foreach (FryingRecipeSO fryingRecipeSO in FryingRecipeSOArray)
        {
            if (fryingRecipeSO.input == inputObjectSO)
            {
                nowFyingRecipeSO = fryingRecipeSO;
                return true;
            }

        }
        return false;
    }

    private FoodSO GetFryingOutput(FoodSO inputObjectSO)
    {
        foreach (FryingRecipeSO fryingRecipeSO in FryingRecipeSOArray)
        {
            if (fryingRecipeSO.input == inputObjectSO)
            {
                return fryingRecipeSO.output;
            }
        }
        return null;
    }

    private bool ConfirmBurnedRecipe(FoodSO inputObjectSO)
    {
        foreach (BurnedRecipeSO BurnedRecipeSO in BurnedRecipeSOArray)
        {
            if (BurnedRecipeSO.input == inputObjectSO)
            {
                nowBurnedRecipeSO = BurnedRecipeSO;
                return true;
            }

        }
        return false;
    }

    private FoodSO GetBurnedOutput(FoodSO inputObjectSO)
    {
        foreach (BurnedRecipeSO BurnedRecipeSO in BurnedRecipeSOArray)
        {
            if (BurnedRecipeSO.input == inputObjectSO)
            {
                return BurnedRecipeSO.output;
            }

        }
        return null;
    }

    protected override void TakeAwayObject()
    {
        base.TakeAwayObject();
        isFry = false;
        nowState = StovingState.Idle;
        fryingEffect.SetActive(false);
    }
    public enum StovingState
    {
        Idle, Frying, Fried, Burned,
    }
}
