using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class DoorColorRandomizer : MonoBehaviour
{
    private MaterialPropertyBlock _mpb, _mpb2;

    public MeshRenderer renderA;
    public MeshRenderer renderB;

    [Range(0.0f, 1.0f)] public float colorSliderA;
    [Range(0.0f, 1.0f)] public float colorSliderB;

    private static readonly int BaseColorMapSt = Shader.PropertyToID("_BaseColorMap_ST");

    public MaterialPropertyBlock Mpb
    {
        get
        {
            if (_mpb == null)
                _mpb = new MaterialPropertyBlock();

            return _mpb;
        }
    }
    
    public MaterialPropertyBlock Mpb2
    {
        get
        {
            if (_mpb2 == null)
                _mpb2 = new MaterialPropertyBlock();

            return _mpb2;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (renderA != null)
        {
            Mpb.SetVector(BaseColorMapSt, new Vector4(1.0f, 1.0f, colorSliderA, 0.0f));
            renderA.SetPropertyBlock(Mpb);
        }

        if (renderB != null)
        {
            Mpb2.SetVector(BaseColorMapSt, new Vector4(1.0f, 1.0f, colorSliderB, 0.0f));
            renderB.SetPropertyBlock(Mpb2);
        }
    }
}