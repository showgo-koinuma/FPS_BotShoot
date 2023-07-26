using System.Collections;
using UnityEngine;

[RequireComponent (typeof(Rigidbody), (typeof(Collider)))]
public class ImpulseGrenadeController : MonoBehaviour
{
    [SerializeField] float _speed = 10f;
    /// <summary>‚­‚Á‚Â‚¢‚Ä‚©‚ç”š”­‚·‚é‚Ü‚Å‚ÌŽžŠÔ</summary>
    [SerializeField] float _boomTime = 0.25f;
    [SerializeField] GameObject _impulseImpactObject;
    Rigidbody _rb;
    Vector3 _stickPos;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.AddForce(Camera.main.transform.forward * _speed, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        _stickPos = collision.contacts[0].point;
        StartCoroutine(nameof(Stick));
    }

    IEnumerator Stick()
    {
        _rb.velocity = Vector3.zero;
        yield return new WaitForSeconds(_boomTime);
        if (_impulseImpactObject)
        {
            Instantiate(_impulseImpactObject).transform.position = _stickPos;
        }
        Destroy(gameObject);
    }
}
