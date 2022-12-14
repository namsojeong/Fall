using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    private float spawnDelay = 5f;

    private void Start()
    {
        StartSpawn();
    }

    public void StartSpawn()
    {
        StartCoroutine(SpawnMonster());
    }

    private IEnumerator SpawnMonster()
    {
        while(true)
        {
            yield return new WaitForSeconds(spawnDelay);
            SetMonster();

        }
    }

    private void SetMonster()
    {
        GameObject monster = ObjectPool.Instance.GetObject(PoolObjectType.BOMB_MONSTER);
        monster.transform.position = transform.position;
        monster.transform.parent = null;
        monster.transform.rotation = Quaternion.identity;
    } 

}
