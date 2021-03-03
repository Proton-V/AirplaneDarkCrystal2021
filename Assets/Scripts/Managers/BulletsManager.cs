using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BulletsManager : MonoBehaviour
{
    public static BulletsManager Instance { get; set; }

    public GameObject BulletsParrent, BulletPrefab;

    [SerializeField]
    [Range(10,50)]
    private int _playerBulletsLimit = 10;

    [SerializeField]
    [Range(0f, 3f)]
    private float _bulletSpawnTime = 1f;
    private float? _timeRespawn = 0f;
    private bool _isSpawned = false;

    [HideInInspector]
    public List<BulletController> SpawnBullets = new List<BulletController>();

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        Timer();
    }

    private void Timer()
    {
        _timeRespawn += Time.deltaTime;
        if (_timeRespawn > _bulletSpawnTime)
        {
            _timeRespawn = 0f;
            if (!_isSpawned) _isSpawned = true;
        }
    }

    public void Spawn(Vector3 spawnPoint,Vector3 forwardVector, PlayerTypes playerType)
    {
        BulletController bullet;
        if (playerType == PlayerTypes.Player)
        {
            if (!_isSpawned || SpawnBullets.Count >= _playerBulletsLimit) return;
            _isSpawned = false;

            bullet = Instantiate(BulletsManager.Instance.BulletPrefab, BulletsManager.Instance.BulletsParrent.transform).GetComponent<BulletController>();
            SpawnBullets.Add(bullet);
        }
        else
        {
            bullet = Instantiate(BulletsManager.Instance.BulletPrefab, BulletsManager.Instance.BulletsParrent.transform).GetComponent<BulletController>();
            bullet.name += "Enemy";
        }


        bullet.Init(spawnPoint, forwardVector);

    }


    public enum PlayerTypes
    {
        Player,
        Enemy
    }
}
