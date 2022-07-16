using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// パーティクルクラス
/// </summary>
public class  Parts : MonoBehaviour
{
    [SerializeField,Tooltip("生成物")] Transform obj = null;
    [Tooltip("ループ")]
    public bool loop = false;
    [Tooltip("重力")]
    public bool isGravity = false;
    [Tooltip("重力値")]
    public float gravity = -0.1f;
    [Tooltip("跳ねるか")]
    public bool bounds = false;
    [Tooltip("最大パーティクル数")]
    public int maxParticles = 20;
    [Tooltip("エフェクトの余命")]
    public float effectTime = 1;
    [Tooltip("パーティクルの余命")]
    public float particlesLifeTime = 1;
    [Tooltip("パーティクルの速度")]
    public float particleSpeed = 1;
    [Tooltip("パーティクルの初ランダム回転")]
    public bool randomRotate = false;
    [Tooltip("パーティクルの回転")]
    public float particleRotate = 0;
    [Tooltip("パーティクル生成最小頻度")]
    public float minTimeInst = 0.01f;
    [Tooltip("パーティクルはランダムサイズ")]
    public bool randomSize = false;

    P[] ps;
    int tale = 0;
    float time = 0;
    float timer = 0;
    float effectTimer = 0;

    /// <summary>
    /// 次に使うパーティクルの位置を返す
    /// </summary>
    int Get { get {
            tale++;
            return tale % maxParticles; 
        } 
    }

    void Start()
    {
        ps = new P[maxParticles];
        for(int i = 0; i < maxParticles; i++)
        {
            float size = (randomSize ? Random.value : 1);
            Instantiate(obj, //生成オブジェ
                transform.position,//位置　　回転↓
           (randomRotate ? Quaternion.Euler(0, 0, Random.Range(0, 360)) : Quaternion.identity),
                transform)//親オブジェ
                .localScale = new Vector3(size,size);

            ps[i] = new P(Random.value + particleSpeed);
        }
        tale = maxParticles / 10;
        //1割は開始
        for (int i = 0; i < tale; i++)
            transform.GetChild(i).gameObject.SetActive(true);
        time = Random.Range(0, 0.2f);
    }

    void Update()
    {
        InstParticles();

        Transform t;
        for(int i = 0; i < transform.childCount; i++)
        {
            //子オブジェの中からアクティブなのを探す
            if (transform.GetChild(i).gameObject.activeSelf)
            {
                t = transform.GetChild(i);

                ps[i].time += Time.deltaTime;
                if (ps[i].time > particlesLifeTime)
                {
                    ps[i].time = 0;
                    t.gameObject.SetActive(false);
                    continue;
                }

                if(isGravity)ps[i].direY += gravity;
                //パーティクルの移動
                t.position += new Vector3(ps[i].direX * Time.deltaTime, ps[i].direY * Time.deltaTime) ;
                //パーティクルの回転
                t.Rotate(0,0,particleRotate * Time.deltaTime);
                if (bounds) 
                    if (t.position.y <= 0)
                    {
                        t.position = new Vector3(t.position.x, 0, 0);
                        ps[i].direY *= -1;
                    }
            }
        }

    }

    /// <summary>
    /// パーティクル生成
    /// </summary>
    void InstParticles()
    {
        effectTimer += Time.deltaTime;
        if (!loop && effectTimer >= effectTime)//エフェクトの寿命
        {
            //パーティクルが生存なら待つ
            foreach (Transform t in transform)
                if (t.gameObject.activeSelf) return;
            //パーティクルなしなら削除
            Destroy(gameObject);
        }
        else if (loop || tale < maxParticles)//寿命までは生成
        {
            timer += Time.deltaTime;
            if (timer >= time)
            {
                timer = 0;
                //次の生成までの時間をランダムで設定
                time = Random.Range(minTimeInst, 0.2f);
                int num = Get;
                transform.GetChild(num).gameObject.SetActive(true);
                transform.GetChild(num).position = transform.position;
            }
        }
    }

    /// <summary>
    /// パーティクルの移動方向と寿命を管理
    /// </summary>
    class P
    {
        public float direX = 0;
        public float direY = 0;
        public float time = 0;
        public P(float speed)
        {
            direX = Random.Range(-1,1f) * speed;
            direY = Random.Range(-1, 1f) * speed;
            time = 0;
        }
    }
}
