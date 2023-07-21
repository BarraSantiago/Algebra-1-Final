using UnityEngine;

public class IdentifySideOnPlane : MonoBehaviour
{
    [SerializeField] PlaneCreator[] planesCreator;
    Mesh _mesh;
    Vector3[] _vertices;
    GameObject _myObject;
    Material _renderMat;
    Vector3[] _objectPosition;
    private int _color1 = Shader.PropertyToID("_Color");

    public void Start()
    {
        _myObject = gameObject;
        _mesh = _myObject.GetComponent<MeshFilter>().mesh;
        _objectPosition = new Vector3[_mesh.vertices.Length];
    }

    void Update()
    {
        _vertices = _mesh.vertices;
        _renderMat = GetComponent<MeshRenderer>().material;
        _mesh = _myObject.GetComponent<MeshFilter>().mesh;

        for (var i = 0; i < _vertices.Length; i++)
        {
            _objectPosition[i] = transform.TransformPoint(_vertices[i]);
        }

        foreach (var t in planesCreator)
        {
            int counter = 0;
            foreach (var t1 in _objectPosition)
            {
                if (!t.Plane.GetSide(t1))
                {
                    counter++;
                }
            }

            if (counter >= _mesh.vertices.Length)
            {
                _renderMat.SetColor(_color1, Color.red);
                break;
            }
            _renderMat.SetColor(_color1, Color.green);
        }
    }
}