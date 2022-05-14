using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DeadInsect : MonoBehaviour
{
    public float captureTime = 0;
    public float reward = 40;

    public Transform canvasTransform;
    public TextMeshProUGUI rewardStateText;
    public Image captureStateImage;
    public Image captureOwnerImage;

    [HideInInspector]
    public int owningPlayerID = 0;
    [HideInInspector]
    public int capturingPlayerID = 0;
    [HideInInspector]
    public int playersTriggered = 0;
    [HideInInspector]
    public float captureTimer = 0;

    private void Start() {
        canvasTransform.rotation = Quaternion.identity;
    }

    private void Update() {
        rewardStateText.text = $"{(int)reward} pts";

        if(owningPlayerID != 0){
            Debug.Log($"Player {GameManager.Instance.playersIDs[owningPlayerID]} is the owner ({GetInstanceID()})!");
            captureOwnerImage.color = GameManager.Instance.playerColors[GameManager.Instance.playersIDs[owningPlayerID] - 1];
            captureOwnerImage.fillAmount = 1;
            reward -= Time.deltaTime;
            GameManager.Instance.playersScores[GameManager.Instance.playersIDs[owningPlayerID] - 1] += Time.deltaTime;
        }
        else{
            captureOwnerImage.fillAmount = 0;
        }

        if(capturingPlayerID != 0){
            captureStateImage.color = GameManager.Instance.playerColors[GameManager.Instance.playersIDs[capturingPlayerID] - 1];
            captureTimer -= Time.deltaTime;
            captureStateImage.fillAmount = (captureTime-captureTimer)/captureTime;
        }
        else{
            captureTimer = captureTime;
            captureStateImage.fillAmount = 0;
        }
        if(reward <= 0) DeleteDeadInsect();
    }

    private void DeleteDeadInsect(){
        Destroy(gameObject);
    }
}
