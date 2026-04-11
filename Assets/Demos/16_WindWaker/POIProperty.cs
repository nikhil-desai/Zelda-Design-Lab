using UnityEngine;

public class POIProperty : MonoBehaviour
{
    [Tooltip("Higher number = Higher Priority. Link will look at 10s before 1s.")]
    public int priority = 1;
}