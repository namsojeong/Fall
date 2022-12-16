using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    private float spawnDelay = 5f;

    public bool isDefault=true;

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
        GameObject monster;
        if (isDefault)monster = ObjectPool.Instance.GetObject(PoolObjectType.Bomb_DefaultMonster);
        else monster = ObjectPool.Instance.GetObject(PoolObjectType.BOMB_MONSTER);
        monster.transform.position = transform.position;
        monster.transform.parent = null;
        monster.transform.rotation = Quaternion.identity;
    } 

}
