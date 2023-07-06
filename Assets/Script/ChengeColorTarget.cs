using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.ReorderableList.Element_Adder_Menu;
using UnityEngine;

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
