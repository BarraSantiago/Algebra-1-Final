using UnityEngine;

public class PlaneCreator : MonoBehaviour
{
    public Plane Plane
    {
        private set;
        get;
    }
    
    void Update()
    {
        Transform objectTransform = transform;
        Plane = new Plane(objectTransform.up, objectTransform.position);
    }
}
