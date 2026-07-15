using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Object
{
    public Turret nowTurret;

    [Header("×Óµ¯³õÊ¼»¯")]
    public Vector3 originScale;
    public float originSpeed;
    public GameObject patty;
    public GameObject burgerBunUp;
    public GameObject burgerBunDown;
    public List<GameObject> pattys = new List<GameObject>();
    public bool hasUsed;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void AddEffects()
    {
        foodFunction = GetComponent<IFoodFunction>();
        Component[] components = GetComponents<Component>();
        foreach (Component component in components)
        {
            IFoodNature foodNac = component as IFoodNature;
            if (foodNac != null)
            {
                foodNatures.Add(foodNac);
            }
        }
    }


    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (isFly)
        {
            bulletHP--;
            if (bulletHP<=0)
            {
                Destroy(gameObject);
                //nowTurret.ReturnObject(this.gameObject);
            }
            isFly = false;
        }
    }

    protected override void FlyDestroyCounter()
    {
        if (isFly)
            TimeCounter -= Time.deltaTime;
        if (TimeCounter <= 0)
        {
            if(nowTurret != null)
            {
                Destroy(gameObject);
                //nowTurret.ReturnObject(this.gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
            isFly = false;
        }
    }

    public void FitOut(Turret turret)
    {
        nowTurret = turret;
    }

    public void Enchuntment()
    {
        AddEffects();
        if (foodNatures.Count > 0)
        {
            foreach (IFoodNature nature in foodNatures)
            {
                nature.OnFoodNature(this);
            }
        }
    }

    public override void Cast(Vector2 direction)
    {
        Enchuntment();
        isFly = true;
         rb.velocity = direction * flySpeed;

        if(foodFunction != null)
        {
            foodFunction.OnFoodFunction(this);
        }
        nowParent = null;
    }

    public void AddPatty(int pattyNumber)
    {
        Instantiate(burgerBunDown, transform.position, Quaternion.identity,this.transform);
        for (int i = 1; i <= pattyNumber; i++)
        {
            GameObject newPatty = Instantiate(patty,new Vector3(transform.position.x,transform.position.y + i*0.25f,transform.position.z),Quaternion.identity,transform);
            pattys.Add(newPatty);
        }
        Instantiate(burgerBunUp, new Vector3(transform.position.x, transform.position.y + (pattyNumber + 1) * 0.25f, transform.position.z), Quaternion.identity, this.transform);
    }

    private GameObject AdjustPivot(GameObject original, Vector3 newLocalPivot)
    {
        GameObject newParent = new GameObject(original.name + "_PivotAdjusted");
        original.transform.SetParent(newParent.transform);
        newParent.transform.position = original.transform.position;
        original.transform.localPosition -= newLocalPivot;
        return newParent;
    }
}
