using UnityEngine;

//TODO: comentar esta classe
public abstract class SingletonDestroyable<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            return instance;
        }
        set
        {
            if (null == instance)
            {
                instance = value;
            }
            else if (instance != value) //destroy anyone that tries to make its place
            {
                Destroy(value.gameObject);
            }
        }
    }

    public virtual void Awake()
    {
        Instance = this as T;
    }

    protected virtual void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}
