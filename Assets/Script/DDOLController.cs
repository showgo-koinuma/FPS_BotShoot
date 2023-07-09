using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDOLController : MonoBehaviour
{
    public static DDOLController instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

    }
}
