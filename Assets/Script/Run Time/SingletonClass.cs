using UnityEngine;

public abstract class SingletonClass<T> : MonoBehaviour where T : Component
{
    private static T instance;

    public static T Instance { get { return instance; } }

    // This is needed for the NUnit test
    public void SetInstance() 
    {
        if (instance == null)
            instance = this as T;
        else
            Destroy(gameObject);
    }
    protected virtual void Awake() 
    {
        SetInstance();
    }
}
