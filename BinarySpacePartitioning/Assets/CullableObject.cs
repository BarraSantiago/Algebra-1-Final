using System;
using UnityEngine;

public class CullableObject : MonoBehaviour
{
    public static Action onCullableObjectCreation;
    
    public short roomId;
    public Vector3[] _objectVertices;
    public Mesh _meshFilter;
    
    private Vector3[] _vertices;
    private Material _renderMat;
    private int _color1;
    private Transform _lastPosition;

    private void Awake()
    {
        _renderMat = GetComponent<MeshRenderer>().material;
        _meshFilter = GetComponent<MeshFilter>().mesh;
        _objectVertices = new Vector3[_meshFilter.vertices.Length];
        _color1 = Shader.PropertyToID("_Color");
        _lastPosition = transform;
    }

    private void OnEnable()
    {
        onCullableObjectCreation?.Invoke();
    }

    private void OnDisable()
    {
        onCullableObjectCreation?.Invoke();
    }

    private void Update()
    {
        CheckInsideOfBounds();
    }

    public void SetColor(Color color)
    {
        _renderMat.SetColor(_color1, color);
    }

    private void CheckInsideOfBounds()
    {
        if(_lastPosition == transform) return;
        
        _vertices = _meshFilter.vertices;

        for (var i = 0; i < _vertices.Length; i++)
        {
            _objectVertices[i] = transform.TransformPoint(_vertices[i]);
        }

        _lastPosition = transform;
    }
}