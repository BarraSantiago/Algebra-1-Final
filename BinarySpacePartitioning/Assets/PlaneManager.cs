using System;
using UnityEngine;

public class PlaneManager : MonoBehaviour
{
    [SerializeField] private GameObject[] planeObjects;

    private Plane[] _planes;
    private Camera _camera;
    private float _lastFOV;
    private Vector3 _lastPosition;

    private void Start()
    {
        _camera = Camera.main;
        _lastFOV = _camera!.fieldOfView;
        _lastPosition = _camera.transform.position;

        _planes = GeometryUtility.CalculateFrustumPlanes(_camera);

        for (int i = 0; i < 6; ++i)
        {
            planeObjects[i].transform.position = -_planes[i].normal * _planes[i].distance;
            planeObjects[i].transform.rotation = Quaternion.FromToRotation(Vector3.up, _planes[i].normal);
        }
    }

    private void LateUpdate()
    {
        const float epsilon = 1e-05f;
        
        if (Math.Abs(_camera.fieldOfView - _lastFOV) > epsilon)
        {
            _planes = GeometryUtility.CalculateFrustumPlanes(_camera);
            OnFieldOfViewChanged();
            _lastFOV = _camera.fieldOfView;
        }

        if (transform.position != _lastPosition)
        {
            _planes = GeometryUtility.CalculateFrustumPlanes(_camera);
            OnCameraMoved();
            _lastPosition = transform.position;
        }
    }

    private void OnFieldOfViewChanged()
    {
        for (int i = 0; i < 6; ++i)
        {
            planeObjects[i].transform.position = -_planes[i].normal * _planes[i].distance;
        }
    }

    private void OnCameraMoved()
    {
        for (int i = 0; i < 6; ++i)
        {
            planeObjects[i].transform.rotation = Quaternion.FromToRotation(Vector3.up, _planes[i].normal);
        }
    }
}