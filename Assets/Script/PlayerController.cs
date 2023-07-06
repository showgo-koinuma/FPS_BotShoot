using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    float _isGroundedLength;
    /// <summary>空中での方向転換のスピード</summary>
    float _turnSpeed = 3;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _isGroundedLength = GetComponent<CapsuleCollider>().height / 2 + 0.1f;
    }

    void Update()
    {
        // 水平移動処理
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");
        Vector3 dir = new Vector3(h, 0, v); // 移動方向
        dir = Camera.main.transform.TransformDirection(dir); //カメラ基準のベクトルに直す
        dir.y = 0;
        dir = dir.normalized;
        Vector3 velo = dir * _moveSpeed;
        velo.y = _rb.velocity.y;
        if (!IsGround())
        {
            // 空中でゆっくり方向転換が可能
            velo = _rb.velocity;
            if (!(dir.magnitude == 0f))
            {
                // 速度の大きさを保持しながら向きを少しずつ変える
                Vector2 airDir = Vector2.Lerp(new Vector2(_rb.velocity.x, _rb.velocity.z), new Vector2(dir.x, dir.z) * _rb.velocity.magnitude, Time.deltaTime * _turnSpeed); // Slerp使えばmagnitudeを維持できるけど...
                velo = new Vector3(airDir.x, _rb.velocity.y, airDir.y);
            }
            _rb.velocity = velo;
        }
        else
        {
            _rb.velocity = velo;
        }

        // Jump処理
        if (Input.GetButtonDown("Jump") && IsGround())
        {
            _rb.AddForce(Vector3.up * _jumpPower, ForceMode.Impulse);
        }
    }

    /// <summary>地面に接地しているかを判定する</summary>
    /// <returns></returns>
    bool IsGround()
    {
        Vector3 start = transform.position;
        Vector3 end = start + Vector3.down * _isGroundedLength;
        Debug.DrawLine(start, end);
        bool isGround = Physics.Linecast(start, end);
        return isGround;
    }
}
