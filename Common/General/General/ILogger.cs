using System;
using System.Collections.Generic;
using System.Text;


public interface ILogger
{
    void Log(string msg);
    void LogWarning(string msg);
    void LogError(string msg);
}

