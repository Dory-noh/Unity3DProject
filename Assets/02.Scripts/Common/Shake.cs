using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    public Transform shakeCamera;
    public bool shakeRotate = false;
    private Vector3 originPos;
    private Quaternion originRot;
    void Start()
    {
        originPos = shakeCamera.position;
        originRot = shakeCamera.rotation;
    }
    public IEnumerator ShakeCamera(float duration = 0.05f, float magnitudePos = 0.03f, float magnitudeRot = 0.1f)
    {
        float passTime = 0.0f; //시간 누적할 변수
        while (passTime < duration) //흔들릴 시간 동안 while 루프를 선회
        {
            Vector3 shakePos = Random.insideUnitSphere;
            shakeCamera.position = shakePos * magnitudePos;
            if (shakeRotate) //불규칙한 회적을 사용할 경우라면
            {   //불규칙한 회전 값을 펄린 노이즈 함수를 이용해 추출한다.
                Vector3 shakeRot = new Vector3(0f, 0f, Mathf.PerlinNoise(Time.time*magnitudeRot,0f));
                shakeCamera.rotation = Quaternion.Euler(shakeRot);
            }
            passTime +=Time.deltaTime; //흔들린 시간 누적
            yield return null; //한프레임 쉬고
        }
        shakeCamera.position = originPos;
        shakeCamera.rotation = originRot;
    }
}
