#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
using RealStatePtr = UniLua.ILuaState;
using LuaCSFunction = UniLua.CSharpFunctionDelegate;
#else
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLua.LuaDLL.lua_CSFunction;
#endif

using XLua;
using System.Collections.Generic;


namespace XLua.CSObjectWrap
{
    using Utils = XLua.Utils;
    public class UnityEngineDebugWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(UnityEngine.Debug);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 17, 3, 1);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "DrawLine", _m_DrawLine_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DrawRay", _m_DrawRay_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Break", _m_Break_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DebugBreak", _m_DebugBreak_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Log", _m_Log_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "LogFormat", _m_LogFormat_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "LogError", _m_LogError_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "LogErrorFormat", _m_LogErrorFormat_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ClearDeveloperConsole", _m_ClearDeveloperConsole_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "LogException", _m_LogException_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "LogWarning", _m_LogWarning_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "LogWarningFormat", _m_LogWarningFormat_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Assert", _m_Assert_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "AssertFormat", _m_AssertFormat_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "LogAssertion", _m_LogAssertion_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "LogAssertionFormat", _m_LogAssertionFormat_xlua_st_);
            
			
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "unityLogger", _g_get_unityLogger);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "developerConsoleVisible", _g_get_developerConsoleVisible);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "isDebugBuild", _g_get_isDebugBuild);
            
			Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "developerConsoleVisible", _s_set_developerConsoleVisible);
            
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					UnityEngine.Debug __cl_gen_ret = new UnityEngine.Debug();
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Debug constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DrawLine_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 2&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)) 
                {
                    UnityEngine.Vector3 start;translator.Get(L, 1, out start);
                    UnityEngine.Vector3 end;translator.Get(L, 2, out end);
                    
                    UnityEngine.Debug.DrawLine( start, end );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 3&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& translator.Assignable<UnityEngine.Color>(L, 3)) 
                {
                    UnityEngine.Vector3 start;translator.Get(L, 1, out start);
                    UnityEngine.Vector3 end;translator.Get(L, 2, out end);
                    UnityEngine.Color color;translator.Get(L, 3, out color);
                    
                    UnityEngine.Debug.DrawLine( start, end, color );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 4&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& translator.Assignable<UnityEngine.Color>(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    UnityEngine.Vector3 start;translator.Get(L, 1, out start);
                    UnityEngine.Vector3 end;translator.Get(L, 2, out end);
                    UnityEngine.Color color;translator.Get(L, 3, out color);
                    float duration = (float)LuaAPI.lua_tonumber(L, 4);
                    
                    UnityEngine.Debug.DrawLine( start, end, color, duration );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 5&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& translator.Assignable<UnityEngine.Color>(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 5)) 
                {
                    UnityEngine.Vector3 start;translator.Get(L, 1, out start);
                    UnityEngine.Vector3 end;translator.Get(L, 2, out end);
                    UnityEngine.Color color;translator.Get(L, 3, out color);
                    float duration = (float)LuaAPI.lua_tonumber(L, 4);
                    bool depthTest = LuaAPI.lua_toboolean(L, 5);
                    
                    UnityEngine.Debug.DrawLine( start, end, color, duration, depthTest );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Debug.DrawLine!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DrawRay_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 2&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)) 
                {
                    UnityEngine.Vector3 start;translator.Get(L, 1, out start);
                    UnityEngine.Vector3 dir;translator.Get(L, 2, out dir);
                    
                    UnityEngine.Debug.DrawRay( start, dir );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 3&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& translator.Assignable<UnityEngine.Color>(L, 3)) 
                {
                    UnityEngine.Vector3 start;translator.Get(L, 1, out start);
                    UnityEngine.Vector3 dir;translator.Get(L, 2, out dir);
                    UnityEngine.Color color;translator.Get(L, 3, out color);
                    
                    UnityEngine.Debug.DrawRay( start, dir, color );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 4&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& translator.Assignable<UnityEngine.Color>(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    UnityEngine.Vector3 start;translator.Get(L, 1, out start);
                    UnityEngine.Vector3 dir;translator.Get(L, 2, out dir);
                    UnityEngine.Color color;translator.Get(L, 3, out color);
                    float duration = (float)LuaAPI.lua_tonumber(L, 4);
                    
                    UnityEngine.Debug.DrawRay( start, dir, color, duration );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 5&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& translator.Assignable<UnityEngine.Color>(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 5)) 
                {
                    UnityEngine.Vector3 start;translator.Get(L, 1, out start);
                    UnityEngine.Vector3 dir;translator.Get(L, 2, out dir);
                    UnityEngine.Color color;translator.Get(L, 3, out color);
                    float duration = (float)LuaAPI.lua_tonumber(L, 4);
                    bool depthTest = LuaAPI.lua_toboolean(L, 5);
                    
                    UnityEngine.Debug.DrawRay( start, dir, color, duration, depthTest );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Debug.DrawRay!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Break_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    UnityEngine.Debug.Break(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DebugBreak_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    UnityEngine.Debug.DebugBreak(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Log_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 1&& translator.Assignable<object>(L, 1)) 
                {
                    object message = translator.GetObject(L, 1, typeof(object));
                    
                    UnityEngine.Debug.Log( message );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 2&& translator.Assignable<object>(L, 1)&& translator.Assignable<UnityEngine.Object>(L, 2)) 
                {
                    object message = translator.GetObject(L, 1, typeof(object));
                    UnityEngine.Object context = (UnityEngine.Object)translator.GetObject(L, 2, typeof(UnityEngine.Object));
                    
                    UnityEngine.Debug.Log( message, context );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Debug.Log!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LogFormat_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count >= 1&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& (LuaTypes.LUA_TNONE == LuaAPI.lua_type(L, 2) || translator.Assignable<object>(L, 2))) 
                {
                    string format = LuaAPI.lua_tostring(L, 1);
                    object[] args = translator.GetParams<object>(L, 2);
                    
                    UnityEngine.Debug.LogFormat( format, args );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count >= 2&& translator.Assignable<UnityEngine.Object>(L, 1)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaTypes.LUA_TNONE == LuaAPI.lua_type(L, 3) || translator.Assignable<object>(L, 3))) 
                {
                    UnityEngine.Object context = (UnityEngine.Object)translator.GetObject(L, 1, typeof(UnityEngine.Object));
                    string format = LuaAPI.lua_tostring(L, 2);
                    object[] args = translator.GetParams<object>(L, 3);
                    
                    UnityEngine.Debug.LogFormat( context, format, args );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Debug.LogFormat!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LogError_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 1&& translator.Assignable<object>(L, 1)) 
                {
                    object message = translator.GetObject(L, 1, typeof(object));
                    
                    UnityEngine.Debug.LogError( message );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 2&& translator.Assignable<object>(L, 1)&& translator.Assignable<UnityEngine.Object>(L, 2)) 
                {
                    object message = translator.GetObject(L, 1, typeof(object));
                    UnityEngine.Object context = (UnityEngine.Object)translator.GetObject(L, 2, typeof(UnityEngine.Object));
                    
                    UnityEngine.Debug.LogError( message, context );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Debug.LogError!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LogErrorFormat_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count >= 1&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& (LuaTypes.LUA_TNONE == LuaAPI.lua_type(L, 2) || translator.Assignable<object>(L, 2))) 
                {
                    string format = LuaAPI.lua_tostring(L, 1);
                    object[] args = translator.GetParams<object>(L, 2);
                    
                    UnityEngine.Debug.LogErrorFormat( format, args );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count >= 2&& translator.Assignable<UnityEngine.Object>(L, 1)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaTypes.LUA_TNONE == LuaAPI.lua_type(L, 3) || translator.Assignable<object>(L, 3))) 
                {
                    UnityEngine.Object context = (UnityEngine.Object)translator.GetObject(L, 1, typeof(UnityEngine.Object));
                    string format = LuaAPI.lua_tostring(L, 2);
                    object[] args = translator.GetParams<object>(L, 3);
                    
                    UnityEngine.Debug.LogErrorFormat( context, format, args );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Debug.LogErrorFormat!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ClearDeveloperConsole_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    UnityEngine.Debug.ClearDeveloperConsole(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LogException_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 1&& translator.Assignable<System.Exception>(L, 1)) 
                {
                    System.Exception exception = (System.Exception)translator.GetObject(L, 1, typeof(System.Exception));
                    
                    UnityEngine.Debug.LogException( exception );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 2&& translator.Assignable<System.Exception>(L, 1)&& translator.Assignable<UnityEngine.Object>(L, 2)) 
                {
                    System.Exception exception = (System.Exception)translator.GetObject(L, 1, typeof(System.Exception));
                    UnityEngine.Object context = (UnityEngine.Object)translator.GetObject(L, 2, typeof(UnityEngine.Object));
                    
                    UnityEngine.Debug.LogException( exception, context );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Debug.LogException!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LogWarning_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 1&& translator.Assignable<object>(L, 1)) 
                {
                    object message = translator.GetObject(L, 1, typeof(object));
                    
                    UnityEngine.Debug.LogWarning( message );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 2&& translator.Assignable<object>(L, 1)&& translator.Assignable<UnityEngine.Object>(L, 2)) 
                {
                    object message = translator.GetObject(L, 1, typeof(object));
                    UnityEngine.Object context = (UnityEngine.Object)translator.GetObject(L, 2, typeof(UnityEngine.Object));
                    
                    UnityEngine.Debug.LogWarning( message, context );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Debug.LogWarning!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LogWarningFormat_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count >= 1&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& (LuaTypes.LUA_TNONE == LuaAPI.lua_type(L, 2) || translator.Assignable<object>(L, 2))) 
                {
                    string format = LuaAPI.lua_tostring(L, 1);
                    object[] args = translator.GetParams<object>(L, 2);
                    
                    UnityEngine.Debug.LogWarningFormat( format, args );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count >= 2&& translator.Assignable<UnityEngine.Object>(L, 1)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaTypes.LUA_TNONE == LuaAPI.lua_type(L, 3) || translator.Assignable<object>(L, 3))) 
                {
                    UnityEngine.Object context = (UnityEngine.Object)translator.GetObject(L, 1, typeof(UnityEngine.Object));
                    string format = LuaAPI.lua_tostring(L, 2);
                    object[] args = translator.GetParams<object>(L, 3);
                    
                    UnityEngine.Debug.LogWarningFormat( context, format, args );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Debug.LogWarningFormat!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Assert_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 1&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 1)) 
                {
                    bool condition = LuaAPI.lua_toboolean(L, 1);
                    
                    UnityEngine.Debug.Assert( condition );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 2&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 1)&& translator.Assignable<UnityEngine.Object>(L, 2)) 
                {
                    bool condition = LuaAPI.lua_toboolean(L, 1);
                    UnityEngine.Object context = (UnityEngine.Object)translator.GetObject(L, 2, typeof(UnityEngine.Object));
                    
                    UnityEngine.Debug.Assert( condition, context );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 2&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 1)&& translator.Assignable<object>(L, 2)) 
                {
                    bool condition = LuaAPI.lua_toboolean(L, 1);
                    object message = translator.GetObject(L, 2, typeof(object));
                    
                    UnityEngine.Debug.Assert( condition, message );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 2&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 1)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    bool condition = LuaAPI.lua_toboolean(L, 1);
                    string message = LuaAPI.lua_tostring(L, 2);
                    
                    UnityEngine.Debug.Assert( condition, message );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 3&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 1)&& translator.Assignable<object>(L, 2)&& translator.Assignable<UnityEngine.Object>(L, 3)) 
                {
                    bool condition = LuaAPI.lua_toboolean(L, 1);
                    object message = translator.GetObject(L, 2, typeof(object));
                    UnityEngine.Object context = (UnityEngine.Object)translator.GetObject(L, 3, typeof(UnityEngine.Object));
                    
                    UnityEngine.Debug.Assert( condition, message, context );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 3&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 1)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<UnityEngine.Object>(L, 3)) 
                {
                    bool condition = LuaAPI.lua_toboolean(L, 1);
                    string message = LuaAPI.lua_tostring(L, 2);
                    UnityEngine.Object context = (UnityEngine.Object)translator.GetObject(L, 3, typeof(UnityEngine.Object));
                    
                    UnityEngine.Debug.Assert( condition, message, context );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Debug.Assert!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AssertFormat_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count >= 2&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 1)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaTypes.LUA_TNONE == LuaAPI.lua_type(L, 3) || translator.Assignable<object>(L, 3))) 
                {
                    bool condition = LuaAPI.lua_toboolean(L, 1);
                    string format = LuaAPI.lua_tostring(L, 2);
                    object[] args = translator.GetParams<object>(L, 3);
                    
                    UnityEngine.Debug.AssertFormat( condition, format, args );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count >= 3&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 1)&& translator.Assignable<UnityEngine.Object>(L, 2)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& (LuaTypes.LUA_TNONE == LuaAPI.lua_type(L, 4) || translator.Assignable<object>(L, 4))) 
                {
                    bool condition = LuaAPI.lua_toboolean(L, 1);
                    UnityEngine.Object context = (UnityEngine.Object)translator.GetObject(L, 2, typeof(UnityEngine.Object));
                    string format = LuaAPI.lua_tostring(L, 3);
                    object[] args = translator.GetParams<object>(L, 4);
                    
                    UnityEngine.Debug.AssertFormat( condition, context, format, args );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Debug.AssertFormat!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LogAssertion_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 1&& translator.Assignable<object>(L, 1)) 
                {
                    object message = translator.GetObject(L, 1, typeof(object));
                    
                    UnityEngine.Debug.LogAssertion( message );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 2&& translator.Assignable<object>(L, 1)&& translator.Assignable<UnityEngine.Object>(L, 2)) 
                {
                    object message = translator.GetObject(L, 1, typeof(object));
                    UnityEngine.Object context = (UnityEngine.Object)translator.GetObject(L, 2, typeof(UnityEngine.Object));
                    
                    UnityEngine.Debug.LogAssertion( message, context );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Debug.LogAssertion!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LogAssertionFormat_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count >= 1&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& (LuaTypes.LUA_TNONE == LuaAPI.lua_type(L, 2) || translator.Assignable<object>(L, 2))) 
                {
                    string format = LuaAPI.lua_tostring(L, 1);
                    object[] args = translator.GetParams<object>(L, 2);
                    
                    UnityEngine.Debug.LogAssertionFormat( format, args );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count >= 2&& translator.Assignable<UnityEngine.Object>(L, 1)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaTypes.LUA_TNONE == LuaAPI.lua_type(L, 3) || translator.Assignable<object>(L, 3))) 
                {
                    UnityEngine.Object context = (UnityEngine.Object)translator.GetObject(L, 1, typeof(UnityEngine.Object));
                    string format = LuaAPI.lua_tostring(L, 2);
                    object[] args = translator.GetParams<object>(L, 3);
                    
                    UnityEngine.Debug.LogAssertionFormat( context, format, args );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Debug.LogAssertionFormat!");
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_unityLogger(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.PushAny(L, UnityEngine.Debug.unityLogger);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_developerConsoleVisible(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushboolean(L, UnityEngine.Debug.developerConsoleVisible);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_isDebugBuild(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushboolean(L, UnityEngine.Debug.isDebugBuild);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_developerConsoleVisible(RealStatePtr L)
        {
		    try {
                
			    UnityEngine.Debug.developerConsoleVisible = LuaAPI.lua_toboolean(L, 1);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
