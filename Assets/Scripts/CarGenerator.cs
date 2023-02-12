using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarGenerator : MonoBehaviour
{
    [SerializeField] private GameObject carPrefab;
    [SerializeField] private Quaternion rotation;
    [Range(1, 100)] [SerializeField] private int amount = 25;

    private void Awake()
    {
        for (int i = 0; i < amount; i++)
        {
            Instantiate(carPrefab, transform.position, rotation, transform);
        }
    }
}
