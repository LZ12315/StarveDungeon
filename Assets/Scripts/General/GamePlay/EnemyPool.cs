using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyPool : MonoBehaviour
{
    //private ObjectPool<GameObject> pool;

    //private GameObject enemy;
    //private Vector3 spawnPosition;

    //public int enemyCount;

    //private Queue<GameObject> availableEnemies = new Queue<GameObject>();

    //private void Awake()
    //{
    //    //pool = new ObjectPool<GameObject>(createFunc, actionOnGet, actionOnRelease, actionOnDestroy, true, 100, 1000);
    //}

    //private void OnEnable()
    //{
        
    //}

    //private void FillPool(GameObject enemyPrefab, Vector3 spawnPos)
    //{
    //    var newEnemy = Instantiate(enemyPrefab,spawnPos,Quaternion.identity);

        
    //}

    //public void ReturnPool(GameObject nowEnemy)
    //{
    //    nowEnemy.SetActive(false);

    //    availableEnemies.Enqueue(nowEnemy);
    //}

    ////GameObject createFunc()
    ////{
    ////    GameObject currentGameObject = obj;
    ////    return currentGameObject;
    ////}

    ////void actionOnGet(GameObject obj)
    ////{
    ////    if (pool != null)
    ////    {
    ////        obj.gameObject.SetActive(true);
    ////    }
    ////}

    ////void actionOnRelease(GameObject obj)
    ////{
    ////    if (pool != null)
    ////    {
    ////        obj.gameObject.SetActive(false);
    ////    }
    ////}

    ////void actionOnDestroy(GameObject obj)
    ////{
    ////    Destroy(obj);
    ////}

    ////void GetEnemyPrefab(GameObject enemyPrefab, Vector3 spawnPos)
    ////{
    ////    currentEnemy = enemyPrefab;
    ////    spawnPosition = spawnPos;
    ////}
}
