using UnityEngine;

/// <summary>�ۑ�p�F���ƃZ�b�g�����}�e���A���ɏ��Ԃɐ؂�ւ��</summary>
public class ChengeColorTarget : TargetController
{
    public override bool OnHit(float damage, Collider hitCollider)
    {
        ScoreManager.Instance.ResetRanking();
        return false;
    }
}
