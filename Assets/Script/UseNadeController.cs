using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseNadeController : MonoBehaviour
{
    [SerializeField] GameObject _nadeObject;
    [SerializeField] float _coolDown = 3f;
    float coolDownTimer = 3;

    void Update()
    {
        coolDownTimer += Time.deltaTime;
        if (Input.GetButtonDown("Fire2"))
        {
            if (coolDownTimer >= _coolDown)
            {
                Instantiate(_nadeObject).transform.position = Camera.main.transform.position + Camera.main.transform.forward;
                coolDownTimer = 0;
            }
            else
            {
                // cool downíÜÇÃèàóù
                Debug.Log("cool down now");
            }
        }
    }
}
