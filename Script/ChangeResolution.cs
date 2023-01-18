using UnityEngine;

public class ChangeResolution : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod]
    static void OnRuntimeMethodLoad()
    {
        Screen.SetResolution( 1080, 5120, true, 60);
    }
}
