using UnityEngine;

public class ObjectCuller : MonoBehaviour
{
    private Camera _mainCamera;
    private CullableObject[] _cullableObjects;
    private GameObject[] _cullableObjectsInScene;
    private PlaneCreator[] _frustumPlanes;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void Start()
    {
        GameObject[] planes = GameObject.FindGameObjectsWithTag("FoVPlane");

        _frustumPlanes = new PlaneCreator[planes.Length];

        for (int i = 0; i < planes.Length; i++)
        {
            _frustumPlanes[i] = planes[i].GetComponent<PlaneCreator>();
        }

        UpdateCullableObjects();
    }

    private void OnEnable()
    {
        CullableObject.onCullableObjectCreation += UpdateCullableObjects;
    }

    private void OnDisable()
    {
        CullableObject.onCullableObjectCreation -= UpdateCullableObjects;
    }

    private void Update()
    {
        foreach (CullableObject cObject in _cullableObjects)
        {
            if (CheckFrustum(cObject))
            {
                cObject.SetColor(Color.green);
            }
            else
            {
                cObject.SetColor(Color.red);
            }
        }
    }

    private bool CheckFrustum(CullableObject cObject)
    {
        foreach (PlaneCreator t in _frustumPlanes)
        {
            if (!IsInFrustum(t, cObject))
            {
                return false;
            }
        }

        return true;
    }

    private bool IsInFrustum(PlaneCreator planeCreator, CullableObject cObject)
    {
        int counter = 0;
        foreach (Vector3 t1 in cObject._objectVertices)
        {
            if (!planeCreator.Plane.GetSide(t1))
            {
                counter++;
            }
        }

        return counter < cObject._meshFilter.vertices.Length;
    }

    private void UpdateCullableObjects()
    {
        _cullableObjectsInScene = GameObject.FindGameObjectsWithTag("Cullable");

        int objectsLength = _cullableObjectsInScene.Length;
        
        if (_cullableObjects == null || _cullableObjects.Length != objectsLength)
        {
            _cullableObjects = new CullableObject[objectsLength];
        }
        
        for (int i = 0; i < objectsLength; i++)
        {
            _cullableObjects[i] = _cullableObjectsInScene[i].GetComponent<CullableObject>();
        }
    }
}