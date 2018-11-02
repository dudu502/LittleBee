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
    public class UnityEngineGameObjectWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(UnityEngine.GameObject);
			Utils.BeginObjectRegister(type, L, translator, 0, 12, 8, 3);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetComponent", _m_GetComponent);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetComponentInChildren", _m_GetComponentInChildren);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetComponentInParent", _m_GetComponentInParent);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetComponents", _m_GetComponents);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetComponentsInChildren", _m_GetComponentsInChildren);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetComponentsInParent", _m_GetComponentsInParent);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetActive", _m_SetActive);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CompareTag", _m_CompareTag);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SendMessageUpwards", _m_SendMessageUpwards);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SendMessage", _m_SendMessage);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "BroadcastMessage", _m_BroadcastMessage);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddComponent", _m_AddComponent);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "transform", _g_get_transform);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "layer", _g_get_layer);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "activeSelf", _g_get_activeSelf);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "activeInHierarchy", _g_get_activeInHierarchy);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "isStatic", _g_get_isStatic);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "tag", _g_get_tag);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "scene", _g_get_scene);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "gameObject", _g_get_gameObject);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "layer", _s_set_layer);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "isStatic", _s_set_isStatic);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "tag", _s_set_tag);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 6, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "CreatePrimitive", _m_CreatePrimitive_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "FindGameObjectWithTag", _m_FindGameObjectWithTag_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "FindWithTag", _m_FindWithTag_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "FindGameObjectsWithTag", _m_FindGameObjectsWithTag_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Find", _m_Find_xlua_st_);
            
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 2 && (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
				{
					string name = LuaAPI.lua_tostring(L, 2);
					
					UnityEngine.GameObject __cl_gen_ret = new UnityEngine.GameObject(name);
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					UnityEngine.GameObject __cl_gen_ret = new UnityEngine.GameObject();
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				if(LuaAPI.lua_gettop(L) >= 2 && (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && (LuaTypes.LUA_TNONE == LuaAPI.lua_type(L, 3) || translator.Assignable<System.Type>(L, 3)))
				{
					string name = LuaAPI.lua_tostring(L, 2);
					System.Type[] components = translator.GetParams<System.Type>(L, 3);
					
					UnityEngine.GameObject __cl_gen_ret = new UnityEngine.GameObject(name, components);
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.GameObject constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CreatePrimitive_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.PrimitiveType type;translator.Get(L, 1, out type);
                    
                        UnityEngine.GameObject __cl_gen_ret = UnityEngine.GameObject.CreatePrimitive( type );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetComponent(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.GameObject __cl_gen_to_be_invoked = (UnityEngine.GameObject)translator.FastGetCSObj(L, 1);
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 2&& translator.Assignable<System.Type>(L, 2)) 
                {
                    System.Type type = (System.Type)translator.GetObject(L, 2, typeof(System.Type));
                    
                        UnityEngine.Component __cl_gen_ret = __cl_gen_to_be_invoked.GetComponent( type );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                if(__gen_param_count == 2&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    string type = LuaAPI.lua_tostring(L, 2);
                    
                        UnityEngine.Component __cl_gen_ret = __cl_gen_to_be_invoked.GetComponent( type );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.GameObject.GetComponent!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetComponentInChildren(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.GameObject __cl_gen_to_be_invoked = (UnityEngine.GameObject)translator.FastGetCSObj(L, 1);
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 2&& translator.Assignable<System.Type>(L, 2)) 
                {
                    System.Type type = (System.Type)translator.GetObject(L, 2, typeof(System.Type));
                    
                        UnityEngine.Component __cl_gen_ret = __cl_gen_to_be_invoked.GetComponentInChildren( type );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                if(__gen_param_count == 3&& translator.Assignable<System.Type>(L, 2)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)) 
                {
                    System.Type type = (System.Type)translator.GetObject(L, 2, typeof(System.Type));
                    bool includeInactive = LuaAPI.lua_toboolean(L, 3);
                    
                        UnityEngine.Component __cl_gen_ret = __cl_gen_to_be_invoked.GetComponentInChildren( type, includeInactive );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.GameObject.GetComponentInChildren!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetComponentInParent(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.GameObject __cl_gen_to_be_invoked = (UnityEngine.GameObject)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Type type = (System.Type)translator.GetObject(L, 2, typeof(System.Type));
                    
                        UnityEngine.Component __cl_gen_ret = __cl_gen_to_be_invoked.GetComponentInParent( type );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetComponents(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.GameObject __cl_gen_to_be_invoked = (UnityEngine.GameObject)translator.FastGetCSObj(L, 1);
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 2&& translator.Assignable<System.Type>(L, 2)) 
                {
                    System.Type type = (System.Type)translator.GetObject(L, 2, typeof(System.Type));
                    
                        UnityEngine.Component[] __cl_gen_ret = __cl_gen_to_be_invoked.GetComponents( type );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                if(__gen_param_count == 3&& translator.Assignable<System.Type>(L, 2)&& translator.Assignable<System.Collections.Generic.List<UnityEngine.Component>>(L, 3)) 
                {
                    System.Type type = (System.Type)translator.GetObject(L, 2, typeof(System.Type));
                    System.Collections.Generic.List<UnityEngine.Component> results = (System.Collections.Generic.List<UnityEngine.Component>)translator.GetObject(L, 3, typeof(System.Collections.Generic.List<UnityEngine.Component>));
                    
                    __cl_gen_to_be_invoked.GetComponents( type, results );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.GameObject.GetComponents!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetComponentsInChildren(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.GameObject __cl_gen_to_be_invoked = (UnityEngine.GameObject)translator.FastGetCSObj(L, 1);
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 2&& translator.Assignable<System.Type>(L, 2)) 
                {
                    System.Type type = (System.Type)translator.GetObject(L, 2, typeof(System.Type));
                    
                        UnityEngine.Component[] __cl_gen_ret = __cl_gen_to_be_invoked.GetComponentsInChildren( type );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                if(__gen_param_count == 3&& translator.Assignable<System.Type>(L, 2)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)) 
                {
                    System.Type type = (System.Type)translator.GetObject(L, 2, typeof(System.Type));
                    bool includeInactive = LuaAPI.lua_toboolean(L, 3);
                    
                        UnityEngine.Component[] __cl_gen_ret = __cl_gen_to_be_invoked.GetComponentsInChildren( type, includeInactive );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.GameObject.GetComponentsInChildren!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetComponentsInParent(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.GameObject __cl_gen_to_be_invoked = (UnityEngine.GameObject)translator.FastGetCSObj(L, 1);
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 2&& translator.Assignable<System.Type>(L, 2)) 
                {
                    System.Type type = (System.Type)translator.GetObject(L, 2, typeof(System.Type));
                    
                        UnityEngine.Component[] __cl_gen_ret = __cl_gen_to_be_invoked.GetComponentsInParent( type );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                if(__gen_param_count == 3&& translator.Assignable<System.Type>(L, 2)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)) 
                {
                    System.Type type = (System.Type)translator.GetObject(L, 2, typeof(System.Type));
                    bool includeInactive = LuaAPI.lua_toboolean(L, 3);
                    
                        UnityEngine.Component[] __cl_gen_ret = __cl_gen_to_be_invoked.GetComponentsInParent( type, includeInactive );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.GameObject.GetComponentsInParent!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetActive(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.GameObject __cl_gen_to_be_invoked = (UnityEngine.GameObject)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    bool value = LuaAPI.lua_toboolean(L, 2);
                    
                    __cl_gen_to_be_invoked.SetActive( value );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CompareTag(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.GameObject __cl_gen_to_be_invoked = (UnityEngine.GameObject)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string tag = LuaAPI.lua_tostring(L, 2);
                    
                        bool __cl_gen_ret = __cl_gen_to_be_invoked.CompareTag( tag );
                        LuaAPI.lua_pushboolean(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_FindGameObjectWithTag_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string tag = LuaAPI.lua_tostring(L, 1);
                    
                        UnityEngine.GameObject __cl_gen_ret = UnityEngine.GameObject.FindGameObjectWithTag( tag );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_FindWithTag_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string tag = LuaAPI.lua_tostring(L, 1);
                    
                        UnityEngine.GameObject __cl_gen_ret = UnityEngine.GameObject.FindWithTag( tag );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_FindGameObjectsWithTag_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string tag = LuaAPI.lua_tostring(L, 1);
                    
                        UnityEngine.GameObject[] __cl_gen_ret = UnityEngine.GameObject.FindGameObjectsWithTag( tag );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SendMessageUpwards(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.GameObject __cl_gen_to_be_invoked = (UnityEngine.GameObject)translator.FastGetCSObj(L, 1);
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 2&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    string methodName = LuaAPI.lua_tostring(L, 2);
                    
                    __cl_gen_to_be_invoked.SendMessageUpwards( methodName );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<object>(L, 3)) 
                {
                    string methodName = LuaAPI.lua_tostring(L, 2);
                    object value = translator.GetObject(L, 3, typeof(object));
                    
                    __cl_gen_to_be_invoked.SendMessageUpwards( methodName, value );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<UnityEngine.SendMessageOptions>(L, 3)) 
                {
                    string methodName = LuaAPI.lua_tostring(L, 2);
                    UnityEngine.SendMessageOptions options;translator.Get(L, 3, out options);
                    
                    __cl_gen_to_be_invoked.SendMessageUpwards( methodName, options );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 4&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<object>(L, 3)&& translator.Assignable<UnityEngine.SendMessageOptions>(L, 4)) 
                {
                    string methodName = LuaAPI.lua_tostring(L, 2);
                    object value = translator.GetObject(L, 3, typeof(object));
                    UnityEngine.SendMessageOptions options;translator.Get(L, 4, out options);
                    
                    __cl_gen_to_be_invoked.SendMessageUpwards( methodName, value, options );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.GameObject.SendMessageUpwards!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SendMessage(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.GameObject __cl_gen_to_be_invoked = (UnityEngine.GameObject)translator.FastGetCSObj(L, 1);
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 2&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    string methodName = LuaAPI.lua_tostring(L, 2);
                    
                    __cl_gen_to_be_invoked.SendMessage( methodName );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<object>(L, 3)) 
                {
                    string methodName = LuaAPI.lua_tostring(L, 2);
                    object value = translator.GetObject(L, 3, typeof(object));
                    
                    __cl_gen_to_be_invoked.SendMessage( methodName, value );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<UnityEngine.SendMessageOptions>(L, 3)) 
                {
                    string methodName = LuaAPI.lua_tostring(L, 2);
                    UnityEngine.SendMessageOptions options;translator.Get(L, 3, out options);
                    
                    __cl_gen_to_be_invoked.SendMessage( methodName, options );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 4&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<object>(L, 3)&& translator.Assignable<UnityEngine.SendMessageOptions>(L, 4)) 
                {
                    string methodName = LuaAPI.lua_tostring(L, 2);
                    object value = translator.GetObject(L, 3, typeof(object));
                    UnityEngine.SendMessageOptions options;translator.Get(L, 4, out options);
                    
                    __cl_gen_to_be_invoked.SendMessage( methodName, value, options );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.GameObject.SendMessage!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_BroadcastMessage(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.GameObject __cl_gen_to_be_invoked = (UnityEngine.GameObject)translator.FastGetCSObj(L, 1);
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 2&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    string methodName = LuaAPI.lua_tostring(L, 2);
                    
                    __cl_gen_to_be_invoked.BroadcastMessage( methodName );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<object>(L, 3)) 
                {
                    string methodName = LuaAPI.lua_tostring(L, 2);
                    object parameter = translator.GetObject(L, 3, typeof(object));
                    
                    __cl_gen_to_be_invoked.BroadcastMessage( methodName, parameter );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<UnityEngine.SendMessageOptions>(L, 3)) 
                {
                    string methodName = LuaAPI.lua_tostring(L, 2);
                    UnityEngine.SendMessageOptions options;translator.Get(L, 3, out options);
                    
                    __cl_gen_to_be_invoked.BroadcastMessage( methodName, options );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 4&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<object>(L, 3)&& translator.Assignable<UnityEngine.SendMessageOptions>(L, 4)) 
                {
                    string methodName = LuaAPI.lua_tostring(L, 2);
                    object parameter = translator.GetObject(L, 3, typeof(object));
                    UnityEngine.SendMessageOptions options;translator.Get(L, 4, out options);
                    
                    __cl_gen_to_be_invoked.BroadcastMessage( methodName, parameter, options );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.GameObject.BroadcastMessage!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddComponent(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.GameObject __cl_gen_to_be_invoked = (UnityEngine.GameObject)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Type componentType = (System.Type)translator.GetObject(L, 2, typeof(System.Type));
                    
                        UnityEngine.Component __cl_gen_ret = __cl_gen_to_be_invoked.AddComponent( componentType );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Find_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string name = LuaAPI.lua_tostring(L, 1);
                    
                        UnityEngine.GameObject __cl_gen_ret = UnityEngine.GameObject.Find( name );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_transform(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.GameObject __cl_gen_to_be_invoked = (UnityEngine.GameObject)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.transform);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_layer(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.GameObject __cl_gen_to_be_invoked = (UnityEngine.GameObject)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, __cl_gen_to_be_invoked.layer);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_activeSelf(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.GameObject __cl_gen_to_be_invoked = (UnityEngine.GameObject)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, __cl_gen_to_be_invoked.activeSelf);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_activeInHierarchy(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.GameObject __cl_gen_to_be_invoked = (UnityEngine.GameObject)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, __cl_gen_to_be_invoked.activeInHierarchy);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_isStatic(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.GameObject __cl_gen_to_be_invoked = (UnityEngine.GameObject)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, __cl_gen_to_be_invoked.isStatic);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_tag(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.GameObject __cl_gen_to_be_invoked = (UnityEngine.GameObject)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, __cl_gen_to_be_invoked.tag);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_scene(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.GameObject __cl_gen_to_be_invoked = (UnityEngine.GameObject)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.scene);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_gameObject(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.GameObject __cl_gen_to_be_invoked = (UnityEngine.GameObject)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.gameObject);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_layer(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.GameObject __cl_gen_to_be_invoked = (UnityEngine.GameObject)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.layer = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_isStatic(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.GameObject __cl_gen_to_be_invoked = (UnityEngine.GameObject)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.isStatic = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_tag(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.GameObject __cl_gen_to_be_invoked = (UnityEngine.GameObject)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.tag = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
