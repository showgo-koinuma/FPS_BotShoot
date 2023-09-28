using UnityEngine;

/// <summary>課題用：撃つとセットしたマテリアルに順番に切り替わる</summary>
public class ChengeColorTarget : TargetController
{
    public override bool OnHit(float damage, Collider hitCollider)
    {
        ScoreManager.Instance.ResetRanking();
        return false;
    }
}
