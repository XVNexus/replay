using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KillzoneController : MonoBehaviour
{
    // If the player enters a kill zone, reset their velocity and teleport them back to their spawn point
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            collider.gameObject.GetComponent<PlayerController>().Kill();
        }
    }
}
