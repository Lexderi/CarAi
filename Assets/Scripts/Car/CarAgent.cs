using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

[RequireComponent(typeof(CCar))]
public class CarAgent : Agent
{
    private CCar carController;
    private new Rigidbody2D rigidbody;
    [SerializeField] private int framesUntilDecision = 5;
    private int framesCounted;

    private int checkpointIndex = -1;

    private void Start()
    {
        MGame.OnGameReset += EndEpisode;
    }

    private void FixedUpdate()
    {
        if (framesCounted >= framesUntilDecision)
        {
            RequestDecision();
            framesCounted = 0;
        }
        else framesCounted++;
    }

    #region Reward Stuff
    public void ActivateCheckpoint(int id)
    {
        // sees if its the next checkpoint
        if ((checkpointIndex == 0 && id == MCheckpoint.Instance.Checkpoints.Length - 1) ||
            id == checkpointIndex - 1)
        {
            AddReward(-1);
            checkpointIndex = id;
            return;
        }
        if (id != checkpointIndex + 1) return;
        
        AddReward(1);

        // succes!!!11!! yaaaaaaaaay
        checkpointIndex = id;

        // if it has done one lap
        if (checkpointIndex < MCheckpoint.Instance.Checkpoints.Length - 1) return;
        
        checkpointIndex = -1;
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        // tries to activate checkpoint if it got hit by one
        if (!collider.CompareTag("Checkpoint")) return;

        int id = collider.GetComponent<CCheckpoint>().id;
        ActivateCheckpoint(id);
    }
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.otherCollider.CompareTag("Wall")) AddReward(-.1f);
    //}
    #endregion

    public override void CollectObservations(VectorSensor sensor)
    {
        if (rigidbody == null)
        {
            sensor.AddObservation(Vector2.zero);
            sensor.AddObservation(0);
            return;
        }
        sensor.AddObservation(rigidbody.velocity);
        sensor.AddObservation(rigidbody.angularVelocity);
    }

    public override void Initialize()
    {
        carController = GetComponent<CCar>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        // drive
        if (actions.DiscreteActions[0] == 1) StartCoroutine(MoveForwards());
        if (actions.DiscreteActions[1] == 1) StartCoroutine(MoveBackwards());

        // steer
        if (actions.DiscreteActions[2] == 1) StartCoroutine(SteerLeft());
        if (actions.DiscreteActions[2] == 2) StartCoroutine(SteerRight());
    }

    private IEnumerator SteerRight()
    {
        for (int i = 0; i <= framesUntilDecision; i++)
        {
            carController.SteerRight();
            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator SteerLeft()
    {
        for (int i = 0; i <= framesUntilDecision; i++)
        {
            carController.SteerLeft();
            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator MoveBackwards()
    {
        for (int i = 0; i <= framesUntilDecision; i++)
        {
            carController.MoveBackwards();
            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator MoveForwards()
    {
        for (int i = 0; i <= framesUntilDecision; i++)
        {
            carController.MoveForwards();
            yield return new WaitForFixedUpdate();
        }
    }

    public override void OnEpisodeBegin()
    {
    // I have no idea why this happens, it's extremely cursed, but for some reason this can be null
        if (this == null)
            return;

        rigidbody.MovePosition(transform.parent.position);
        transform.rotation = Quaternion.Euler(0, 0, 270);
        rigidbody.velocity = Vector2.zero;
        rigidbody.angularVelocity = 0;
        checkpointIndex = -1;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> discreteActionsOut = actionsOut.DiscreteActions;

        discreteActionsOut[0] = (int)Mathf.Clamp01(Input.GetAxisRaw("Vertical")) ;
        discreteActionsOut[1] = (int)Mathf.Clamp(Input.GetAxisRaw("Vertical"), -1, 0) == -1 ? 1 : 0;
        discreteActionsOut[2] =
            (int)Input.GetAxisRaw("Horizontal") == -1 ? 1 : (int)Input.GetAxisRaw("Horizontal") == 0 ? 0 : 2;
    }
}
