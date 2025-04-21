using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 5, -10);
    public float smoothSpeed = 1.0f;

    private void LateUpdate()
    {
        //LateUpdate �� ���� ī�޶� �÷��̾��� �̵��� ��� ó���� ���Ŀ� ���󰡰� �ϱ� ����
        Vector3 desiredposition = target.position + offset;
        Vector3 smoothPosition = Vector3.Lerp(transform.position, desiredposition, smoothSpeed);
        transform.position = smoothPosition;

        transform.LookAt(target.position);
    }
}
