using UnityEngine;
using DG.Tweening;

/// <summary>points‚ğ‰•œ‚·‚é</summary>
public class DOPathScript : MonoBehaviour
{
    [SerializeField] Vector3[] points;
    [SerializeField] float _speed = 5;
    [SerializeField] LoopType _loopType = LoopType.Yoyo;

    private void Start()
    {
        float dis = 0;
        for (int i = 0; i < points.Length - 1; i++)
        {
            dis += Vector3.Distance(points[i], points[i + 1]);
        }
        transform.DOPath(points, dis / _speed).SetEase(Ease.Linear).SetLoops(-1, _loopType);
    }
}
