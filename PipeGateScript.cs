using UnityEngine;

public class PipeGateScript : MonoBehaviour
{

    public LogicScript logic;

    private bool hasBeenTriggered = false;  // Ensures the trigger is activated only once


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            logic.addScore(1);
        }

        // Ensure each is triggered only once
        if (!hasBeenTriggered && collision.CompareTag("Player"))
        {
            hasBeenTriggered = true;  // Set flag to true to prevent duplicate triggers
            logic.addScore(1);
            Debug.Log("PipeGate triggered once: " + gameObject.name);
        }
    }
}
