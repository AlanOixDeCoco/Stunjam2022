using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}

    [SerializeField] private EnvironmentController environmentController;
    [SerializeField] private int maxDeadInsects;
    [SerializeField] private AnimationCurve spawnTimerMultiplier;
    [SerializeField] private int maxBirdPoop;

    public Dictionary<int, int> playersIDs = new Dictionary<int, int>();
    public Color[] playerColors;
    public UnityEngine.U2D.Animation.SpriteLibraryAsset[] playerSpriteLibraries;
    public TextMeshProUGUI[] playersScoreTexts;
    public TextMeshProUGUI winningPlayerText;
    public Animator wipersAnimator;
    private int playersCount = 0;
 
    public int birdPoopsCount = 0, deadInsectsCount = 0;
    private bool isDeadInsectSpawning = false;
    private bool shouldDeadInsectSpawn = false;
    private bool ended = false;
    public GameObject instructionsText;

    public List<float> playersScores = new List<float>();

    private void Awake() {
        if(Instance == null && Instance != this){
            Instance = this;
        }
        else
            Destroy(this);
    }

    private void Start() {

    }

    public void StartRound()
    {
        shouldDeadInsectSpawn = true;
        PlayerInputManager.instance.DisableJoining();
        instructionsText.SetActive(false);
    }

    public void EndRound()
    {
        environmentController.clearDeadInsects();
        shouldDeadInsectSpawn = false;
        wipersAnimator.SetTrigger("End");
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.Play();
    }

    private void Update() {
        if(!isDeadInsectSpawning && shouldDeadInsectSpawn && (deadInsectsCount < maxDeadInsects)){
            SpawnDeadInsect(
                spawnTimerMultiplier.Evaluate(Mathf.Clamp(deadInsectsCount - 1, 0, deadInsectsCount)),
                spawnTimerMultiplier.Evaluate(deadInsectsCount)
            );
        }
        for(int i=0; i<playersCount; i++){
            playersScoreTexts[i].text = $"Player {i+1}: {((int)playersScores[i])}";
            if (playersScores[i] >= 100 && !ended)
            {
                ended = true;
                EndRound();
                winningPlayerText.text = $"Player {i + 1} wins the game !";
                winningPlayerText.color = playerColors[i];
            }
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
        if (shouldDeadInsectSpawn)
        {
            func.Invoke();
        }
        isDeadInsectSpawning = false;
    }
    #endregion =======
}
