using UnityEngine;

/// <summary>
/// �^�O��Target�̃Q�[���I�u�W�F�N�g��Controller
/// </summary>
public abstract class TargetController : MonoBehaviour
{
    /// <summary>tag��Target����hit�����Ƃ��Ă΂��</summary>
    /// <param name="damage"></param>
    /// <param name="hitCollider"></param>
    public abstract bool OnHit(float damage, Collider hitCollider);
}
