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
    public class UnityEngineVector3Wrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(UnityEngine.Vector3);
			Utils.BeginObjectRegister(type, L, translator, 6, 6, 6, 3);
			Utils.RegisterFunc(L, Utils.OBJ_META_IDX, "__add", __AddMeta);
            Utils.RegisterFunc(L, Utils.OBJ_META_IDX, "__sub", __SubMeta);
            Utils.RegisterFunc(L, Utils.OBJ_META_IDX, "__unm", __UnmMeta);
            Utils.RegisterFunc(L, Utils.OBJ_META_IDX, "__mul", __MulMeta);
            Utils.RegisterFunc(L, Utils.OBJ_META_IDX, "__div", __DivMeta);
            Utils.RegisterFunc(L, Utils.OBJ_META_IDX, "__eq", __EqMeta);
            
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Set", _m_Set);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Scale", _m_Scale);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetHashCode", _m_GetHashCode);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Equals", _m_Equals);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Normalize", _m_Normalize);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ToString", _m_ToString);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "normalized", _g_get_normalized);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "magnitude", _g_get_magnitude);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "sqrMagnitude", _g_get_sqrMagnitude);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "x", _g_get_x);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "y", _g_get_y);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "z", _g_get_z);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "x", _s_set_x);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "y", _s_set_y);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "z", _s_set_z);
            
			
			Utils.EndObjectRegister(type, L, translator, __CSIndexer, __NewIndexer,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 25, 10, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "Slerp", _m_Slerp_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SlerpUnclamped", _m_SlerpUnclamped_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "OrthoNormalize", _m_OrthoNormalize_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "RotateTowards", _m_RotateTowards_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Lerp", _m_Lerp_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "LerpUnclamped", _m_LerpUnclamped_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "MoveTowards", _m_MoveTowards_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SmoothDamp", _m_SmoothDamp_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Scale", _m_Scale_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Cross", _m_Cross_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Reflect", _m_Reflect_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Normalize", _m_Normalize_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Dot", _m_Dot_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Project", _m_Project_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ProjectOnPlane", _m_ProjectOnPlane_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Angle", _m_Angle_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SignedAngle", _m_SignedAngle_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Distance", _m_Distance_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ClampMagnitude", _m_ClampMagnitude_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Magnitude", _m_Magnitude_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SqrMagnitude", _m_SqrMagnitude_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Min", _m_Min_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Max", _m_Max_xlua_st_);
            
			
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "kEpsilon", UnityEngine.Vector3.kEpsilon);
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "zero", _g_get_zero);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "one", _g_get_one);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "forward", _g_get_forward);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "back", _g_get_back);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "up", _g_get_up);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "down", _g_get_down);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "left", _g_get_left);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "right", _g_get_right);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "positiveInfinity", _g_get_positiveInfinity);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "negativeInfinity", _g_get_negativeInfinity);
            
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 4 && LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4))
				{
					float x = (float)LuaAPI.lua_tonumber(L, 2);
					float y = (float)LuaAPI.lua_tonumber(L, 3);
					float z = (float)LuaAPI.lua_tonumber(L, 4);
					
					UnityEngine.Vector3 __cl_gen_ret = new UnityEngine.Vector3(x, y, z);
					translator.PushUnityEngineVector3(L, __cl_gen_ret);
                    
					return 1;
				}
				if(LuaAPI.lua_gettop(L) == 3 && LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3))
				{
					float x = (float)LuaAPI.lua_tonumber(L, 2);
					float y = (float)LuaAPI.lua_tonumber(L, 3);
					
					UnityEngine.Vector3 __cl_gen_ret = new UnityEngine.Vector3(x, y);
					translator.PushUnityEngineVector3(L, __cl_gen_ret);
                    
					return 1;
				}
				
				if (LuaAPI.lua_gettop(L) == 1)
				{
				    translator.PushUnityEngineVector3(L, default(UnityEngine.Vector3));
			        return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Vector3 constructor!");
            
        }
        
		
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        public static int __CSIndexer(RealStatePtr L)
        {
			try {
			    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				
				if (translator.Assignable<UnityEngine.Vector3>(L, 1) && LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2))
				{
					
					UnityEngine.Vector3 __cl_gen_to_be_invoked;translator.Get(L, 1, out __cl_gen_to_be_invoked);
					int index = LuaAPI.xlua_tointeger(L, 2);
					LuaAPI.lua_pushboolean(L, true);
					LuaAPI.lua_pushnumber(L, __cl_gen_to_be_invoked[index]);
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
        public static int __NewIndexer(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
			try {
				
				if (translator.Assignable<UnityEngine.Vector3>(L, 1) && LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3))
				{
					
					UnityEngine.Vector3 __cl_gen_to_be_invoked;translator.Get(L, 1, out __cl_gen_to_be_invoked);
					int key = LuaAPI.xlua_tointeger(L, 2);
					__cl_gen_to_be_invoked[key] = (float)LuaAPI.lua_tonumber(L, 3);
					LuaAPI.lua_pushboolean(L, true);
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
			
			LuaAPI.lua_pushboolean(L, false);
            return 1;
        }
		
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __AddMeta(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
			
				if (translator.Assignable<UnityEngine.Vector3>(L, 1) && translator.Assignable<UnityEngine.Vector3>(L, 2))
				{
					UnityEngine.Vector3 leftside;translator.Get(L, 1, out leftside);
					UnityEngine.Vector3 rightside;translator.Get(L, 2, out rightside);
					
					translator.PushUnityEngineVector3(L, leftside + rightside);
					
					return 1;
				}
            
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to right hand of + operator, need UnityEngine.Vector3!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __SubMeta(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
			
				if (translator.Assignable<UnityEngine.Vector3>(L, 1) && translator.Assignable<UnityEngine.Vector3>(L, 2))
				{
					UnityEngine.Vector3 leftside;translator.Get(L, 1, out leftside);
					UnityEngine.Vector3 rightside;translator.Get(L, 2, out rightside);
					
					translator.PushUnityEngineVector3(L, leftside - rightside);
					
					return 1;
				}
            
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to right hand of - operator, need UnityEngine.Vector3!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __UnmMeta(RealStatePtr L)
        {
            
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
                UnityEngine.Vector3 rightside;translator.Get(L, 1, out rightside);
                translator.PushUnityEngineVector3(L, - rightside);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __MulMeta(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
			
				if (translator.Assignable<UnityEngine.Vector3>(L, 1) && LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2))
				{
					UnityEngine.Vector3 leftside;translator.Get(L, 1, out leftside);
					float rightside = (float)LuaAPI.lua_tonumber(L, 2);
					
					translator.PushUnityEngineVector3(L, leftside * rightside);
					
					return 1;
				}
            
			
				if (LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 1) && translator.Assignable<UnityEngine.Vector3>(L, 2))
				{
					float leftside = (float)LuaAPI.lua_tonumber(L, 1);
					UnityEngine.Vector3 rightside;translator.Get(L, 2, out rightside);
					
					translator.PushUnityEngineVector3(L, leftside * rightside);
					
					return 1;
				}
            
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to right hand of * operator, need UnityEngine.Vector3!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __DivMeta(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
			
				if (translator.Assignable<UnityEngine.Vector3>(L, 1) && LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2))
				{
					UnityEngine.Vector3 leftside;translator.Get(L, 1, out leftside);
					float rightside = (float)LuaAPI.lua_tonumber(L, 2);
					
					translator.PushUnityEngineVector3(L, leftside / rightside);
					
					return 1;
				}
            
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to right hand of / operator, need UnityEngine.Vector3!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __EqMeta(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
			
				if (translator.Assignable<UnityEngine.Vector3>(L, 1) && translator.Assignable<UnityEngine.Vector3>(L, 2))
				{
					UnityEngine.Vector3 leftside;translator.Get(L, 1, out leftside);
					UnityEngine.Vector3 rightside;translator.Get(L, 2, out rightside);
					
					LuaAPI.lua_pushboolean(L, leftside == rightside);
					
					return 1;
				}
            
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to right hand of == operator, need UnityEngine.Vector3!");
            
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Slerp_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Vector3 a;translator.Get(L, 1, out a);
                    UnityEngine.Vector3 b;translator.Get(L, 2, out b);
                    float t = (float)LuaAPI.lua_tonumber(L, 3);
                    
                        UnityEngine.Vector3 __cl_gen_ret = UnityEngine.Vector3.Slerp( a, b, t );
                        translator.PushUnityEngineVector3(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SlerpUnclamped_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Vector3 a;translator.Get(L, 1, out a);
                    UnityEngine.Vector3 b;translator.Get(L, 2, out b);
                    float t = (float)LuaAPI.lua_tonumber(L, 3);
                    
                        UnityEngine.Vector3 __cl_gen_ret = UnityEngine.Vector3.SlerpUnclamped( a, b, t );
                        translator.PushUnityEngineVector3(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OrthoNormalize_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 2&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)) 
                {
                    UnityEngine.Vector3 normal;translator.Get(L, 1, out normal);
                    UnityEngine.Vector3 tangent;translator.Get(L, 2, out tangent);
                    
                    UnityEngine.Vector3.OrthoNormalize( ref normal, ref tangent );
                    translator.PushUnityEngineVector3(L, normal);
                        translator.UpdateUnityEngineVector3(L, 1, normal);
                        
                    translator.PushUnityEngineVector3(L, tangent);
                        translator.UpdateUnityEngineVector3(L, 2, tangent);
                        
                    
                    
                    
                    return 2;
                }
                if(__gen_param_count == 3&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& translator.Assignable<UnityEngine.Vector3>(L, 3)) 
                {
                    UnityEngine.Vector3 normal;translator.Get(L, 1, out normal);
                    UnityEngine.Vector3 tangent;translator.Get(L, 2, out tangent);
                    UnityEngine.Vector3 binormal;translator.Get(L, 3, out binormal);
                    
                    UnityEngine.Vector3.OrthoNormalize( ref normal, ref tangent, ref binormal );
                    translator.PushUnityEngineVector3(L, normal);
                        translator.UpdateUnityEngineVector3(L, 1, normal);
                        
                    translator.PushUnityEngineVector3(L, tangent);
                        translator.UpdateUnityEngineVector3(L, 2, tangent);
                        
                    translator.PushUnityEngineVector3(L, binormal);
                        translator.UpdateUnityEngineVector3(L, 3, binormal);
                        
                    
                    
                    
                    return 3;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Vector3.OrthoNormalize!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RotateTowards_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Vector3 current;translator.Get(L, 1, out current);
                    UnityEngine.Vector3 target;translator.Get(L, 2, out target);
                    float maxRadiansDelta = (float)LuaAPI.lua_tonumber(L, 3);
                    float maxMagnitudeDelta = (float)LuaAPI.lua_tonumber(L, 4);
                    
                        UnityEngine.Vector3 __cl_gen_ret = UnityEngine.Vector3.RotateTowards( current, target, maxRadiansDelta, maxMagnitudeDelta );
                        translator.PushUnityEngineVector3(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Lerp_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Vector3 a;translator.Get(L, 1, out a);
                    UnityEngine.Vector3 b;translator.Get(L, 2, out b);
                    float t = (float)LuaAPI.lua_tonumber(L, 3);
                    
                        UnityEngine.Vector3 __cl_gen_ret = UnityEngine.Vector3.Lerp( a, b, t );
                        translator.PushUnityEngineVector3(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LerpUnclamped_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Vector3 a;translator.Get(L, 1, out a);
                    UnityEngine.Vector3 b;translator.Get(L, 2, out b);
                    float t = (float)LuaAPI.lua_tonumber(L, 3);
                    
                        UnityEngine.Vector3 __cl_gen_ret = UnityEngine.Vector3.LerpUnclamped( a, b, t );
                        translator.PushUnityEngineVector3(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_MoveTowards_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Vector3 current;translator.Get(L, 1, out current);
                    UnityEngine.Vector3 target;translator.Get(L, 2, out target);
                    float maxDistanceDelta = (float)LuaAPI.lua_tonumber(L, 3);
                    
                        UnityEngine.Vector3 __cl_gen_ret = UnityEngine.Vector3.MoveTowards( current, target, maxDistanceDelta );
                        translator.PushUnityEngineVector3(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SmoothDamp_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 4&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& translator.Assignable<UnityEngine.Vector3>(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    UnityEngine.Vector3 current;translator.Get(L, 1, out current);
                    UnityEngine.Vector3 target;translator.Get(L, 2, out target);
                    UnityEngine.Vector3 currentVelocity;translator.Get(L, 3, out currentVelocity);
                    float smoothTime = (float)LuaAPI.lua_tonumber(L, 4);
                    
                        UnityEngine.Vector3 __cl_gen_ret = UnityEngine.Vector3.SmoothDamp( current, target, ref currentVelocity, smoothTime );
                        translator.PushUnityEngineVector3(L, __cl_gen_ret);
                    translator.PushUnityEngineVector3(L, currentVelocity);
                        translator.UpdateUnityEngineVector3(L, 3, currentVelocity);
                        
                    
                    
                    
                    return 2;
                }
                if(__gen_param_count == 5&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& translator.Assignable<UnityEngine.Vector3>(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)) 
                {
                    UnityEngine.Vector3 current;translator.Get(L, 1, out current);
                    UnityEngine.Vector3 target;translator.Get(L, 2, out target);
                    UnityEngine.Vector3 currentVelocity;translator.Get(L, 3, out currentVelocity);
                    float smoothTime = (float)LuaAPI.lua_tonumber(L, 4);
                    float maxSpeed = (float)LuaAPI.lua_tonumber(L, 5);
                    
                        UnityEngine.Vector3 __cl_gen_ret = UnityEngine.Vector3.SmoothDamp( current, target, ref currentVelocity, smoothTime, maxSpeed );
                        translator.PushUnityEngineVector3(L, __cl_gen_ret);
                    translator.PushUnityEngineVector3(L, currentVelocity);
                        translator.UpdateUnityEngineVector3(L, 3, currentVelocity);
                        
                    
                    
                    
                    return 2;
                }
                if(__gen_param_count == 6&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& translator.Assignable<UnityEngine.Vector3>(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)) 
                {
                    UnityEngine.Vector3 current;translator.Get(L, 1, out current);
                    UnityEngine.Vector3 target;translator.Get(L, 2, out target);
                    UnityEngine.Vector3 currentVelocity;translator.Get(L, 3, out currentVelocity);
                    float smoothTime = (float)LuaAPI.lua_tonumber(L, 4);
                    float maxSpeed = (float)LuaAPI.lua_tonumber(L, 5);
                    float deltaTime = (float)LuaAPI.lua_tonumber(L, 6);
                    
                        UnityEngine.Vector3 __cl_gen_ret = UnityEngine.Vector3.SmoothDamp( current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime );
                        translator.PushUnityEngineVector3(L, __cl_gen_ret);
                    translator.PushUnityEngineVector3(L, currentVelocity);
                        translator.UpdateUnityEngineVector3(L, 3, currentVelocity);
                        
                    
                    
                    
                    return 2;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Vector3.SmoothDamp!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Set(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Vector3 __cl_gen_to_be_invoked;translator.Get(L, 1, out __cl_gen_to_be_invoked);
            
            
                
                {
                    float newX = (float)LuaAPI.lua_tonumber(L, 2);
                    float newY = (float)LuaAPI.lua_tonumber(L, 3);
                    float newZ = (float)LuaAPI.lua_tonumber(L, 4);
                    
                    __cl_gen_to_be_invoked.Set( newX, newY, newZ );
                    
                    
                        translator.UpdateUnityEngineVector3(L, 1, __cl_gen_to_be_invoked);
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Scale_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Vector3 a;translator.Get(L, 1, out a);
                    UnityEngine.Vector3 b;translator.Get(L, 2, out b);
                    
                        UnityEngine.Vector3 __cl_gen_ret = UnityEngine.Vector3.Scale( a, b );
                        translator.PushUnityEngineVector3(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Scale(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Vector3 __cl_gen_to_be_invoked;translator.Get(L, 1, out __cl_gen_to_be_invoked);
            
            
                
                {
                    UnityEngine.Vector3 scale;translator.Get(L, 2, out scale);
                    
                    __cl_gen_to_be_invoked.Scale( scale );
                    
                    
                        translator.UpdateUnityEngineVector3(L, 1, __cl_gen_to_be_invoked);
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Cross_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Vector3 lhs;translator.Get(L, 1, out lhs);
                    UnityEngine.Vector3 rhs;translator.Get(L, 2, out rhs);
                    
                        UnityEngine.Vector3 __cl_gen_ret = UnityEngine.Vector3.Cross( lhs, rhs );
                        translator.PushUnityEngineVector3(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetHashCode(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Vector3 __cl_gen_to_be_invoked;translator.Get(L, 1, out __cl_gen_to_be_invoked);
            
            
                
                {
                    
                        int __cl_gen_ret = __cl_gen_to_be_invoked.GetHashCode(  );
                        LuaAPI.xlua_pushinteger(L, __cl_gen_ret);
                    
                    
                        translator.UpdateUnityEngineVector3(L, 1, __cl_gen_to_be_invoked);
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Equals(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Vector3 __cl_gen_to_be_invoked;translator.Get(L, 1, out __cl_gen_to_be_invoked);
            
            
                
                {
                    object other = translator.GetObject(L, 2, typeof(object));
                    
                        bool __cl_gen_ret = __cl_gen_to_be_invoked.Equals( other );
                        LuaAPI.lua_pushboolean(L, __cl_gen_ret);
                    
                    
                        translator.UpdateUnityEngineVector3(L, 1, __cl_gen_to_be_invoked);
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Reflect_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Vector3 inDirection;translator.Get(L, 1, out inDirection);
                    UnityEngine.Vector3 inNormal;translator.Get(L, 2, out inNormal);
                    
                        UnityEngine.Vector3 __cl_gen_ret = UnityEngine.Vector3.Reflect( inDirection, inNormal );
                        translator.PushUnityEngineVector3(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Normalize_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Vector3 value;translator.Get(L, 1, out value);
                    
                        UnityEngine.Vector3 __cl_gen_ret = UnityEngine.Vector3.Normalize( value );
                        translator.PushUnityEngineVector3(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Normalize(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Vector3 __cl_gen_to_be_invoked;translator.Get(L, 1, out __cl_gen_to_be_invoked);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.Normalize(  );
                    
                    
                        translator.UpdateUnityEngineVector3(L, 1, __cl_gen_to_be_invoked);
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Dot_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Vector3 lhs;translator.Get(L, 1, out lhs);
                    UnityEngine.Vector3 rhs;translator.Get(L, 2, out rhs);
                    
                        float __cl_gen_ret = UnityEngine.Vector3.Dot( lhs, rhs );
                        LuaAPI.lua_pushnumber(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Project_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Vector3 vector;translator.Get(L, 1, out vector);
                    UnityEngine.Vector3 onNormal;translator.Get(L, 2, out onNormal);
                    
                        UnityEngine.Vector3 __cl_gen_ret = UnityEngine.Vector3.Project( vector, onNormal );
                        translator.PushUnityEngineVector3(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ProjectOnPlane_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Vector3 vector;translator.Get(L, 1, out vector);
                    UnityEngine.Vector3 planeNormal;translator.Get(L, 2, out planeNormal);
                    
                        UnityEngine.Vector3 __cl_gen_ret = UnityEngine.Vector3.ProjectOnPlane( vector, planeNormal );
                        translator.PushUnityEngineVector3(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Angle_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Vector3 from;translator.Get(L, 1, out from);
                    UnityEngine.Vector3 to;translator.Get(L, 2, out to);
                    
                        float __cl_gen_ret = UnityEngine.Vector3.Angle( from, to );
                        LuaAPI.lua_pushnumber(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SignedAngle_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Vector3 from;translator.Get(L, 1, out from);
                    UnityEngine.Vector3 to;translator.Get(L, 2, out to);
                    UnityEngine.Vector3 axis;translator.Get(L, 3, out axis);
                    
                        float __cl_gen_ret = UnityEngine.Vector3.SignedAngle( from, to, axis );
                        LuaAPI.lua_pushnumber(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Distance_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Vector3 a;translator.Get(L, 1, out a);
                    UnityEngine.Vector3 b;translator.Get(L, 2, out b);
                    
                        float __cl_gen_ret = UnityEngine.Vector3.Distance( a, b );
                        LuaAPI.lua_pushnumber(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ClampMagnitude_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Vector3 vector;translator.Get(L, 1, out vector);
                    float maxLength = (float)LuaAPI.lua_tonumber(L, 2);
                    
                        UnityEngine.Vector3 __cl_gen_ret = UnityEngine.Vector3.ClampMagnitude( vector, maxLength );
                        translator.PushUnityEngineVector3(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Magnitude_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Vector3 vector;translator.Get(L, 1, out vector);
                    
                        float __cl_gen_ret = UnityEngine.Vector3.Magnitude( vector );
                        LuaAPI.lua_pushnumber(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SqrMagnitude_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Vector3 vector;translator.Get(L, 1, out vector);
                    
                        float __cl_gen_ret = UnityEngine.Vector3.SqrMagnitude( vector );
                        LuaAPI.lua_pushnumber(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Min_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Vector3 lhs;translator.Get(L, 1, out lhs);
                    UnityEngine.Vector3 rhs;translator.Get(L, 2, out rhs);
                    
                        UnityEngine.Vector3 __cl_gen_ret = UnityEngine.Vector3.Min( lhs, rhs );
                        translator.PushUnityEngineVector3(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Max_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Vector3 lhs;translator.Get(L, 1, out lhs);
                    UnityEngine.Vector3 rhs;translator.Get(L, 2, out rhs);
                    
                        UnityEngine.Vector3 __cl_gen_ret = UnityEngine.Vector3.Max( lhs, rhs );
                        translator.PushUnityEngineVector3(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ToString(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Vector3 __cl_gen_to_be_invoked;translator.Get(L, 1, out __cl_gen_to_be_invoked);
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 1) 
                {
                    
                        string __cl_gen_ret = __cl_gen_to_be_invoked.ToString(  );
                        LuaAPI.lua_pushstring(L, __cl_gen_ret);
                    
                    
                        translator.UpdateUnityEngineVector3(L, 1, __cl_gen_to_be_invoked);
                    
                    
                    return 1;
                }
                if(__gen_param_count == 2&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    string format = LuaAPI.lua_tostring(L, 2);
                    
                        string __cl_gen_ret = __cl_gen_to_be_invoked.ToString( format );
                        LuaAPI.lua_pushstring(L, __cl_gen_ret);
                    
                    
                        translator.UpdateUnityEngineVector3(L, 1, __cl_gen_to_be_invoked);
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Vector3.ToString!");
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_normalized(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Vector3 __cl_gen_to_be_invoked;translator.Get(L, 1, out __cl_gen_to_be_invoked);
                translator.PushUnityEngineVector3(L, __cl_gen_to_be_invoked.normalized);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_magnitude(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Vector3 __cl_gen_to_be_invoked;translator.Get(L, 1, out __cl_gen_to_be_invoked);
                LuaAPI.lua_pushnumber(L, __cl_gen_to_be_invoked.magnitude);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_sqrMagnitude(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Vector3 __cl_gen_to_be_invoked;translator.Get(L, 1, out __cl_gen_to_be_invoked);
                LuaAPI.lua_pushnumber(L, __cl_gen_to_be_invoked.sqrMagnitude);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_zero(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.PushUnityEngineVector3(L, UnityEngine.Vector3.zero);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_one(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.PushUnityEngineVector3(L, UnityEngine.Vector3.one);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_forward(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.PushUnityEngineVector3(L, UnityEngine.Vector3.forward);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_back(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.PushUnityEngineVector3(L, UnityEngine.Vector3.back);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_up(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.PushUnityEngineVector3(L, UnityEngine.Vector3.up);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_down(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.PushUnityEngineVector3(L, UnityEngine.Vector3.down);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_left(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.PushUnityEngineVector3(L, UnityEngine.Vector3.left);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_right(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.PushUnityEngineVector3(L, UnityEngine.Vector3.right);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_positiveInfinity(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.PushUnityEngineVector3(L, UnityEngine.Vector3.positiveInfinity);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_negativeInfinity(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.PushUnityEngineVector3(L, UnityEngine.Vector3.negativeInfinity);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_x(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Vector3 __cl_gen_to_be_invoked;translator.Get(L, 1, out __cl_gen_to_be_invoked);
                LuaAPI.lua_pushnumber(L, __cl_gen_to_be_invoked.x);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_y(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Vector3 __cl_gen_to_be_invoked;translator.Get(L, 1, out __cl_gen_to_be_invoked);
                LuaAPI.lua_pushnumber(L, __cl_gen_to_be_invoked.y);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_z(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Vector3 __cl_gen_to_be_invoked;translator.Get(L, 1, out __cl_gen_to_be_invoked);
                LuaAPI.lua_pushnumber(L, __cl_gen_to_be_invoked.z);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_x(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Vector3 __cl_gen_to_be_invoked;translator.Get(L, 1, out __cl_gen_to_be_invoked);
                __cl_gen_to_be_invoked.x = (float)LuaAPI.lua_tonumber(L, 2);
            
                translator.UpdateUnityEngineVector3(L, 1, __cl_gen_to_be_invoked);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_y(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Vector3 __cl_gen_to_be_invoked;translator.Get(L, 1, out __cl_gen_to_be_invoked);
                __cl_gen_to_be_invoked.y = (float)LuaAPI.lua_tonumber(L, 2);
            
                translator.UpdateUnityEngineVector3(L, 1, __cl_gen_to_be_invoked);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_z(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Vector3 __cl_gen_to_be_invoked;translator.Get(L, 1, out __cl_gen_to_be_invoked);
                __cl_gen_to_be_invoked.z = (float)LuaAPI.lua_tonumber(L, 2);
            
                translator.UpdateUnityEngineVector3(L, 1, __cl_gen_to_be_invoked);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
