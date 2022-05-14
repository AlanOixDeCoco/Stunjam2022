using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeadInsect : MonoBehaviour
{
    public float captureTime = 0;
    public float reward = 40;

    public TextMeshProUGUI playerText;

    [HideInInspector]
    public int owningPlayerID = 0;
    [HideInInspector]
    public int capturingPlayerID = 0;
    [HideInInspector]
    public int playersTriggered = 0;
    [HideInInspector]
    public float captureTimer = 0;

    private void Update() {
        if(owningPlayerID != 0){
            Debug.Log($"Player {GameManager.Instance.playersIDs[owningPlayerID]} is the owner ({GetInstanceID()})!");
            playerText.text = $"Player {GameManager.Instance.playersIDs[owningPlayerID]}";
            reward -= Time.deltaTime;
            GameManager.Instance.playersScores[GameManager.Instance.playersIDs[owningPlayerID] - 1] += Time.deltaTime;
        }
        else
            playerText.text = "";

        if(capturingPlayerID != 0){
            captureTimer -= Time.deltaTime;
        }
        else{
            captureTimer = captureTime;
        }
    }

}
