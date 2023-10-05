using System;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : IService, IInputManager
{
    public enum InputType
    {
        Unknown,

        Mouse,
    }

    private readonly object _sync = new object();

    private List<InputEvent> _inputHandlers;

    private int _customFreeIndex;

    public bool CanHandleInput { get; set; }

    /// <summary>
    ///     Registers the input handler by type and code.
    /// </summary>
    /// <returns>The input registration index.</returns>
    /// <param name="type">Type.</param>
    /// <param name="inputCode">Input code.</param>
    /// <param name="onInputUp">On input up, with position on screen.</param>
    /// <param name="onInputDown">On input down, with position on screen.</param>
    /// <param name="onInput">On input, with position on screen.</param>
    public int RegisterInputHandler(InputType type, int inputCode, Action<Vector3> onInputUp = null, Action<Vector3> onInputDown = null,
        Action<Vector3> onInput = null)
    {
        lock (_sync)
        {
            InputEvent item = new InputEvent
            {
                Code = inputCode,
                InputCallback = onInput,
                InputDownCallback = onInputDown,
                InputUpCallback = onInputUp,
                Type = type,
                Index = _customFreeIndex++
            };

            _inputHandlers.Add(item);

            return item.Index;
        }
    }

    public void UnregisterInputHandler(int index)
    {
        lock (_sync)
        {
            InputEvent inputHandler = _inputHandlers.Find(x => x.Index == index);

            if (inputHandler != null)
            {
                _inputHandlers.Remove(inputHandler);
            }
        }
    }

    public void Init()
    {
        CanHandleInput = true;

        _inputHandlers = new List<InputEvent>();
    }

    public void Update()
    {
        if (CanHandleInput)
        {
            if (_inputHandlers.Count > 0)
            {
                lock (_sync)
                {
                    HandleInput();
                }
            }
        }
    }

    public void Dispose()
    {
        _inputHandlers.Clear();
    }

    private void HandleInput()
    {
        InputEvent item;
        for (int i = 0; i < _inputHandlers.Count; i++)
        {
            item = _inputHandlers[i];

            switch (item.Type)
            {
                case InputType.Mouse:
                    {
                        Vector3 mousePosition = Input.mousePosition;

                        if (Input.GetMouseButtonDown(item.Code))
                        {
                            item.InvokeInputDownCallback(mousePosition);
                        }

                        if (Input.GetMouseButtonUp(item.Code))
                        {
                            item.InvokeInputUpCallback(mousePosition);
                        }

                        if (Input.GetMouseButton(item.Code))
                        {
                            item.InvokeInputCallback(mousePosition);
                        }
                    }
                    break;
            }
        }
    }
}

public class InputEvent
{
    public int Index { get; set; }

    public int Code { get; set; }

    public bool Started { get; set; }

    public InputManager.InputType Type;

    public Action<Vector3> InputUpCallback;

    public Action<Vector3> InputDownCallback;

    public Action<Vector3> InputCallback;

    public void InvokeInputUpCallback(Vector3 positionOnScreen)
    {
        InputUpCallback?.Invoke(positionOnScreen);
    }

    public void InvokeInputDownCallback(Vector3 positionOnScreen)
    {
        InputDownCallback?.Invoke(positionOnScreen);
    }

    public void InvokeInputCallback(Vector3 positionOnScreen)
    {
        InputCallback?.Invoke(positionOnScreen);
    }
}
