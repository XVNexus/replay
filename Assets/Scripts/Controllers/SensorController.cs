using UnityEngine;

public class SensorController : MonoBehaviour
{
    int overlaps;

    public bool isTriggered {
        get {
            return overlaps > 0;
        }
    }

    // Keep track of how many objects are hitting this trigger to see if the player is on the ground
    void OnTriggerEnter2D() => overlaps++;
    void OnTriggerExit2D() => overlaps--;
}
