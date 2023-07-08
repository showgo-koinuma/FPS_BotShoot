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
        /// <summary>ロードしたいシーンの名前指定</summary>
        Name,
        /// <summary>ロードしたいシーンのインデックス指定</summary>
        BuildIndex,
    }
}
