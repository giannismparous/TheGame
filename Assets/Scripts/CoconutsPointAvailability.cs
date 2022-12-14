using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoconutsPointAvailability : MonoBehaviour
{
    private bool available;

    void Awake() {
        available = true;
    }

    void OnTriggerStay2D(Collider2D other)
    {

        if (!other.CompareTag("Player"))available = false;

    }

    void OnTriggerExit2D(Collider2D other)
    {

        available = true;

    }

    public bool IsAvailable() {
        return available;
    }
}
