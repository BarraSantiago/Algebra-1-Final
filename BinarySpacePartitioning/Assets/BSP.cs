using System;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

public class BSP : MonoBehaviour
{
    [SerializeField] private static Object[] objects;
    private IdentifySideOnPlane _identifySideOnPlane;
    private Material _renderMat;
    private Camera _camera;

    void Start()
    {
        _camera = Camera.main;
        objects = FindObjectsOfType(typeof(IdentifySideOnPlane));
    }

    void Update()
    {
        SortObjectsArray();
        CheckOverlaps();
        CheckActiveRoom();
    }

    void SortObjectsArray()
    {
        if (_camera != null)
        {
            Vector3 cameraPos = _camera.transform.position;

            for (int i = 0; i < objects.Length; i++)
            {
                for (int j = 0; j < objects.Length - 1; j++)
                {
                    double object1Distance = Vector3.Distance(objects[j].GameObject().transform.position, cameraPos);
                    double object2Distance =
                        Vector3.Distance(objects[j + 1].GameObject().transform.position, cameraPos);

                    if (object1Distance > object2Distance)
                    {
                        (objects[j], objects[j + 1]) = (objects[j + 1], objects[j]);
                    }
                }
            }
        }
    }

    void CheckOverlaps()
    {
        foreach (Object t in objects)
        {
            t.GetComponent<MeshRenderer>().enabled = true;
        }

        for (int i = 0; i < objects.Length; i++)
        {
            if (!objects[i].GetComponent<IdentifySideOnPlane>().isActive) continue;
            for (int j = i + 1; j < objects.Length; j++)
            {
                IdentifySideOnPlane side1 = objects[i].GetComponent<IdentifySideOnPlane>();
                IdentifySideOnPlane side2 = objects[j].GetComponent<IdentifySideOnPlane>();
                float minX = side1.minX;
                float maxX = side1.maxX;
                float minY = side1.minY;
                float maxY = side1.maxY;
                float minZ = side1.minZ;
                float maxZ = side1.maxZ;

                float minX2 = side2.minX;
                float maxX2 = side2.maxX;
                float minY2 = side2.minY;
                float maxY2 = side2.maxY;
                float minZ2 = side2.minZ;
                float maxZ2 = side2.maxZ;

                if (!(minX <= minX2) || !(maxX >= maxX2)) continue;
                if (!(minY <= minY2) || !(maxY >= maxY2)) continue;
                if (!(minZ <= minZ2) || !(maxZ >= maxZ2)) continue;
                objects[j].GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }

    public static void CheckActiveRoom()
    {
        foreach (Object t in objects)
        {
            if (t.GetComponent<MeshRenderer>().enabled)
            {
                EnableTaggedRoom(t.GameObject().tag);
            }
        }
    }

    public static void EnableTaggedRoom(String tag)
    {
        //GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(tag);
        foreach (Object t in objects)
        {
            if (t.GameObject().CompareTag(tag)) t.GetComponent<IdentifySideOnPlane>().isActive = true;
        } 
    }
}