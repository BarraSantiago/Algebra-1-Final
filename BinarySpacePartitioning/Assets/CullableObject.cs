using System;
using UnityEngine;

public struct Extremes
{
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;
    public float minZ;
    public float maxZ;
}
public class CullableObject : MonoBehaviour
{
    public static Action onCullableObjectCreation;
    
    public short roomId;
    public Vector3[] objectVertices;
    public Mesh meshFilter;
    public Extremes extremes;
    
    private Vector3[] _vertices;
    private Material _renderMat;
    private int _color1;
    private Transform _lastPosition;

    private void Awake()
    {
        _renderMat = GetComponent<MeshRenderer>().material;
        meshFilter = GetComponent<MeshFilter>().mesh;
        objectVertices = new Vector3[meshFilter.vertices.Length];
        _color1 = Shader.PropertyToID("_Color");
        _lastPosition = transform;
        _vertices = meshFilter.vertices;

        CheckMinMax();
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
        if(_lastPosition != transform) UpdateObjectVertices();
    }

    public void SetColor(Color color)
    {
        _renderMat.SetColor(_color1, color);
    }

    private void UpdateObjectVertices()
    {
        _vertices = meshFilter.vertices;

        CheckMinMax();
        
        for (var i = 0; i < _vertices.Length; i++)
        {
            objectVertices[i] = transform.TransformPoint(_vertices[i]);
        }

        _lastPosition = transform;
    }
    
    void CheckMinMax()
    {
        extremes.minX = Single.MaxValue;
        extremes.maxX = Single.MinValue;
        extremes.minY = Single.MaxValue;
        extremes.maxY = Single.MinValue;
        extremes.minZ = Single.MaxValue;
        extremes.maxZ = Single.MinValue;

        foreach (Vector3 t in _vertices)
        {
            if (extremes.minX > transform.TransformPoint(t).x) extremes.minX = transform.TransformPoint(t).x;
            if (extremes.maxX < transform.TransformPoint(t).x) extremes.maxX = transform.TransformPoint(t).x;
            if (extremes.minY > transform.TransformPoint(t).y) extremes.minY = transform.TransformPoint(t).y;
            if (extremes.maxY < transform.TransformPoint(t).y) extremes.maxY = transform.TransformPoint(t).y;
            if (extremes.minZ > transform.TransformPoint(t).z) extremes.minZ = transform.TransformPoint(t).z;
            if (extremes.maxZ < transform.TransformPoint(t).z) extremes.maxZ = transform.TransformPoint(t).z;
        }
    }
}