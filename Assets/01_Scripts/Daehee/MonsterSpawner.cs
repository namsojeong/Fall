using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public float spawnDelay = 5f;

    public bool isDefault=true;

    public List<Transform> spawnPoints;

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
            spawnDelay = Random.Range(1.0f, 2.5f);
        }
    }

    private void SetMonster()
    {
        GameObject monster;
        if (isDefault)monster = ObjectPool.Instance.GetObject(PoolObjectType.Bomb_DefaultMonster);
        else monster = ObjectPool.Instance.GetObject(PoolObjectType.BOMB_MONSTER);
        monster.SetActive(false);
        monster.transform.position = spawnPoints[Random.Range(0, spawnPoints.Count)].position;
        monster.transform.parent = null;
        monster.transform.rotation = Quaternion.identity;
        monster.SetActive(true);
    } 

}
