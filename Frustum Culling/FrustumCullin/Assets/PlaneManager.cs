using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneManager : MonoBehaviour
{
    [SerializeField] private GameObject[] planes;

    void Start()
    {
        // Calculate the planes from the main camera's view frustum
        Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        // Create a "Plane" GameObject aligned to each of the calculated planes
        for (int i = 0; i < frustumPlanes.Length; ++i)
        {
            planes[i].transform.position = -frustumPlanes[i].normal * frustumPlanes[i].distance;
            planes[i].transform.rotation = Quaternion.FromToRotation(Vector3.up, frustumPlanes[i].normal);
        }
    }
}
