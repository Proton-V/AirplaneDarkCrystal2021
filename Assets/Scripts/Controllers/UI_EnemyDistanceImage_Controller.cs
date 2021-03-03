using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class UI_EnemyDistanceImage_Controller : MonoBehaviour
{
    private Camera _cam;

    private float _factorSize = 5, _minSize = 30, _maxSize = 100;
    private float maxX = 0, maxY = 0;


    private Vector2 _enemyLogoPosOnScreen
    {
        get
        {
            Vector3 enemyScreenPos = _enemyScreenPoint;

            float xClamp = Mathf.Clamp(enemyScreenPos.x, -maxX, maxX);
            float yClamp = Mathf.Clamp(enemyScreenPos.y, -maxY, maxY);
            if (!IsOffscreen)
            {
                _pos = PosOnScreen.center;
                return new Vector2(xClamp, yClamp);
            }
            else
            {
                if (enemyScreenPos.x < -Screen.width / 2 || enemyScreenPos.x > Screen.width / 2)
                {
                    _pos = enemyScreenPos.x < -Screen.width / 2? PosOnScreen.left : PosOnScreen.right;
                    return new Vector2(enemyScreenPos.x < 0 ? -maxX : maxX, yClamp);
                }
                else
                {
                    _pos = enemyScreenPos.y < -Screen.height / 2 ? PosOnScreen.down : PosOnScreen.up;
                    return new Vector2(xClamp, enemyScreenPos.y < 0 ? -maxY : maxY);
                }
            }
        }
    }

    private Vector2 _enemyScreenPoint
    {
        get
        {
            return (Vector2)_cam.WorldToScreenPoint(_enemy.transform.position) + new Vector2(-Screen.width / 2, -Screen.height / 2);
        }
    }

    private float _angle
    {
        get
        {
            Vector3 indicatorRotation = Quaternion.LookRotation(_player.transform.position - _enemy.transform.position, Vector3.forward).eulerAngles;
            return indicatorRotation.z;
        }
    }

    public bool IsOffscreen = true;

    private EnemyController _enemy;

    [SerializeField]
    private Image _image;
    [SerializeField]
    private TMPro.TextMeshProUGUI _textDistance;

    private GameObject _player;

    private PosOnScreen _pos = PosOnScreen.center;

    public void Init(EnemyController enemy)
    {
        maxX = Screen.width / 2 - _maxSize;
        maxY = Screen.height / 2 - _maxSize;

        GameManager gm = GameManager.Instance;
        _cam = gm.PlayerCamera;
        _player = gm.Player;
        _enemy = enemy;
    }

    private void Update()
    {
        SetPos();
    }

    public void SetPos()
    {
        float distance = Vector3.Distance(_player.transform.position, _enemy.transform.position);
        ChangeVisual(!IsOffscreen, distance);
        transform.localPosition = IsOffscreen? Vector2.Lerp(transform.localPosition,
            _enemyLogoPosOnScreen, Time.deltaTime * 5f) : _enemyLogoPosOnScreen;

        ///....
        switch (_pos)
        {
            case PosOnScreen.up:
                if (_angle > -90 && _angle < 90 ||
                    _angle > 270)
                {
                    transform.localEulerAngles = new Vector3(0, 0, _angle);
                }
                else
                {
                    transform.localEulerAngles = new Vector3(0, 0, _angle - 180);
                }
                break;
            case PosOnScreen.down:
                if (_angle < -90 && _angle > -270 ||
                    _angle > 90 && _angle < 270)
                {
                    transform.localEulerAngles = new Vector3(0, 0, _angle);
                }
                else
                {
                    transform.localEulerAngles = new Vector3(0, 0, _angle - 180);
                }
                break;

            case PosOnScreen.right:
                if (_angle < 0 && _angle > -180 ||
                    _angle > 180 && _angle < 360)
                {
                    transform.localEulerAngles = new Vector3(0, 0, _angle);
                }
                else
                {
                    transform.localEulerAngles = new Vector3(0, 0, _angle - 180);
                }
                break;
            case PosOnScreen.left:
                if (_angle < -180 && _angle > -360||
                    _angle > 0 && _angle < 180)
                {
                    transform.localEulerAngles = new Vector3(0, 0, _angle);
                }
                else
                {
                    transform.localEulerAngles = new Vector3(0, 0, _angle - 180);
                }
                break;

        }


    }


    private void ChangeVisual(bool visible, float distance)
    {
        _image.enabled = !visible;
        float imageSize = Mathf.Clamp(distance * _factorSize, _minSize, _maxSize);
        _image.GetComponent<RectTransform>().sizeDelta = new Vector2(imageSize, imageSize);
        _textDistance.text = ((int)distance).ToString();
        _textDistance.enabled = visible;
    }

    public enum PosOnScreen
    {
        center,
        left,
        right,
        up,
        down
    }
}
