using System;
using UnityEngine;

public interface IInputManager
{
    bool CanHandleInput { get; set; }

    int RegisterInputHandler(
        InputManager.InputType type, int inputCode, Action<Vector3> onInputUp = null, Action<Vector3> onInputDown = null,
        Action<Vector3> onInput = null, Action<object> onInputEndParametrized = null);

    void UnregisterInputHandler(int index);
}