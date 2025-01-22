using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private static BackgroundMusic instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // Ensures this GameObject persists across scenes
        }
        else
        {
            Destroy(gameObject);  // Prevent duplicate music objects
        }
    }
}