using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 物理挙動クラス
/// </summary>
public class Rigider : MonoBehaviour
{
    [SerializeField,Tooltip("重力値をインスペクタから設定できるように")]
    float pyhs = -1f;
    [SerializeField,Tooltip("最大加速度")]float maxVelocity = 50;
    public Vector3 velocity;
    //地面に着いたかどうか
    public bool isGround { get { return transform.position.y <= 0; } }

    void Update()
    {
        //空中の時重力をかける
        if (velocity.y > 0 || transform.position.y != 0)
        {
            //最大加速度以下なら移動量加算
            if(Mathf.Abs(velocity.y) <= maxVelocity) 
                velocity.y += pyhs;

            if(transform.position.y < 0)
            {
                transform.position = new Vector3(transform.position.x, 0, 0);
                velocity.y = 0;
            }

        }

        //移動
        transform.position += velocity * Time.deltaTime;
    }

    public void AddForce(Vector3 alpha)
    {
        velocity += alpha;
    }
}
