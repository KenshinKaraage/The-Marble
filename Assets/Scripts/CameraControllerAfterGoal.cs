using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerAfterGoal : MonoBehaviour
{
    public bool IsActive { set; get; }

    [SerializeField] Vector3 startPosition;
    [SerializeField] float moveSpeed;
    [SerializeField] float sensitivity;
    private void Update()
    {
        if (!IsActive)
        {
            return;
        }

        //ˆÊ’uˆÚ“®
        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 moveVector = (transform.forward * moveDir.z + transform.right * moveDir.x).normalized * moveSpeed * Time.deltaTime * sensitivity;

        transform.position += moveVector;

        if (Input.GetKey(KeyCode.Space))
        {
            transform.position += Vector3.up * moveSpeed * Time.deltaTime * sensitivity;
        }
        else if(Input.GetKey(KeyCode.LeftShift))
        {
            transform.position -= Vector3.up * moveSpeed * Time.deltaTime * sensitivity;
        }

        //‰ñ“]
        float eulerX = transform.eulerAngles.x;
        eulerX -= Input.GetAxisRaw("Mouse Y") * sensitivity;

        float eulerY = transform.eulerAngles.y;
        eulerY += Input.GetAxisRaw("Mouse X") * sensitivity;

        transform.eulerAngles = new Vector3(eulerX, eulerY, transform.eulerAngles.z);
    }

    public void ResetPosition()
    {
        transform.position = startPosition;
    }
}
