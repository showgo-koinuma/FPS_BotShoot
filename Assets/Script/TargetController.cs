using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// タグがTargetのゲームオブジェクトのController
/// </summary>
public class TargetController : MonoBehaviour
{
    public void OnHit()
    {
        Destroy(gameObject);
    }
}
