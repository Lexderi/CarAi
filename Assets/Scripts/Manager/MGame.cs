using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class MGame : MonoBehaviour
{
    public static MGame Instance { get; private set; }

    public static event Action OnGameReset;
    
    [SerializeField] private float timeLimit;
    [SerializeField] private TMP_Text elapsedTimeText;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    private void Start()
    {
        StartCoroutine(RestartLoop());
    }

    private IEnumerator RestartLoop()
    {
        float time = 0;

        while (true)
        {
            time += Time.deltaTime;

            elapsedTimeText.text = Mathf.Floor(time).ToString(CultureInfo.InvariantCulture).PadLeft(2, '0');

            if (time >= timeLimit)
            {
                time = 0;
                OnGameReset?.Invoke();
            }


            yield return null;
        }
    }
}
