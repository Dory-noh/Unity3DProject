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
                                                 //부드러움 x 빠르게 처리할 때
        var offset = Time.time * scrollSpeed * Input.GetAxisRaw(input.vertical);
                    //Time.time - Input.GetAxisRaw() 사용
                    //Time.deltatime - Input.GetAxis() 사용
        _renderer.material.SetTextureOffset("_MainTex",new Vector2(0f, offset));
                                            //Diffuse로 된 이미지
        _renderer.material.SetTextureOffset("_BumpMap", new Vector2(0f, offset));
                                            //윤곽을 표현하는 Normal Map
        //단일맵 texture면 아래와 같이 처리한다. -> 울퉁불퉁 윤곽 텍스쳐 없을 때..
        //_renderer.material.mainTextureOffset = new Vector2();
    }
}
