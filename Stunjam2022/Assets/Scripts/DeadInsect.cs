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
    public UnityEngine.U2D.Animation.SpriteLibrary sl;
    public Animator animator;
    public GameObject[] maggots;

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
        maggots = new GameObject[5];
    }

    private void Update() {
        rewardStateText.text = $"{(int)reward} pts";

        if(owningPlayerID != 0){
            //Debug.Log($"Player {GameManager.Instance.playersIDs[owningPlayerID]} is the owner ({GetInstanceID()})!");
            captureOwnerImage.color = GameManager.Instance.playerColors[GameManager.Instance.playersIDs[owningPlayerID] - 1];
            //sl.spriteLibraryAsset = GameManager.Instance.playerSpriteLibraries[GameManager.Instance.playersIDs[owningPlayerID] - 1];
            captureOwnerImage.fillAmount = 0;
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
        GameManager.Instance.deadInsectsCount--;
        Destroy(gameObject);
    }
}
