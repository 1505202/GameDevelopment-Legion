using UnityEngine;
using UnityEngine.UI;


public class ClockUI : MonoBehaviour 
{
    [SerializeField] private RectTransform leftMask;
    [SerializeField] private RectTransform rightMask;

    [SerializeField] private Gradient gradient;

    private Image leftUnderlayImage;
    private Image rightUnderlayImage;

    float maxLeftMaskWidth;
    float maxRightMaskWidth;

    private void Start()
    {
        maxLeftMaskWidth = leftMask.sizeDelta.x;
        maxRightMaskWidth = rightMask.sizeDelta.x;

        leftUnderlayImage = leftMask.GetChild(0).GetComponent<Image>();
        rightUnderlayImage = rightMask.GetChild(0).GetComponent<Image>();
    }

    private void Update()
    {
        float normalizedLength = GameManager.Instance.NormalizedTime() ;

        leftMask.sizeDelta = new Vector2(maxLeftMaskWidth * normalizedLength, leftMask.sizeDelta.y);
        rightMask.sizeDelta = new Vector2(maxRightMaskWidth * normalizedLength, rightMask.sizeDelta.y);

        leftUnderlayImage.color = gradient.Evaluate(normalizedLength);
        rightUnderlayImage.color = gradient.Evaluate(normalizedLength);
    }

}
