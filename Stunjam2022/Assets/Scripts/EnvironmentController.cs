using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentController : MonoBehaviour
{
    [Header("Global Settings")]
    [SerializeField] private float leftSpawnLimit;
    [SerializeField] private float rightSpawnLimit;
    [SerializeField] private float topSpawnLimit;
    [SerializeField] private float bottomSpawnLimit;

    [Header("Bird Poops")]
    [SerializeField] private Transform birdPoopsHandler;
    [SerializeField] private GameObject[] birdPoopPrefabs;
    private List<BirdPoop> birdPoops;

    [Header("Dead Insects")]
    [SerializeField] private Transform deadInsectsHandler;
    [SerializeField] private GameObject[] deadInsectPrefabs;
    private List<DeadInsect> deadInsects = new List<DeadInsect>();

    public void SpawnDeadInsect(){
        GameObject newDeadInsect = Instantiate(
            deadInsectPrefabs[(int)Random.Range(0, deadInsectPrefabs.Length)],
            new Vector2(
                Random.Range(leftSpawnLimit, rightSpawnLimit),
                Random.Range(bottomSpawnLimit, topSpawnLimit)
            ),
            new Quaternion(0, 0, 0, 0),
            deadInsectsHandler
        );
        newDeadInsect.transform.Rotate(Vector3.forward, Random.Range(0, 360f));
        deadInsects.Add(newDeadInsect.GetComponent<DeadInsect>());
    }

    public void clearDeadInsects()
    {
        foreach (DeadInsect insect in deadInsects)
        {
            insect.UnsetReward();
        }
    }
}
