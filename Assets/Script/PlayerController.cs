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
    Rigidbody _rb;
    /// <summary>接地判定の距離</summary>
    //float _isGroundedLength = 1.1f;
    /// <summary>空中での方向転換のスピード</summary>
    float _turnSpeed = 3;
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
        if (_jumped)
        {
            _jumpedTimer += Time.deltaTime;
            if (_jumpedTimer > 0.2f)
            {
                _jumped = false;
                _jumpedTimer = 0;
            }
            IsGround = false;
        }

        // 水平移動処理
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");
        Vector3 dir = new Vector3(h, 0, v); // 移動方向
        dir = Camera.main.transform.TransformDirection(dir); //カメラ基準のベクトルに直す
        dir.y = 0;
        dir = dir.normalized;
        Vector3 velo = dir * _moveSpeed;
        velo.y = _rb.velocity.y;
        if (!IsGround)
        {
            // 空中でゆっくり方向転換が可能
            velo = _rb.velocity;
            if (!(dir.magnitude == 0f))
            {
                // 速度の大きさを保持しながら向きを少しずつ変える
                Vector2 airDir = Vector2.Lerp(new Vector2(_rb.velocity.x, _rb.velocity.z), new Vector2(dir.x, dir.z) * _rb.velocity.magnitude, Time.deltaTime * _turnSpeed);
                velo = new Vector3(airDir.x, _rb.velocity.y, airDir.y);
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
                _jumped = true;
                _jumpedTimer = 0;
                velo.y = _jumpPower;
            }

            _rb.velocity = velo; // 接地中はvelocityを書き換える
        }

        // Jump処理
        //if (Input.GetButtonDown("Jump") && IsGround)
        //{
        //    _jumped = true;
        //    _jumpedTimer = 0;
        //    _rb.AddForce(Vector3.up * _jumpPower, ForceMode.Impulse);
        //}
    }

    /// <summary>接地しているかを判定する</summary>
    /// <returns></returns>
    //bool IsGround()
    //{
    //    Vector3 start = transform.position;
    //    Vector3 end = start + Vector3.down * _isGroundedLength;
    //    Debug.DrawLine(start, end);
    //    bool isGround = Physics.Linecast(start, end);
    //    return isGround;
    //}

    private void OnTriggerStay(Collider other)
    {
        IsGround = true;
    }
    private void OnTriggerExit(Collider other)
    {
        IsGround = false;
    }
}
