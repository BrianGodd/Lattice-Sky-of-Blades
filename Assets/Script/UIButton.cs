using UnityEngine;
using UnityEngine.EventSystems;

public class UIButton : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler
{
    public float hoverScale = 1.1f;
    public float speed = 10f;

    Vector3 normalScale;
    Vector3 targetScale;

    void Awake()
    {
        normalScale = transform.localScale;
        targetScale = normalScale;
    }

    void Update()
    {
        transform.localScale = Vector3.Lerp(
            transform.localScale,
            targetScale,
            Time.unscaledDeltaTime * speed
        );
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetScale = normalScale * hoverScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetScale = normalScale;
    }
}
