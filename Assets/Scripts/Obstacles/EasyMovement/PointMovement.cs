using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointMovement : MonoBehaviour
{
    [SerializeField] Transform movementPointParent;
    List<Transform> movementPointTransforms = new List<Transform>();
    [SerializeField] float moveSpeed;
    [SerializeField] bool isLooping;

    int nowMovementPoint;
    bool isBacking;

    private void Start()
    {
        foreach (Transform child in movementPointParent)
        {
            movementPointTransforms.Add(child);
        }

        nowMovementPoint = 0;
        isBacking = false;
    }

    private void Update()
    {
        if (!InputManager.instance.IsAttractive)
        {
            return;
        }
        //点まで移動する
        transform.position = Vector3.MoveTowards(transform.position, movementPointTransforms[nowMovementPoint].position, Time.deltaTime * moveSpeed);
        //距離が縮まったら、次の点を設定。
        if (Vector3.Distance(transform.position, movementPointTransforms[nowMovementPoint].position) < 0.1f)
        {
            nowMovementPoint += isBacking ? -1 : 1;
            if (isLooping)
            {
                if (nowMovementPoint >= movementPointTransforms.Count)
                {
                    nowMovementPoint = 0;
                }
            }
            else
            {
                if (nowMovementPoint >= movementPointTransforms.Count -1 || nowMovementPoint <= 0)
                {
                    isBacking = !isBacking;
                }
            }
        }
    }
}
