using System.Collections;
using UnityEngine;

[RequireComponent (typeof(Rigidbody), (typeof(Collider)))]
public class ImpulseGrenadeController : MonoBehaviour
{
    [SerializeField] float _speed = 10f;
    /// <summary>‚­‚Á‚Â‚¢‚Ä‚©‚ç”š”­‚·‚é‚Ü‚Å‚ÌŽžŠÔ</summary>
    [SerializeField] float _boomTime = 0.25f;
    [SerializeField] GameObject _impulseImpactObject;
    [SerializeField] AudioClip _throwSound;
    Rigidbody _rb;
    Vector3 _stickPos;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.AddForce(Camera.main.transform.forward * _speed, ForceMode.Impulse);
        DDOLGameManagerController.instans.GetComponent<AudioSource>().PlayOneShot(_throwSound);
    }

    private void OnCollisionEnter(Collision collision)
    {
        _stickPos = collision.contacts[0].point;
        StartCoroutine(nameof(Stick));
    }

    IEnumerator Stick()
    {
        _rb.isKinematic = true;
        yield return new WaitForSeconds(_boomTime);
        if (_impulseImpactObject)
        {
            Instantiate(_impulseImpactObject).transform.position = _stickPos;
        }
        Destroy(gameObject);
    }
}
