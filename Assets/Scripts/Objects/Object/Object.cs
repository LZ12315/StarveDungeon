using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Object : MonoBehaviour
{
    public Rigidbody2D rb;
    public FoodSO foodSO;
    [SerializeField]public IObjectParent nowParent;

    [Header("子弹属性")]
    public bool isFly;
    public float flySpeed;
    public Vector3 rotateSpeed;
    public float TimeCounter;
    public float bulletHP;
    public float availableFrequency = 0;
    public IFoodFunction foodFunction;
    public IFoodNature foodNature;
    public List<IFoodNature> foodNatures = new List<IFoodNature>();

    //[Header("塔防属性")]
    //public float availableDuration;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (GetComponent<IFoodFunction>() != null)
            foodFunction = GetComponent<IFoodFunction>();
        if (GetComponent<IFoodNature>() != null)
            foodNature = GetComponent<IFoodNature>();
        bulletHP = availableFrequency;
    }

    private void OnEnable()
    {
        isFly = false;
    }

    protected virtual void Update()
    {
        FlyDestroyCounter();
        //FlyRotate();
        //TraceWithParent();
    }

    #region 取放相关

    public FoodSO ReturnObjectSO()
    {
        return foodSO;
    }

    public void SetParent(IObjectParent newParent)
    {
        nowParent = newParent;
        newParent.ComeNewObject(this);
        //this.transform.position = nowParent.ReturnHoldPlace1();
    }

    public IObjectParent GetParent()
    {
        return nowParent;
    }
    #endregion

    #region 发射相关

    public virtual void Cast(Vector2 direction)
    {
        isFly = true;
        rb.velocity = direction * flySpeed;

        if (foodFunction != null)
        {
            foodFunction.OnFoodFunction(this);
        }
        if(foodNature != null)
        {
            foodNature.OnFoodNature(this);
        }
        nowParent.ClearNowObject();
        nowParent = null;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        bulletHP--;
        if (bulletHP <= 0 && isFly)
        {
            Debug.Log(bulletHP);
            Destroy(gameObject);
        }
    }

    protected virtual void FlyDestroyCounter()
    {
        if (isFly)
            TimeCounter -= Time.deltaTime;
        if (TimeCounter <= 0 )
        {
            Destroy(gameObject);
            isFly = false;
        }
    }

    private void FlyRotate()
    {
        if (isFly)
        {
            transform.Rotate(rotateSpeed * Time.deltaTime);
        }
    }

    #endregion

    #region 烹饪相关

    public void CookingDestroy()
    {
        Destroy(gameObject);
        nowParent.ClearNowObject();
    }


    public static Object SpawnIngredients(FoodSO ingredientSO,IObjectParent ingredientParent)
    {
        GameObject nowIngredientPrefab = Instantiate(ingredientSO.prefab, ingredientParent.ReturnHoldPlace1(), Quaternion.identity);
        Object nowIngredient = nowIngredientPrefab.GetComponent<Object>();
        nowIngredient.SetParent(ingredientParent);
        return nowIngredient;
    }

    #endregion
}
