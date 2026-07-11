using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CameraController : MonoBehaviourPunCallbacks
{
    [SerializeField] Vector3 offset;
    [SerializeField] Transform followerTransform;
    [SerializeField] float sensitivity;
    [SerializeField] float cameraRotationMinX;
    [SerializeField] float cameraRotationMaxX;

    private void Update()
    {
        if (!CameraControllerManager.instance.IsAttractive)
        {
            return;
        }
        if (photonView.IsMine)
        {
            //まずビューポイントを回転させる
            Vector3 pos = followerTransform.position + offset;

            //角度のXを求める
            float eulerX = Camera.main.gameObject.transform.eulerAngles.x;
            eulerX -= Input.GetAxisRaw("Mouse Y") * sensitivity;

            if (eulerX < 0f || eulerX > 180f)
            {
                eulerX = Mathf.Max(eulerX, cameraRotationMinX);

            }
            else
            {
                eulerX = Mathf.Min(eulerX, cameraRotationMaxX);
            }
            //角度のXを増やす
            Quaternion angleAxis = Quaternion.Euler(eulerX, followerTransform.eulerAngles.y, 0f);

            pos -= followerTransform.position;
            pos = angleAxis * pos;
            pos += followerTransform.position;

            Camera.main.gameObject.transform.position = pos;

            float eulerY = followerTransform.eulerAngles.y;

            


            Camera.main.gameObject.transform.eulerAngles = new Vector3(
                eulerX,
                eulerY,
                Camera.main.gameObject.transform.eulerAngles.z
                );
        }
    }

}
