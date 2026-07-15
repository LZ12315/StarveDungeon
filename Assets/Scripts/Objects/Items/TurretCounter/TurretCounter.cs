using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class TurretCounter : BaseCounter, IInteractable
{
    [Header("属性存储")]
    public int storageQuantity;
    [SerializeField] public List<Property> Properties = new List<Property>();
    [Serializable]
    public struct Property
    {
        [SerializeField] public Object food;
        [SerializeField] public IFoodFunction function;
        [SerializeField] public IFoodNature nature;

        public Property(Object food, IFoodFunction funk, IFoodNature nat)
        {
            this.food = food;
            function = funk;
            nature = nat;
        }
        public void functionUpdate(IFoodFunction funk)
        {
            function = funk;
        }
    };

    [Header("子弹")]
    public bool canTake;
    public FoodSO bulletPrefab;
    public GameObject nowBulletObject;
    public Bullet nowBullet;

    [Header("烹饪相关")]
    public float cookDuration = 5f;
    [SerializeField]public List<ITurretCounterFunction> counterFunctions = new List<ITurretCounterFunction>();
    protected bool isCook;
    protected float cookTimer;
    public GameObject fryingEffect;

    [Header("UI相关")]
    public ProgressBarUI progressBarUI;
    protected float progressPercent;

    protected override void Awake()
    {
        base.Awake();
        progressBarUI = GetComponentInChildren<ProgressBarUI>();
        progressBarUI.gameObject.SetActive(false);

        Component[] components = GetComponents<Component>();
        foreach (Component component in components)
        {
            ITurretCounterFunction turretCounterFunc = component as ITurretCounterFunction;
            if (turretCounterFunc != null)
            {
                counterFunctions.Add(turretCounterFunc);
            }
        }
    }

    protected override void Update()
    {
        base.Update();
        BulletFollow();
        CookingTimer();
        if (isCook)
        {
            progressBarUI.SetPercent(progressPercent);
        }
    }

    private void FixedUpdate()
    {
    }

    public void OnConfirmAction(PlayerInteract player)
    {
        CounterInteract(player);
    }

    public void OnSpecialAction(PlayerInteract player)
    {
        SetSubject(player);
    }

    public override void CounterInteract(PlayerInteract player)
    {
        ObjectInteract(player);
    }

    public override void ObjectInteract(IObjectParent player)
    {
        this.player = player;
        if (nowBullet != null && canTake && (player.ReturnNowObject1() == null || player.ReturnNowObject2() == null))
        {
            TakeAwayObject();
        }
        else if (nowBullet == null && Properties.Count<storageQuantity && player.ReturnNowObject1() != null && player.ReturnNowObject1().GetComponent<Bullet>() == null)
        {
            GetObject();
        }
        else if (Properties.Count >= storageQuantity)
        {
            Debug.Log("Error");
        }
    }

    protected override void TakeAwayObject()
    {
        nowBullet.SetParent(player);
        ClearNowObject();
        nowBulletObject = null;
        nowBullet = null;
    }

    public override void GetObject()
    {
        if (player.ReturnNowObject1() != null)
        {
            nowObject = player.ReturnNowObject1();
            nowObject.SetParent(this);
            player.ClearNowObject();
        }
        else if (player.ReturnNowObject1() == null && player.ReturnNowObject2() != null)
        {
            nowObject = player.ReturnNowObject2();
            nowObject.SetParent(this);
            player.ClearNowObject();
        }

        PropertyUpdate();
    }

    protected virtual void PropertyUpdate()
    {

        nowObject.gameObject.SetActive(false);

        if (Properties.Count < storageQuantity)
        {
            if (nowObject.GetComponent<IFoodFunction>() != null && Properties.Count != 0)
            {
                bool haveFunction = false;
                for (int i = 0; i < Properties.Count; i++)
                {
                    if (Properties[i].function != null)
                    {
                        Properties[i] = new Property(nowObject, nowObject.GetComponent<IFoodFunction>(), nowObject.GetComponent<IFoodNature>());
                        haveFunction = true;
                    }
                }
                if (!haveFunction)
                {
                    Properties.Add(new Property(nowObject, nowObject.GetComponent<IFoodFunction>(), nowObject.GetComponent<IFoodNature>()));
                }
            }
            else
            {
                Properties.Add(new Property(nowObject, nowObject.GetComponent<IFoodFunction>(), nowObject.GetComponent<IFoodNature>()));
            }
            canTake = false;
        }
        if (Properties.Count >= storageQuantity)
        {
            StartCoroutine(CookBullet());
        }
    }

    protected virtual IEnumerator CookBullet()
    {
        CreateNewBullet();
        nowBulletObject.SetActive(false);

        foreach (var property in Properties)
        {
            if (property.function != null)
            {
                IFoodFunction newFunction = ComponentCopier.CopyComponent<IFoodFunction>(property.food.gameObject, nowBulletObject);
            }
            if (property.nature != null)
            {
                IFoodNature newNature = ComponentCopier.CopyComponent<IFoodNature>(property.food.gameObject, nowBulletObject);
            }
        }
        progressBarUI.gameObject.SetActive(true);
        isCook = true;

        yield return new WaitForSeconds(cookDuration);
        
        isCook = false;
        progressBarUI.gameObject.SetActive(false);
        cookTimer = 0;
        progressPercent = 0;
        canTake = true;
        nowBulletObject.SetActive(true);
        Properties.Clear();
    }

    private void CookingTimer()
    {
        if(isCook)
        {
            cookTimer += Time.deltaTime;
            progressPercent = cookTimer / cookDuration;
        }
    }

    protected virtual void CreateNewBullet()
    {
        Destroy(nowBulletObject);
        nowBullet = null;
        nowBulletObject = Instantiate(bulletPrefab.prefab, transform.position, Quaternion.identity);
        nowBullet = nowBulletObject.GetComponent<Bullet>();
        nowBullet.AddPatty(storageQuantity);

        for (int i = 0; i < storageQuantity; i++)
        {
            FoodSO foodso = Properties[i].food.foodSO;
            nowBullet.pattys[i].GetComponent<SpriteRenderer>().sprite = foodso.cookedSprite;
        }
    }

    private void BulletFollow()
    {
        if(nowBullet != null)
        {
            nowBullet.transform.position = objectPoint.position;
        }
    }

}
