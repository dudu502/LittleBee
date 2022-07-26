using Managers.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    class UILayerNode
    {
        public Transform transform;
        public LinkedList<UIView> viewerLinkedList;
        public UILayerNode(Transform transformRoot)
        {
            transform = transformRoot;
            viewerLinkedList = new LinkedList<UIView>();
        }

    }
    public enum Layer
    {
        Bottom,
        Middle,
        Top,
    }
    public class UIModule : MonoBehaviour, IModule
    {
        Dictionary<Layer, UILayerNode> LayerViewDict;
        public void Init()
        {
            LayerViewDict = new Dictionary<Layer, UILayerNode>();
            LayerViewDict[Layer.Bottom] = new UILayerNode(transform.Find("Bottom").transform);
            LayerViewDict[Layer.Middle] = new UILayerNode(transform.Find("Middle").transform);
            LayerViewDict[Layer.Top] = new UILayerNode(transform.Find("Top").transform);

        }
        public void Push(string uiPrefabFullName, Layer layer, object data)
        {
            StartCoroutine(PushCor(uiPrefabFullName, layer, data));
        }
        private IEnumerator PushCor(string uiPrefabFullName, Layer layer, object data)
        {
            if (LayerViewDict.TryGetValue(layer, out UILayerNode layerNode))
            {
                ResourceRequest resourceRequest = Resources.LoadAsync<GameObject>(uiPrefabFullName);
                yield return resourceRequest;
                GameObject uiGo = UnityEngine.Object.Instantiate(resourceRequest.asset, layerNode.transform) as GameObject;
                uiGo.name = uiPrefabFullName;
                UIView uiView = uiGo.GetComponent<UIView>();
                _Push(uiView, layer, data);
            }
        }
        private void _Push(UIView viewer, Layer layer, object data)
        {
            if (viewer == null) throw new System.NullReferenceException();
            if (LayerViewDict.TryGetValue(layer, out UILayerNode layerNode))
            {
                if (layerNode.viewerLinkedList.Count > 0)
                {
                    UIView currentView = layerNode.viewerLinkedList.Last.Value;
                    currentView.OnPause();
                }
                layerNode.viewerLinkedList.AddLast(viewer);
                viewer.OnInit();
                viewer.OnShow(data);
            }
        }

        public bool Pop(Layer layer, UIView view)
        {
            if (view != null && LayerViewDict.TryGetValue(layer, out UILayerNode layerNode))
            {
                if (layerNode.viewerLinkedList.Count > 0)
                {
                    if (layerNode.viewerLinkedList.Last.Value == view)
                    {
                        layerNode.viewerLinkedList.Remove(view);
                        view.OnClose();
                        if (layerNode.viewerLinkedList.Count > 0)
                            layerNode.viewerLinkedList.Last.Value.OnResume();
                    }
                    else
                    {
                        layerNode.viewerLinkedList.Remove(view);
                        view.OnClose();
                    }
                    return true;
                }
            }
            return false;
        }

        public bool Pop(Layer layer)
        {
            if (LayerViewDict.TryGetValue(layer, out UILayerNode layerNode))
            {
                if (layerNode.viewerLinkedList.Count > 0)
                {
                    UIView currentView = layerNode.viewerLinkedList.Last.Value;
                    layerNode.viewerLinkedList.RemoveLast();
                    currentView.OnClose();
                    if (layerNode.viewerLinkedList.Count > 0)
                    {
                        layerNode.viewerLinkedList.Last.Value.OnResume();
                    }
                    return true;
                }
            }
            return false;
        }
    }
}
