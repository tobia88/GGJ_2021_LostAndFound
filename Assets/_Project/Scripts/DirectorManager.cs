using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class DirectorManager : MonoBehaviour
{
    private PlayableDirector _director;
    
    public PlayableAsset rewindTransition;
    
    private bool _isPlaying;

    private void Awake()
    {
        _director = GetComponent<PlayableDirector>();
        _director.Stop();
    }

    private void Update()
    {
        if (_director.state == PlayState.Playing)
        {
            return;
        }
        
        if (GameMng.Instance.TimeLeft <= 5.0f)
        {
            _director.Play(rewindTransition);
            // _isPlaying = true;
            // StartCoroutine(TimelineUpdate());
        }
    }

    IEnumerator TimelineUpdate()
    {
        yield return new WaitForSeconds(0.1f);
        Debug.Log(_director.state);
    }
}
