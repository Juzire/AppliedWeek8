using UnityEngine;

public class Item : MonoBehaviour
{
    public Vector3 itemPosition;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            spriteRenderer.color = GetRandomColor();
    }

    private void Update()
    {
        if (CameraComponent.Instance == null) return;

        float focal = CameraComponent.Instance.focalLength;
        float perspective = focal / (focal + itemPosition.z);

        float baseScale = 0.08f;

        float scaleBoost = Mathf.Lerp(baseScale, 0.12f, Mathf.InverseLerp(2f, 0f, itemPosition.z));
        float finalScale = perspective * scaleBoost;

        transform.localScale = Vector3.one * finalScale;
        transform.position = new Vector2(itemPosition.x, itemPosition.y) * perspective;
    }

    private Color GetRandomColor()
    {
        return new Color(Random.value, Random.value, Random.value);
    }
}
