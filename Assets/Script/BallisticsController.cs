using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallisticsController : MonoBehaviour
{
    float _maxShootRange = 100;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        RaycastHit hit;
        //レイを飛ばして、ヒットしたオブジェクトの情報を得る
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, _maxShootRange))
        {
            Debug.Log(hit.collider.gameObject.name);
            if (hit.collider.gameObject.tag == "Target")
            {
                TargetController target = hit.collider.gameObject.GetComponent<TargetController>();
                target.OnHit();
            }
        }
    }
}
