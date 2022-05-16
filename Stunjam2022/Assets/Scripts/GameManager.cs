using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}

    [SerializeField] private EnvironmentController environmentController;
    [SerializeField] private int maxDeadInsects;
    [SerializeField] private AnimationCurve spawnTimerMultiplier;
    [SerializeField] private int maxBirdPoop;

    public Dictionary<int, int> playersIDs = new Dictionary<int, int>();
    public Color[] playerColors;
    public Sprite[] playerSprites;
    public TextMeshProUGUI[] playersScoreTexts;
    private int playersCount = 0;
 
    public int birdPoopsCount = 0, deadInsectsCount = 0;
    private bool isDeadInsectSpawning = false;

    public List<float> playersScores = new List<float>();

    private void Awake() {
        if(Instance == null && Instance != this){
            Instance = this;
        }
        else
            Destroy(this);
    }

    private void Start() {
        SpawnDeadInsect(5, 5);
    }

    private void Update() {
        if(!isDeadInsectSpawning && (deadInsectsCount < maxDeadInsects)){
            SpawnDeadInsect(
                spawnTimerMultiplier.Evaluate(Mathf.Clamp(deadInsectsCount - 1, 0, deadInsectsCount)),
                spawnTimerMultiplier.Evaluate(deadInsectsCount)
            );
        }
        for(int i=0; i<playersCount; i++){
            playersScoreTexts[i].text = $"Player {i+1}: {((int)playersScores[i])}";
        }
    }

    private void SpawnDeadInsect(float minTimer, float maxTimer){
        isDeadInsectSpawning = true;
        float randomNumber = UnityEngine.Random.Range(minTimer, maxTimer);
        StartCoroutine(RunAfterTimer(randomNumber, () => {
            environmentController.SpawnDeadInsect();
            deadInsectsCount++;
        }));
    }

    public void AddPlayer(int playerID){
        playersCount++;
        playersIDs.Add(playerID, playersCount);
        playersScores.Add(0);
    }

    #region COROUTINES
    IEnumerator RunAfterTimer(float timer, Action func){
        float endTime = Time.time + timer;
        while(Time.time < endTime){
            yield return null;
        }
        func.Invoke();
        isDeadInsectSpawning = false;
    }
    #endregion =======
}
