using UnityEngine;

public class ForceAspect : MonoBehaviour
{
    private const float targetAspect = 16f / 9f;

    void Update()
    {
        int width = Screen.width;
        int height = Screen.height;

        if (width < 400 || height < 400)
            return;

        float currentAspect = (float)width / height;

        if (Mathf.Abs(currentAspect - targetAspect) > 0.01f)
        {
            int correctedWidth = Mathf.RoundToInt(height * targetAspect);
            if (correctedWidth < 400) return;

            Screen.SetResolution(correctedWidth, height, FullScreenMode.Windowed);
        }
    }
}