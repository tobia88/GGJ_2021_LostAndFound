using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecorderManager : MonoBehaviour
{
    public List<SnapData[]> snapDatas = new List<SnapData[]>();
    public CharacterReplayer replayerPrefab;

    public static List<CharacterReplayer> replayers = new List<CharacterReplayer>(); 

    public void EnqueueSnaps(List<SnapData> snaps)
    {
        snapDatas.Add(snaps.ToArray());
    }

    public void SetupGhosts()
    {
        CleanGhosts();
        foreach (var snapData in snapDatas )
        {
            var inst = Instantiate(replayerPrefab);
            inst.SetSnapData(snapData);
            
            replayers.Add(inst);
        }
    }

    public void CleanGhosts()
    {
        foreach (var replayer in replayers)
        {
            Destroy(replayer.gameObject);
        }
        
        replayers.Clear();
    }
}
