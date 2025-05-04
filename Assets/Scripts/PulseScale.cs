using UnityEngine;

public class PulseScale : MonoBehaviour
{
    [Header("Scale Settings")]
    public Vector3 minScale = Vector3.one;          // Scale nhỏ nhất (1)
    public Vector3 maxScale = Vector3.one * 2f;     // Scale lớn nhất (2)
    public float speed = 1f;                        // Tốc độ scale (điều chỉnh trong Inspector)

    private bool scalingUp = true;
    private Vector3 targetScale;

    void Start()
    {
        transform.localScale = minScale;
        targetScale = maxScale;
    }

    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * speed);
        if (Vector3.Distance(transform.localScale, targetScale) < 0.01f)
        {
            scalingUp = !scalingUp;
            targetScale = scalingUp ? maxScale : minScale;
        }
    }
}
