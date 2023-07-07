using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// タグがTargetのゲームオブジェクトのController
/// </summary>
public abstract class TargetController : MonoBehaviour
{
    /// <summary>tagがTargetだとhitしたとき呼ばれる</summary>
    /// <param name="damage"></param>
    /// <param name="hitCollider"></param>
    public abstract void OnHit(float damage, Collider hitCollider);
}
