using UnityEngine;

public class Goal : MonoBehaviour
{
    public string targetObjectName; // The object name that can score
    public int scoreValue = 1;      // How many points to give
    private bool objectInside = false;

    void OnTriggerEnter(Collider other)
    {
        if (!objectInside && other.gameObject.name == targetObjectName)
        {
            GlobalManager.Instance.AddScore(scoreValue);
            objectInside = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (objectInside && other.gameObject.name == targetObjectName)
        {
            GlobalManager.Instance.RemoveScore(scoreValue);
            objectInside = false;
        }
    }
}
