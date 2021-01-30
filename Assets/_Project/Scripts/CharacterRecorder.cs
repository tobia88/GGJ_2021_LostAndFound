using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapData
{
    public float time;
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 velocity;
    public bool isGrounded;
}

public class CharacterRecorder : MonoBehaviour
{
    private Coroutine _snapshotUpdate;
    
    public float timeSnapInterval = 0.15f;
    public Character controller;

    public List<SnapData> snapDatas = new List<SnapData>();

    private void Awake()
    {
        controller = GetComponent<Character>();
    }

    public void Reset()
    {
        if( _snapshotUpdate != null )
            StopCoroutine(_snapshotUpdate);
        
        snapDatas.Clear();

        controller.enabled = false;
        
        transform.position = GameMng.Instance.startPoint.position;
        transform.rotation = GameMng.Instance.startPoint.rotation;

        StartCoroutine(ResetPlayerController());
        
        TakeSnapshot();
        
        _snapshotUpdate = StartCoroutine(SnapperUpdate());
    }

    IEnumerator ResetPlayerController()
    {
        yield return 1;
        controller.enabled = true;
    }

    IEnumerator SnapperUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeSnapInterval);
            TakeSnapshot();
        }
    }

    private void TakeSnapshot()
    {
        snapDatas.Add(new SnapData()
        {
            position = transform.position, 
            rotation = transform.rotation,
            time = GameMng.Instance.timePassCurrentTurn,
            velocity = controller.Controller.velocity,
            isGrounded = controller.IsGrounded
        });
    }
}
