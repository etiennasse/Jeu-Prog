﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnController : MonoBehaviour
{
    public GameObject firstEnemy;
    public GameObject secondEnemy;
    public GameObject thirdEnemy;
    private Transform spawnPoint;

    public  float waveDelay = 5f;
    private float waveDelayTimer = 0f;
    public float waveEnnemyCount = 3f;
    private float waveNumberOfEnemiesSpawned = 0f;

    public const float enemySpawnDelay = 1f;
    private float enemySpawnDelayTimer = 0f;

    private bool CurrentlySpawningEnemies = false;

    void Start()
    {
        this.spawnPoint = this.transform;
    }

    void Update()
    {
        UpdateWaveTimer();

        if (TimerIsOver() && !CurrentlySpawningEnemies)
        {
            InstantiateNewWave();
        }
        else if (TimerIsOver() && CurrentlySpawningEnemies)
        {
            if (!WaveEnemiesAllSpawned())
            {
                if (!EnemySpawnDelayTimerIsOver())
                    UpdateSpawnTimer();
                else
                {
                    InitiateNewEnemy();
                }
            }
            else
            {
                EndCurrentWave();
            }
        }
    }

    private void UpdateSpawnTimer()
    {
        enemySpawnDelayTimer -= Time.deltaTime;
    }


    private void UpdateWaveTimer()
    {
        waveDelayTimer -= Time.deltaTime;
    }

    private void InstantiateNewWave()
    {
        waveNumberOfEnemiesSpawned = 0;
        CurrentlySpawningEnemies = true;
        ResetTimer();
    }


    private void InitiateNewEnemy()
    {
        var index = Random.Range(1f, 4f);
        waveNumberOfEnemiesSpawned++;
        SpawnEnemiesFromIndex((int)index, (int)waveNumberOfEnemiesSpawned);
        ResetSpawnTimer();
    }

    private void EndCurrentWave()
    {
        CurrentlySpawningEnemies = false;
        if (waveEnnemyCount <= 5)
        {
            waveEnnemyCount++;
        }
        ResetSpawnTimer();
    }

    private void SpawnEnemiesFromIndex(int index, int num)
    {
        Quaternion rot = Quaternion.Euler(0, -90, 0);

        if (index == 1)
            Instantiate(firstEnemy, spawnPoint.transform.position + new Vector3(num,0, 0), rot);
        else if (index == 2)
            Instantiate(secondEnemy, spawnPoint.transform.position + new Vector3(num, 0, 0), rot);
        else if (index == 3)
            Instantiate(thirdEnemy, spawnPoint.transform.position + new Vector3(num, 0, 0), rot);
    }

    private void ResetTimer()
    {
        waveDelayTimer = waveDelay;
    }

    private void ResetSpawnTimer()
    {
        enemySpawnDelayTimer = enemySpawnDelay;
    }

    private bool TimerIsOver()
    {
        return this.waveDelayTimer <= 0f;
    }

    private bool EnemySpawnDelayTimerIsOver()
    {
        return this.enemySpawnDelayTimer <= 0f;
    }

    private bool WaveEnemiesAllSpawned()
    {
        return this.waveNumberOfEnemiesSpawned == this.waveEnnemyCount;
    }
}
