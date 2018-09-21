using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnController : MonoBehaviour {

    public GameObject firstEnemy;
    public GameObject secondEnemy;
    public GameObject thirdEnemy;
    public Transform spawnPoint;

    public const float waveDelay = 2f;
    private float waveDelayTimer = 0f;

    //private float waveEnnemyCount = 3f;

    void Update () {
        UpdateTimer();

        if(TimerIsOver())
        {
            InitiateNewEnemy();
        }
	}

    private void UpdateTimer()
    {
        waveDelayTimer -= Time.deltaTime;
    }

    /*private void InitiateNewWave()
    {
        SpawnEnemies();
        waveEnnemyCount++;
        ResetTimer();
    }*/

    private void InitiateNewEnemy()
    {
        var index = Random.Range(1f, 4f);
        SpawnEnemiesFromIndex((int)index);
        ResetTimer();
    }

    /*private void SpawnEnemies()
    {
        float index = 0;
        for(int i = 0;i<waveEnnemyCount;i++)
        {
            index = Random.Range(1f, 4f);
            SpawnEnemiesFromIndex((int)index);
            
        }
    }*/

    private void SpawnEnemiesFromIndex(int index)
    {
        Quaternion rot = Quaternion.Euler(0, -90, 0);

        if(index == 1)
            Instantiate(firstEnemy, spawnPoint.transform.position, rot);
        else if (index == 2)
            Instantiate(secondEnemy, spawnPoint.transform.position, rot);
        else if (index == 3)
            Instantiate(thirdEnemy, spawnPoint.transform.position, rot);
    }

    private void ResetTimer()
    {
        waveDelayTimer = waveDelay;
    }

    private bool TimerIsOver()
    {
        return this.waveDelayTimer <= 0f;
    }
}
