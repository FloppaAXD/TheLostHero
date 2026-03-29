using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class CutsceneCave : MonoBehaviour
{
    [SerializeField] private Image[] frames;
    [SerializeField] private float fadeDuration = 0.5f;

    void Awake()
    {
        foreach (Image frame in frames)
        {
            frame.enabled = false;
            Color c = frame.color;
            c.a = 1f;
            frame.color = c;
        }
    }

    void Start()
    {
        StartCoroutine(ShowFrames());
    }

    IEnumerator ShowFrames()
    {
        for (int i = 0; i <= 8; i++)
        {
            frames[i].enabled = true;

            yield return new WaitUntil(() => Input.anyKeyDown);
            yield return null;

            frames[i].enabled = false;
        }

        yield return StartCoroutine(FadeIn(frames[9], fadeDuration));
        yield return new WaitUntil(() => Input.anyKeyDown);
        yield return null;
        frames[9].enabled = false;

        yield return StartCoroutine(FadeIn(frames[10], fadeDuration));
        yield return new WaitUntil(() => Input.anyKeyDown);

        SceneManager.LoadScene(0);
    }

    IEnumerator FadeIn(Image img, float duration)
    {
        Color c = img.color;
        c.a = 0f;
        img.color = c;

        img.enabled = true;

        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Clamp01(t / duration);
            img.color = c;
            yield return null;
        }

        c.a = 1f;
        img.color = c;
    }
}