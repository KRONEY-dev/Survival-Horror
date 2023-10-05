using UnityEngine;

public class BasePage : BaseUIElement, IPage
{
    protected override string PrefabRootPath => "Prefabs/UI/Pages/";
    protected override string PrefabName => "";
    protected override Transform Parent => UiManager.PagesParent;

    public virtual bool IsPermanent => false;

    public virtual void Init() { }

    public virtual void Show()
    {
        BaseShow();
    }

    public virtual void Show(object data)
    {
        BaseShow();
    }

    public virtual void Hide()
    {
        if (!IsUniqueShowHide)
        {
            UiManager.FadeAnimation(SelfObject, false);
        }
    }

    public virtual void Update() { }

    public virtual void Dispose() { }

    private void BaseShow()
    {
        if (!IsUniqueShowHide)
        {
            UiManager.FadeAnimation(SelfObject, true);
        }
    }
}