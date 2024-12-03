using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class GoalStarAnimation : MonoBehaviour
{
    private RectTransform rectTransform;
    [SerializeField] private Vector2 startPosition;  // 屏幕中央
    [SerializeField] private Vector2 endPosition;      // 最终位置会自动设置
    [SerializeField] private Vector3 startScale = Vector3.one * 2f;  // 起始大小
    [SerializeField] private Vector3 endScale = Vector3.one;         // 最终大小
    [SerializeField] private float animationDuration = 1.5f;
    [SerializeField] private float startDelay = 0.5f;
    
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        // 保存当前位置作为终点位置
        endPosition = rectTransform.anchoredPosition;
    }
    
    private void Start()
    {
        // 设置初始位置和大小
        rectTransform.anchoredPosition = startPosition;
        rectTransform.localScale = startScale;
        
        // 创建动画序列
        Sequence starAnimation = DOTween.Sequence();
        
        starAnimation.AppendInterval(startDelay);
        
        // 使用 DOAnchorPos 而不是 DOMove
        starAnimation.Append(rectTransform.DOAnchorPos(endPosition, animationDuration)
            .SetEase(Ease.OutQuad));
            
        starAnimation.Join(rectTransform.DOScale(endScale, animationDuration)
            .SetEase(Ease.OutQuad));
    }
}