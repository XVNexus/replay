using UnityEngine;

public class GoalController : MonoBehaviour
{
    // When the player touches the goal, signal that the level has been completed
    public void OnTriggerEnter2D()
    {
        EventSystem.current.TriggerLevelComplete();
    }
}
