using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アニメーションクラス
/// </summary>
public class Anim : MonoBehaviour
{
    [SerializeField,Tooltip("アニメーションの更新時間")] float updateTime = 0;
    [SerializeField,Tooltip("アニメーション")] Sprite[] anims = null;
    //画像表示クラス
    SpriteRenderer sr;
    float time = 0;
    [System.NonSerialized]public int num;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        if (anims == null || anims.Length == 0)
        {
            enabled = false;
            return;
        }
    }
    void OnEnable()
    {
        sr.sprite = anims[0];
    }

    //任意のタイミングでアニメーション
    public void Updater()
    {
        time += Time.deltaTime;
        if(time >= updateTime)
        {
            time = 0;
            sr.sprite = anims[num++];
            if (num >= anims.Length) num = 0;
        }
    }
    public void ChangePic(int n)
    {
        num = n;
        sr.sprite = anims[num];
    }
}
