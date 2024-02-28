using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class PivotPointSet : MonoBehaviour
{
    [SerializeField] Rigidbody rigidbody;
    [SerializeField] Vector3 pivotPoint;
    private void Awake()
    {
        rigidbody.centerOfMass = pivotPoint;
    }
}
