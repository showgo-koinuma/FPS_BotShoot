using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>�e�������̃R���|�[�l���g</summary>
public class BallisticsController : MonoBehaviour
{
    [SerializeField] int _maxBullets = 30;
    [SerializeField] float _reloadTime = 2;
    [SerializeField] int _damage = 20;
    [SerializeField] float _shootInterval = 0.12f;
    [SerializeField] TextMeshProUGUI _maxBulletsText; // �}�K�W���T�C�Y�e�L�X�g
    [SerializeField] TextMeshProUGUI _remainingBulletsText; // �c�e�\���i���j
    int _remainingBullets;
    float _maxShootRange = 100;
    bool _canShoot = true;
    bool _reloading;

    private void Start()
    {
        _remainingBullets = _maxBullets;
        _maxBulletsText.text = "/ " + _maxBullets.ToString();
    }

    void Update()
    {
        if (Input.GetButton("Fire1") && _canShoot)
        {
            if (_remainingBullets > 0)
            {
                Shoot();
            }
            else if (!_reloading) // �c�e0�̂Ƃ��̓����[�h�ɓ���
            {
                StartCoroutine(nameof(Reload));
            }
        }
        if (Input.GetButtonDown("Reload") && _remainingBullets < _maxBullets && !_reloading)
        {
            StartCoroutine(nameof(Reload)); // �����[�h���Ă�
        }


        // �c�e�\���i���j
        _remainingBulletsText.text = _remainingBullets.ToString();
    }

    /// <summary>�e������</summary>
    void Shoot()
    {
        RaycastHit hit;
        //���C���΂��āA�q�b�g�����I�u�W�F�N�g�̏��𓾂�
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, _maxShootRange))
        {
            Debug.Log(hit.collider.gameObject.name);
            if (hit.collider.gameObject.tag == "Enemy")
            {
                TargetController target = hit.collider.gameObject.GetComponent<TargetController>();
                target.OnHit(_damage, hit.collider); // �q�b�g�����I�u�W�F�N�g��OnHit���Ă�
            }
        }
        _remainingBullets--;
        StartCoroutine(nameof(RapidFire));
    }

    /// <summary>�����[�h����</summary>
    IEnumerator Reload()
    {
        Debug.Log("�����[�h");
        _reloading = true;
        _canShoot = false;
        yield return new WaitForSeconds(_reloadTime);
        _canShoot = true;
        _remainingBullets = _maxBullets;
        _reloading = false;
    }

    /// <summary>�A�ˑ��x����</summary>
    IEnumerator RapidFire()
    {
        _canShoot = false;
        yield return new WaitForSeconds(_shootInterval);
        _canShoot = true; // _shootInterval�b�����҂���true��Ԃ�
    }
}
