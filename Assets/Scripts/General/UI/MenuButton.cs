using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuButton : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{
    public Sprite selectedSprite;
    private Image image;
    private Sprite originSprite;

    private void Awake()
    {
        image = GetComponent<Image>();
        originSprite = image.sprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(selectedSprite != null)
        {
            image.sprite = selectedSprite;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.sprite = originSprite;
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
    }
}
