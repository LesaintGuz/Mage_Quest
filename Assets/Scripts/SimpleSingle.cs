using UnityEngine;

public class SimpleSingle<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance = null;
    public static T si
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<T>();
                if (instance == null)
                    Debug.LogWarning("SimpleSingle error : Object " + typeof(T) + " never created or already destroyed");
            }
            return instance;
        }
    }
}
