using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialGlow : MonoBehaviour
{
    public Color flashColor = Color.red; // The color to flash to
    public float flashSpeed = 2f; // Speed of flashing effect
    public bool isFlashing = true; // Flag to determine whether the object should be flashing
    private Image targetImage; // Reference to the Image component
    private Color originalColor;

    void Start()
    {
        // Get the Image component from the Button's Target Graphic
        targetImage = GetComponent<Image>();

        if (targetImage != null)
        {
            originalColor = targetImage.color;
        }
    }

    void Update()
    {
        if (targetImage != null && isFlashing)
        {
            // Create a pulsing effect by interpolating between the original and flash color
            float t = Mathf.Abs(Mathf.Sin(Time.unscaledTime * flashSpeed));
            targetImage.color = Color.Lerp(originalColor, flashColor, t);
        }
    }

    // Function to start flashing
    public void StartFlashing()
    {
        isFlashing = true;
    }

    // Function to stop flashing
    public void StopFlashing()
    {
        isFlashing = false;
        if (targetImage != null)
        {
            // Reset to the original color
            targetImage.color = originalColor;
        }
    }
}
