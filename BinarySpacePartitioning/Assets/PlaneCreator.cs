using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneCreator : MonoBehaviour
{
    [SerializeField] private Transform planeTransform;

    public Plane Plane
    {
        private set;
        get;
    }
    
    void Update()
    {
        Plane = new Plane(planeTransform.up, planeTransform.position);
    }
}
