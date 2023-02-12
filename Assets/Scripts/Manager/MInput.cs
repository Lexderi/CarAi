using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MInput : MonoBehaviour
{
    public static MInput Instance { get; private set; }

    [Header("Controls")] 
    [SerializeField] private KeyCode moveForwardsKey;
    [SerializeField] private KeyCode moveBackwardsKey;
    [SerializeField] private KeyCode steerRightKey;
    [SerializeField] private KeyCode steerLeftKey;
    [Space]
    [SerializeField] private CCar mainCar;
    private bool moveForward;
    private bool moveBackwards;
    private bool steerRight;
    private bool steerLeft;

    private void Update()
    {
        if (Input.GetKey(moveForwardsKey)) moveForward = true;
        if (Input.GetKey(moveBackwardsKey)) moveBackwards = true;
        if (Input.GetKey(steerRightKey)) steerRight = true;
        if (Input.GetKey(steerLeftKey)) steerLeft = true;
    }

    private void FixedUpdate()
    {
        if (moveForward) mainCar.MoveForwards();
        if (moveBackwards) mainCar.MoveBackwards();
        if (steerLeft) mainCar.SteerLeft();
        if (steerRight) mainCar.SteerRight();

        moveForward = false;
        moveBackwards = false;
        steerRight = false;
        steerLeft = false;
    }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);

        DontDestroyOnLoad(gameObject);
    }
}
