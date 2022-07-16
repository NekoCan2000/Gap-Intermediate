using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 背景スクロール調整クラス
/// </summary>
public class Backgrounder : MonoBehaviour
{
    [SerializeField,Tooltip("0～1で背景が動く倍率を変化させる")]
    float ratio = 0;
    [SerializeField,Tooltip("ワープする距離")]
    float transDist = 0;
    float maxPlayerDist = 0;//ワープ条件の距離
    float timer = 1;
    float dist = 0;

    void Start()
    {
        maxPlayerDist = transDist / 2;
    }

    void Update()
    {
        //プレイヤーの移動方向によって左右移動
        transform.position +=
            (Vector3.right * Director.cameraDirection) * ratio * Time.deltaTime;

        //一定時間毎に距離計測
        timer += Time.deltaTime;
        if (timer > 0.4)
        {
            timer = 0;
            dist = Camera.main.transform.position.x - transform.position.x;

            if (Mathf.Abs(dist) >= maxPlayerDist)//指定距離離れたらテレポート
            {
                Vector3 treePosition = transform.position;
                treePosition.x += transDist * Mathf.Sign(dist);
                transform.position = treePosition;
            }
        }
    }
}
