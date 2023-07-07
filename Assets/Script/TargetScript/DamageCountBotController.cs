using TMPro;
using UnityEngine;

public class DamageCountBotController : TargetController
{
    Collider[] _botCollider;
    TextMeshProUGUI _damageCountText;
    float _damageCount = 0;

    private void Start()
    {
        _botCollider = GetComponents<Collider>();
        _damageCountText = transform.Find("Canvas").Find("DamageCounterText").GetComponent<TextMeshProUGUI>();
        _damageCountText.text = _damageCount.ToString("000");
    }

    public override void OnHit(float damage, Collider hitCollider)
    {
        if (hitCollider == _botCollider[1]) // 頭に当たったとき
        {
            _damageCount += damage * 2.5f;
        }
        else // それ以外に当たったとき
        {
            _damageCount += damage;
        }
        _damageCountText.text = _damageCount.ToString("000");
    }
}
