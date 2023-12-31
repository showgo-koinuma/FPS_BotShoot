using TMPro;
using UnityEngine;

public class DamageCountBotController : TargetController
{
    [SerializeField] float _countResetTime = 3;
    Collider[] _botCollider;
    TextMeshProUGUI _damageCountText;
    float _damageCount = 0;
    float _damageTimer;
    bool _counting;

    private void Start()
    {
        _botCollider = GetComponents<Collider>();
        _damageCountText = transform.Find("Canvas").Find("DamageCounterText").GetComponent<TextMeshProUGUI>();
        _damageCountText.text = _damageCount.ToString("000");
    }

    private void Update()
    {
        if (_counting)
        { 
            _damageTimer += Time.deltaTime; 
            if (_damageTimer > _countResetTime)
            {
                _damageCount = 0;
                _damageCountText.text = _damageCount.ToString("000");
                _counting = false;
            }
        }
    }

    public override bool OnHit(float damage, Collider hitCollider)
    {
        _counting = true;
        _damageTimer = 0;
        if (hitCollider == _botCollider[1]) // 頭に当たったとき
        {
            _damageCount += damage * 2.5f;
            _damageCountText.text = _damageCount.ToString("000");
            return true;
        }
        else // それ以外に当たったとき
        {
            _damageCount += damage;
            _damageCountText.text = _damageCount.ToString("000");
            return false;
        }
    }
}
