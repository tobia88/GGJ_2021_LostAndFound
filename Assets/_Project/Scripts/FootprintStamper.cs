using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootprintStamper : MonoBehaviour
{
    public static List<GameObject> All = new List<GameObject>();
    
    public Transform stampTransLeft;
    public Transform stampTransRight;

    public GameObject footprintDecalLeftPrefab;
    public GameObject footprintDecalRightPrefab;

    public static void ClearAll()
    {
        foreach (var footprintStamper in All)
        {
            Destroy(footprintStamper.gameObject);
        }
        
        All.Clear();
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
}
