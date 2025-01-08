using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class Drag : MonoBehaviour,IDragHandler, IBeginDragHandler, IEndDragHandler
{
    Transform itemTr;
    Transform itemListTr;
    Transform inventoryTr;
    CanvasGroup canvasGroup;
    public static GameObject draggingItem = null;

    void Start()
    {
        inventoryTr = GameObject.Find("Inventory").transform;
        itemTr = transform;
        itemListTr = GameObject.Find("ItemList").transform; //부모가 계속 바뀌기 때문에 이름으로 찾음
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void OnDrag(PointerEventData eventData) //드래그 중일 때
    {
        itemTr.position = Input.mousePosition;
    }

    public void OnBeginDrag(PointerEventData eventData) //드래그 시작했을 때
    {
        this.transform.SetParent(inventoryTr, false); //월드 좌표 아니므로 false; //itemList가 부모일 때 Grid Layout Group으로 인해 SlotList에 옮기기 어려우므로 드래그를 시작하면 선택한 item의 부모를 inventory로 바꾼다.
        draggingItem = this.gameObject;
        canvasGroup.blocksRaycasts = false; //ui이벤트 받지 않음
    }

    public void OnEndDrag(PointerEventData eventData) //드래그 끝냈을 때
    {
        draggingItem = null; //다시 draggingItem은 null이 된다.
        canvasGroup.blocksRaycasts = true;

        if(itemTr.parent == inventoryTr) //drag가 끝난 시점에 itemTr.parent의 부모가 inventroyTr이라는 건 slot에 item이 들어가지 않았다는 것이다. 다시 item리스트에 item을 자식으로 넣는다.
        {
            itemTr.SetParent(itemListTr, false);
        }
    }
}
