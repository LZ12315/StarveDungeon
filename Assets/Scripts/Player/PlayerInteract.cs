using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour,IObjectParent,IRewardSubject
{
    public PlayerInputControl playerInput;
    public PlayerController playerControl;
    public PlayerAnimation playerAnimation;

    [Header("½»»¥¼ì²â")]
    public float checkDistance;
    public LayerMask itemsLayer;
    private Vector2 lastDir;
    public float minInteractDistance = 4f;
    [SerializeField]public IInteractable targetItem;
    public GameObject targetObject;
    public bool canPlaceSomething;
    public bool canInteract;

    [Header("³ø·¿ÎïÆ·½»»¥")]
    public Transform objectPoint1;
    public Transform objectPoint2;
    public Object nowObject1;
    public Object nowObject2;
    public Object nowBullet;
    public FoodSO foodSO;
    public IObjectParent dropPoint;
    public FoodSO dropPointSO;

    [Header("½±Àø½»»¥")]
    public Transform rewardPoint;
    [SerializeField]public IReward nowReward;
    public bool canUseReward = true;

    private void Awake()
    {
        playerInput = new PlayerInputControl();
        playerControl = GetComponent<PlayerController>();
        playerAnimation = GetComponent<PlayerAnimation>();
    }

    private void OnEnable()
    {
        playerInput.Enable();
        playerInput.GamePlay.Confirm.started += OnConfirm;
        playerInput.GamePlay.Special.started += OnSpecial;
    }

    private void Update()
    {
        CheckCanPress();
        CheckCanUseReward();
        SetFoodHoldPoint();
        HoldObject();
    }

    private void FixedUpdate()
    {
    }

    private void SetFoodHoldPoint()
    {
        int point = 0;
        if(nowObject1 != null)
        {
            point++;
        }
        if(nowObject2 != null)
        {
            point++;
        }
        playerAnimation.foodHoldNumber = point;
    }

    private void OnConfirm(InputAction.CallbackContext context)
    {
        if (!canPlaceSomething && canInteract)
        {
            targetItem.OnConfirmAction(this);
        }

        //if (canPlaceSomething && !canInteract)
        //{
        //    if (GameManager.instance.inWave)
        //    {
        //        //PlaceDropPoint();
        //        //ClearNowObject();
        //    }
        //    else
        //    {
        //        UseReward();
        //    }
        //}
    }

    private void OnSpecial(InputAction.CallbackContext context)
    {
        if(nowReward != null && canPlaceSomething)
        {
            UseReward();
        }
        if (canInteract && nowReward == null && nowObject1 == null)
        {
            targetItem.OnSpecialAction(this);
        }
    }

    public void CheckCanPress()
    {
        if(playerControl.inputDir != Vector2.zero)
            lastDir = playerControl.inputDir;

        if(nowObject1 != null || nowReward != null || nowObject2!=null)
        {
            canPlaceSomething = true;
        }

        if (Physics2D.Raycast(transform.position,lastDir,checkDistance,itemsLayer))
        {
            RaycastHit2D raycastHit = Physics2D.Raycast(transform.position,lastDir,checkDistance,itemsLayer);
            if (raycastHit.collider.CompareTag("Interactable") && raycastHit.collider.GetComponent<IReward>() != nowReward)
            {
                if(targetItem != null && targetObject != null)
                {
                    targetItem.NotSelected();
                    targetItem = null;
                    targetObject = null;
                }

                canInteract = true;
                canPlaceSomething = false;
                targetItem = raycastHit.collider.GetComponent<IInteractable>();
                targetObject = raycastHit.collider.gameObject;
                targetItem.OnSelected();
            }
        }
        else
        {
            if (targetItem != null && Vector3.Distance(this.transform.position,targetObject.transform.position) >= minInteractDistance)
            {
                canInteract = false;
                targetItem.NotSelected();
                targetItem = null;
                targetObject = null;
            }

            if (targetItem == null && nowObject1 == null)
            {
                canInteract = false;
            }
        }
    }

    private void PlaceDropPoint()
    {
        if (nowObject1 != null)
        {
            GameObject dropPointPrefab = Instantiate(dropPointSO.prefab, transform.position, Quaternion.identity);

            dropPoint = dropPointPrefab.GetComponent<IObjectParent>();

            this.nowObject1.SetParent(dropPoint);

            canPlaceSomething = false;
        }
    }

    public void ComeNewObject(Object newFood)
    {
        if (nowObject1 == null)
        {
            if(newFood.GetComponent<Bullet>() != null)
            {
                nowObject1 = newFood;
                nowBullet = newFood;
                playerControl.Reloading(nowBullet);
            }
            else
            {
                nowObject1 = newFood;
                newFood.transform.position = objectPoint1.position;
            }
        }
        else if (nowObject2 == null && nowBullet == null)
        {
            if (newFood.GetComponent<Bullet>() != null)
            {
                Destroy(nowObject1.gameObject);
                ClearNowObject();
                nowObject1 = newFood;
                nowBullet = newFood;
                playerControl.Reloading(nowBullet);
            }
            else
            {
                nowObject2 = newFood;
                newFood.transform.position = objectPoint2.position;
            }
        }
    }

    private void HoldObject()
    {
        if(nowObject1 != null && nowObject1.isFly == false)
        {
            nowObject1.transform.position = objectPoint1.position;
        }
        if (nowObject2 != null && nowObject2.isFly == false)
        {
            nowObject2.transform.position = objectPoint2.position;
        }
    }

    public void ClearNowObject()
    {
        if (nowObject1 != null)
        {
            nowObject1 = null;
            nowObject1 = nowObject2;
            nowObject2 = null;
        }
        else if (nowObject2 != null)
        {
            nowObject2 = null;
        }
        if(nowBullet != null)
        {
            nowBullet = null;
            playerControl.bullets.Clear();
        }
    }

    public Vector3 ReturnHoldPlace1()
    {
        return objectPoint1.position;
    }

    public Vector3 ReturnHoldPlace2()
    {
        return objectPoint2.position;
    }

    public Object ReturnNowObject1()
    {
        return nowObject1;
    }

    public Object ReturnNowObject2()
    {
        return nowObject2;
    }

    public void ObjectInteract(IObjectParent player)
    {
    }

    #region ½±Àø½»»¥
    public void ComeNewReward(IReward reward)
    {
        if(nowReward == null)
        {
            nowReward = reward;
        }
        else
        {
            UseReward();
            nowReward = reward;
        }
    }

    public Vector3 ReturnRewardPoint()
    {
        return rewardPoint.position;
    }

    public IReward ReturnNowReward()
    {
        return nowReward;
    }

    public void UseReward()
    {
        nowReward.RewardApply();
        nowReward.ClearNowSubject();
        ClearNowReward();
        canPlaceSomething = false;
    }

    public void ClearNowReward()
    {
        if(nowReward != null)
        {
            nowReward = null;
        }
    }

    private void CheckCanUseReward()
    {
        if(GameManager.instance.fortTranform != null)
        {
            float distance = Vector3.Distance(transform.position, GameManager.instance.fortTranform.position);
            if (distance < GameManager.instance.fortFollowDistance && nowReward != null)
                canUseReward = true;
        }
    }

    public Transform ReturnSubjectTransform()
    {
        return transform;
    }

    #endregion
}
