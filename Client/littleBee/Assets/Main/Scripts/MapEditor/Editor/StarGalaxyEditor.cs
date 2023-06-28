
using Synchronize.Game.Lockstep.MapEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace MapEditor.Editor
{
    [UnityEditor.CustomEditor(typeof(StarGalaxy))]
    class StarGalaxyEditor:UnityEditor.Editor
    {
   
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            StarGalaxy galaxy = target as StarGalaxy;
            if(GUILayout.Button("AddGamePrefab"))
            {
                galaxy.AddGamePrefab();
            }
            if (GUILayout.Button("Export"))
            {
                ExportStarInfos(galaxy);
            }
            if (GUILayout.Button("Import"))
            {
                ImportMapSteam(galaxy);
            }
            if(GUILayout.Button("Clear"))
            {
                ClearElements(galaxy);
            }
        }

        private void ClearElements(StarGalaxy galaxy)
        {          
            var list = galaxy.GetComponents<StarObject>();
            if (list != null && list.Length > 0)
            {
                for (int i = 0; i < list.Length; ++i)
                {
                    DestroyImmediate(list[i]);
                }
            }

            var asteroids = galaxy.GetComponents<AsteroidBelt>();
            if(asteroids!=null&& asteroids.Length>0)
            {
                for (int i = 0; i < asteroids.Length; ++i)
                {
                    DestroyImmediate(asteroids[i]);
                }
            }
        }

        private void ImportMapSteam(StarGalaxy galaxy)
        {
            if (null == galaxy.m_ImportMapStream || null == galaxy.m_ImportMapStream.text)
            {
                Debug.LogError("map stream null");
                return;
            }
            ClearElements(galaxy);
            galaxy.AddImportMaySteam();
        }

        private void ExportStarInfos(StarGalaxy galaxy)
        {
            if (string.IsNullOrEmpty(galaxy.m_GalaxyName))
            {
                Debug.LogError("Set GalaxyName");
                return;
            }

            StarGalaxyInfo sgInfo = new StarGalaxyInfo();
            sgInfo.GalaxyName = galaxy.m_GalaxyName;
            sgInfo.MapWidth = galaxy.m_MapWidth;
            sgInfo.MapHeight = galaxy.m_MapHeight;
            sgInfo.Stars = new List<StarObjectInfo>();
            sgInfo.Belts = new List<AsteroidBeltInfo>();
            var stars = galaxy.GetComponents<StarObject>();
            for (int i = 0; i < stars.Length; ++i)
            {
                sgInfo.Stars.Add(stars[i].m_Info);
            }

            var asteroids = galaxy.GetComponents<AsteroidBelt>();
            for(int i=0;i<asteroids.Length;++i)
            {
                sgInfo.Belts.Add(asteroids[i].m_Info);
            }

            System.IO.File.WriteAllText(Application.dataPath + "/Resources/Configs/Maps/" + galaxy.m_GalaxyName + ".json",LitJson.JsonMapper.ToJson(sgInfo));
            AssetDatabase.Refresh();
        }
    }
}
