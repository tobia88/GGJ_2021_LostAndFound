using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonClock : MonoBehaviour
{
    public Transform pointSecond;
    [Range(0.0f, 1.0f)] public float damp = 0.5f;

    private Quaternion pointSecondTargetRotation;

    private void Update()
    {
        float angle = (1.0f - Mathf.Clamp01(GameMng.Instance.timePassCurrentTurn / GameMng.Instance.timeDurationPerTurn)) * 360.0f;
        pointSecondTargetRotation = Quaternion.Euler(Vector3.forward * angle);
        pointSecond.transform.localRotation = Quaternion.Slerp(
            pointSecond.transform.localRotation,
            pointSecondTargetRotation,
            damp
        );
    }
}