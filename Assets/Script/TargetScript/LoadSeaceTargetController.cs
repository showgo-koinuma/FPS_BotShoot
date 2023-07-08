using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSeaceTargetController : TargetController
{
    [SerializeField] Format _format;
    [SerializeField] string _name;
    [SerializeField] int _index;

    public override void OnHit(float damage, Collider hitCollider)
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
