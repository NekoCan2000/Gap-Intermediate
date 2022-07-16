using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーコントローラー
/// </summary>
public class Player : MonoBehaviour
{
    [SerializeField,Tooltip("移動速度")] float speed = 1;
    [SerializeField,Tooltip("移動アニメーション")] Anim animMove = null;
    [SerializeField,Tooltip("停止画像")] SpriteRenderer animStop = null;
    [SerializeField,Tooltip("ジャンプアニメーション")] Anim animJump = null;
    [SerializeField,Tooltip("矢継ぎアニメーション")] Anim animShot = null;
    [SerializeField,Tooltip("腕の回転位置")] Transform armJoint = null;
    [SerializeField,Tooltip("生成矢")] Arrow arrow = null;
    [SerializeField, Tooltip("最大エフェクト")] Parts maxPowEffect = null;
    //重力
    Rigider rigid;
    Parts powEffect = null;
    float power = 0;
    enum Mode
    {
        Stop, Walk, Jump
    }
    Mode mode = Mode.Stop;

    void Start()
    {
        rigid = GetComponent<Rigider>();
    }

    void Update()
    {
        InputCheck();

        AnimUpdate();

        RotateArm();
    }

    void InputCheck()
    {
        float dire = 0;

        //右矢印入力
        if (Input.GetKey(KeyCode.RightArrow))
        {
            dire = speed;
            mode = Mode.Walk;
        }
        //左矢印入力
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            dire = -speed;
            mode = Mode.Walk;
        }

        if (rigid.isGround)//地に足のついた
        {
            if (dire == 0) //移動していないならSTOP
            {
                mode = Mode.Stop;
            }
            //上矢印入力
            if (Input.GetKey(KeyCode.UpArrow))
            {
                mode = Mode.Jump;

                //移動
                rigid.AddForce(new Vector3(0,25));
            }
        }
        else//空中
        {
            mode = Mode.Jump;
            animJump.ChangePic(rigid.velocity.y > 0 ? 0 : 1);
        }

        //マウスクリック
        if (Input.GetMouseButton(0))
        {
            if (power < 1)
                power += Time.deltaTime;
            else
            {
                power = 1;
                if (powEffect == null) 
                    powEffect = Instantiate(maxPowEffect,
                        armJoint.position, Quaternion.identity,
                        transform);
            }
            //長押しの時間で表示する画像を変更
            animShot.ChangePic((int)(power * 10));
        }
        else if (Input.GetMouseButtonUp(0))
        {
            //矢を生成
            Arrow a = Instantiate(arrow, armJoint.position + armJoint.right * 2 , armJoint.localRotation);
            a.Speed = power * 40;
            power = 0;
            animShot.ChangePic(0);
            if(powEffect != null)Destroy(powEffect.gameObject);
        }

        //各アニメーションの向きを設定
        animStop.transform.localScale = 
            animMove.transform.localScale = 
            animJump.transform.localScale =
            new Vector3((dire != 0 ? Mathf.Sign(dire) : animStop.transform.localScale.x), 1, 1);

        //移動
        rigid.velocity.x = dire;

        //アニメーションの表示非表示
        animStop.enabled = mode == Mode.Stop;
        animMove.gameObject.SetActive(mode == Mode.Walk);
        animJump.gameObject.SetActive(mode == Mode.Jump);
    }

    void AnimUpdate()
    {
        switch (mode)
        {
            case Mode.Walk:
                animMove.Updater();
                break;
        }
    }

    void RotateArm()
    {
        //マウスとプレイヤーの位置を算出
        Vector3 toDirec = 
            (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)armJoint.position;
        //マウスの方向に腕を回転
        armJoint.localRotation = Quaternion.FromToRotation(Vector3.right,toDirec);
        //マウスがプレイヤーより左にあるなら画をY反転
        armJoint.transform.localScale = new Vector3(1, Mathf.Sign(toDirec.x), 1);

    }
}
