using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{

    [SerializeField]
    [Range(10f, 50f)]
    private float _bulletSpeed = 15f;
    [SerializeField]
    [Range(30f, 50f)]
    private float _bulletDistance = 30f;

    private event Action ShootAct = null, HideAct = null;

    private void Awake()
    {
        HideAct += new Action(() =>
        {
            BulletsManager.Instance.SpawnBullets.Remove(this);
            Destroy(this.gameObject);
        });
    }

    public void Init(Vector3 spawnPoint, Vector3 forwardVector)
    {
        transform.position = spawnPoint;

        ShootAct += new Action(() => 
        {
            transform.position += forwardVector * _bulletSpeed * Time.deltaTime;
            if (Vector3.Distance(spawnPoint, transform.position) > _bulletDistance)
            {
                Hide();
            }
        });
    }

    private void Update()
    {
        ShootAct?.Invoke();
    }

    public void Hide()
    {
        HideAct?.Invoke();
    }
}
