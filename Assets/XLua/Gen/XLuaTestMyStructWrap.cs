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
    public class XLuaTestMyStructWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(XLuaTest.MyStruct);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 4, 4);
			
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "a", _g_get_a);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "b", _g_get_b);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "c", _g_get_c);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "e", _g_get_e);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "a", _s_set_a);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "b", _s_set_b);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "c", _s_set_c);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "e", _s_set_e);
            
			
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
				if(LuaAPI.lua_gettop(L) == 3 && LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3))
				{
					int p1 = LuaAPI.xlua_tointeger(L, 2);
					int p2 = LuaAPI.xlua_tointeger(L, 3);
					
					XLuaTest.MyStruct __cl_gen_ret = new XLuaTest.MyStruct(p1, p2);
					translator.PushXLuaTestMyStruct(L, __cl_gen_ret);
                    
					return 1;
				}
				
				if (LuaAPI.lua_gettop(L) == 1)
				{
				    translator.PushXLuaTestMyStruct(L, default(XLuaTest.MyStruct));
			        return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to XLuaTest.MyStruct constructor!");
            
        }
        
		
        
		
        
        
        
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_a(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XLuaTest.MyStruct __cl_gen_to_be_invoked;translator.Get(L, 1, out __cl_gen_to_be_invoked);
                LuaAPI.xlua_pushinteger(L, __cl_gen_to_be_invoked.a);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_b(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XLuaTest.MyStruct __cl_gen_to_be_invoked;translator.Get(L, 1, out __cl_gen_to_be_invoked);
                LuaAPI.xlua_pushinteger(L, __cl_gen_to_be_invoked.b);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_c(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XLuaTest.MyStruct __cl_gen_to_be_invoked;translator.Get(L, 1, out __cl_gen_to_be_invoked);
                translator.PushDecimal(L, __cl_gen_to_be_invoked.c);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_e(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XLuaTest.MyStruct __cl_gen_to_be_invoked;translator.Get(L, 1, out __cl_gen_to_be_invoked);
                translator.PushXLuaTestPedding(L, __cl_gen_to_be_invoked.e);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_a(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XLuaTest.MyStruct __cl_gen_to_be_invoked;translator.Get(L, 1, out __cl_gen_to_be_invoked);
                __cl_gen_to_be_invoked.a = LuaAPI.xlua_tointeger(L, 2);
            
                translator.UpdateXLuaTestMyStruct(L, 1, __cl_gen_to_be_invoked);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_b(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XLuaTest.MyStruct __cl_gen_to_be_invoked;translator.Get(L, 1, out __cl_gen_to_be_invoked);
                __cl_gen_to_be_invoked.b = LuaAPI.xlua_tointeger(L, 2);
            
                translator.UpdateXLuaTestMyStruct(L, 1, __cl_gen_to_be_invoked);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_c(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XLuaTest.MyStruct __cl_gen_to_be_invoked;translator.Get(L, 1, out __cl_gen_to_be_invoked);
                decimal __cl_gen_value;translator.Get(L, 2, out __cl_gen_value);
				__cl_gen_to_be_invoked.c = __cl_gen_value;
            
                translator.UpdateXLuaTestMyStruct(L, 1, __cl_gen_to_be_invoked);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_e(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XLuaTest.MyStruct __cl_gen_to_be_invoked;translator.Get(L, 1, out __cl_gen_to_be_invoked);
                XLuaTest.Pedding __cl_gen_value;translator.Get(L, 2, out __cl_gen_value);
				__cl_gen_to_be_invoked.e = __cl_gen_value;
            
                translator.UpdateXLuaTestMyStruct(L, 1, __cl_gen_to_be_invoked);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
