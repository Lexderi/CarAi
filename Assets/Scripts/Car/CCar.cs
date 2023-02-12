using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CCar : MonoBehaviour
{
    [SerializeField] private float force = 1;
    [SerializeField] private float backwardsForceMultipliyer = 0.2f;
    [SerializeField] private float rotationSpeed = 100;
    [HideInInspector] private new Rigidbody2D rigidbody;

    private void OnEnable()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    #region Car Controlls
    public void MoveForwards()
    {
        rigidbody.AddForce(transform.rotation * Vector2.up * (force * Time.fixedDeltaTime));
    }

    public void MoveBackwards()
    {
        rigidbody.AddForce(transform.rotation * Vector2.down * (force * Time.fixedDeltaTime * backwardsForceMultipliyer));
    }

    public void SteerLeft()
    {
        //transform.Rotate(0, 0, rotationSpeed * Time.fixedDeltaTime * Rigidbody.velocity.magnitude);
        transform.Rotate(0, 0, rotationSpeed * Time.fixedDeltaTime);
    }
    
    public void SteerRight()
    {
        //transform.Rotate(0, 0, -rotationSpeed * Time.fixedDeltaTime * Rigidbody.velocity.magnitude);
        transform.Rotate(0, 0, -rotationSpeed * Time.fixedDeltaTime);
    }
    #endregion
}
