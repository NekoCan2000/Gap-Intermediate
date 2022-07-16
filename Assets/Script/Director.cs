using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// ゲーム管理者
/// </summary>
public class Director : MonoBehaviour
{
    [SerializeField,Tooltip("的")] GameObject target = null;
    [SerializeField,Tooltip("スコア表示テキスト")] Text scoreText = null;
    [SerializeField, Tooltip("時間表示テキスト")] Text timeText = null;
    public UnityEvent toResult; //結果表示イベント
    public static float cameraDirection = 0; //カメラが動く方向
    int score = 0;
    int nowScore = 0;
    float timer = 0;
    float time = 0;
    float countDown = 0;
    Coroutine scoreCoroutine = null;
    int mode = 0;

    void Update()
    {
        if(mode == 1)
        {
            //的生成
            time += Time.deltaTime;
            if (time >= timer)
            {
                time = 0;
                RandomTime();
                InstTarget();
            }

            //カウントダウン
            countDown -= Time.deltaTime;
            if((int)countDown <= 0)
            {
                toResult.Invoke();
            }
            timeText.text = (int)countDown + "";
        }
        
    }
    void GameStart()
    {
        time = 0;
        nowScore = score = 0;
        countDown = 30;
        RandomTime();
        InstTarget();
        Point(0);
    }
    void RandomTime()
    {
        timer = Random.Range(1, 5);
    }
    void InstTarget()
    {
        //カメラより先の場所で的生成
        Instantiate(target,
            Camera.main.transform.position + Vector3.right * 35,
            Quaternion.identity);
    }

    public void Point(int point)
    {
        if (mode != 1) return;
        score += point * 10;
        if (scoreCoroutine == null)
            scoreCoroutine = StartCoroutine(Scorerer());
    }
    //スコアが変化するコルーチン
    IEnumerator Scorerer()
    {
        while (nowScore <= score)
        {
            scoreText.text = "Score:" + nowScore++;
            yield return null;
        }
        scoreCoroutine = null;
    }
    //インスペクタから参照
    public void ChangeMode(int mode)
    {
        this.mode = mode;
        switch (mode)
        {
            case 0:
                break;

            case 1:
                GameStart();
                break;

            case 2:
                break;
        }
    }
}
