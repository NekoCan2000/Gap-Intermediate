using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 当たり判定クラス
/// </summary>
public class Col : MonoBehaviour
{
    //当たり判定半径
    public float radius;
    //位置調整
    public Vector3 offset;
    /// <summary>
    /// 当たり判定
    /// </summary>
    /// <param name="col">検証コライダー</param>
    /// <returns>是非</returns>
    public bool CheckHit(Col col)
    {
        return (transform.position + offset - col.transform.position + col.offset).sqrMagnitude
            < (radius + col.radius) * (radius + col.radius);
    }
    /// <summary>
    /// 衝突時に呼ばれる関数
    /// </summary>
    public virtual void OnHit()
    {
        print(gameObject.name);
    }

    /// <summary>
    /// 破壊アニメーション
    /// </summary>
    /// <returns></returns>
    protected IEnumerator DestroyCoroutine()
    {
        //マテリアル取得
        Material mat = GetComponentInChildren<SpriteRenderer>().material;
        float t = 0;
        //シェーダを利用した破壊アニメーション
        while (t < 1)
        {
            mat.SetFloat("_Threshold", t);
            t += Time.deltaTime * 2;
            yield return null;
        }

        Destroy(gameObject);
    }
}
