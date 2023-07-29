using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Room : MonoBehaviour
{
    private List<Transform> wallTransforms = new List<Transform>();
    public List<IdentifySideOnPlane> doors = new List<IdentifySideOnPlane>();
    [SerializeField] List<Room> adyacentRooms = new List<Room>();
    private Bounds boundsRoom;
    private Transform camTransform;
    public bool isVisible;
    public short roomId;
    private void Awake()
    {
        wallTransforms = GetComponentsInChildren<Transform>().ToList();
    }

    private void Start()
    {
        camTransform = Camera.main.transform;
        boundsRoom.max = Vector3.one * -1000;
        boundsRoom.min = Vector3.one * 1000;
        for (int i = 0; i < wallTransforms.Count; i++)
        {
            MeshFilter meshFilter = wallTransforms[i].GetComponent<MeshFilter>();

            if (!meshFilter) continue;

            for (int j = 0; j < meshFilter.mesh.vertices.Length; j++)
            {
                Vector3 pos = wallTransforms[i].TransformPoint(meshFilter.mesh.vertices[i]);
                if (pos.x > boundsRoom.max.x) boundsRoom.max = new Vector3(pos.x, boundsRoom.max.y, boundsRoom.max.z);
                if (pos.y > boundsRoom.max.y) boundsRoom.max = new Vector3(boundsRoom.max.x, pos.y, boundsRoom.max.z);
                if (pos.z > boundsRoom.max.z) boundsRoom.max = new Vector3(boundsRoom.max.x, boundsRoom.max.y, pos.z);

                if (pos.x < boundsRoom.min.x) boundsRoom.min = new Vector3(pos.x, boundsRoom.min.y, boundsRoom.min.z);
                if (pos.y < boundsRoom.min.y) boundsRoom.min = new Vector3(boundsRoom.min.x, pos.y, boundsRoom.min.z);
                if (pos.z < boundsRoom.min.z) boundsRoom.min = new Vector3(boundsRoom.min.x, boundsRoom.min.y, pos.z);
            }
        }
    }

    private void Update()
    {
        isVisible = false;

        if (boundsRoom.Contains(camTransform.position))
        {
            isVisible = true;
            //BSP.EnableTaggedRoom(tag);
        }
        else
        {
            for (int i = 0; i < doors.Count; i++)
            {
                for (int j = 0; j < doors[i].GetMesh.vertices.Length; j++)
                {
                    if (doors[i].CheckFrustum())
                    {
                        //BSP.EnableTaggedRoom(tag);

                        isVisible = true;
                        break;
                    }
                }
                if (isVisible) break;
            }
        }
    }
}