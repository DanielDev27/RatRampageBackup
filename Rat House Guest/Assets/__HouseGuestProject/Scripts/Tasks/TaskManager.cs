using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance;

    [Header("Task lists")]
    [SerializeField] GameObject bathroomTasksObject;
    [SerializeField] GameObject bedroomTasksObject;
    [SerializeField] GameObject kitchenTasksObject;
    [SerializeField] GameObject livingRoomTasksObject;
    [Header("Destructible Objects")]
    [SerializeField] List<DestructibleObjectManager> bathTask1 = new List<DestructibleObjectManager>();
    [SerializeField] List<DestructibleObjectManager> bathTask2 = new List<DestructibleObjectManager>();
    [SerializeField] List<DestructibleObjectManager> bathTask3 = new List<DestructibleObjectManager>();
    [SerializeField] List<DestructibleObjectManager> bedTask1 = new List<DestructibleObjectManager>();
    [SerializeField] List<DestructibleObjectManager> bedTask2 = new List<DestructibleObjectManager>();
    [SerializeField] List<DestructibleObjectManager> bedTask3 = new List<DestructibleObjectManager>();
    [SerializeField] List<DestructibleObjectManager> kitchenTask1 = new List<DestructibleObjectManager>();
    [SerializeField] List<DestructibleObjectManager> kitchenTask2 = new List<DestructibleObjectManager>();
    [SerializeField] List<DestructibleObjectManager> kitchenTask3 = new List<DestructibleObjectManager>();
    [SerializeField] List<DestructibleObjectManager> livingTask1 = new List<DestructibleObjectManager>();
    [SerializeField] List<DestructibleObjectManager> livingTask2 = new List<DestructibleObjectManager>();
    [SerializeField] List<DestructibleObjectManager> livingTask3 = new List<DestructibleObjectManager>();
    [Header("Check Marks")]
    [SerializeField] List<GameObject> bathroomChecks = new List<GameObject>();
    [SerializeField] List<GameObject> bedroomChecks = new List<GameObject>();
    [SerializeField] List<GameObject> kitchenChecks = new List<GameObject>();
    [SerializeField] List<GameObject> livingRoomChecks = new List<GameObject>();

    Task bath1;
    Task bath2;
    Task bath3;
    Task bed1;
    Task bed2;
    Task bed3;
    Task kitchen1;
    Task kitchen2;
    Task kitchen3;
    Task livingRoom1;
    Task livingRoom2;
    Task livingRoom3;
    [Header("Debug")]
    [SerializeField] Rooms room;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        bath1 = new Task(bathTask1, false);
        bath2 = new Task(bathTask2, false);
        bath3 = new Task(bathTask3, false);
        bed1 = new Task(bedTask1, false);
        bed2 = new Task(bedTask2, false);
        bed3 = new Task(bedTask3, false);
        kitchen1 = new Task(kitchenTask1, false);
        kitchen2 = new Task(kitchenTask2, false);
        kitchen3 = new Task(kitchenTask3, false);
        livingRoom1 = new Task(livingTask1, false);
        livingRoom2 = new Task(livingTask2, false);
        livingRoom3 = new Task(livingTask3, false);

        SetBathroom();
    }
    public void UpdateTasks()
    {
        CheckCompletion(bath1, bathroomChecks);
        CheckCompletion(bath2, bathroomChecks);
        CheckCompletion(bath3, bathroomChecks);
        CheckCompletion(bed1, bedroomChecks);
        CheckCompletion(bed2, bedroomChecks);
        CheckCompletion(bed3, bedroomChecks);
        CheckCompletion(kitchen1, kitchenChecks);
        CheckCompletion(kitchen2, kitchenChecks);
        CheckCompletion(kitchen3, kitchenChecks);
        CheckCompletion(livingRoom1, livingRoomChecks);
        CheckCompletion(livingRoom2, livingRoomChecks);
        CheckCompletion(livingRoom3, livingRoomChecks);
    }
    void CheckCompletion(Task _task, List<GameObject> checks)
    {
        foreach (DestructibleObjectManager destructibleObject in _task.DestructibleObjectManagers)
        {
            if (destructibleObject.isDestroyed)
            {
                _task.IsTaskComplete = true;
                Debug.Log(destructibleObject.name + " was destroyed");
                if (_task == bath1)
                {
                    bath1.IsTaskComplete = true;
                    checks[0].SetActive(true);
                }
                if (_task == bath2)
                {
                    bath2.IsTaskComplete = true;
                    checks[1].SetActive(true);
                }
                if (_task == bath3)
                {
                    bath3.IsTaskComplete = true;
                    checks[2].SetActive(true);
                }
                if (_task == bed1)
                {
                    bed1.IsTaskComplete = true;
                    checks[0].SetActive(true);
                }
                if (_task == bed2)
                {
                    bed2.IsTaskComplete = true;
                    checks[1].SetActive(true);
                }
                if (_task == bed3)
                {
                    bed3.IsTaskComplete = true;
                    checks[2].SetActive(true);
                }
                if (_task == kitchen1)
                {
                    kitchen1.IsTaskComplete = true;
                    checks[0].SetActive(true);
                }
                if (_task == kitchen2)
                {
                    kitchen2.IsTaskComplete = true;
                    checks[1].SetActive(true);
                }
                if (_task == kitchen3)
                {
                    kitchen3.IsTaskComplete = true;
                    checks[2].SetActive(true);
                }
                if (_task == livingRoom1)
                {
                    livingRoom1.IsTaskComplete = true;
                    checks[0].SetActive(true);
                }
                if (_task == livingRoom2)
                {
                    livingRoom2.IsTaskComplete = true;
                    checks[1].SetActive(true);
                }
                if (_task == livingRoom3)
                {
                    livingRoom3.IsTaskComplete = true;
                    checks[2].SetActive(true);
                }

                break;
            }
        }
        CheckGameOver();
    }

    public void CheckGameOver()
    {
        int _count = 0;
        List<bool> tasksCompletion = new List<bool>(){bath1.IsTaskComplete, bath2.IsTaskComplete, bath3.IsTaskComplete, bed1.IsTaskComplete,bed2.IsTaskComplete,
        bed3.IsTaskComplete, kitchen1.IsTaskComplete, kitchen2.IsTaskComplete, kitchen3.IsTaskComplete, livingRoom1.IsTaskComplete, livingRoom2.IsTaskComplete,
        livingRoom3.IsTaskComplete};

        foreach (bool taskComplete in tasksCompletion)
        {
            if (!taskComplete)
            {
                break;
            }
            else
            {
                _count++;
                if (_count == tasksCompletion.Count)
                {
                    GameManager.Instance.GameComplete();
                }
            }
        }
    }

    public void SetBathroom()
    {
        bathroomTasksObject.SetActive(true);
        bedroomTasksObject.SetActive(false);
        kitchenTasksObject.SetActive(false);
        livingRoomTasksObject.SetActive(false);
        room = Rooms.Bathroom;
    }
    public void SetBedroom()
    {
        bathroomTasksObject.SetActive(false);
        bedroomTasksObject.SetActive(true);
        kitchenTasksObject.SetActive(false);
        livingRoomTasksObject.SetActive(false);
        room = Rooms.Bedroom;
    }
    public void SetKitchen()
    {
        bathroomTasksObject.SetActive(false);
        bedroomTasksObject.SetActive(false);
        kitchenTasksObject.SetActive(true);
        livingRoomTasksObject.SetActive(false);
        room = Rooms.Kitchen;
    }
    public void SetLivingRoom()
    {
        bathroomTasksObject.SetActive(false);
        bedroomTasksObject.SetActive(false);
        kitchenTasksObject.SetActive(false);
        livingRoomTasksObject.SetActive(true);
        room = Rooms.LivingRoom;
    }

}
public enum Rooms
{
    Bathroom, Bedroom, Kitchen, LivingRoom,
}
public class Task
{
    public List<DestructibleObjectManager> DestructibleObjectManagers = new List<DestructibleObjectManager>();
    public bool IsTaskComplete;
    public Task(List<DestructibleObjectManager> destructibleObjectManagers, bool isTaskCompete)
    {
        DestructibleObjectManagers = destructibleObjectManagers;
        IsTaskComplete = isTaskCompete;
    }
}
