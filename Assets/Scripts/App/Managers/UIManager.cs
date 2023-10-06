using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class UIManager : IUIManager, IService
{
    private const float FadeAnimationDuration = 0.6f;

    public List<IPage> CurrentPages { get; private set; }
    public GameObject Canvas { get; set; }
    public Camera UICamera { get; private set; }

    public Transform PagesParent { get; private set; }
    public Transform MainInterfaceParent { get; private set; }

    private bool _inited;

    private List<IPage> _uiPages;

    private ISoundManager _soundManager;

    public void Init()
    {
        Canvas = GameObject.Find("Canvas");

        UICamera = GameObject.Find("CameraContainer/UICamera").GetComponent<Camera>();

        GameObject uIElementsParent = Object.Instantiate(Resources.Load<GameObject>("Prefabs/UI/UIElementsParent"), Canvas.transform);
        uIElementsParent.name = "UIElementsParent";

        PagesParent = uIElementsParent.transform.Find("Pages");
        MainInterfaceParent = uIElementsParent.transform.Find("MainInterface");

        CurrentPages = new List<IPage>();
        _uiPages = new List<IPage>
        {
            new IntroPage(),
            new HomePage(),
            new GamePage(),
            new GameEndPage()
        };

        UIElementsInit();

        _soundManager = GameClient.Get<ISoundManager>();
    }

    public void Update()
    {
        if (!_inited)
            return;

        foreach (IPage page in _uiPages)
            page.Update();
    }

    public void Dispose()
    {
        foreach (IPage page in _uiPages)
            page.Dispose();
    }

    public void SetPage<T>(object message = null, bool hideAll = false) where T : IPage
    {
        if (CurrentPages.Find(item => item.GetType() == typeof(T)) != null)
            return;

        if (hideAll)
        {
            HideAllPages();
        }
        else
        {
            for (int i = 0; i < CurrentPages.Count; i++)
            {
                if (!CurrentPages[i].IsPermanent)
                {
                    CurrentPages[i].Hide();
                    CurrentPages.RemoveAt(i--);
                }
            }
        }

        foreach (IPage page in _uiPages)
        {
            if (page is T)
            {
                CurrentPages.Add(page);
                
                if (message == null)
                {
                    page.Show();
                }
                else
                {
                    page.Show(message);
                }

                if(!page.IsPermanent)
                {
                    _soundManager.SetBackgroundMusicByPage<T>();
                }
                break;
            }
        }
    }

    public void HideAllPages()
    {
        foreach (IPage page in _uiPages)
        {
            page.Hide();
        }

        CurrentPages.Clear();
    }

    public void FadeAnimation(GameObject uiObject, bool fadeIn, Action callback = null)
    {
        if (!uiObject.TryGetComponent(out CanvasGroup canvasGroup))
        {
            canvasGroup = uiObject.AddComponent<CanvasGroup>();
        }

        canvasGroup.DOComplete(true);
        canvasGroup.interactable = false;
        canvasGroup.alpha = fadeIn ? 0f : 1f;

        uiObject.SetActive(true);
        canvasGroup.DOFade(fadeIn ? 1f : 0f, FadeAnimationDuration).OnComplete(() =>
        {
            canvasGroup.interactable = true;
            Object.Destroy(canvasGroup);

            if(!fadeIn)
            {
                uiObject.SetActive(false);
            }

            callback?.Invoke();
        });
    }

    public T GetPage<T>() where T : IPage
    {
        IPage page = null;
        foreach (var _page in _uiPages)
        {
            if (_page is T)
            {
                page = _page;
                break;
            }
        }

        return (T)page;
    }

    private void UIElementsInit()
    {
        foreach (IPage page in _uiPages)
            page.Init();

        _inited = true;
    }
}