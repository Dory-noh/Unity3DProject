using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DataInfo;

public class Drag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    Transform itemTr;
    Transform itemListTr;
    Transform inventoryTr;

    CanvasGroup CanvasGroup;
    public static GameObject draggingItem = null;
    void Start()
    {
        itemTr = GetComponent<Transform>();
        itemListTr = GameObject.Find("ItemList").transform;
        inventoryTr = GameObject.Find("Inventory").transform;
        CanvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        itemTr.position = Input.mousePosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        this.transform.SetParent(inventoryTr, false);
        draggingItem = this.gameObject;

        CanvasGroup.blocksRaycasts = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        draggingItem = null;
        CanvasGroup.blocksRaycasts = true;
        if (itemTr.parent == inventoryTr)
        {
            this.transform.SetParent(itemListTr, false);
            GameManager.Instance.RemoveItem(GetComponent<ItemInfo>().itemData);
        }
        
    }
}
