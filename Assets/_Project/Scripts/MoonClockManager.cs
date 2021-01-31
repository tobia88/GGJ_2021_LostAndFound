using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[ExecuteAlways]
public class MoonClockManager : MonoBehaviour
{
    private Material _bgSharedMat;
    
    public List<MoonClock> moonClocks = new List<MoonClock>();

    [Range(1.0f, 1000.0f)]
    public float intensity = 8.0f;
    private static readonly int EmissiveColorID = Shader.PropertyToID("_EmissiveColor");

    public Material bgSharedMat
    {
        get
        {
            if (_bgSharedMat != null)
                return _bgSharedMat;
            
            if (moonClocks.Count > 0)
                _bgSharedMat = moonClocks[0].bgRender.sharedMaterial;

            return _bgSharedMat;
        }
    }

    [ContextMenu("Setup Moonclocks")]
    public void SetupMoonClocks()
    {
        #if UNITY_EDITOR
            var scn = SceneManager.GetActiveScene();
            var objs = scn.GetRootGameObjects();

            foreach (var obj in objs)
            {
                var tmpMoonClocks = obj.GetComponentsInChildren<MoonClock>();
                Debug.Log($"MoonClock Count: {tmpMoonClocks.Length}");

                foreach (var clock in tmpMoonClocks)
                {
                    if( !moonClocks.Contains(clock))
                        moonClocks.Add(clock);
                }
            }
            
            UnityEditor.EditorUtility.SetDirty(gameObject);
        #endif
    }

    private void Update()
    {
        if( bgSharedMat == null )
            return;

        var color = bgSharedMat.GetColor(EmissiveColorID);
        var rawIntensity = (color.r + color.g + color.b) / 3f;
        var rawColor = color * 1f / rawIntensity;
        var newColor = rawColor * intensity * intensity;
        bgSharedMat.SetColor(EmissiveColorID, newColor);
    }
}
