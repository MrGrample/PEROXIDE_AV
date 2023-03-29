using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{

    [SerializeField] float g = 1f;
    static float G;
    public static List<Rigidbody> attractors = new List<Rigidbody>();
    public static List<Rigidbody> attractees = new List<Rigidbody>();
    public static bool isSimulatingLive = true;
    public static float pushRadiusHash;
    public static float minIslandDiastanceHash;

    [SerializeField] private float pushRadius = 5f;
    [SerializeField] private float minIslandDistance = 10f;

    [SerializeField] AudioPeer audioPeer;

    public static AudioPeer audioPeerHash;



    private void FixedUpdate()
    {
        G = g;
        pushRadiusHash = pushRadius;
        minIslandDiastanceHash = minIslandDistance;
        audioPeerHash = audioPeer;
        if(isSimulatingLive)
        {
            SimulateGravities();
        }
    }

    public static void SimulateGravities()
    {
        foreach(Rigidbody attractor in attractors)
        {
            foreach (Rigidbody attractee in attractees)
            {

                if(audioPeerHash._audioBandBuffer[0] <= 0.2f)
                {
                    attractee.gameObject.GetComponent<GravityAttraction>().ChangeGroup();
                }

                if (attractor != attractee)
                {
                    AddGravityForce(attractor, attractee);
                }
            }
        }
    }

    public static void AddGravityForce(Rigidbody attractor, Rigidbody target)
    {
        float massProduct = attractor.mass * target.mass * G;

        Vector3 difference = attractor.position - target.position;
        float distance = difference.magnitude;

        float unScaledMagnitude;
        if (attractor.gameObject.tag != "MainIsland")
        {
            unScaledMagnitude = massProduct / Mathf.Pow(distance, 2);
        }
        else
        {
            unScaledMagnitude = Mathf.Pow(distance, 2) / attractor.mass;
        }
        float forceMagnitude = G * unScaledMagnitude;

        Vector3 forceDirection = difference.normalized;

        Vector3 forceVector = forceDirection * forceMagnitude;
        
        if (target.gameObject.GetComponent<GravityAttraction>().attractionGroup == attractor.gameObject.GetComponent<GravityAttraction>().attractionGroup || (attractor.gameObject.tag == "MainIsland" && distance >= minIslandDiastanceHash))
        {
            if (distance <= 5f)
            {
                target.AddForce(forceVector * 3);
            }
            else
            {
                target.AddForce(forceVector);
            }
        }
        else
        {
            if (attractor.isKinematic && distance <= pushRadiusHash)
            {
                target.AddForce(-forceVector / 16f);
            }
        }
    }


}
