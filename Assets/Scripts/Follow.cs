using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    [SerializeField] private Transform target;    
    public Vector3 offset;
    [Range(1, 10)] public float SmoothFactor;

    private void FixedUpdate()
    {
        Stalk();
    }

    void Stalk() 
    {
        Vector3 targetPosition = target.position + offset;
        Vector3 smoothPosition = Vector3.Lerp(transform.position, targetPosition, SmoothFactor * Time.fixedDeltaTime);
        transform.position = targetPosition;
    }
}
