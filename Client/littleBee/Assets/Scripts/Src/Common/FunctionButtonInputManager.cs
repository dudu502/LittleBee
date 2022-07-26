using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class FunctionButtonInputManager: MonoBehaviour
{
    public enum Function : byte
    {
        None = 0,
        FIRE = 1,
    }
    public static FunctionButtonInputManager Instance { get; private set; }
    public Function Func = Function.None;
    private void Awake()
    {
        Instance = this;
    }
    public void Reset()
    {
        Func = Function.None;
    }
    private void Update()
    {
        
    }
}

