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
    public class UnityEngineAnimationClipWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(UnityEngine.AnimationClip);
			Utils.BeginObjectRegister(type, L, translator, 0, 5, 8, 5);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SampleAnimation", _m_SampleAnimation);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetCurve", _m_SetCurve);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "EnsureQuaternionContinuity", _m_EnsureQuaternionContinuity);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ClearCurves", _m_ClearCurves);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddEvent", _m_AddEvent);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "length", _g_get_length);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "frameRate", _g_get_frameRate);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "wrapMode", _g_get_wrapMode);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "localBounds", _g_get_localBounds);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "legacy", _g_get_legacy);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "humanMotion", _g_get_humanMotion);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "empty", _g_get_empty);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "events", _g_get_events);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "frameRate", _s_set_frameRate);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "wrapMode", _s_set_wrapMode);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "localBounds", _s_set_localBounds);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "legacy", _s_set_legacy);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "events", _s_set_events);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 1, 0, 0);
			
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					UnityEngine.AnimationClip __cl_gen_ret = new UnityEngine.AnimationClip();
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.AnimationClip constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SampleAnimation(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.AnimationClip __cl_gen_to_be_invoked = (UnityEngine.AnimationClip)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.GameObject go = (UnityEngine.GameObject)translator.GetObject(L, 2, typeof(UnityEngine.GameObject));
                    float time = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    __cl_gen_to_be_invoked.SampleAnimation( go, time );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetCurve(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.AnimationClip __cl_gen_to_be_invoked = (UnityEngine.AnimationClip)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string relativePath = LuaAPI.lua_tostring(L, 2);
                    System.Type type = (System.Type)translator.GetObject(L, 3, typeof(System.Type));
                    string propertyName = LuaAPI.lua_tostring(L, 4);
                    UnityEngine.AnimationCurve curve = (UnityEngine.AnimationCurve)translator.GetObject(L, 5, typeof(UnityEngine.AnimationCurve));
                    
                    __cl_gen_to_be_invoked.SetCurve( relativePath, type, propertyName, curve );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_EnsureQuaternionContinuity(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.AnimationClip __cl_gen_to_be_invoked = (UnityEngine.AnimationClip)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.EnsureQuaternionContinuity(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ClearCurves(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.AnimationClip __cl_gen_to_be_invoked = (UnityEngine.AnimationClip)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.ClearCurves(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddEvent(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.AnimationClip __cl_gen_to_be_invoked = (UnityEngine.AnimationClip)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.AnimationEvent evt = (UnityEngine.AnimationEvent)translator.GetObject(L, 2, typeof(UnityEngine.AnimationEvent));
                    
                    __cl_gen_to_be_invoked.AddEvent( evt );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_length(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.AnimationClip __cl_gen_to_be_invoked = (UnityEngine.AnimationClip)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, __cl_gen_to_be_invoked.length);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_frameRate(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.AnimationClip __cl_gen_to_be_invoked = (UnityEngine.AnimationClip)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, __cl_gen_to_be_invoked.frameRate);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_wrapMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.AnimationClip __cl_gen_to_be_invoked = (UnityEngine.AnimationClip)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.wrapMode);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_localBounds(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.AnimationClip __cl_gen_to_be_invoked = (UnityEngine.AnimationClip)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineBounds(L, __cl_gen_to_be_invoked.localBounds);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_legacy(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.AnimationClip __cl_gen_to_be_invoked = (UnityEngine.AnimationClip)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, __cl_gen_to_be_invoked.legacy);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_humanMotion(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.AnimationClip __cl_gen_to_be_invoked = (UnityEngine.AnimationClip)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, __cl_gen_to_be_invoked.humanMotion);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_empty(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.AnimationClip __cl_gen_to_be_invoked = (UnityEngine.AnimationClip)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, __cl_gen_to_be_invoked.empty);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_events(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.AnimationClip __cl_gen_to_be_invoked = (UnityEngine.AnimationClip)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.events);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_frameRate(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.AnimationClip __cl_gen_to_be_invoked = (UnityEngine.AnimationClip)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.frameRate = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_wrapMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.AnimationClip __cl_gen_to_be_invoked = (UnityEngine.AnimationClip)translator.FastGetCSObj(L, 1);
                UnityEngine.WrapMode __cl_gen_value;translator.Get(L, 2, out __cl_gen_value);
				__cl_gen_to_be_invoked.wrapMode = __cl_gen_value;
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_localBounds(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.AnimationClip __cl_gen_to_be_invoked = (UnityEngine.AnimationClip)translator.FastGetCSObj(L, 1);
                UnityEngine.Bounds __cl_gen_value;translator.Get(L, 2, out __cl_gen_value);
				__cl_gen_to_be_invoked.localBounds = __cl_gen_value;
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_legacy(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.AnimationClip __cl_gen_to_be_invoked = (UnityEngine.AnimationClip)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.legacy = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_events(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.AnimationClip __cl_gen_to_be_invoked = (UnityEngine.AnimationClip)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.events = (UnityEngine.AnimationEvent[])translator.GetObject(L, 2, typeof(UnityEngine.AnimationEvent[]));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
