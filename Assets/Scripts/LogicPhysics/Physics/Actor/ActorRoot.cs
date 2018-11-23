using Assets.Scripts.Common;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameLogic
{
    public class ActorRoot : PooledClassObject, IUpdateLogic
    {
        public string name = string.Empty;

        public bool isMovable = true;

        public bool isRotatable = true;

        public uint ObjID;

        [HideInInspector]
        [NonSerialized]
        public PoolObjHandle<ActorRoot> SelfPtr = default(PoolObjHandle<ActorRoot>);

       

        public Transform myTransform;

        private GameObject OriginalActorMesh;

        private Animation OriginalMeshAnim;



        private byte _bInitVisibleDelay;


       

        [HideInInspector]
        [NonSerialized]
        public VCollisionShape shape;

       

        private VInt3 _location;

        [HideInInspector]
        [NonSerialized]
        private VInt3 _forward = VInt3.forward;

        [HideInInspector]
        [NonSerialized]
        private Quaternion _rotation = Quaternion.identity;

        [HideInInspector]
        [NonSerialized]
        public VInt groundY = 0;

        [HideInInspector]
        [NonSerialized]
        public bool hasReachedNavEdge;

        [HideInInspector]
        [NonSerialized]
        public VInt pickFlyY = 0;

        [HideInInspector]
        [NonSerialized]
        public bool AttackOrderReady = true;

        [HideInInspector]
        [NonSerialized]
        private List<object> slotList = new List<object>();

        [HideInInspector]
        [NonSerialized]
        private bool bChildUpdate;


        [HideInInspector]
        [NonSerialized]
        public VInt3 BornPos;

        public bool bOneKiller;

        [HideInInspector]
        [NonSerialized]
        public bool isRecycled;

        public GameObject ActorMesh
        {
            get;
            protected set;
        }

        public Animation ActorMeshAnimation
        {
            get;
            protected set;
        }

        private bool bActive
        {
            get
            {
                return this.gameObject.activeSelf;
            }
        }

        public VInt3 location
        {
            get
            {
                return this._location;
            }
            set
            {
                bool flag = this._location.x != value.x || this._location.z != value.z;
                this._location = value;
                if (this.shape != null)
                {
                    this.shape.dirty = true;
                }

                if (this.bChildUpdate)
                {
                    this.UpdateActorRootSlot();
                }
            }
        }

        public VInt3 forward
        {
            get
            {
                return this._forward;
            }
            set
            {
                this._forward = value;
                if (this.shape != null)
                {
                    this.shape.dirty = true;
                }
                if (this.bChildUpdate)
                {
                    this.UpdateActorRootSlot();
                }
            }
        }

        public Quaternion rotation
        {
            get
            {
                return this._rotation;
            }
            set
            {
                if (this.isRotatable)
                {
                    this._rotation = value;
                }
            }
        }

        public GameObject gameObject
        {
            get
            {
                /*                return (!this.ObjLinker) ? null : this.ObjLinker.gameObject;*/
                return this.gameObject;
            }
        }



        public bool VisibleIniting
        {
            get
            {
                return this._bInitVisibleDelay > 0;
            }
        }

 

   

        public T GetComponent<T>() where T : Component
        {
            if (this.gameObject)
            {
                return this.gameObject.GetComponent<T>();
            }
            return (T)((object)null);
        }

      

        public void RecordOriginalActorMesh()
        {
            this.OriginalActorMesh = this.ActorMesh;
            this.OriginalMeshAnim = this.ActorMeshAnimation;
        }


        public ActorRootSlot CreateActorRootSlot(PoolObjHandle<ActorRoot> _child, VInt3 _parentPos, VInt3 _trans)
        {
            ActorRootSlot actorRootSlot = new ActorRootSlot(_child, _parentPos, _trans);
            this.slotList.Add(actorRootSlot);
            this.bChildUpdate = true;
            return actorRootSlot;
        }

        public bool RemoveActorRootSlot(ActorRootSlot _slot)
        {
            bool result = this.slotList.Remove(_slot);
            if (this.slotList.Count == 0)
            {
                this.bChildUpdate = false;
            }
            return result;
        }

        private void UpdateActorRootSlot()
        {
            for (int i = 0; i < this.slotList.Count; i++)
            {
                ActorRootSlot actorRootSlot = (ActorRootSlot)this.slotList[i];
                actorRootSlot.Update(this);
            }
        }

        public void UpdateLogic(int delta)
        {

        }

       
    }
}
