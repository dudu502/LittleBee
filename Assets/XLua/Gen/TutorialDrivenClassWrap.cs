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
using Tutorial;

namespace XLua.CSObjectWrap
{
    using Utils = XLua.Utils;
    public class TutorialDrivenClassWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Tutorial.DrivenClass);
			Utils.BeginObjectRegister(type, L, translator, 1, 12, 2, 2);
			Utils.RegisterFunc(L, Utils.OBJ_META_IDX, "__add", __AddMeta);
            
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "DMFunc", _m_DMFunc);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ComplexFunc", _m_ComplexFunc);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "TestFunc", _m_TestFunc);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "DefaultValueFunc", _m_DefaultValueFunc);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "VariableParamsFunc", _m_VariableParamsFunc);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "EnumTestFunc", _m_EnumTestFunc);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CallEvent", _m_CallEvent);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "TestLong", _m_TestLong);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetCalc", _m_GetCalc);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetSomeData", _m_GetSomeData);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GenericMethodOfString", _m_GenericMethodOfString);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "TestEvent", _e_TestEvent);
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "DMF", _g_get_DMF);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TestDelegate", _g_get_TestDelegate);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "DMF", _s_set_DMF);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "TestDelegate", _s_set_TestDelegate);
            
			
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
					
					Tutorial.DrivenClass __cl_gen_ret = new Tutorial.DrivenClass();
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Tutorial.DrivenClass constructor!");
            
        }
        
		
        
		
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __AddMeta(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
			
				if (translator.Assignable<Tutorial.DrivenClass>(L, 1) && translator.Assignable<Tutorial.DrivenClass>(L, 2))
				{
					Tutorial.DrivenClass leftside = (Tutorial.DrivenClass)translator.GetObject(L, 1, typeof(Tutorial.DrivenClass));
					Tutorial.DrivenClass rightside = (Tutorial.DrivenClass)translator.GetObject(L, 2, typeof(Tutorial.DrivenClass));
					
					translator.Push(L, leftside + rightside);
					
					return 1;
				}
            
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to right hand of + operator, need Tutorial.DrivenClass!");
            
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DMFunc(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Tutorial.DrivenClass __cl_gen_to_be_invoked = (Tutorial.DrivenClass)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.DMFunc(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ComplexFunc(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Tutorial.DrivenClass __cl_gen_to_be_invoked = (Tutorial.DrivenClass)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Tutorial.Param1 p1;translator.Get(L, 2, out p1);
                    int p2 = LuaAPI.xlua_tointeger(L, 3);
                    string p3;
                    System.Action luafunc = translator.GetDelegate<System.Action>(L, 4);
                    System.Action csfunc;
                    
                        double __cl_gen_ret = __cl_gen_to_be_invoked.ComplexFunc( p1, ref p2, out p3, luafunc, out csfunc );
                        LuaAPI.lua_pushnumber(L, __cl_gen_ret);
                    LuaAPI.xlua_pushinteger(L, p2);
                        
                    LuaAPI.lua_pushstring(L, p3);
                        
                    translator.Push(L, csfunc);
                        
                    
                    
                    
                    return 4;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_TestFunc(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Tutorial.DrivenClass __cl_gen_to_be_invoked = (Tutorial.DrivenClass)translator.FastGetCSObj(L, 1);
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 2&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    int i = LuaAPI.xlua_tointeger(L, 2);
                    
                    __cl_gen_to_be_invoked.TestFunc( i );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 2&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    string i = LuaAPI.lua_tostring(L, 2);
                    
                    __cl_gen_to_be_invoked.TestFunc( i );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Tutorial.DrivenClass.TestFunc!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DefaultValueFunc(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Tutorial.DrivenClass __cl_gen_to_be_invoked = (Tutorial.DrivenClass)translator.FastGetCSObj(L, 1);
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 4&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 4) || LuaAPI.lua_type(L, 4) == LuaTypes.LUA_TSTRING)) 
                {
                    int a = LuaAPI.xlua_tointeger(L, 2);
                    string b = LuaAPI.lua_tostring(L, 3);
                    string c = LuaAPI.lua_tostring(L, 4);
                    
                    __cl_gen_to_be_invoked.DefaultValueFunc( a, b, c );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)) 
                {
                    int a = LuaAPI.xlua_tointeger(L, 2);
                    string b = LuaAPI.lua_tostring(L, 3);
                    
                    __cl_gen_to_be_invoked.DefaultValueFunc( a, b );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 2&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    int a = LuaAPI.xlua_tointeger(L, 2);
                    
                    __cl_gen_to_be_invoked.DefaultValueFunc( a );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 1) 
                {
                    
                    __cl_gen_to_be_invoked.DefaultValueFunc(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Tutorial.DrivenClass.DefaultValueFunc!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_VariableParamsFunc(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Tutorial.DrivenClass __cl_gen_to_be_invoked = (Tutorial.DrivenClass)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int a = LuaAPI.xlua_tointeger(L, 2);
                    string[] strs = translator.GetParams<string>(L, 3);
                    
                    __cl_gen_to_be_invoked.VariableParamsFunc( a, strs );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_EnumTestFunc(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Tutorial.DrivenClass __cl_gen_to_be_invoked = (Tutorial.DrivenClass)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Tutorial.TestEnum e;translator.Get(L, 2, out e);
                    
                        Tutorial.TestEnum __cl_gen_ret = __cl_gen_to_be_invoked.EnumTestFunc( e );
                        translator.PushTutorialTestEnum(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CallEvent(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Tutorial.DrivenClass __cl_gen_to_be_invoked = (Tutorial.DrivenClass)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.CallEvent(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_TestLong(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Tutorial.DrivenClass __cl_gen_to_be_invoked = (Tutorial.DrivenClass)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    long n = LuaAPI.lua_toint64(L, 2);
                    
                        ulong __cl_gen_ret = __cl_gen_to_be_invoked.TestLong( n );
                        LuaAPI.lua_pushuint64(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetCalc(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Tutorial.DrivenClass __cl_gen_to_be_invoked = (Tutorial.DrivenClass)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        Tutorial.Calc __cl_gen_ret = __cl_gen_to_be_invoked.GetCalc(  );
                        translator.PushAny(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetSomeData(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Tutorial.DrivenClass __cl_gen_to_be_invoked = (Tutorial.DrivenClass)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        int __cl_gen_ret = __cl_gen_to_be_invoked.GetSomeData(  );
                        LuaAPI.xlua_pushinteger(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GenericMethodOfString(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Tutorial.DrivenClass __cl_gen_to_be_invoked = (Tutorial.DrivenClass)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.GenericMethodOfString(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_DMF(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Tutorial.DrivenClass __cl_gen_to_be_invoked = (Tutorial.DrivenClass)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, __cl_gen_to_be_invoked.DMF);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TestDelegate(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Tutorial.DrivenClass __cl_gen_to_be_invoked = (Tutorial.DrivenClass)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.TestDelegate);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_DMF(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Tutorial.DrivenClass __cl_gen_to_be_invoked = (Tutorial.DrivenClass)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.DMF = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_TestDelegate(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Tutorial.DrivenClass __cl_gen_to_be_invoked = (Tutorial.DrivenClass)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.TestDelegate = translator.GetDelegate<System.Action<string>>(L, 2);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
		
		
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _e_TestEvent(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    int __gen_param_count = LuaAPI.lua_gettop(L);
			Tutorial.DrivenClass __cl_gen_to_be_invoked = (Tutorial.DrivenClass)translator.FastGetCSObj(L, 1);
                System.Action __gen_delegate = translator.GetDelegate<System.Action>(L, 3);
                if (__gen_delegate == null) {
                    return LuaAPI.luaL_error(L, "#3 need System.Action!");
                }
				
				if (__gen_param_count == 3)
				{
					
					if (LuaAPI.xlua_is_eq_str(L, 2, "+")) {
						__cl_gen_to_be_invoked.TestEvent += __gen_delegate;
						return 0;
					} 
					
					
					if (LuaAPI.xlua_is_eq_str(L, 2, "-")) {
						__cl_gen_to_be_invoked.TestEvent -= __gen_delegate;
						return 0;
					} 
					
				}
			} catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
			LuaAPI.luaL_error(L, "invalid arguments to Tutorial.DrivenClass.TestEvent!");
            return 0;
        }
        
		
		
    }
}
