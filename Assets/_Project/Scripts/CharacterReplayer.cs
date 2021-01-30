using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using UnityEngine;

public class CharacterReplayer : MonoBehaviour
{
    public SnapData[] snapDatas;

    public int currentFrameIndex;
    public CharacterReplayerAnimator animator;

    [Range(0.0f, 1.0f)]
    public float damp = 0.1f;

    private Vector3 _targetPosition, _targetVelocity;
    private Quaternion _targetRotation;
    private Vector3 _velocity;
    
    public bool isGrounded;

    public float HorizontalSpeed => new Vector2(_velocity.x, _velocity.z).magnitude;
    public float VerticalSpeed => _velocity.y;

    private void Awake()
    {
        animator = GetComponent<CharacterReplayerAnimator>();
    }

    public void SetSnapData(SnapData[] snapDatas)
    {
        this.snapDatas = snapDatas;
        
        if( this.snapDatas == null || this.snapDatas.Length <= 0 )
            return;

        currentFrameIndex = 0;
        SyncToSnap(snapDatas[currentFrameIndex]);
    }
    private void Update()
    {
        if (snapDatas == null || currentFrameIndex >= snapDatas.Length - 1)
            return;

        var time = GameMng.Instance.timePassCurrentTurn;
        
        if ( time > snapDatas[currentFrameIndex + 1].time )
        {
            currentFrameIndex++;

            if (currentFrameIndex >= snapDatas.Length - 1)
            {
                SyncToSnap(snapDatas[currentFrameIndex]);
                return;
            }
        }
        
        var lastTimeStamp = snapDatas[currentFrameIndex].time;
        var currentTimeStamp = snapDatas[currentFrameIndex + 1].time;

        var lerp = Mathf.InverseLerp(lastTimeStamp, currentTimeStamp, time);
        
        Debug.Log("Player Lerp: " + lerp);
        
        LerpBetweenSnaps(snapDatas[currentFrameIndex], snapDatas[currentFrameIndex + 1], time);
        
        var prevPos = transform.position;
        
        transform.position = Vector3.Lerp(transform.position, _targetPosition, damp);
        transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, damp);
        _velocity = Vector3.Lerp(_velocity, _targetVelocity, damp);
    }

    private void SyncToSnap(SnapData snap)
    {
        transform.position = snap.position;
        transform.rotation = snap.rotation;
        isGrounded = snap.isGrounded;
    }

    private void LerpBetweenSnaps(SnapData snapA, SnapData snapB, float lerp)
    {
        
        _targetPosition = Vector3.Lerp(
            snapA.position,
            snapB.position,
            lerp
        );
        
        _targetVelocity = Vector3.Lerp(
            snapA.velocity,
            snapB.velocity,
            lerp
        );

        _targetRotation = Quaternion.Lerp(
            snapA.rotation,
            snapB.rotation,
            lerp
        );
        

        isGrounded = (lerp <= 0.5) ? snapA.isGrounded : snapB.isGrounded;
    }
}