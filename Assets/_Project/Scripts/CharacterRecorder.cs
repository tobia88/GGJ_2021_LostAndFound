using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapData
{
    public Vector3 position;
    public Quaternion rotation;
}

public class CharacterRecorder : MonoBehaviour
{
    public float timeSnapInterval = 0.5f;

    public Queue<SnapData> snapDatas = new Queue<SnapData>();

    private void Start()
    {
        StartCoroutine(SnapperUpdate());
    }

    IEnumerator SnapperUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeSnapInterval);
            snapDatas.Enqueue(new SnapData(){ position = transform.position, rotation = transform.rotation});
        }
    }
}
