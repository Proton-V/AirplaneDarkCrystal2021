using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IControll
{
    [HideInInspector]
    public bool _isLocalPlayer = true;
    
    public GameObject _bulletSpawnPoint;
    [SerializeField]
    private ParticleSystem _boostParticle = null;

    [SerializeField]
    [Range(1f, 3f)]
    private float _speed = 2;
    [SerializeField]
    [Range(1f, 5f)]
    private float _boostSpeed = 2f;

    private bool _isBoost = false, _onLeftArrow = false, _onRightArrow = false, _onUpArrow = false, _onDownArrow = false, _isShoot = false;

    #region IControll
    public bool IsBoost
    {
        get
        {
            return _isLocalPlayer ? Input.GetKey(KeyCode.LeftShift) : _isBoost;
        }
        set
        {
            _isBoost = value;
        }
    }
    public bool IsShoot
    {
        get
        {
            return _isLocalPlayer ? Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl) : _isShoot;
        }
        set
        {
            _isShoot = value;
        }
    }
    public bool OnLeftArrow
    {
        get
        {
            return _isLocalPlayer ? Input.GetKey(KeyCode.LeftArrow) : _onLeftArrow;
        }
        set
        {
            _onLeftArrow = value;
        }
    }
    public bool OnRightArrow
    {
        get
        {
            return _isLocalPlayer ? Input.GetKey(KeyCode.RightArrow) : _onRightArrow;
        }
        set
        {
            _onRightArrow = value;
        }
    }
    public bool OnUpArrow
    {
        get
        {
            return _isLocalPlayer ? Input.GetKey(KeyCode.UpArrow) : _onUpArrow;
        }
        set
        {
            _onUpArrow = value;
        }
    }
    public bool OnDownArrow
    {
        get
        {
            return _isLocalPlayer ? Input.GetKey(KeyCode.DownArrow) : _onDownArrow;
        }
        set
        {
            _onDownArrow = value;
        }
    }
    #endregion

    Vector3 eulerVector = Vector3.zero;

    public virtual event Action Shoot = null;


    public virtual void Awake()
    {
        Shoot += PlayerController_Shoot;
    }

    private void PlayerController_Shoot()
    {
        BulletsManager.Instance.Spawn(_bulletSpawnPoint.transform.position, transform.forward, BulletsManager.PlayerTypes.Player);
    }


    public virtual void Update()
    {
        if (IsShoot) Shoot?.Invoke();
        UpdateBoostParticle();
        transform.position += transform.forward * _speed * Time.deltaTime * (IsBoost? _boostSpeed : 1);

        eulerVector += new Vector3(OnDownArrow? 1: OnUpArrow? -1 : 0, 0, OnLeftArrow? 1: OnRightArrow? -1 : 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(eulerVector), Time.deltaTime);
    }

    private void UpdateBoostParticle()
    {
        if(_boostParticle!=null)
            if (IsBoost)
                _boostParticle.Play();
            else
            {
                _boostParticle.Pause();
                _boostParticle.Clear();
            }
    }
}
