using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityAttraction : MonoBehaviour
{
    Rigidbody rigidBody;
    public bool IsAttractee//property 
    {
        get
        {
            return isAttractee;
        }
        set
        {
            if (value == true)
            {
                if (!Gravity.attractees.Contains(this.GetComponent<Rigidbody>()))
                {
                    Gravity.attractees.Add(rigidBody);
                }

            }
            else if (value == false)
            {
                Gravity.attractees.Remove(rigidBody);
            }
            isAttractee = value;
        }
    }
    public bool IsAttractor//property
    {
        get
        {
            return isAttractor;
        }
        set
        {
            if (value == true)
            {
                if (!Gravity.attractors.Contains(this.GetComponent<Rigidbody>()))
                    Gravity.attractors.Add(rigidBody);
            }
            else if (value == false)
            {
                Gravity.attractors.Remove(rigidBody);
            }
            isAttractor = value;
        }
    }
    [SerializeField] bool isAttractor;
    [SerializeField] bool isAttractee;
    public int attractionGroup = 0;

    [SerializeField] Vector3 initialVelocity;
    [SerializeField] bool applyInitialVelocityOnStart = false;

    private void Awake()
    {
        rigidBody = this.GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        IsAttractor = isAttractor;
        IsAttractee = isAttractee; 
    }

    void Start()
    {

        if (isAttractee)
        {
            attractionGroup = Random.Range(0, 8);
        }

        if(applyInitialVelocityOnStart)
        {
            ApplyVelocity(initialVelocity);
        }
    }

    private void OnDisable()
    {
        Gravity.attractors.Remove(rigidBody);
        Gravity.attractees.Remove(rigidBody);
    }

    private void ApplyVelocity(Vector3 Velocity)
    {
        rigidBody.AddForce(initialVelocity, ForceMode.Impulse);
    }

    public void ChangeGroup()
    {
        if (isAttractee)
        {
            attractionGroup = Random.Range(0, 8);
        }
    }
}
