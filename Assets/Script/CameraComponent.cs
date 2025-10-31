using UnityEngine;

public class CameraComponent : MonoBehaviour
{
    public static CameraComponent Instance;
    public float focalLength = 5f;

    private void Awake()
    {
        Instance = this;
    }
}
