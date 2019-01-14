﻿using UnityEngine;
using System.Collections;
using System;

namespace SGF.Debuger
{
    /// <summary>
    /// Description： custom debuger,can control debug or record in memory writing in thread.
    /// Author： chiuanwei
    /// CreateTime: 2015-2-12
    /// </summary>

	
    public class Logger
    {
        static public bool EnableLog = true;
        
        static public void Log(object message)
        {
            Log(message, null);
        }

        static public void Log(object message, string customType)
        {
            if (EnableLog)
            {
                Console.Log(message,customType);
            }
        }

        static public void Log(object message, string customType,Color col)
        {
            if (EnableLog)
            {
                Console.Log(message, customType, col);
            }
        }

        static public void LogError(object message)
        {
            LogError(message, null);
        }
        static public void LogError(object message, string customType)
        {
            if (EnableLog)
            {
                Console.LogError(message, customType);
            }
        }
        static public void LogWarning(object message)
        {
            LogWarning(message, null);
        }
        static public void LogWarning(object message, string customType)
        {
            if (EnableLog)
            {
                Console.LogWarning(message, customType);
            }
        }

        public static void RegisterCommand(string commandString, Func<string[],object> commandCallback, string CMD_Discribes)
        {
            if(EnableLog)
                Console.RegisterCommand(commandString, commandCallback, CMD_Discribes);
        }

        public static void UnRegisterCommand(string commandString)
        {
            if(EnableLog)
                Console.UnRegisterCommand(commandString);
        }

    }
}