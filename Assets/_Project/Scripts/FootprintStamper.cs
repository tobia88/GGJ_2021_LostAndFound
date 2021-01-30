using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootprintStamper : MonoBehaviour
{
    private Vector3 _lastIdleFootprintPos;
    
    public static List<GameObject> All = new List<GameObject>();
    
    public Transform stampTransLeft;
    public Transform stampTransRight;

    public GameObject footprintDecalLeftPrefab;
    public GameObject footprintDecalRightPrefab;

    public bool canCreateIdleFootprint;
    public float idleFootprintDistThres = 0.1f;

    public static void ClearAll()
    {
        foreach (var footprintStamper in All)
        {
            Destroy(footprintStamper.gameObject);
        }
        
        All.Clear();
    }

    private void Update()
    {
        if( canCreateIdleFootprint )
            return;

        var sqrDist = Vector3.SqrMagnitude(_lastIdleFootprintPos - transform.position);

        if (sqrDist >= idleFootprintDistThres * idleFootprintDistThres)
            canCreateIdleFootprint = true;
    }

    [ContextMenu("Create Footprint Left")]
    public void CreateFootprintLeft()
    {
        var stamp = Instantiate(footprintDecalLeftPrefab);
        stamp.transform.position = stampTransLeft.position;
        stamp.transform.rotation = stampTransLeft.rotation;
        All.Add(stamp);
    }
    
    [ContextMenu("Create Footprint Right")]
    public void CreateFootprintRight()
    {
        var stamp = Instantiate(footprintDecalRightPrefab);
        stamp.transform.position = stampTransRight.position;
        stamp.transform.rotation = stampTransRight.rotation;
        All.Add(stamp);
    }

    public void CreateIdleFootprint()
    {
        if(!canCreateIdleFootprint)
            return;
        
        CreateFootprintLeft();
        CreateFootprintRight();

        canCreateIdleFootprint = false;
        _lastIdleFootprintPos = transform.position;
    }
}
