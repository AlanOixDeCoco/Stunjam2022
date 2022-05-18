using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class PlayerManager : MonoBehaviour
{
    private void Awake() {
        GameManager.Instance.AddPlayer(GetInstanceID());
        GetComponentInChildren<SpriteLibrary>().spriteLibraryAsset = GameManager.Instance.playerSpriteLibraries[GameManager.Instance.playersIDs[GetInstanceID()] - 1];
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "DeadInsect"){
            DeadInsect deadInsect = other.GetComponent<DeadInsect>();
            //Debug.Log($"{GetInstanceID()} began triggering {other.GetInstanceID()}");

            deadInsect.playersTriggered++;
            if(deadInsect.playersTriggered <= 1 && (deadInsect.owningPlayerID != GetInstanceID())){
                deadInsect.capturingPlayerID = GetInstanceID();
            }
            else{
                deadInsect.capturingPlayerID = 0; 
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if(other.tag == "DeadInsect"){
            DeadInsect deadInsect = other.GetComponent<DeadInsect>();
            if((deadInsect.captureTimer <= 0) && (deadInsect.capturingPlayerID == GetInstanceID())){
                deadInsect.owningPlayerID = GetInstanceID();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.tag == "DeadInsect"){
            DeadInsect deadInsect = other.GetComponent<DeadInsect>();
            //Debug.Log($"{GetInstanceID()} ended triggering {other.GetInstanceID()}");

            deadInsect.playersTriggered--;
            deadInsect.capturingPlayerID = 0;
        }
    }


}
