using DG.Tweening;
using UnityEngine;

[ExecuteAlways]
public class GridLayoutGO : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] public float spacingX = 0; // Khoảng cách ngang
    [SerializeField] public float spacingY = 0; // Khoảng cách dọc
    [SerializeField] public int maxColumns = 0; // Số cột tối đa, 0 = không giới hạn
    [SerializeField] public Vector2 padding = Vector2.zero; // Khoảng cách lề
    [SerializeField] public bool autoRepositionOnStart = true;
    [SerializeField] public bool centerPivot = false;

    [Header("Alignment")]
    [SerializeField] public HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left;
    [SerializeField] public VerticalAlignment verticalAlignment = VerticalAlignment.Top;

    [Header("Size Adjustment")]
    [SerializeField] private bool useBoundsForSize = false; // Sử dụng bounds của renderer để tính kích thước
    [SerializeField] private Vector2 defaultChildSize = new Vector2(1f, 1f); // Kích thước mặc định nếu không tìm thấy bounds

    [Header("Animation Settings")]
    [SerializeField] private bool useAnimation = true; // Bật/tắt animation
    [SerializeField] private float animationDuration = 0.5f; // Thời gian chuyển động
    [SerializeField] private Ease animationEase = Ease.InOutQuad;
    public enum HorizontalAlignment { Left, Center, Right }
    public enum VerticalAlignment { Top, Middle, Bottom }

    private void Start()
    {
        if (autoRepositionOnStart)
        {
            Invoke( "RepositionChildren", 0.2f);
        }
    }

    public void RepositionChildren()
    {
        if (transform.childCount == 0) return;

        int count = transform.childCount;
        int columns = maxColumns > 0 ? Mathf.Min(maxColumns, count) : count;
        int rows = maxColumns > 0 ? Mathf.CeilToInt((float)count / columns) : 1;

        // Tính kích thước của phần tử
        Vector2 childSize = useBoundsForSize ? CalculateChildSizeFromBounds() : defaultChildSize;

        for (int i = 0; i < count; i++)
        {
            Transform child = transform.GetChild(i);
            int row = i / columns;
            int col = i % columns;

            // Tính vị trí cơ bản
            float xPos = col * (childSize.x + spacingX) + padding.x;
            float yPos = -row * (childSize.y + spacingY) - padding.y;

            // Căn chỉnh ngang
            if (horizontalAlignment == HorizontalAlignment.Center)
                xPos += (columns - 1) * (childSize.x + spacingX) * -0.5f;
            else if (horizontalAlignment == HorizontalAlignment.Right)
                xPos += (columns - 1) * (childSize.x + spacingX) * -1f;

            // Căn chỉnh dọc
            if (verticalAlignment == VerticalAlignment.Middle)
                yPos += (rows - 1) * (childSize.y + spacingY) * 0.5f;
            else if (verticalAlignment == VerticalAlignment.Bottom)
                yPos += (rows - 1) * (childSize.y + spacingY);

            // Cập nhật vị trí
            Vector3 localPos = new Vector3(xPos, yPos, 0f);
            if (centerPivot)
            {
                localPos.x -= (columns - 1) * (childSize.x + spacingX) * 0.5f;
                localPos.y += (rows - 1) * (childSize.y + spacingY) * 0.5f;
            }

            if (useAnimation && !Application.isEditor || Application.isPlaying)
            {
                child?.DOLocalMove(localPos, animationDuration).SetEase(animationEase);
            }
            else
            {
                child.localPosition = localPos;
            }
        }
    }

    private Vector2 CalculateChildSizeFromBounds()
    {
        Vector2 size = defaultChildSize;
        int count = transform.childCount;

        for (int i = 0; i < count; i++)
        {
            Transform child = transform.GetChild(i);
            Renderer renderer = child.GetComponent<Renderer>();
            if (renderer != null)
            {
                Bounds bounds = renderer.bounds;
                size.x = Mathf.Max(size.x, bounds.size.x);
                size.y = Mathf.Max(size.y, bounds.size.y);
            }
            else
            {
                // Nếu không có renderer, thử lấy Collider
                Collider2D collider2D = child.GetComponent<Collider2D>();
                if (collider2D != null)
                {
                    Bounds bounds = collider2D.bounds;
                    size.x = Mathf.Max(size.x, bounds.size.x);
                    size.y = Mathf.Max(size.y, bounds.size.y);
                }
            }
        }

        return size;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        RepositionChildren();
    }
#endif
}