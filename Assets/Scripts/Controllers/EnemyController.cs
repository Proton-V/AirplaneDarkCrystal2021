using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyController : PlayerController
{
    [SerializeField]
    private UI_EnemyDistanceImage_Controller _uiPrefab;
    [SerializeField]
    [Range(0f, 1f)]
    private float _killParticleTime = 0.5f;

    private float _distanceCheckBullet = 2.0f;

    private bool _randomBool
    {
        get
        {
            return UnityEngine.Random.value > 0.5 ? true : false;
        }
    }
    private bool _isKilled = false;

    override
    public event Action Shoot = null;

    private UI_EnemyDistanceImage_Controller _imageUI;

    override
    public void Awake()
    {
        Shoot += EnemyController_Shoot;
        _isLocalPlayer = false;
        RandomInputWithDelay();
        CreateUI();
    }

    #region ImageUI(Create and controll)
    private void CreateUI()
    {
        _imageUI = Instantiate(_uiPrefab, GameManager.Instance.CanvasGame.transform);
        _imageUI.Init(this);

    }
    void OnBecameInvisible()
    {
        _imageUI.IsOffscreen = true;
    }
    //Turns off the indicator if object is onscreen.
    void OnBecameVisible()
    {
        _imageUI.IsOffscreen = false;
    }
    #endregion
    private void EnemyController_Shoot()
    {
        BulletsManager.Instance.Spawn(_bulletSpawnPoint.transform.position, transform.forward, BulletsManager.PlayerTypes.Enemy);
    }

    override
    public void Update()
    {
        base.Update();
        if (!_isKilled)
        {
            CheckBullets();
        }
    }

    private void CheckBullets()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 3f);
        foreach (var hitCollider in hitColliders)
        {
            if(hitCollider.gameObject.tag == "Bullet")
            {
                float distance = Vector3.Distance(transform.position, hitCollider.transform.position);
                if (distance < _distanceCheckBullet)
                {
                    _isKilled = true;
                    KillEnemy(hitCollider);
                }
                else
                {
                    Shoot?.Invoke();
                }
            }
        }
    }

    private void KillEnemy(Collider hitCollider)
    {
        hitCollider.GetComponent<BulletController>().Hide();
        Enemy_SpawnController.Instance.EnemyList.Remove(this.gameObject);
        KillWithParticle();
    }
    private async void KillWithParticle()
    {
        Destroy(_imageUI.gameObject);
        GetComponent<ParticleSystem>().Play();
        await Task.Delay((int)(_killParticleTime * 1000));
        Destroy(this.gameObject);
    }


    #region RandomInput

    private async void RandomInputWithDelay()
    {
        SetRandomInput();
        await Task.Delay(UnityEngine.Random.Range(1000, 3000));
        RandomInputWithDelay();
    }

    private void SetRandomInput()
    {
        IsBoost = _randomBool;
        OnLeftArrow = _randomBool;
        OnRightArrow = _randomBool;
        OnUpArrow = _randomBool;
        OnDownArrow = _randomBool;
    }

    #endregion

}
