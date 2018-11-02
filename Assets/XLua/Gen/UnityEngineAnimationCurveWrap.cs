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
    public class UnityEngineAnimationCurveWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(UnityEngine.AnimationCurve);
			Utils.BeginObjectRegister(type, L, translator, 0, 5, 4, 3);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Evaluate", _m_Evaluate);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddKey", _m_AddKey);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "MoveKey", _m_MoveKey);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RemoveKey", _m_RemoveKey);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SmoothTangents", _m_SmoothTangents);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "keys", _g_get_keys);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "length", _g_get_length);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "preWrapMode", _g_get_preWrapMode);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "postWrapMode", _g_get_postWrapMode);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "keys", _s_set_keys);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "preWrapMode", _s_set_preWrapMode);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "postWrapMode", _s_set_postWrapMode);
            
			
			Utils.EndObjectRegister(type, L, translator, __CSIndexer, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 3, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "Linear", _m_Linear_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "EaseInOut", _m_EaseInOut_xlua_st_);
            
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) >= 1 && (LuaTypes.LUA_TNONE == LuaAPI.lua_type(L, 2) || translator.Assignable<UnityEngine.Keyframe>(L, 2)))
				{
					UnityEngine.Keyframe[] keys = translator.GetParams<UnityEngine.Keyframe>(L, 2);
					
					UnityEngine.AnimationCurve __cl_gen_ret = new UnityEngine.AnimationCurve(keys);
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					UnityEngine.AnimationCurve __cl_gen_ret = new UnityEngine.AnimationCurve();
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.AnimationCurve constructor!");
            
        }
        
		
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        public static int __CSIndexer(RealStatePtr L)
        {
			try {
			    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				
				if (translator.Assignable<UnityEngine.AnimationCurve>(L, 1) && LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2))
				{
					
					UnityEngine.AnimationCurve __cl_gen_to_be_invoked = (UnityEngine.AnimationCurve)translator.FastGetCSObj(L, 1);
					int index = LuaAPI.xlua_tointeger(L, 2);
					LuaAPI.lua_pushboolean(L, true);
					translator.Push(L, __cl_gen_to_be_invoked[index]);
					return 2;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
			
            LuaAPI.lua_pushboolean(L, false);
			return 1;
        }
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Evaluate(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.AnimationCurve __cl_gen_to_be_invoked = (UnityEngine.AnimationCurve)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    float time = (float)LuaAPI.lua_tonumber(L, 2);
                    
                        float __cl_gen_ret = __cl_gen_to_be_invoked.Evaluate( time );
                        LuaAPI.lua_pushnumber(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddKey(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.AnimationCurve __cl_gen_to_be_invoked = (UnityEngine.AnimationCurve)translator.FastGetCSObj(L, 1);
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    float time = (float)LuaAPI.lua_tonumber(L, 2);
                    float value = (float)LuaAPI.lua_tonumber(L, 3);
                    
                        int __cl_gen_ret = __cl_gen_to_be_invoked.AddKey( time, value );
                        LuaAPI.xlua_pushinteger(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                if(__gen_param_count == 2&& translator.Assignable<UnityEngine.Keyframe>(L, 2)) 
                {
                    UnityEngine.Keyframe key;translator.Get(L, 2, out key);
                    
                        int __cl_gen_ret = __cl_gen_to_be_invoked.AddKey( key );
                        LuaAPI.xlua_pushinteger(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.AnimationCurve.AddKey!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_MoveKey(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.AnimationCurve __cl_gen_to_be_invoked = (UnityEngine.AnimationCurve)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int index = LuaAPI.xlua_tointeger(L, 2);
                    UnityEngine.Keyframe key;translator.Get(L, 3, out key);
                    
                        int __cl_gen_ret = __cl_gen_to_be_invoked.MoveKey( index, key );
                        LuaAPI.xlua_pushinteger(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RemoveKey(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.AnimationCurve __cl_gen_to_be_invoked = (UnityEngine.AnimationCurve)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int index = LuaAPI.xlua_tointeger(L, 2);
                    
                    __cl_gen_to_be_invoked.RemoveKey( index );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SmoothTangents(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.AnimationCurve __cl_gen_to_be_invoked = (UnityEngine.AnimationCurve)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int index = LuaAPI.xlua_tointeger(L, 2);
                    float weight = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    __cl_gen_to_be_invoked.SmoothTangents( index, weight );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Linear_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    float timeStart = (float)LuaAPI.lua_tonumber(L, 1);
                    float valueStart = (float)LuaAPI.lua_tonumber(L, 2);
                    float timeEnd = (float)LuaAPI.lua_tonumber(L, 3);
                    float valueEnd = (float)LuaAPI.lua_tonumber(L, 4);
                    
                        UnityEngine.AnimationCurve __cl_gen_ret = UnityEngine.AnimationCurve.Linear( timeStart, valueStart, timeEnd, valueEnd );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_EaseInOut_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    float timeStart = (float)LuaAPI.lua_tonumber(L, 1);
                    float valueStart = (float)LuaAPI.lua_tonumber(L, 2);
                    float timeEnd = (float)LuaAPI.lua_tonumber(L, 3);
                    float valueEnd = (float)LuaAPI.lua_tonumber(L, 4);
                    
                        UnityEngine.AnimationCurve __cl_gen_ret = UnityEngine.AnimationCurve.EaseInOut( timeStart, valueStart, timeEnd, valueEnd );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_keys(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.AnimationCurve __cl_gen_to_be_invoked = (UnityEngine.AnimationCurve)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.keys);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_length(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.AnimationCurve __cl_gen_to_be_invoked = (UnityEngine.AnimationCurve)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, __cl_gen_to_be_invoked.length);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_preWrapMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.AnimationCurve __cl_gen_to_be_invoked = (UnityEngine.AnimationCurve)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.preWrapMode);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_postWrapMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.AnimationCurve __cl_gen_to_be_invoked = (UnityEngine.AnimationCurve)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.postWrapMode);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_keys(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.AnimationCurve __cl_gen_to_be_invoked = (UnityEngine.AnimationCurve)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.keys = (UnityEngine.Keyframe[])translator.GetObject(L, 2, typeof(UnityEngine.Keyframe[]));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_preWrapMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.AnimationCurve __cl_gen_to_be_invoked = (UnityEngine.AnimationCurve)translator.FastGetCSObj(L, 1);
                UnityEngine.WrapMode __cl_gen_value;translator.Get(L, 2, out __cl_gen_value);
				__cl_gen_to_be_invoked.preWrapMode = __cl_gen_value;
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_postWrapMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.AnimationCurve __cl_gen_to_be_invoked = (UnityEngine.AnimationCurve)translator.FastGetCSObj(L, 1);
                UnityEngine.WrapMode __cl_gen_value;translator.Get(L, 2, out __cl_gen_value);
				__cl_gen_to_be_invoked.postWrapMode = __cl_gen_value;
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
