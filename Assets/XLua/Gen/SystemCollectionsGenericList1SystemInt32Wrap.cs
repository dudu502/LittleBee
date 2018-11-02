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
    public class SystemCollectionsGenericList_1_SystemInt32_Wrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(System.Collections.Generic.List<int>);
			Utils.BeginObjectRegister(type, L, translator, 0, 29, 2, 1);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Add", _m_Add);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddRange", _m_AddRange);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AsReadOnly", _m_AsReadOnly);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "BinarySearch", _m_BinarySearch);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Clear", _m_Clear);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Contains", _m_Contains);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CopyTo", _m_CopyTo);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Exists", _m_Exists);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Find", _m_Find);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "FindAll", _m_FindAll);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "FindIndex", _m_FindIndex);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "FindLast", _m_FindLast);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "FindLastIndex", _m_FindLastIndex);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ForEach", _m_ForEach);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetEnumerator", _m_GetEnumerator);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetRange", _m_GetRange);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "IndexOf", _m_IndexOf);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Insert", _m_Insert);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "InsertRange", _m_InsertRange);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LastIndexOf", _m_LastIndexOf);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Remove", _m_Remove);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RemoveAll", _m_RemoveAll);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RemoveAt", _m_RemoveAt);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RemoveRange", _m_RemoveRange);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Reverse", _m_Reverse);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Sort", _m_Sort);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ToArray", _m_ToArray);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "TrimExcess", _m_TrimExcess);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "TrueForAll", _m_TrueForAll);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "Capacity", _g_get_Capacity);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Count", _g_get_Count);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "Capacity", _s_set_Capacity);
            
			
			Utils.EndObjectRegister(type, L, translator, __CSIndexer, __NewIndexer,
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
					
					System.Collections.Generic.List<int> __cl_gen_ret = new System.Collections.Generic.List<int>();
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				if(LuaAPI.lua_gettop(L) == 2 && translator.Assignable<System.Collections.Generic.IEnumerable<int>>(L, 2))
				{
					System.Collections.Generic.IEnumerable<int> collection = (System.Collections.Generic.IEnumerable<int>)translator.GetObject(L, 2, typeof(System.Collections.Generic.IEnumerable<int>));
					
					System.Collections.Generic.List<int> __cl_gen_ret = new System.Collections.Generic.List<int>(collection);
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				if(LuaAPI.lua_gettop(L) == 2 && LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2))
				{
					int capacity = LuaAPI.xlua_tointeger(L, 2);
					
					System.Collections.Generic.List<int> __cl_gen_ret = new System.Collections.Generic.List<int>(capacity);
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to System.Collections.Generic.List<int> constructor!");
            
        }
        
		
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        public static int __CSIndexer(RealStatePtr L)
        {
			try {
			    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				
				if (translator.Assignable<System.Collections.Generic.List<int>>(L, 1) && LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2))
				{
					
					System.Collections.Generic.List<int> __cl_gen_to_be_invoked = (System.Collections.Generic.List<int>)translator.FastGetCSObj(L, 1);
					int index = LuaAPI.xlua_tointeger(L, 2);
					LuaAPI.lua_pushboolean(L, true);
					LuaAPI.xlua_pushinteger(L, __cl_gen_to_be_invoked[index]);
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
				
				if (translator.Assignable<System.Collections.Generic.List<int>>(L, 1) && LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3))
				{
					
					System.Collections.Generic.List<int> __cl_gen_to_be_invoked = (System.Collections.Generic.List<int>)translator.FastGetCSObj(L, 1);
					int key = LuaAPI.xlua_tointeger(L, 2);
					__cl_gen_to_be_invoked[key] = LuaAPI.xlua_tointeger(L, 3);
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
        static int _m_Add(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                System.Collections.Generic.List<int> __cl_gen_to_be_invoked = (System.Collections.Generic.List<int>)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int item = LuaAPI.xlua_tointeger(L, 2);
                    
                    __cl_gen_to_be_invoked.Add( item );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddRange(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                System.Collections.Generic.List<int> __cl_gen_to_be_invoked = (System.Collections.Generic.List<int>)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Collections.Generic.IEnumerable<int> collection = (System.Collections.Generic.IEnumerable<int>)translator.GetObject(L, 2, typeof(System.Collections.Generic.IEnumerable<int>));
                    
                    __cl_gen_to_be_invoked.AddRange( collection );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AsReadOnly(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                System.Collections.Generic.List<int> __cl_gen_to_be_invoked = (System.Collections.Generic.List<int>)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        System.Collections.ObjectModel.ReadOnlyCollection<int> __cl_gen_ret = __cl_gen_to_be_invoked.AsReadOnly(  );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_BinarySearch(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                System.Collections.Generic.List<int> __cl_gen_to_be_invoked = (System.Collections.Generic.List<int>)translator.FastGetCSObj(L, 1);
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 2&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    int item = LuaAPI.xlua_tointeger(L, 2);
                    
                        int __cl_gen_ret = __cl_gen_to_be_invoked.BinarySearch( item );
                        LuaAPI.xlua_pushinteger(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                if(__gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<System.Collections.Generic.IComparer<int>>(L, 3)) 
                {
                    int item = LuaAPI.xlua_tointeger(L, 2);
                    System.Collections.Generic.IComparer<int> comparer = (System.Collections.Generic.IComparer<int>)translator.GetObject(L, 3, typeof(System.Collections.Generic.IComparer<int>));
                    
                        int __cl_gen_ret = __cl_gen_to_be_invoked.BinarySearch( item, comparer );
                        LuaAPI.xlua_pushinteger(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                if(__gen_param_count == 5&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& translator.Assignable<System.Collections.Generic.IComparer<int>>(L, 5)) 
                {
                    int index = LuaAPI.xlua_tointeger(L, 2);
                    int count = LuaAPI.xlua_tointeger(L, 3);
                    int item = LuaAPI.xlua_tointeger(L, 4);
                    System.Collections.Generic.IComparer<int> comparer = (System.Collections.Generic.IComparer<int>)translator.GetObject(L, 5, typeof(System.Collections.Generic.IComparer<int>));
                    
                        int __cl_gen_ret = __cl_gen_to_be_invoked.BinarySearch( index, count, item, comparer );
                        LuaAPI.xlua_pushinteger(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to System.Collections.Generic.List<int>.BinarySearch!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Clear(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                System.Collections.Generic.List<int> __cl_gen_to_be_invoked = (System.Collections.Generic.List<int>)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.Clear(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Contains(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                System.Collections.Generic.List<int> __cl_gen_to_be_invoked = (System.Collections.Generic.List<int>)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int item = LuaAPI.xlua_tointeger(L, 2);
                    
                        bool __cl_gen_ret = __cl_gen_to_be_invoked.Contains( item );
                        LuaAPI.lua_pushboolean(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CopyTo(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                System.Collections.Generic.List<int> __cl_gen_to_be_invoked = (System.Collections.Generic.List<int>)translator.FastGetCSObj(L, 1);
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 2&& translator.Assignable<int[]>(L, 2)) 
                {
                    int[] array = (int[])translator.GetObject(L, 2, typeof(int[]));
                    
                    __cl_gen_to_be_invoked.CopyTo( array );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 3&& translator.Assignable<int[]>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    int[] array = (int[])translator.GetObject(L, 2, typeof(int[]));
                    int arrayIndex = LuaAPI.xlua_tointeger(L, 3);
                    
                    __cl_gen_to_be_invoked.CopyTo( array, arrayIndex );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 5&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<int[]>(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)) 
                {
                    int index = LuaAPI.xlua_tointeger(L, 2);
                    int[] array = (int[])translator.GetObject(L, 3, typeof(int[]));
                    int arrayIndex = LuaAPI.xlua_tointeger(L, 4);
                    int count = LuaAPI.xlua_tointeger(L, 5);
                    
                    __cl_gen_to_be_invoked.CopyTo( index, array, arrayIndex, count );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to System.Collections.Generic.List<int>.CopyTo!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Exists(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                System.Collections.Generic.List<int> __cl_gen_to_be_invoked = (System.Collections.Generic.List<int>)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Predicate<int> match = translator.GetDelegate<System.Predicate<int>>(L, 2);
                    
                        bool __cl_gen_ret = __cl_gen_to_be_invoked.Exists( match );
                        LuaAPI.lua_pushboolean(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Find(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                System.Collections.Generic.List<int> __cl_gen_to_be_invoked = (System.Collections.Generic.List<int>)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Predicate<int> match = translator.GetDelegate<System.Predicate<int>>(L, 2);
                    
                        int __cl_gen_ret = __cl_gen_to_be_invoked.Find( match );
                        LuaAPI.xlua_pushinteger(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_FindAll(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                System.Collections.Generic.List<int> __cl_gen_to_be_invoked = (System.Collections.Generic.List<int>)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Predicate<int> match = translator.GetDelegate<System.Predicate<int>>(L, 2);
                    
                        System.Collections.Generic.List<int> __cl_gen_ret = __cl_gen_to_be_invoked.FindAll( match );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_FindIndex(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                System.Collections.Generic.List<int> __cl_gen_to_be_invoked = (System.Collections.Generic.List<int>)translator.FastGetCSObj(L, 1);
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 2&& translator.Assignable<System.Predicate<int>>(L, 2)) 
                {
                    System.Predicate<int> match = translator.GetDelegate<System.Predicate<int>>(L, 2);
                    
                        int __cl_gen_ret = __cl_gen_to_be_invoked.FindIndex( match );
                        LuaAPI.xlua_pushinteger(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                if(__gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<System.Predicate<int>>(L, 3)) 
                {
                    int startIndex = LuaAPI.xlua_tointeger(L, 2);
                    System.Predicate<int> match = translator.GetDelegate<System.Predicate<int>>(L, 3);
                    
                        int __cl_gen_ret = __cl_gen_to_be_invoked.FindIndex( startIndex, match );
                        LuaAPI.xlua_pushinteger(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                if(__gen_param_count == 4&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<System.Predicate<int>>(L, 4)) 
                {
                    int startIndex = LuaAPI.xlua_tointeger(L, 2);
                    int count = LuaAPI.xlua_tointeger(L, 3);
                    System.Predicate<int> match = translator.GetDelegate<System.Predicate<int>>(L, 4);
                    
                        int __cl_gen_ret = __cl_gen_to_be_invoked.FindIndex( startIndex, count, match );
                        LuaAPI.xlua_pushinteger(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to System.Collections.Generic.List<int>.FindIndex!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_FindLast(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                System.Collections.Generic.List<int> __cl_gen_to_be_invoked = (System.Collections.Generic.List<int>)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Predicate<int> match = translator.GetDelegate<System.Predicate<int>>(L, 2);
                    
                        int __cl_gen_ret = __cl_gen_to_be_invoked.FindLast( match );
                        LuaAPI.xlua_pushinteger(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_FindLastIndex(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                System.Collections.Generic.List<int> __cl_gen_to_be_invoked = (System.Collections.Generic.List<int>)translator.FastGetCSObj(L, 1);
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 2&& translator.Assignable<System.Predicate<int>>(L, 2)) 
                {
                    System.Predicate<int> match = translator.GetDelegate<System.Predicate<int>>(L, 2);
                    
                        int __cl_gen_ret = __cl_gen_to_be_invoked.FindLastIndex( match );
                        LuaAPI.xlua_pushinteger(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                if(__gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<System.Predicate<int>>(L, 3)) 
                {
                    int startIndex = LuaAPI.xlua_tointeger(L, 2);
                    System.Predicate<int> match = translator.GetDelegate<System.Predicate<int>>(L, 3);
                    
                        int __cl_gen_ret = __cl_gen_to_be_invoked.FindLastIndex( startIndex, match );
                        LuaAPI.xlua_pushinteger(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                if(__gen_param_count == 4&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<System.Predicate<int>>(L, 4)) 
                {
                    int startIndex = LuaAPI.xlua_tointeger(L, 2);
                    int count = LuaAPI.xlua_tointeger(L, 3);
                    System.Predicate<int> match = translator.GetDelegate<System.Predicate<int>>(L, 4);
                    
                        int __cl_gen_ret = __cl_gen_to_be_invoked.FindLastIndex( startIndex, count, match );
                        LuaAPI.xlua_pushinteger(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to System.Collections.Generic.List<int>.FindLastIndex!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ForEach(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                System.Collections.Generic.List<int> __cl_gen_to_be_invoked = (System.Collections.Generic.List<int>)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Action<int> action = translator.GetDelegate<System.Action<int>>(L, 2);
                    
                    __cl_gen_to_be_invoked.ForEach( action );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetEnumerator(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                System.Collections.Generic.List<int> __cl_gen_to_be_invoked = (System.Collections.Generic.List<int>)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        System.Collections.Generic.List<int>.Enumerator __cl_gen_ret = __cl_gen_to_be_invoked.GetEnumerator(  );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetRange(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                System.Collections.Generic.List<int> __cl_gen_to_be_invoked = (System.Collections.Generic.List<int>)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int index = LuaAPI.xlua_tointeger(L, 2);
                    int count = LuaAPI.xlua_tointeger(L, 3);
                    
                        System.Collections.Generic.List<int> __cl_gen_ret = __cl_gen_to_be_invoked.GetRange( index, count );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IndexOf(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                System.Collections.Generic.List<int> __cl_gen_to_be_invoked = (System.Collections.Generic.List<int>)translator.FastGetCSObj(L, 1);
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 2&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    int item = LuaAPI.xlua_tointeger(L, 2);
                    
                        int __cl_gen_ret = __cl_gen_to_be_invoked.IndexOf( item );
                        LuaAPI.xlua_pushinteger(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                if(__gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    int item = LuaAPI.xlua_tointeger(L, 2);
                    int index = LuaAPI.xlua_tointeger(L, 3);
                    
                        int __cl_gen_ret = __cl_gen_to_be_invoked.IndexOf( item, index );
                        LuaAPI.xlua_pushinteger(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                if(__gen_param_count == 4&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    int item = LuaAPI.xlua_tointeger(L, 2);
                    int index = LuaAPI.xlua_tointeger(L, 3);
                    int count = LuaAPI.xlua_tointeger(L, 4);
                    
                        int __cl_gen_ret = __cl_gen_to_be_invoked.IndexOf( item, index, count );
                        LuaAPI.xlua_pushinteger(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to System.Collections.Generic.List<int>.IndexOf!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Insert(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                System.Collections.Generic.List<int> __cl_gen_to_be_invoked = (System.Collections.Generic.List<int>)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int index = LuaAPI.xlua_tointeger(L, 2);
                    int item = LuaAPI.xlua_tointeger(L, 3);
                    
                    __cl_gen_to_be_invoked.Insert( index, item );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InsertRange(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                System.Collections.Generic.List<int> __cl_gen_to_be_invoked = (System.Collections.Generic.List<int>)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int index = LuaAPI.xlua_tointeger(L, 2);
                    System.Collections.Generic.IEnumerable<int> collection = (System.Collections.Generic.IEnumerable<int>)translator.GetObject(L, 3, typeof(System.Collections.Generic.IEnumerable<int>));
                    
                    __cl_gen_to_be_invoked.InsertRange( index, collection );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LastIndexOf(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                System.Collections.Generic.List<int> __cl_gen_to_be_invoked = (System.Collections.Generic.List<int>)translator.FastGetCSObj(L, 1);
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 2&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    int item = LuaAPI.xlua_tointeger(L, 2);
                    
                        int __cl_gen_ret = __cl_gen_to_be_invoked.LastIndexOf( item );
                        LuaAPI.xlua_pushinteger(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                if(__gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    int item = LuaAPI.xlua_tointeger(L, 2);
                    int index = LuaAPI.xlua_tointeger(L, 3);
                    
                        int __cl_gen_ret = __cl_gen_to_be_invoked.LastIndexOf( item, index );
                        LuaAPI.xlua_pushinteger(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                if(__gen_param_count == 4&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    int item = LuaAPI.xlua_tointeger(L, 2);
                    int index = LuaAPI.xlua_tointeger(L, 3);
                    int count = LuaAPI.xlua_tointeger(L, 4);
                    
                        int __cl_gen_ret = __cl_gen_to_be_invoked.LastIndexOf( item, index, count );
                        LuaAPI.xlua_pushinteger(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to System.Collections.Generic.List<int>.LastIndexOf!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Remove(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                System.Collections.Generic.List<int> __cl_gen_to_be_invoked = (System.Collections.Generic.List<int>)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int item = LuaAPI.xlua_tointeger(L, 2);
                    
                        bool __cl_gen_ret = __cl_gen_to_be_invoked.Remove( item );
                        LuaAPI.lua_pushboolean(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RemoveAll(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                System.Collections.Generic.List<int> __cl_gen_to_be_invoked = (System.Collections.Generic.List<int>)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Predicate<int> match = translator.GetDelegate<System.Predicate<int>>(L, 2);
                    
                        int __cl_gen_ret = __cl_gen_to_be_invoked.RemoveAll( match );
                        LuaAPI.xlua_pushinteger(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RemoveAt(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                System.Collections.Generic.List<int> __cl_gen_to_be_invoked = (System.Collections.Generic.List<int>)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int index = LuaAPI.xlua_tointeger(L, 2);
                    
                    __cl_gen_to_be_invoked.RemoveAt( index );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RemoveRange(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                System.Collections.Generic.List<int> __cl_gen_to_be_invoked = (System.Collections.Generic.List<int>)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int index = LuaAPI.xlua_tointeger(L, 2);
                    int count = LuaAPI.xlua_tointeger(L, 3);
                    
                    __cl_gen_to_be_invoked.RemoveRange( index, count );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Reverse(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                System.Collections.Generic.List<int> __cl_gen_to_be_invoked = (System.Collections.Generic.List<int>)translator.FastGetCSObj(L, 1);
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 1) 
                {
                    
                    __cl_gen_to_be_invoked.Reverse(  );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    int index = LuaAPI.xlua_tointeger(L, 2);
                    int count = LuaAPI.xlua_tointeger(L, 3);
                    
                    __cl_gen_to_be_invoked.Reverse( index, count );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to System.Collections.Generic.List<int>.Reverse!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Sort(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                System.Collections.Generic.List<int> __cl_gen_to_be_invoked = (System.Collections.Generic.List<int>)translator.FastGetCSObj(L, 1);
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 1) 
                {
                    
                    __cl_gen_to_be_invoked.Sort(  );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 2&& translator.Assignable<System.Collections.Generic.IComparer<int>>(L, 2)) 
                {
                    System.Collections.Generic.IComparer<int> comparer = (System.Collections.Generic.IComparer<int>)translator.GetObject(L, 2, typeof(System.Collections.Generic.IComparer<int>));
                    
                    __cl_gen_to_be_invoked.Sort( comparer );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 2&& translator.Assignable<System.Comparison<int>>(L, 2)) 
                {
                    System.Comparison<int> comparison = translator.GetDelegate<System.Comparison<int>>(L, 2);
                    
                    __cl_gen_to_be_invoked.Sort( comparison );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 4&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<System.Collections.Generic.IComparer<int>>(L, 4)) 
                {
                    int index = LuaAPI.xlua_tointeger(L, 2);
                    int count = LuaAPI.xlua_tointeger(L, 3);
                    System.Collections.Generic.IComparer<int> comparer = (System.Collections.Generic.IComparer<int>)translator.GetObject(L, 4, typeof(System.Collections.Generic.IComparer<int>));
                    
                    __cl_gen_to_be_invoked.Sort( index, count, comparer );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to System.Collections.Generic.List<int>.Sort!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ToArray(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                System.Collections.Generic.List<int> __cl_gen_to_be_invoked = (System.Collections.Generic.List<int>)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        int[] __cl_gen_ret = __cl_gen_to_be_invoked.ToArray(  );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_TrimExcess(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                System.Collections.Generic.List<int> __cl_gen_to_be_invoked = (System.Collections.Generic.List<int>)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.TrimExcess(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_TrueForAll(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                System.Collections.Generic.List<int> __cl_gen_to_be_invoked = (System.Collections.Generic.List<int>)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Predicate<int> match = translator.GetDelegate<System.Predicate<int>>(L, 2);
                    
                        bool __cl_gen_ret = __cl_gen_to_be_invoked.TrueForAll( match );
                        LuaAPI.lua_pushboolean(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Capacity(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                System.Collections.Generic.List<int> __cl_gen_to_be_invoked = (System.Collections.Generic.List<int>)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, __cl_gen_to_be_invoked.Capacity);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Count(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                System.Collections.Generic.List<int> __cl_gen_to_be_invoked = (System.Collections.Generic.List<int>)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, __cl_gen_to_be_invoked.Count);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Capacity(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                System.Collections.Generic.List<int> __cl_gen_to_be_invoked = (System.Collections.Generic.List<int>)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.Capacity = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
