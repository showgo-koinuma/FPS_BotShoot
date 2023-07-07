using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDOLController : MonoBehaviour
{
    void Start()
    {
        if (FindObjectsOfType<DDOLController>().Length > 1)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
