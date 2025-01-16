using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DataInfo;

public class Drop : MonoBehaviour, IDropHandler
{


    void Start()
    {

    }
    public void OnDrop(PointerEventData eventData)
    {
        if (gameObject.transform.childCount == 0)
        {
            Drag.draggingItem.transform.SetParent(transform, false);
            Item item = Drag.draggingItem.GetComponent<ItemInfo>().itemData;
            GameManager.Instance.AddItem(item);
        }
            
    }
}
