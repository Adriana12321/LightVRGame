using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
    public Image blackFadeImage;
    public GameObject blackFadeGO;
    public Image whiteFadeImage;
    public GameObject whiteFadeGO;

    public float fadeDuration = 2f;

    private bool isFading = false;

    void Start()
    {
        if (blackFadeImage == null)
        {
            Debug.LogError("[ScreenFader] No fadeImage assigned!");
            return;
        }

        StartCoroutine(FadeFromBlack());
    }

    public IEnumerator FadeFromBlack()
    {

        blackFadeGO.SetActive(true);
        isFading = true;

        float time = 0f;
        Color color = blackFadeImage.color;
        color.a = 1f;
        color = Color.black;
        blackFadeImage.color = color;

        while (time < fadeDuration)
        {
            color.a = Mathf.Lerp(1f, 0f, time / fadeDuration);
            blackFadeImage.color = color;
            time += Time.deltaTime;
            yield return null;
        }

        color.a = 0f;
        blackFadeImage.color = color;
        blackFadeGO.SetActive(false); // Hide the panel
        isFading = false;
    }

    public void FadeToWhite()
    {
        if (!isFading)
        {
            whiteFadeGO.SetActive(true); // Ensure the panel is visible
            StartCoroutine(FadeToWhiteRoutine());
        }
    }

    private IEnumerator FadeToWhiteRoutine()
    {
        isFading = true;

        float time = 0f;
        Color color = Color.white;
        color.a = 0f;
        whiteFadeImage.color = color;

        while (time < fadeDuration)
        {
            color.a = Mathf.Lerp(0f, 1f, time / fadeDuration);
            whiteFadeImage.color = color;
            time += Time.deltaTime;
            yield return null;
        }

        color.a = 1f;
        whiteFadeImage.color = color;
        isFading = false;

        Debug.Log("[ScreenFader] Fade to white complete.");
    }
}
