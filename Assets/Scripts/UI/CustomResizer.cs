using UnityEngine;

public class ContentResizer : MonoBehaviour
{
    [SerializeField] private RectTransform _parentRectTransform;

    private void Update()
    {
        ResizeToFit();
    }

    public void ResizeToFit()
    {
        float totalWidth = 0f;
        float maxHeight = 0f;

        foreach (RectTransform child in _parentRectTransform)
        {
            if (child.gameObject.activeSelf)
            {
                totalWidth += child.rect.width;
                float childTop = child.anchoredPosition.y + (child.pivot.y * child.rect.height);
                float childBottom = child.anchoredPosition.y - ((1 - child.pivot.y) * child.rect.height);
                float childHeight = Mathf.Abs(childTop - childBottom);

                if (childHeight > maxHeight)
                {
                    maxHeight = childHeight;
                }
            }
        }

        _parentRectTransform.sizeDelta = new Vector2(totalWidth, maxHeight);
    }
}