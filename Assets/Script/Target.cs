using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 的クラス
/// </summary>
public class Target : Col
{
    [SerializeField] Parts effect = null;
    public int point { get; set; }
    Rigider rigid;
    Coroutine c;
    float jump;
    private void Start()
    {
        rigid = GetComponent<Rigider>();
        point = Random.Range(0, 10) + 1;

        //pintに比例して最低5最大30で跳ねる
        jump = point * 3;
        if (jump < 5) jump = 5;

        Manager.AddTargets(this);

        rigid.AddForce(Vector3.left * point / 2);
    }

    void Update()
    {
        //地面に着いたら跳ねる
        if (rigid.isGround) rigid.AddForce(new Vector3(0,jump));
        //一定距離離れたら削除
        if (Camera.main.transform.position.x - transform.position.x > 30)
        {
            if(c == null)c = StartCoroutine( RemoveList());
        }
    }

    IEnumerator RemoveList()
    {
        Manager.RemoveTargets(this);
        //Managerクラスでのエラー対策で1フレーム遅らせる
        yield return null; 
        Destroy(gameObject);
    }

    public override void OnHit()
    {
        //当たり判定削除
        Manager.HitTargets(this);
        //ランダム回転
        transform.GetChild(0).Rotate(0,0,Random.Range(0, 360));
        //破壊アニメーション
        StartCoroutine(DestroyCoroutine());
        //破壊パーティクル
        Parts p = Instantiate(effect, transform.position, transform.rotation,transform);
        p.particleSpeed = point;
    }

}
