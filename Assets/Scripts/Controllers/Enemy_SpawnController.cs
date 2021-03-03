using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector3Ext
{
    static float random(float scatter)
    {
        return Random.Range(-scatter, scatter);
    }

    public static Vector3 RandomPoint(this Vector3 vector, Vector3 point, float scatter)
    {
        return point + new Vector3(random(scatter), random(scatter), random(scatter));
    }
}

public class Enemy_SpawnController : MonoBehaviour
{
    public static Enemy_SpawnController Instance { get; set; }

    [SerializeField]
    private GameObject _enemyParent, _enemyPrefab;
    [SerializeField]
    [Range(1, 100)]
    private int _spawnCount = 5;
    [SerializeField]
    [Range(10, 100)]
    private int _scatter = 20;

    public List<GameObject> EnemyList = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        for(int i = 0; i < _spawnCount; i++)
        {
            GameObject newEnemy = Instantiate(_enemyPrefab, _enemyParent.transform);
            newEnemy.transform.position = new Vector3().RandomPoint(transform.position, _scatter);

            EnemyList.Add(newEnemy);
        }
    }
}
