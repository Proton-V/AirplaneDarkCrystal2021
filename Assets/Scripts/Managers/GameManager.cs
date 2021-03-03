using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }
    
    public RectTransform CanvasGame;
    [HideInInspector]
    public GameObject Player;
    [HideInInspector]
    public Camera PlayerCamera;

    private void Awake()
    {
        Instance = this;
        Player = Resources.FindObjectsOfTypeAll<PlayerController>()
            .First((player) => player._isLocalPlayer == true).gameObject;
        PlayerCamera = Camera.main;
    }
}
