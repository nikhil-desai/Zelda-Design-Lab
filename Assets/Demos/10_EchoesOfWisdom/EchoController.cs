using UnityEngine;

public class EchoController : MonoBehaviour {
    [Header("Settings")]
    public MonoBehaviour coreMovementScript;
    public KeyCode copyKey = KeyCode.F;
    public KeyCode spawnKey = KeyCode.E;
    public float scanRange = 5f;
    public float spawnDist = 2.5f;

    [Header("Current State")]
    public GameObject learnedPrefab; // The type we are currently holding

    void Update() {
        // 1. SINGLE KEY COPY (The Siphon)
        if (Input.GetKeyDown(copyKey)) {
            TryCopyTarget();
        }

        // 2. SPAWN LOGIC
        if (Input.GetKeyDown(spawnKey)) {
            if (learnedPrefab != null) {
                SpawnLogic();
            } else {
                Debug.LogWarning("Lab Error: No Echo learned yet! Point at an object and press " + copyKey);
            }
        }
    }

    void TryCopyTarget() {
        // 1. LOWER THE ORIGIN: Start at 0.5 instead of 1.0
        // 2. TILT THE BEAM: Aim it slightly toward the floor (Vector3.down * 0.2f)
        Vector3 rayOrigin = transform.position + Vector3.up * 0.5f;
        Vector3 rayDirection = (transform.forward - Vector3.up * 0.2f).normalized;

        Ray ray = new Ray(rayOrigin, rayDirection);
        
        // VISUAL DEBUG: The yellow line in Scene View will now tilt down
        Debug.DrawRay(ray.origin, ray.direction * scanRange, Color.yellow, 1.0f);

        if (Physics.Raycast(ray, out RaycastHit hit, scanRange)) {
            if (hit.collider.TryGetComponent(out IEchoable echo)) {
                learnedPrefab = echo.PrefabOrigin;
                Debug.Log($"<color=cyan>🧪 LAB SUCCESS:</color> Copied <b>{learnedPrefab.name}</b>!");
            } else {
                Debug.Log("Hit: " + hit.collider.name + " (No IEchoable found)");
            }
        } else {
            Debug.Log("Siphon missed everything.");
        }
    }

    void SpawnLogic() {
        Vector3 targetPos = transform.position + (transform.forward * spawnDist);
        
        // Cast down to find the floor
        if (Physics.Raycast(targetPos + Vector3.up * 5f, Vector3.down, out RaycastHit hit)) {
            targetPos = hit.point;
            
            // Scaler lift
            float yOffset = (learnedPrefab.transform.localScale.y / 2f);
            targetPos.y += yOffset;

            EchoManager.Instance.RequestEcho(learnedPrefab, targetPos, transform.rotation);
        }
    }
}