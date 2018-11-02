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
    public class UnityEngineWWWWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(UnityEngine.WWW);
			Utils.BeginObjectRegister(type, L, translator, 0, 2, 14, 1);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoadImageIntoTexture", _m_LoadImageIntoTexture);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Dispose", _m_Dispose);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "assetBundle", _g_get_assetBundle);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "bytes", _g_get_bytes);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "bytesDownloaded", _g_get_bytesDownloaded);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "error", _g_get_error);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "isDone", _g_get_isDone);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "progress", _g_get_progress);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "responseHeaders", _g_get_responseHeaders);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "text", _g_get_text);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "texture", _g_get_texture);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "textureNonReadable", _g_get_textureNonReadable);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "threadPriority", _g_get_threadPriority);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "uploadProgress", _g_get_uploadProgress);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "url", _g_get_url);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "keepWaiting", _g_get_keepWaiting);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "threadPriority", _s_set_threadPriority);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 4, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "EscapeURL", _m_EscapeURL_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "UnEscapeURL", _m_UnEscapeURL_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "LoadFromCacheOrDownload", _m_LoadFromCacheOrDownload_xlua_st_);
            
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 2 && (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
				{
					string url = LuaAPI.lua_tostring(L, 2);
					
					UnityEngine.WWW __cl_gen_ret = new UnityEngine.WWW(url);
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				if(LuaAPI.lua_gettop(L) == 3 && (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && translator.Assignable<UnityEngine.WWWForm>(L, 3))
				{
					string url = LuaAPI.lua_tostring(L, 2);
					UnityEngine.WWWForm form = (UnityEngine.WWWForm)translator.GetObject(L, 3, typeof(UnityEngine.WWWForm));
					
					UnityEngine.WWW __cl_gen_ret = new UnityEngine.WWW(url, form);
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				if(LuaAPI.lua_gettop(L) == 3 && (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING))
				{
					string url = LuaAPI.lua_tostring(L, 2);
					byte[] postData = LuaAPI.lua_tobytes(L, 3);
					
					UnityEngine.WWW __cl_gen_ret = new UnityEngine.WWW(url, postData);
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				if(LuaAPI.lua_gettop(L) == 4 && (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING) && translator.Assignable<System.Collections.Generic.Dictionary<string, string>>(L, 4))
				{
					string url = LuaAPI.lua_tostring(L, 2);
					byte[] postData = LuaAPI.lua_tobytes(L, 3);
					System.Collections.Generic.Dictionary<string, string> headers = (System.Collections.Generic.Dictionary<string, string>)translator.GetObject(L, 4, typeof(System.Collections.Generic.Dictionary<string, string>));
					
					UnityEngine.WWW __cl_gen_ret = new UnityEngine.WWW(url, postData, headers);
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.WWW constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadImageIntoTexture(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.WWW __cl_gen_to_be_invoked = (UnityEngine.WWW)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Texture2D texture = (UnityEngine.Texture2D)translator.GetObject(L, 2, typeof(UnityEngine.Texture2D));
                    
                    __cl_gen_to_be_invoked.LoadImageIntoTexture( texture );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Dispose(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.WWW __cl_gen_to_be_invoked = (UnityEngine.WWW)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.Dispose(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_EscapeURL_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 1&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)) 
                {
                    string s = LuaAPI.lua_tostring(L, 1);
                    
                        string __cl_gen_ret = UnityEngine.WWW.EscapeURL( s );
                        LuaAPI.lua_pushstring(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                if(__gen_param_count == 2&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& translator.Assignable<System.Text.Encoding>(L, 2)) 
                {
                    string s = LuaAPI.lua_tostring(L, 1);
                    System.Text.Encoding e = (System.Text.Encoding)translator.GetObject(L, 2, typeof(System.Text.Encoding));
                    
                        string __cl_gen_ret = UnityEngine.WWW.EscapeURL( s, e );
                        LuaAPI.lua_pushstring(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.WWW.EscapeURL!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UnEscapeURL_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 1&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)) 
                {
                    string s = LuaAPI.lua_tostring(L, 1);
                    
                        string __cl_gen_ret = UnityEngine.WWW.UnEscapeURL( s );
                        LuaAPI.lua_pushstring(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                if(__gen_param_count == 2&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& translator.Assignable<System.Text.Encoding>(L, 2)) 
                {
                    string s = LuaAPI.lua_tostring(L, 1);
                    System.Text.Encoding e = (System.Text.Encoding)translator.GetObject(L, 2, typeof(System.Text.Encoding));
                    
                        string __cl_gen_ret = UnityEngine.WWW.UnEscapeURL( s, e );
                        LuaAPI.lua_pushstring(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.WWW.UnEscapeURL!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadFromCacheOrDownload_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 2&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    string url = LuaAPI.lua_tostring(L, 1);
                    int version = LuaAPI.xlua_tointeger(L, 2);
                    
                        UnityEngine.WWW __cl_gen_ret = UnityEngine.WWW.LoadFromCacheOrDownload( url, version );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                if(__gen_param_count == 3&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    string url = LuaAPI.lua_tostring(L, 1);
                    int version = LuaAPI.xlua_tointeger(L, 2);
                    uint crc = LuaAPI.xlua_touint(L, 3);
                    
                        UnityEngine.WWW __cl_gen_ret = UnityEngine.WWW.LoadFromCacheOrDownload( url, version, crc );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                if(__gen_param_count == 2&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& translator.Assignable<UnityEngine.Hash128>(L, 2)) 
                {
                    string url = LuaAPI.lua_tostring(L, 1);
                    UnityEngine.Hash128 hash;translator.Get(L, 2, out hash);
                    
                        UnityEngine.WWW __cl_gen_ret = UnityEngine.WWW.LoadFromCacheOrDownload( url, hash );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                if(__gen_param_count == 3&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& translator.Assignable<UnityEngine.Hash128>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    string url = LuaAPI.lua_tostring(L, 1);
                    UnityEngine.Hash128 hash;translator.Get(L, 2, out hash);
                    uint crc = LuaAPI.xlua_touint(L, 3);
                    
                        UnityEngine.WWW __cl_gen_ret = UnityEngine.WWW.LoadFromCacheOrDownload( url, hash, crc );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                if(__gen_param_count == 3&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& translator.Assignable<UnityEngine.CachedAssetBundle>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    string url = LuaAPI.lua_tostring(L, 1);
                    UnityEngine.CachedAssetBundle cachedBundle;translator.Get(L, 2, out cachedBundle);
                    uint crc = LuaAPI.xlua_touint(L, 3);
                    
                        UnityEngine.WWW __cl_gen_ret = UnityEngine.WWW.LoadFromCacheOrDownload( url, cachedBundle, crc );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                if(__gen_param_count == 2&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& translator.Assignable<UnityEngine.CachedAssetBundle>(L, 2)) 
                {
                    string url = LuaAPI.lua_tostring(L, 1);
                    UnityEngine.CachedAssetBundle cachedBundle;translator.Get(L, 2, out cachedBundle);
                    
                        UnityEngine.WWW __cl_gen_ret = UnityEngine.WWW.LoadFromCacheOrDownload( url, cachedBundle );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.WWW.LoadFromCacheOrDownload!");
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_assetBundle(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.WWW __cl_gen_to_be_invoked = (UnityEngine.WWW)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.assetBundle);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_bytes(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.WWW __cl_gen_to_be_invoked = (UnityEngine.WWW)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, __cl_gen_to_be_invoked.bytes);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_bytesDownloaded(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.WWW __cl_gen_to_be_invoked = (UnityEngine.WWW)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, __cl_gen_to_be_invoked.bytesDownloaded);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_error(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.WWW __cl_gen_to_be_invoked = (UnityEngine.WWW)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, __cl_gen_to_be_invoked.error);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_isDone(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.WWW __cl_gen_to_be_invoked = (UnityEngine.WWW)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, __cl_gen_to_be_invoked.isDone);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_progress(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.WWW __cl_gen_to_be_invoked = (UnityEngine.WWW)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, __cl_gen_to_be_invoked.progress);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_responseHeaders(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.WWW __cl_gen_to_be_invoked = (UnityEngine.WWW)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.responseHeaders);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_text(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.WWW __cl_gen_to_be_invoked = (UnityEngine.WWW)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, __cl_gen_to_be_invoked.text);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_texture(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.WWW __cl_gen_to_be_invoked = (UnityEngine.WWW)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.texture);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_textureNonReadable(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.WWW __cl_gen_to_be_invoked = (UnityEngine.WWW)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.textureNonReadable);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_threadPriority(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.WWW __cl_gen_to_be_invoked = (UnityEngine.WWW)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.threadPriority);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_uploadProgress(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.WWW __cl_gen_to_be_invoked = (UnityEngine.WWW)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, __cl_gen_to_be_invoked.uploadProgress);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_url(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.WWW __cl_gen_to_be_invoked = (UnityEngine.WWW)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, __cl_gen_to_be_invoked.url);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_keepWaiting(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.WWW __cl_gen_to_be_invoked = (UnityEngine.WWW)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, __cl_gen_to_be_invoked.keepWaiting);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_threadPriority(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.WWW __cl_gen_to_be_invoked = (UnityEngine.WWW)translator.FastGetCSObj(L, 1);
                UnityEngine.ThreadPriority __cl_gen_value;translator.Get(L, 2, out __cl_gen_value);
				__cl_gen_to_be_invoked.threadPriority = __cl_gen_value;
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
