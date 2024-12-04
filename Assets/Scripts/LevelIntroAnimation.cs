using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class LevelIntroAnimation : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public RectTransform goalStar;
    [SerializeField] public TextMeshProUGUI goalText;
    [SerializeField] public Image goalStarImage;  // 星星的Image组件
    [SerializeField] public Image overlayMask;
    
    [Header("Animation Settings")]
    [SerializeField] public Vector2 starStartPosition = new Vector2(0, 0);  // 屏幕中心
    [SerializeField] public Vector2 starEndPosition;  // 左上角位置
    [SerializeField] public float textFadeDuration = 1f;
    [SerializeField] public float starMoveDuration = 1.5f;
    [SerializeField] public float overlayFadeDuration = 1f;
    [SerializeField] public float initialDelay = 0.5f;
    
    private void Start()
    {
        // 初始化设置
        starEndPosition = goalStar.anchoredPosition;
        SetupInitialState();
        StartIntroSequence();
    }
    
    private void SetupInitialState()
    {
        // 设置初始状态
        goalStar.anchoredPosition = starStartPosition;
        goalStar.localScale = Vector3.one * 3f;  // 初始大一点
        goalStarImage.color = new Color(1, 1, 1, 0); // 完全透明
        
        // 设置文字初始透明
        goalText.alpha = 0f;
        goalText.rectTransform.anchoredPosition = new Vector2(-100f, 0f);  // 在星星左边
        
        // 设置遮罩初始状态
        Color overlayColor = new Color(0, 0, 0, 0.7f);
        overlayMask.color = overlayColor;
    }
    
    private void StartIntroSequence()
    {
        Sequence introSequence = DOTween.Sequence();
        
        introSequence.Append(overlayMask.DOFade(0.7f, 0.3f));
        
        // Fade in text and star simultaneously
        introSequence.Append(goalText.DOFade(1f, textFadeDuration));
        introSequence.Join(goalStarImage.DOFade(1f, textFadeDuration));
        introSequence.Join(goalText.rectTransform.DOAnchorPos(
            new Vector2(-150f, 0f), textFadeDuration).SetEase(Ease.OutQuad));
        
        introSequence.AppendInterval(0.5f);
        
        // Fade out text then move star
        introSequence.Append(goalText.DOFade(0f, textFadeDuration));
        
        introSequence.Append(goalStar.DOAnchorPos(starEndPosition, starMoveDuration)
            .SetEase(Ease.OutQuad));
        introSequence.Join(goalStar.DOScale(Vector3.one, starMoveDuration));
        
        introSequence.Append(overlayMask.DOFade(0f, overlayFadeDuration));
        
        introSequence.OnComplete(() => {
            Destroy(overlayMask.gameObject);
            Destroy(goalText.gameObject);
        });
    }
}