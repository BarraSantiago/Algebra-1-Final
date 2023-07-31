using System.Collections.Generic;
using UnityEngine;

public class ObjectCuller : MonoBehaviour
{
    [SerializeField] private Room[] rooms;
    [SerializeField] private PlaneCreator[] planes;
    [SerializeField] private float maxDistance = 10f;
    [SerializeField] private float stepSize = 0.1f;
    [SerializeField] private int numRays = 10;

    private Vector3[] _points;

    private Camera _mainCamera;
    private CullableObject[] _cullableObjects;
    private Dictionary<int, Room> _idRoom;
    private const string CullableName = "Cullable";

    private void Awake()
    {
        _mainCamera = Camera.main;

        _points = new Vector3[numRays];
    }

    private void Start()
    {
        UpdateCullableObjects();

        _idRoom = new Dictionary<int, Room>();

        foreach (Room room in rooms)
        {
            _idRoom[room.roomId] = room;
        }
    }

    private void OnEnable()
    {
        PlaneManager.OnCameraChangeEvent += Checkobjects;

        CullableObject.onCullableObjectCreation += UpdateCullableObjects;
    }

    private void OnDisable()
    {
        PlaneManager.OnCameraChangeEvent -= Checkobjects;

        CullableObject.onCullableObjectCreation -= UpdateCullableObjects;
    }

    private void Checkobjects()
    {
        for (int i = 0; i < _idRoom.Count; i++)
        {
            _idRoom[i].MakeMembersInvisible();
        }

        CreatePoints();

        // se revisa la habitacion y se hace visible si contiene algun punto
        for (short i = 0; i < _idRoom.Count; i++)
        {
            foreach (Vector3 point in _points)
            {
                if (CheckPointInsideRoom(i, point))
                {
                    _idRoom[i].MakeMembersVisible(); 
                    break;
                }
            }
        }

        // comprobacion de objetos cullable
        for (int i = 0; i < _cullableObjects.Length; i++)
        {
            if (_idRoom[_cullableObjects[i].roomId].isVisible) continue;

            short roomId = _cullableObjects[i].roomId;

            bool objectPrinted = CheckFrustum(_cullableObjects[i]);

            if (objectPrinted)
            {
                _idRoom[roomId].MakeMembersVisible();
            }
        }
    }

    private bool CheckPointInsideRoom(short id, Vector3 position)
    {
        Extremes roomExtremes = _idRoom[id].roomExtremes;

        return position.x <= roomExtremes.maxX && position.x >= roomExtremes.minX &&
               position.y <= roomExtremes.maxY && position.y >= roomExtremes.minY &&
               position.z <= roomExtremes.maxZ && position.z >= roomExtremes.minZ;
    }

    private bool CheckFrustum(CullableObject cObject)
    {
        for (int i = 0; i < planes.Length; i++)
        {
            if (!IsInFrustum(planes[i].Plane, cObject))
            {
                return false;
            }
        }

        return true;
    }

    private bool IsInFrustum(Plane plane, CullableObject cObject)
    {
        int counter = 0;
        foreach (Vector3 t1 in cObject.objectVertices)
        {
            if (!plane.GetSide(t1))
            {
                counter++;
            }
        }

        return counter < cObject.meshFilter.vertices.Length;
    }

    private bool CheckPointInFrustum(Vector3 point)
    {
        int counter = 0;

        for (int i = 0; i < planes.Length; i++)
        {
            if (planes[i].Plane.GetSide(point)) counter++;
        }

        return counter >= planes.Length;
    }

    private void UpdateCullableObjects()
    {
        GameObject[] cullableObjectsInScene = GameObject.FindGameObjectsWithTag(CullableName);

        int objectsLength = cullableObjectsInScene.Length;

        if (_cullableObjects == null || _cullableObjects.Length != objectsLength)
        {
            _cullableObjects = new CullableObject[objectsLength];
        }

        for (int i = 0; i < objectsLength; i++)
        {
            _cullableObjects[i] = cullableObjectsInScene[i].GetComponent<CullableObject>();
        }
    }

    private void CreatePoints()
    {
        for (int i = 0; i < _points.Length; i++)
        {
            Vector3 direction = _mainCamera.transform.forward;
            direction.y = 0f;
            direction = Quaternion.Euler(0f, i * 360f / numRays, 0f) * direction;

            float distance = 0f;

            while (distance < maxDistance)
            {
                distance += stepSize;

                _points[i] = _mainCamera.transform.position + direction * distance;

                if (CheckPointInFrustum(_points[i]))
                {
                    if (Physics.CheckSphere(_points[i], stepSize))
                    {
                        Debug.DrawLine(_mainCamera.transform.position, _points[i], Color.red);
                        break;
                    }

                    Debug.DrawLine(_mainCamera.transform.position, _points[i], Color.green);
                }
            }
        }
    }

    private void TraverseRooms(short roomId, Vector3 point, HashSet<short> visited)
    {
        if (CheckPointInsideRoom(roomId, point) && !visited.Contains(roomId))
        {
            visited.Add(roomId);
            _idRoom[roomId].MakeMembersVisible();

            foreach (short adjacentRoom in _idRoom[roomId].adjacentRoomsIds)
            {
                TraverseRooms(adjacentRoom, point, visited);
            }
        }
    }
}