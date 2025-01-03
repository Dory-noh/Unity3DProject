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
        float passTime = 0.0f; //�ð� ������ ����
        while (passTime < duration) //��鸱 �ð� ���� while ������ ��ȸ
        {
            Vector3 shakePos = Random.insideUnitSphere;
            shakeCamera.position = shakePos * magnitudePos;
            if (shakeRotate) //�ұ�Ģ�� ȸ���� ����� �����
            {   //�ұ�Ģ�� ȸ�� ���� �޸� ������ �Լ��� �̿��� �����Ѵ�.
                Vector3 shakeRot = new Vector3(0f, 0f, Mathf.PerlinNoise(Time.time*magnitudeRot,0f));
                shakeCamera.rotation = Quaternion.Euler(shakeRot);
            }
            passTime +=Time.deltaTime; //��鸰 �ð� ����
            yield return null; //�������� ����
        }
        shakeCamera.position = originPos;
        shakeCamera.rotation = originRot;
    }
}
