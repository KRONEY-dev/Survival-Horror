using TMPro;
using DG.Tweening;
using Extensions;
using System;

public class IntroPage : BasePage
{
    private const float PreIntroAnimationTime = 1f;
    private const float IntroAnimationTime = 1.5f;

    protected override string PrefabName => "IntroPage";

    private TextMeshProUGUI _introLabelText;

    private Action _introCompleteEvent;

    public override void Init()
    {
        _introLabelText = SelfTransform.Find("Panel_Intro/Text_Label").GetComponent<TextMeshProUGUI>();
    }

    public override void Show(object data)
    {
        base.Show(data);

        _introCompleteEvent = data as Action;

        _introLabelText.SetAlpha(0);
        IntroAnimation();
    }

    private void IntroAnimation()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(PreIntroAnimationTime);
        sequence.Append(_introLabelText.DOFade(1, IntroAnimationTime));
        sequence.Append(_introLabelText.DOFade(0, IntroAnimationTime));
        sequence.AppendCallback(new TweenCallback(_introCompleteEvent));
    }
}
