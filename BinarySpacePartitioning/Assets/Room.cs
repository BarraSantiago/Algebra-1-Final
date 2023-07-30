using System;
using System.Collections.Generic;
using UnityEngine;


public class Room : MonoBehaviour
{
    public short roomId;
    public Extremes roomExtremes;
    public short[] adjacentRoomsIds;
    public bool isVisible;

    private const string CullableName = "Cullable";
    private List<CullableObject> _roomMembers;

    private void Awake()
    {
        _roomMembers = new List<CullableObject>();

        GameObject[] cullableObjects = GameObject.FindGameObjectsWithTag(CullableName);

        foreach (GameObject cullableObject in cullableObjects)
        {
            if (cullableObject.GetComponent<CullableObject>().roomId == roomId)
            {
                _roomMembers.Add(cullableObject.GetComponent<CullableObject>());
            }
        }
    }

    private void OnEnable()
    {
        CullableObject.onCullableObjectCreation += CheckRoomObjects;
    }

    private void OnDisable()
    {
        CullableObject.onCullableObjectCreation -= CheckRoomObjects;
    }

    public void MakeMembersVisible()
    {
        foreach (CullableObject cObject in _roomMembers)
        {
            cObject.SetColor(Color.green);
        }

        isVisible = true;
    }

    public void MakeMembersInvisible()
    {
        foreach (CullableObject cObject in _roomMembers)
        {
            cObject.SetColor(Color.red);
        }

        isVisible = false;
    }

    private void CheckRoomObjects()
    {
        roomExtremes.minX = Single.MaxValue;
        roomExtremes.maxX = Single.MinValue;
        roomExtremes.minY = Single.MaxValue;
        roomExtremes.maxY = Single.MinValue;
        roomExtremes.minZ = Single.MaxValue;
        roomExtremes.maxZ = Single.MinValue;

        foreach (CullableObject roomMember in _roomMembers)
        {
            if (roomMember.roomId == roomId)
            {
                if (roomExtremes.minX > roomMember.extremes.minX) roomExtremes.minX = roomMember.extremes.minX;
                if (roomExtremes.maxX < roomMember.extremes.maxX) roomExtremes.maxX = roomMember.extremes.maxX;
                if (roomExtremes.minY > roomMember.extremes.minY) roomExtremes.minY = roomMember.extremes.minY;
                if (roomExtremes.maxY < roomMember.extremes.maxY) roomExtremes.maxY = roomMember.extremes.maxY;
                if (roomExtremes.minZ > roomMember.extremes.minZ) roomExtremes.minZ = roomMember.extremes.minZ;
                if (roomExtremes.maxZ < roomMember.extremes.maxZ) roomExtremes.maxZ = roomMember.extremes.maxZ;
            }
        }

        for (int i = 0; i < _roomMembers.Count; i++)
        {
        }
    }
}