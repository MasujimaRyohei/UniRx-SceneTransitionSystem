using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using UniRx;

public class Fade : MonoBehaviour
{
    private IFade fade;

    private float cutoutRange;

    public UnityEvent onFinishFadeIn;
    public UnityEvent onFinishFadeOut;

    private void Awake()
    {
        Initialize();
    }

    private void Start()
    {
        fade.Range = cutoutRange;
    }

    private void Initialize()
    {
        fade = GetComponent<IFade>();
    }

    private void OnValidate()
    {
        Initialize();
        fade.Range = cutoutRange;
    }

    private IEnumerator FadeoutCoroutine(IObserver<float> observer, float time)
    {
        float endTime = Time.timeSinceLevelLoad + time * (cutoutRange);

        WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

        while (Time.timeSinceLevelLoad <= endTime)
        {
            cutoutRange = (endTime - Time.timeSinceLevelLoad) / time;
            fade.Range = cutoutRange;
            yield return waitForEndOfFrame;
        }
        cutoutRange = 0;
        fade.Range = cutoutRange;

        observer.OnCompleted();
    }

    private IEnumerator FadeinCoroutine(IObserver<float> observer, float time)
    {
        float endTime = Time.timeSinceLevelLoad + time * (1 - cutoutRange);

        WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

        while (Time.timeSinceLevelLoad <= endTime)
        {
            cutoutRange = 1 - ((endTime - Time.timeSinceLevelLoad) / time);
            fade.Range = cutoutRange;
            yield return waitForEndOfFrame;
        }
        cutoutRange = 1;
        fade.Range = cutoutRange;

        observer.OnCompleted();
    }

    public IDisposable FadeOut(float time)
    {
        StopAllCoroutines();
        return Observable.FromCoroutine<float>(observer => FadeoutCoroutine(observer, time)).Subscribe(null, _ =>
        {
            if (onFinishFadeOut != null)
                onFinishFadeOut.Invoke();
        });
    }

    public IDisposable FadeIn(float time)
    {
        StopAllCoroutines();
        return Observable.FromCoroutine<float>(observer => FadeinCoroutine(observer, time)).Subscribe(null, _ =>
        {
            if (onFinishFadeIn != null)
                onFinishFadeIn.Invoke();
        });
    }
}