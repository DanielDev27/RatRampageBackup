using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "Tasks", menuName = "Rat House Guest/Tasks", order = 0)]
public class Tasks : ScriptableObject
{
    [SerializeField] public Rooms taskRoom;
    [SerializeField] public bool isTaskComplete;
    [SerializeField] public List<DestructibleObjectManager> destructibleObjects = new List<DestructibleObjectManager>();

    public void SetTaskComplete()
    {
        isTaskComplete = true;
    }


}
