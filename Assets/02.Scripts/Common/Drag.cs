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
        itemListTr = GameObject.Find("ItemList").transform; //�θ� ��� �ٲ�� ������ �̸����� ã��
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void OnDrag(PointerEventData eventData) //�巡�� ���� ��
    {
        itemTr.position = Input.mousePosition;
    }

    public void OnBeginDrag(PointerEventData eventData) //�巡�� �������� ��
    {
        this.transform.SetParent(inventoryTr, false); //���� ��ǥ �ƴϹǷ� false; //itemList�� �θ��� �� Grid Layout Group���� ���� SlotList�� �ű�� �����Ƿ� �巡�׸� �����ϸ� ������ item�� �θ� inventory�� �ٲ۴�.
        draggingItem = this.gameObject;
        canvasGroup.blocksRaycasts = false; //ui�̺�Ʈ ���� ����
    }

    public void OnEndDrag(PointerEventData eventData) //�巡�� ������ ��
    {
        draggingItem = null; //�ٽ� draggingItem�� null�� �ȴ�.
        canvasGroup.blocksRaycasts = true;

        if(itemTr.parent == inventoryTr) //drag�� ���� ������ itemTr.parent�� �θ� inventroyTr�̶�� �� slot�� item�� ���� �ʾҴٴ� ���̴�. �ٽ� item����Ʈ�� item�� �ڽ����� �ִ´�.
        {
            itemTr.SetParent(itemListTr, false);
        }
    }
}
