using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �^�O��Target�̃Q�[���I�u�W�F�N�g��Controller
/// </summary>
public class TargetController : MonoBehaviour
{
    public void OnHit()
    {
        Destroy(gameObject);
    }
}
