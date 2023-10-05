using System;
using System.Collections.Generic;
using UnityEngine;

public interface IUIManager
{
    GameObject Canvas { get; }
    Camera UICamera { get; }
    List<IPage> CurrentPages { get; }
    Transform PagesParent { get; }
    Transform MainInterfaceParent { get; }

    void SetPage<T>(object message = null, bool hideAll = false) where T : IPage;
    void HideAllPages();

    void FadeAnimation(GameObject uiObject, bool fadeIn, Action callback = null);

    T GetPage<T>() where T : IPage;
}