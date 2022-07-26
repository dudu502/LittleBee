using System.Collections.Generic;
using UnityEngine;
public class InputKeyCodeState
{
    public KeyCode Key;
    public bool Active = false;
}
/// <summary>
/// 
/// </summary>
public class InputManager:MonoBehaviour
{
    List<InputKeyCodeState> Status;
    public static InputManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
        Status = new List<InputKeyCodeState>();

        Register(new KeyCode[] { KeyCode.A,KeyCode.S,KeyCode.D,KeyCode.W,KeyCode.Space});
    }
    public bool IsKeyCodeActive(KeyCode key)
    {
        foreach(InputKeyCodeState state in Status)
        {
            if (state.Key == key)
                return state.Active;
        }
        return false;
    }
    public bool IsAnyKeyCodeActive()
    {
        foreach (InputKeyCodeState state in Status)
        {
            if (state.Active) return true;
        }
        return false;
    }
    public void Register(KeyCode[] codes)
    {
        foreach (KeyCode c in codes)
            Status.Add(new InputKeyCodeState() { Key=c});
    }
    private void Update()
    {
        foreach (InputKeyCodeState c in Status)
        {
            if (Input.GetKeyDown(c.Key))
                c.Active = true;
            else if (Input.GetKeyUp(c.Key))
                c.Active = false;
        }
    }
}

