using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Text;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;
    private Coroutine beginNextWaveCoroutine;

    [Header("Wave Data")]
    public int currentWaveCount;
    public float maxEnemiesAllowed;
    public bool reachedMaxEnemies;
    public float enemiesAlive;
    public float enemiesDead;
    public float waveInterval;
    public float spawnerTimer;
    public float waveDuration;
    public float waveTimer;
    public VoidEventSO nextWaveEvent;

    [Header("Enemy Spawn")]
    public List<Transform> enemySpawnBounds;
    public List<Transform> relativeSpawnPoints;
    public List<GameObject> spawnedEnemies = new List<GameObject>();

    [Header("Wave Indicators")]
    public GameObject Arrow;
    private List<GameObject> Arrows = new List<GameObject>();
    public List<GameObject> rewardToSelect;

    [System.Serializable]
    public class Wave
    {
        public string waveName;
        public int spawnCount;  //the number of enemies that had been spawned
        public int waveQuota;   //The total number of enemy to spawn in this wave
        public float spawnInterval;
        public List<Transform> placeToSpawn;
        public List<EnemyGroup> enemyGroups;    //A List of enemies to spawn in this wave
        public List<ItemSO> RewardGroups;
    }

    [System.Serializable]
    public class EnemyGroup
    {
        public string enemyName;
        public int enemyCount;  //The number of enemies to spawn in this wave
        public int spawnCount;  //The number of this type already spawned in this wave
        public GameObject enemyPrefab;
    }

    public List<Wave> waves; //A List of all the waves in the game

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        currentWaveCount = -1;
        GameManager.instance.inWave = false;
        OnBeginNextWave(10);
    }

    private void Update()
    {
        PassWaveInfo();
        EnterRewardTime();
        WaveTimeCounter();

        if (GameManager.instance.inWave)
        {
            spawnerTimer += Time.deltaTime;
            if (spawnerTimer > waves[currentWaveCount].spawnInterval)
            {
                spawnerTimer = 0;
                SpawnEnemies();
            }
        }
    }

    private void PassWaveInfo()
    {
        GameManager.instance.currentWave = currentWaveCount;
    }

    private void WaveTimeCounter()
    {
        if (!GameManager.instance.inWave || currentWaveCount < 0 || currentWaveCount >= waves.Count)
        {
            return;
        }

        waveTimer += Time.deltaTime;
        if(waveTimer >= waveDuration)
        {
            GameManager.instance.inWave = false;
            SpawnRewards();
            OnBeginNextWave(0f);
        }
    }

    public void OnBeginNextWave(float interval)
    {
        if (beginNextWaveCoroutine != null)
        {
            return;
        }

        beginNextWaveCoroutine = StartCoroutine(BeginNextWave(interval));
    }

    private IEnumerator BeginNextWave(float interval)
    {
        int nextWaveIndex = currentWaveCount + 1;
        if (nextWaveIndex >= waves.Count)
        {
            beginNextWaveCoroutine = null;
            yield break;
        }

        waveTimer = 0;
        CameraControl.Instance.SetTargetPlace(new Vector3(0, 0, 0));
        // Spawn arrows that preview the next wave direction.
        foreach (Transform t in waves[nextWaveIndex].placeToSpawn)
        {
            GameObject arrow = Instantiate(Arrow, t.position * 0.75f, Quaternion.identity);
            Arrows.Add(arrow);
            if(t.position.x==0 && t.position.y>0)
            {
                arrow.transform.rotation = Quaternion.Euler(0, 0, 90);
            }
            else if (t.position.x<0 && t.position.y==0)
            {
                arrow.transform.rotation = Quaternion.Euler(0, 0, 180);
            }
            else if (t.position.x==0 && t.position.y<0)
            {
                arrow.transform.rotation = Quaternion.Euler(0, 0, 270);
            }
        }

        yield return new WaitForSeconds(interval);

        nextWaveEvent.RaiseEvent();
        GameManager.instance.inWave = true;
        GameManager.instance.inPrepare = false;
        currentWaveCount = nextWaveIndex;
        GameManager.instance.alivedEnemies.Clear();
        foreach(var arrow in Arrows)
        {
            Destroy(arrow);
        }
        Arrows.Clear();
        CalculateWaveQuota();
        beginNextWaveCoroutine = null;
    }

    #region Enemy Spawn

    private void CalculateWaveQuota()
    {
        int currentWaveQuota = 0;

        foreach (var enemyGroup in waves[currentWaveCount].enemyGroups)
        {
            currentWaveQuota += enemyGroup.enemyCount;
        }

        waves[currentWaveCount].waveQuota = currentWaveQuota;
        waves[currentWaveCount].spawnCount = 0;
        foreach (var enemyGroup in waves[currentWaveCount].enemyGroups)
        {
            enemyGroup.spawnCount = 0;
        }

        Vector3 movePos = new Vector3(0, 0, 0);
        foreach (Transform t in waves[(currentWaveCount)].placeToSpawn)
        {
            movePos += t.position;
        }
        CameraControl.Instance.SetTargetPlace(movePos);

        waveTimer = 0;
        spawnerTimer = 0;
        enemiesAlive = 0;
        enemiesDead = 0;
        reachedMaxEnemies = false;
    }

    private void SpawnEnemies()
    {
        //Check if the minimum of enemies in the wave have been spawned
        if (waves[currentWaveCount].spawnCount < waves[currentWaveCount].waveQuota && !reachedMaxEnemies)
        {
            //Spawn each type of enemy until the quota is filled
            foreach (var enemyGroup in waves[(currentWaveCount)].enemyGroups)
            {
                //Check if the minimum number of enemies of this type have been spawned
                if (enemyGroup.spawnCount < enemyGroup.enemyCount)
                {
                    //Limit the number of enemies alive
                    if (enemiesAlive >= maxEnemiesAllowed)
                    {
                        reachedMaxEnemies = true;
                        return;
                    }
                    Transform spawnPlace = waves[(currentWaveCount)].placeToSpawn[Random.Range(0, waves[(currentWaveCount)].placeToSpawn.Count)];
                    Bounds spawnBound = new Bounds(spawnPlace.position, spawnPlace.localScale);
                    float x = Random.Range(spawnBound.min.x, spawnBound.max.x);
                    float y = Random.Range(spawnBound.min.y, spawnBound.max.y);
                    Vector2 spawnPosition = new Vector2(x, y);
                    GameObject newEnemy = Instantiate(enemyGroup.enemyPrefab, spawnPosition, Quaternion.identity);

                    GameManager.instance.alivedEnemies.Add(newEnemy);
                    enemyGroup.spawnCount++;
                    waves[currentWaveCount].spawnCount++;
                    enemiesAlive++;
                }
            }
        }

        //Reset the reachedMaxEnemies
        if (enemiesAlive < maxEnemiesAllowed)
        {
            reachedMaxEnemies = false;
        }
    }

    public void EnemiesKilled()
    {
        --enemiesAlive;
        ++enemiesDead;
    }

    #endregion

    #region Rewards

    private void SpawnRewards()
    {
        //Vector2 leftPosition = new Vector3(rewardPoint.position.x - XOffset, rewardPoint.position.y, 0);
        //Vector2 middlePosition = new Vector3(rewardPoint.position.x, rewardPoint.position.y, 0);
        //Vector2 rightPosition = new Vector3(rewardPoint.position.x + XOffset, rewardPoint.position.y, 0);
        GameManager.instance.CreatNextWaveDetect();

        RewardUIManager.instance.RewardTime(waves[currentWaveCount].RewardGroups,currentWaveCount);

        //int randomIndex = Random.Range(0, waves[currentWaveCount].spawnRewardGroups.Count);
        //waves[currentWaveCount].spawnedRewards.Add(Instantiate(waves[currentWaveCount].spawnRewardGroups[Random.Range(0, waves[currentWaveCount].spawnRewardGroups.Count)], middlePosition, Quaternion.identity));
        //waves[currentWaveCount].spawnedRewards.Add(Instantiate(waves[currentWaveCount].spawnRewardGroups[Random.Range(0, waves[currentWaveCount].spawnRewardGroups.Count)], leftPosition, Quaternion.identity));
        //waves[currentWaveCount].spawnedRewards.Add(Instantiate(waves[currentWaveCount].spawnRewardGroups[Random.Range(0, waves[currentWaveCount].spawnRewardGroups.Count)], rightPosition, Quaternion.identity));
        //rewardToSelect.AddRange(waves[currentWaveCount].spawnedRewards);
    }


    //public void DestroyOtherRewards(GameObject selectedObject)
    //{
    //    foreach (var reward in waves[currentWaveCount].spawnedRewards)
    //    {
    //        if (reward != selectedObject)
    //        {
    //            Destroy(reward);
    //            GameManager.instance.inPrepare = true;
    //        }
    //    }
    //}

    private void EnterRewardTime()
    {
        if (GameManager.instance.inWave)
        {
            if (enemiesDead >= waves[currentWaveCount].waveQuota)
            {
                if(waveDuration - waveTimer >= 30f)
                {
                    enemiesDead = 0;
                    GameManager.instance.inWave = false;
                    SpawnRewards();
                    OnBeginNextWave(30f);
                }
                else if (waveDuration - waveTimer > 0 && waveDuration - waveTimer < 30f)
                {
                    enemiesDead = 0;
                    GameManager.instance.inWave = false;
                    SpawnRewards();
                    OnBeginNextWave(waveDuration - waveTimer);
                }
            }
        }
    }

    #endregion
}
