using UnityEngine;

public class TotemMember : MonoBehaviour
{
    [Header("State")]
    public TotemMember carrying;   // The hero on my shoulders
    public TotemMember carriedBy;  // The hero I am standing on

    [Header("Settings")]
    public float stackHeight = 1.2f; // Offset for the player above

    public bool IsBase => carriedBy == null && carrying != null;
    public bool IsTop => carrying == null && carriedBy != null;
    public bool IsInTotem => carrying != null || carriedBy != null;

    // Helper to get the very bottom of the stack (The Legs)
    public TotemMember GetStackBase()
    {
        TotemMember current = this;
        while (current.carriedBy != null)
        {
            current = current.carriedBy;
        }
        return current;
    }
}