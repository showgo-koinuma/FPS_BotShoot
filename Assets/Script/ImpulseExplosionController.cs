using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpulseExplosionController : MonoBehaviour
{
    [SerializeField] float _impulsePower = 20f;
    [SerializeField] float _endExplosionTime = 0.2f;
    float _scale;
    float _timer;

    void Start()
    {
        _scale = transform.localScale.x;
    }

    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer > _endExplosionTime)
        {
            float scaleSpeed = _scale * Time.deltaTime / _endExplosionTime;
            transform.localScale -= new Vector3(scaleSpeed, scaleSpeed, scaleSpeed);
            if (transform.localScale.x <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "PlayerCollider")
        {
            GameObject player = other.gameObject.transform.parent.gameObject;
            Rigidbody rb = player.GetComponent<Rigidbody>();
            rb.AddForce((other.gameObject.transform.position - transform.position) * _impulsePower, ForceMode.Impulse);
            player.GetComponent<PlayerController>().RbAddPower();
        }
        Debug.Log(other.gameObject.name);
    }
}
