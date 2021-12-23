using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public Enemy enemyPrefab;
    public Transform spawnPoint;
    
    public float spawnTime = 6f;

    private List<Enemy> _enemies;

    private float timer = 0;
    
    private void Start()
    {
        _enemies = new List<Enemy>();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnTime)
        {
            Instantiate(enemyPrefab, spawnPoint.position, transform.rotation);
            
            timer = 0;
        }
    }

    public void DestroyAll()
    {
        foreach (var enemy in _enemies)
        {
            if (enemy == null)
                continue;

            StartCoroutine(enemy.Die());
        }
    }
}
