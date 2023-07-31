using System;
using UnityEngine;

public class PlaneManager : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;

    public GameObject[] planeObjects;
    public static Action onCameraChangeEvent;

    private float _lastFOV;
    private Vector3 _lastPosition;
    private Quaternion _lastRotation;

    private void Start()
    {
        _lastFOV = mainCamera!.fieldOfView;
        _lastPosition = mainCamera.transform.position;
        
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);

        // Create a "Plane" GameObject aligned to each of the calculated planes
        for (int i = 0; i < 6; ++i)
        {
            planeObjects[i].transform.position = -planes[i].normal * planes[i].distance;
            planeObjects[i].transform.rotation = Quaternion.FromToRotation(Vector3.up, planes[i].normal);
        }
    }

    private void LateUpdate()
    {
        
        const float epsilon = 1e-05f;

        Transform objectTransform = mainCamera.transform;

        bool hasChanged = Math.Abs(mainCamera.fieldOfView - _lastFOV) > epsilon ||
                          objectTransform.position != _lastPosition || objectTransform.rotation != _lastRotation;

        if (hasChanged)
        {
            onCameraChangeEvent?.Invoke();

            _lastFOV = mainCamera.fieldOfView;
            _lastPosition = objectTransform.position;
            _lastRotation = objectTransform.rotation;
        }
    }
}