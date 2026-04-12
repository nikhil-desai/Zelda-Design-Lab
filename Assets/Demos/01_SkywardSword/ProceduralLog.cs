using UnityEngine;
using EzySlice; // Required: Download EzySlice from GitHub/Asset Store

public class ProceduralLog : MonoBehaviour, ICuttable
{
    [Header("Materials")]
    public Material crossSectionMaterial; // Texture for the inside of the log
    
    [Header("Physics")]
    public float cutForce = 5f;

    public void OnCut(Vector3 hitPoint, Vector3 normal)
    {
        // 1. Slice the Mesh
        // This generates a 'Hull' object containing the two new sets of triangles
        SlicedHull hull = gameObject.Slice(hitPoint, normal);

        if (hull != null)
        {
            // 2. Create the new GameObjects from the hull
            GameObject upper = hull.CreateUpperHull(gameObject, crossSectionMaterial);
            GameObject lower = hull.CreateLowerHull(gameObject, crossSectionMaterial);

            // 3. Setup the pieces (Add physics/colliders)
            SetupPiece(upper);
            SetupPiece(lower);

            // 4. Destroy original
            Destroy(gameObject);
        }
    }

    private void SetupPiece(GameObject obj)
    {
        // Procedural meshes need a MeshCollider (must be Convex for Rigidbody)
        MeshCollider collider = obj.AddComponent<MeshCollider>();
        collider.convex = true;

        Rigidbody rb = obj.AddComponent<Rigidbody>();
        
        // Add a bit of 'pop' so the pieces fly apart
        rb.AddExplosionForce(cutForce, obj.transform.position, 1f, 1f, ForceMode.Impulse);
    }
}