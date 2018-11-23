using UnityEngine;
using Renderers;

public class Bullet : MonoBehaviour
{
    #region Recycle
    public static ObjectPool ObjectPool = new ObjectPool(
        () => 
        {
            GameObject obj = GameObject.Instantiate<GameObject>(Resources.Load("Bullet") as GameObject);
            return obj;
        },
        (bullet) => {
            GameObject obj = bullet as GameObject;           
        },
        (bullet)=> {
            GameObject obj = bullet as GameObject;
            obj.GetComponent<MoveActionRenderer>().SetEntityId("");
        });
    #endregion
}
