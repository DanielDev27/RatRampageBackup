using UnityEngine;

public class DestructibleObjectManager : MonoBehaviour
{
    public static DestructibleObjectManager Instance;
    //[SerializeField] Tasks tasks;
    [SerializeField] private GameObject baseObject;
    [SerializeField] private GameObject destroyedObject;
    [SerializeField] DestroyBy destroyBy;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] AudioSource breakingAudio;
    [SerializeField] int collisionCount = 0;
    [SerializeField] bool tough;
    [SerializeField] bool box;
    public bool isDestroyed = false;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        baseObject.SetActive(true);
        if (!box)
        {
            destroyedObject.SetActive(false);
        }
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (!isDestroyed)
        {
            if (destroyBy == DestroyBy.Rat)
            {
                if (other.GetComponent<CharacterController>() != null)
                {
                    if (!tough)
                    {
                        Debug.Log("Collision");
                        baseObject.SetActive(false);
                        destroyedObject.SetActive(true);
                        isDestroyed = true;
                        //tasks.SetTaskComplete();
                        if (breakingAudio != null) { breakingAudio.Play(); }
                        TaskManager.Instance.UpdateTasks();
                    }
                    if (tough)
                    {
                        Rigidbody[] destroyedRBs = destroyedObject.GetComponentsInChildren<Rigidbody>();
                        collisionCount++;
                        if (collisionCount == 1)
                        {
                            baseObject.SetActive(false);
                            destroyedObject.SetActive(true);
                            breakingAudio.Play();
                            foreach (Rigidbody rb in destroyedRBs)
                            {
                                rb.isKinematic = true;
                            }
                        }
                        if (collisionCount == 2)
                        {
                            foreach (Rigidbody rb in destroyedRBs)
                            {
                                rb.isKinematic = false;
                            }
                            breakingAudio.Play();
                            isDestroyed = true;
                            //tasks.SetTaskComplete();
                            breakingAudio.Play();
                            TaskManager.Instance.UpdateTasks();
                        }
                    }
                }
            }
            if (destroyBy == DestroyBy.Ground)
            {
                if (other.gameObject.layer == 6/*Ground Layer*/)
                {
                    if (!box)
                    {
                        baseObject.SetActive(false);
                        destroyedObject.SetActive(true);
                        breakingAudio.Play();
                    }
                    isDestroyed = true;
                    //tasks.SetTaskComplete();
                    TaskManager.Instance.UpdateTasks();
                }
            }
        }
    }
}

public enum DestroyBy
{
    Rat,
    Ground,
}

