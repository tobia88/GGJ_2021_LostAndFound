using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameFinishTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameMng.Instance.SetGamePhase(GamePhases.TurnClean);
            GetComponent<BoxCollider>().enabled = false;
        }
    }
}
