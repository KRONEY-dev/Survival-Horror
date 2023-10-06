using UnityEngine;

public abstract class BaseUIElement
{
    protected abstract string PrefabRootPath { get; }

    protected abstract string PrefabName { get; }

    protected abstract Transform Parent { get; }

    protected readonly GameObject SelfObject;

    protected readonly Transform SelfTransform;

    protected virtual bool IsUniqueShowHide => false;

    protected IUIManager UiManager;
    protected ISoundManager SoundManager;

    protected BaseUIElement()
    {
        UiManager = GameClient.Get<IUIManager>();
        SoundManager = GameClient.Get<ISoundManager>();

        var prefab = Resources.Load<GameObject>($"{PrefabRootPath}{PrefabName}");

        SelfObject = Object.Instantiate(prefab, Parent);
        SelfObject.name = prefab.name;

        SelfTransform = SelfObject.transform;

        SelfObject.SetActive(false);
    }
}