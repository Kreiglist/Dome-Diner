using UnityEngine;

public abstract class Singleton : MonoBehaviour
{
    public static Singleton Instance;

    protected void Awake()
    {
        if (Instance is null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}