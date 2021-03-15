using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLayers : MonoBehaviour
{
    [SerializeField] LayerMask solidObjectsLayer;
    [SerializeField] LayerMask interactablesLayer;
    [SerializeField] LayerMask grassLayer;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] LayerMask fovLayer;

    public static GameLayers i { get; set; }

    void Awake()
    {
        i = this;
    }

    public LayerMask Solid
    {
        get { return solidObjectsLayer; }
    }
    public LayerMask Interactable
    {
        get { return interactablesLayer; }
    }
    public LayerMask Grass
    {
        get { return grassLayer; }
    }
    public LayerMask Player
    {
        get { return playerLayer; }
    }
    public LayerMask Fov
    {
        get { return fovLayer; }
    }

}
