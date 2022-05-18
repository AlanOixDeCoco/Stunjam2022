using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private GameObject maggot;

    private void Awake() {
        GameManager.Instance.AddPlayer(GetInstanceID());
        GetComponentInChildren<SpriteLibrary>().spriteLibraryAsset = GameManager.Instance.playerSpriteLibraries[GameManager.Instance.playersIDs[GetInstanceID()] - 1];
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "DeadInsect"){
            DeadInsect deadInsect = other.GetComponent<DeadInsect>();
            //Debug.Log($"{GetInstanceID()} began triggering {other.GetInstanceID()}");
            deadInsect.playersTriggered++;
            
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if(other.tag == "DeadInsect"){
            DeadInsect deadInsect = other.GetComponent<DeadInsect>();

            if (deadInsect.playersTriggered <= 1 && (deadInsect.owningPlayerID != GetInstanceID()))
            {
                deadInsect.capturingPlayerID = GetInstanceID();
            }
            else
            {
                deadInsect.capturingPlayerID = 0;
            }

            if((deadInsect.captureTimer <= 0) && (deadInsect.capturingPlayerID == GetInstanceID()) && deadInsect.owningPlayerID != deadInsect.capturingPlayerID){

                if(deadInsect.owningPlayerID != 0)
                {
                    foreach(GameObject maggot in deadInsect.maggots)
                    {
                        Destroy(maggot);
                    }
                }

                for (int i = 0; i < 4; i++)
                {
                    GameObject maggots = Instantiate(maggot, deadInsect.transform);
                    float scaleRange = Random.Range(0.6f, 0.8f);
                    maggots.transform.localScale = new Vector2(scaleRange, scaleRange);
                    maggots.transform.localPosition = new Vector2(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f));
                    maggots.transform.Rotate(new Vector3(0, 0, Random.Range(0, 360)));
                    Animator maggotAnimator = maggots.GetComponent<Animator>();
                    maggotAnimator.Play(0, -1, i * 0.04f);
                    SpriteRenderer msr = maggots.GetComponent<SpriteRenderer>();

                    float colorRange = Random.Range(0.8f, 1f);
                    msr.color = new Color(colorRange, colorRange, colorRange);
                    msr.sortingOrder = i;
                    
                    maggots.GetComponent<SpriteLibrary>().spriteLibraryAsset = GameManager.Instance.playerSpriteLibraries[GameManager.Instance.playersIDs[GetInstanceID()] - 1];
                    deadInsect.maggots[i] = maggots;
                }
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
