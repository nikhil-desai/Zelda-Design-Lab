using UnityEngine;

[System.Serializable]
public struct ScheduleItem
{
    public string activityName; 
    public float timeTrigger;   // What time (0-60) to start moving to this spot
    public Transform destination; 
}

[RequireComponent(typeof(CharacterController))]
public class NPCSchedule : MonoBehaviour
{
    public ScheduleItem[] schedule;
    
    [Header("Movement Settings")]
    public float moveSpeed = 8f;   // Increased for "faster" travel
    public float turnSpeed = 15f;  // Fast rotation for snappier turns
    public float stopDistance = 0.5f;

    private int currentTargetIndex = 0;
    private CharacterController _controller;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // 1. Safety Check for the Global Clock
        if (TimeManager.Instance == null || schedule == null || schedule.Length == 0) return;

        float currentTime = TimeManager.Instance.currentTime;

        // 2. Optimized Target Selection
        // We find the correct target based on the clock
        for (int i = 0; i < schedule.Length; i++)
        {
            if (currentTime >= schedule[i].timeTrigger)
            {
                currentTargetIndex = i;
            }
        }

        Transform target = schedule[currentTargetIndex].destination;
        if (target == null) return;

        // 3. Movement Logic
        Vector3 targetPos = target.position;
        targetPos.y = transform.position.y; // Keep vertical height locked
        
        Vector3 moveDir = targetPos - transform.position;

        if (moveDir.magnitude > stopDistance)
        {
            moveDir = moveDir.normalized;
            
            // Physical movement that respects walls
            _controller.Move(moveDir * moveSpeed * Time.deltaTime);

            // Faster Rotation
            Quaternion lookRot = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, turnSpeed * Time.deltaTime);
        }

        // 4. Gravity
        if (!_controller.isGrounded)
        {
            _controller.Move(Vector3.down * 9.81f * Time.deltaTime);
        }
    }
}