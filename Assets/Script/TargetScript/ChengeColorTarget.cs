using UnityEngine;

/// <summary>課題用：撃つとセットしたマテリアルに順番に切り替わる</summary>
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
