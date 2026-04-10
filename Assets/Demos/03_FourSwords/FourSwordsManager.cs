using UnityEngine;

public class FourSwordsManager : MonoBehaviour
{
    public CoopTrigger plateA;
    public CoopTrigger plateB;
    public GameObject rewardBox;

    private bool puzzleSolved = false;

    void Update()
    {
        if (puzzleSolved) return;

        // The "Gate" logic: Both must be true
        if (plateA.isPressed && plateB.isPressed)
        {
            SolvePuzzle();
        }
    }

    void SolvePuzzle()
    {
        puzzleSolved = true;
        rewardBox.SetActive(true);
        
        if(SoundManager.Instance != null) SoundManager.Instance.PlaySuccess();
        Debug.Log("Puzzle Solved! Box appeared.");
    }
}