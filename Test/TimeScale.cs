using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScale : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    [SerializeField] float time;
    private void Update()
    {
        Time.timeScale = time;
    }
}
