using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSeaceTargetController : TargetController
{
    [SerializeField] Format _format;
    [SerializeField] string _name;
    [SerializeField] int _index;
    [SerializeField] GameObject _hitObj;

    private void Start()
    {
        _hitObj.SetActive(false);
    }

    public override bool OnHit(float damage, Collider hitCollider)
    {
        Invoke(nameof(Load), 0.5f);
        _hitObj.SetActive(true);
        return false;
    }

    void Load()
    {
        if (_format == Format.Name)
        {
            SceneManager.LoadScene(_name);
        }
        else
        {
            SceneManager.LoadScene(_index);
        }
    }

    enum Format
    {
        /// <summary>���[�h�������V�[���̖��O�w��</summary>
        Name,
        /// <summary>���[�h�������V�[���̃C���f�b�N�X�w��</summary>
        BuildIndex,
    }
}
