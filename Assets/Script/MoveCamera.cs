using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// カメラ移動クラス
/// </summary>
public class MoveCamera : MonoBehaviour
{
    //追従対象
    public Transform target;

    void Update()
    {
        //滑らかに追従
        transform.position = Vector3.Lerp(
            transform.position, 
            new Vector3(target.position.x,target.position.y,transform.position.z),
            Time.deltaTime);
        //カメラの移動方向を代入
        Director.cameraDirection = target.position.x - transform.position.x;
    }
}
