using UnityEngine;

public class AudioDontDestroy : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
