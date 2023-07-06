using UnityEngine;

/// <summary>�ۑ�p�F���ƃZ�b�g�����}�e���A���ɏ��Ԃɐ؂�ւ��</summary>
public class ChengeColorTarget : TargetController
{
    [SerializeField] Material[] _materials;
    int _colorIndex = 0;

    public override void OnHit(float damage, Collider hitCollider)
    {
        if (_materials.Length == 0) { return; }
        _colorIndex++;
        _colorIndex %= _materials.Length;
        GetComponent<MeshRenderer>().material = _materials[_colorIndex];
    }
}
