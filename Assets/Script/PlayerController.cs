using UnityEngine;
using Cinemachine;
using System.Collections.Generic;

/// <summary>
/// プレイヤーを動かすコンポーネント
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 10;
    [SerializeField] float _jumpPower = 10;
    /// <summary>空中での方向転換のスピード</summary>
    [SerializeField] float _turnSpeed = 3;
    Rigidbody _rb;
    /// <summary>接地判定の距離</summary>
    //float _isGroundedLength = 1.1f;
    bool _jumped;
    float _jumpedTimer;
    bool IsGround;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        //_isGroundedLength = GetComponent<CapsuleCollider>().height / 2 + 0.1f;
    }

    void Update()
    {
        if (_jumped) // 一定時間接地判定にさせない
        {
            _jumpedTimer += Time.deltaTime;
            if (_jumpedTimer > 0.3f)
            {
                _jumped = false;
            }
            IsGround = false;
        }

        // 水平移動処理
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");
        Vector3 dir = new Vector3(h, 0, v); // 移動方向
        dir = Camera.main.transform.TransformDirection(dir); //カメラ基準のベクトルに直す
        dir.y = 0;
        dir = dir.normalized; // 単位化してある水平方向の入力ベクトル
        Vector3 velo = dir * _moveSpeed;
        velo.y = _rb.velocity.y;
        if (!IsGround)
        {
            // 空中でゆっくり方向転換が可能
            velo = _rb.velocity;
            if (dir.magnitude != 0f)
            {
                // 速度の大きさを保持しながら向きを少しずつ変える
                Vector2 startHoriVelo = new Vector2(_rb.velocity.x, _rb.velocity.z);
                float horiMag = startHoriVelo.magnitude;
                if (horiMag < 10f)
                {
                    horiMag = 10;
                }
                Vector2 endHoriVelo = new Vector2(dir.x * horiMag, dir.z * horiMag);
                float turnSpeed = _turnSpeed * Time.deltaTime;
                Vector2 airHoriVelo = endHoriVelo * turnSpeed + startHoriVelo * (1 - turnSpeed);
                velo = new Vector3(airHoriVelo.x, _rb.velocity.y, airHoriVelo.y);
            }
            _rb.velocity = velo;
        }
        else
        {
            if (dir.magnitude == 0f)
            {
                velo.y = -10;
            }

            if (Input.GetButton("Jump"))
            {
                RbAddPower();
                velo.y = _jumpPower;
            }

            _rb.velocity = velo; // 接地中はvelocityを書き換える
        }
    }

    /// <summary>ジャンプなどをしたとき一定時間接地判定にさせないためのメソッド</summary>
    public void RbAddPower()
    {
        _jumpedTimer = 0;
        _jumped = true;
    }

    private void OnTriggerStay(Collider other)
    {
        IsGround = true;
    }
    private void OnTriggerExit(Collider other)
    {
        IsGround = false;
    }
}
