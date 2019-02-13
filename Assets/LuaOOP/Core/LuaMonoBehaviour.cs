using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using System;
public class LuaMonoBehaviour : MonoBehaviour {
	/// <summary>
	/// path of lua file script
	/// </summary>
	public string m_LuaFilePath;
	public TextAsset m_LuaAsset;
	private LuaTable scriptEnv;
	private static LuaEnv luaEnv = new LuaEnv();

	public GameObject PanelLan;
	private Action luaAwake;
	private Action luaStart;
	private Action luaUpdate;
	private Action luaOnEnable;
	private Action luaOnDisable;
	private Action luaOnDestroy;
	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake()
	{
		scriptEnv = luaEnv.NewTable();
		LuaTable meta = luaEnv.NewTable();
		meta.Set("__index",luaEnv.Global);
		scriptEnv.SetMetaTable(meta);
		meta.Dispose();

		scriptEnv.Set("self",this);
		luaEnv.DoString(m_LuaAsset.text,"LuaMonoBehaviour",scriptEnv);
		luaAwake = scriptEnv.Get<Action>("awake");
	 	luaStart = scriptEnv.Get<Action>("start");
		luaUpdate = scriptEnv.Get<Action>("update");
		luaOnEnable = scriptEnv.Get<Action>("onEnable");
		luaOnDisable = scriptEnv.Get<Action>("onDisable");
		luaOnDestroy = scriptEnv.Get<Action>("onDestroy");

		if(luaAwake!=null)
			luaAwake();
	}

	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		if(luaStart!=null)
			luaStart();
	}
	
	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void Update()
	{
		if(luaUpdate!=null)
			luaUpdate();
	}

	/// <summary>
	/// This function is called when the object becomes enabled and active.
	/// </summary>
	void OnEnable()
	{
		if(luaOnEnable!=null)
			luaOnEnable();
	}

	/// <summary>
	/// This function is called when the behaviour becomes disabled or inactive.
	/// </summary>
	void OnDisable()
	{
		if(luaOnDisable!=null)
			luaOnDisable();
	}

	/// <summary>
	/// This function is called when the MonoBehaviour will be destroyed.
	/// </summary>
	void OnDestroy()
	{
		if(luaOnDestroy!=null)
			luaOnDestroy();
		luaOnDestroy=null;
		luaAwake=null;
		luaStart = null;
		luaUpdate=null;
		luaOnDisable=null;
		luaOnEnable=null;
		scriptEnv.Dispose();
		scriptEnv=null;
	}	
}
