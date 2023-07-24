using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsGroundManager : MonoBehaviour
{
    public bool IsGround { get; private set; }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("hit");
    }
}
