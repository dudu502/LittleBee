using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ETFXSceneManager : MonoBehaviour
{
	public bool GUIHide = false;
	public bool GUIHide2 = false;
	public bool GUIHide3 = false;	
	
    public void LoadScene1()  {
		SceneManager.LoadScene ("etfx_explosions");
	}
    public void LoadScene2()  {
        SceneManager.LoadScene ("etfx_explosions2");
	}
    public void LoadScene3()  {
        SceneManager.LoadScene ("etfx_portals");
	}
    public void LoadScene4()  {
		SceneManager.LoadScene ("etfx_magic");
	}
    public void LoadScene5()  {
        SceneManager.LoadScene ("etfx_emojis");
	}
    public void LoadScene6()  {
        SceneManager.LoadScene ("etfx_sparkles");
	}
    public void LoadScene7()  {
        SceneManager.LoadScene ("etfx_fireworks");
	}
    public void LoadScene8()  {
        SceneManager.LoadScene ("etfx_powerups");
    }
    public void LoadScene9()  {
        SceneManager.LoadScene ("etfx_swordcombat");
    }
    public void LoadScene10() {
        SceneManager.LoadScene("etfx_maindemo");
    }
    public void LoadScene11() {
        SceneManager.LoadScene("etfx_combat");
    }
    public void LoadScene12() {
        SceneManager.LoadScene("etfx_2ddemo");
    }
	public void LoadScene13() {
        SceneManager.LoadScene("etfx_missiles");
    }
	
	void Update ()
	 {
 
     if(Input.GetKeyDown(KeyCode.L))
	 {
         GUIHide = !GUIHide;
     
         if (GUIHide)
		 {
             GameObject.Find("CanvasSceneSelect").GetComponent<Canvas> ().enabled = false;
         }
		 else
		 {
             GameObject.Find("CanvasSceneSelect").GetComponent<Canvas> ().enabled = true;
         }
     }
	      if(Input.GetKeyDown(KeyCode.J))
	 {
         GUIHide2 = !GUIHide2;
     
         if (GUIHide2)
		 {
             GameObject.Find("Canvas").GetComponent<Canvas> ().enabled = false;
         }
		 else
		 {
             GameObject.Find("Canvas").GetComponent<Canvas> ().enabled = true;
         }
     }
		if(Input.GetKeyDown(KeyCode.H))
	 {
         GUIHide3 = !GUIHide3;
     
         if (GUIHide3)
		 {
             GameObject.Find("ParticleSysDisplayCanvas").GetComponent<Canvas> ().enabled = false;
         }
		 else
		 {
             GameObject.Find("ParticleSysDisplayCanvas").GetComponent<Canvas> ().enabled = true;
         }
     }
	}	
}