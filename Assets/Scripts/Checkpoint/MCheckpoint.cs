using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCheckpoint : MonoBehaviour
{
    public static MCheckpoint Instance { get; private set; }
    
    public CCheckpoint[] Checkpoints;

    private void Start()
    {
        for (int i = 0; i < Checkpoints.Length; i++)
        {
            CCheckpoint checkpoint = Checkpoints[i];
            checkpoint.id = i;
        }
    }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }
}
