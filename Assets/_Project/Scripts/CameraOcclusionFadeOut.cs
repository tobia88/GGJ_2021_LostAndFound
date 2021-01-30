using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOcclusionFadeOut : MonoBehaviour
{
    private Material _oriMat;
    private MeshRenderer _renderer;

    public Material fadeoutMat;

    // public bool isFadingOut;
    private float fadingSpeed = 1000.0f;
    float fadingAlpha = 0.25f;

    public float currentAlpha = 0.0f;
    public float targetAlpha = 1.0f;
    
    private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");

    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
        _oriMat = _renderer.sharedMaterial;
        currentAlpha = 1.0f;
    }

    public void SetFadeout(bool value )
    {
        // isFadingOut = value;
        targetAlpha = (value) ? fadingAlpha : 1.0f;
        
        if( value ) 
            _renderer.material = fadeoutMat;
    }

    private void Update()
    {
        currentAlpha = Mathf.MoveTowards(currentAlpha, targetAlpha, fadingSpeed * Time.deltaTime);
        
        if (Mathf.Abs(currentAlpha - targetAlpha) <= float.Epsilon)
        {
            currentAlpha = targetAlpha;

            if (currentAlpha >= 1.0f)
            {
                _renderer.material = _oriMat;
                return;
            }
        }
        
        var c = _renderer.material.GetColor(BaseColor);
        c.a = currentAlpha;
        _renderer.material.SetColor(BaseColor, c);
    }
}
