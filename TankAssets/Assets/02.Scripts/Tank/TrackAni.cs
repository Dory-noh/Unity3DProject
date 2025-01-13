using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackAni : MonoBehaviour
{
    private float scrollSpeed = 1f;
    private MeshRenderer _renderer;
    private TankInput input;
    void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
        input = transform.parent.GetComponent<TankInput>();
    }

    void Update()
    {
                                                 //�ε巯�� x ������ ó���� ��
        var offset = Time.time * scrollSpeed * Input.GetAxisRaw(input.vertical);
                    //Time.time - Input.GetAxisRaw() ���
                    //Time.deltatime - Input.GetAxis() ���
        _renderer.material.SetTextureOffset("_MainTex",new Vector2(0f, offset));
                                            //Diffuse�� �� �̹���
        _renderer.material.SetTextureOffset("_BumpMap", new Vector2(0f, offset));
                                            //������ ǥ���ϴ� Normal Map
        //���ϸ� texture�� �Ʒ��� ���� ó���Ѵ�. -> �������� ���� �ؽ��� ���� ��..
        //_renderer.material.mainTextureOffset = new Vector2();
    }
}
