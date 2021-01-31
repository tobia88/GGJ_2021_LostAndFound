using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonClock : MonoBehaviour
{
    private MeshRenderer _bgRender;
    public Transform hand;
    [Range(0.0f, 1.0f)]
    public float damp = 0.5f;

    public int scale = 12;

    public bool byScale = false;

    public MeshRenderer bgRender
    {
        get
        {
            if (_bgRender == null)
                _bgRender = GetComponent<MeshRenderer>();
            
            return _bgRender;
        }
    }
    
    [SerializeField]
    private int _curRotationInt;
    private Quaternion _targetRotation;

    public int CurRotationInt
    {
        get => _curRotationInt;
        set
        {
            if( _curRotationInt == value )
                return;

            _curRotationInt = value;
            float percentage = (float) _curRotationInt / scale;
            _targetRotation = Quaternion.Euler(Vector3.forward * percentage * 360f);
        }
    }
    private void Update()
    {
        if( GameMng.Instance == null )
            return;

        float percentage = 1.0f - Mathf.Clamp01(GameMng.Instance.timePassCurrentTurn / 60.0f);


        if (byScale)
        {
            CurRotationInt = Mathf.FloorToInt(percentage * scale);
        }
        else
        {
            _targetRotation = Quaternion.Euler(Vector3.forward * percentage * 360f);
        }
        
        hand.localRotation = Quaternion.Slerp(hand.localRotation, _targetRotation, damp);
    }
}
