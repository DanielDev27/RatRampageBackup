using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class AIActions : MonoBehaviour
{
    [SerializeField] private string aiName;
    [SerializeField] public Transform aiTransform;
    [SerializeField] public Rigidbody aIRigidbody;
    [SerializeField] public Collider aICollider;
    public ActionObject currentActionObject;
    [SerializeField] public Transform rayTransform;

    [Header("Settings_Movement")]
    [SerializeField] bool isActive = true;
    public NavMeshAgent agent;
    public float waitCount = 0;
    public float remainingDistance;
    public int roamIndex;
    public List<Transform> roamPoints = new List<Transform>();
    public AudioSource[] walkingAudio;
    public AudioSource currentClip;

    [Header("Animation")]
    public CharacterController characterController;
    public static int IsMoving = Animator.StringToHash("isMoving");
    public static int IsAttack = Animator.StringToHash("isAttack");
    public static int IsSearch = Animator.StringToHash("isSearch");
    public static int IdleRange = Animator.StringToHash("IdleRange");

    [Header("Settings_Projectile")]
    [SerializeField] GameObject ignoreLayer;
    [SerializeField] public GameObject[] projectile;
    public float projectileSpeedFactor;
    [SerializeField] public Transform projectileSpawn;
    public int fovAngle;
    public float visionRange;
    public LayerMask ratLayerMask;
    public LayerMask blockedLayers;

    [Header("Debug_Projectile")]
    public Vector3 targetPosition;
    public Transform target;
    public Vector3 targetDirection;
    public float distanceToTarget;

    [Header("Animation")]
    public Animator animator;
    public bool isIdle;
    public bool isMoving;
    public bool isAttacking;
    public bool isSearching;
    public bool canSeeTarget;

    private void Awake()
    {
        ValidateAndInitActionObject();
        aIRigidbody = GetComponent<Rigidbody>();
        aICollider = GetComponent<Collider>();
    }
    private void OnEnable()
    {
        currentActionObject?.OnEnable();
    }
    void OnDisable()
    {
        currentActionObject?.OnDisable();
    }
    void Start()
    {
        RegisterAndConfigureActionObject();
        currentActionObject?.Start();
    }
    void Update()
    {
        if (!isActive)
        {
            return;
        }

        currentActionObject?.Update();
        animator.SetBool(IsMoving, isMoving);
        animator.SetBool(IsAttack, isAttacking);
        animator.SetBool(IsSearch, isSearching);

    }
    void FixedUpdate()
    {
        if (!isActive)
        {
            return;
        }

        if (!isSearching)
        {
            animator.SetFloat(IdleRange, Random.Range(0, 3));
        }
        currentActionObject?.FixedUpdate();
    }
    void OnDrawGizmos()
    {
        currentActionObject?.OnDrawGizmos();
    }
    void ValidateAndInitActionObject()
    {
        if (currentActionObject == null)
        {
            Debug.LogError($"{gameObject.name} disabled");
            gameObject.SetActive(false);
            return;
        }

        currentActionObject = Instantiate(currentActionObject);
        Debug.Log($"{currentActionObject.name} {currentActionObject.GetInstanceID()}");

        currentActionObject?.Awake();
    }
    void RegisterAndConfigureActionObject()
    {
        if (currentActionObject != null)
        {
            currentActionObject.antagonistActions = this;
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == ignoreLayer.layer)
        {
            Physics.IgnoreCollision(collision.collider, aICollider);
        }
    }
    public void SetIdle()
    {
        animator.SetFloat(IdleRange, Random.Range(0, 2));
    }
}

