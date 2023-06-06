using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections;

namespace TrueSync.Collision
{
    public class BlockBox
    {
        public TSRect Rect;
        List<uint> ContainsIds;
        public BlockBox(TSRect rect)
        {
            Rect = rect;
            ContainsIds = new List<uint>();
        }
        public bool ForEachIds(Func<uint,bool> funcRet) 
        {
            for (int i = ContainsIds.Count - 1; i > -1; --i)
            {
                bool ret = funcRet(ContainsIds[i]);
                if (ret)
                    return true;
            }
            return false;
        }
        public void Clear()
        {
            ContainsIds.Clear();
        }
        public void Add(uint id)
        {
            ContainsIds.Add(id);
        }
        public int GetCount() { return ContainsIds.Count; }
    }
    public class NineBlockBoxCollision:ICollisionProvider
    {
        public FP BlockSize { private set; get; }
        public FP WorldWidth { private set; get; }
        public FP WorldHeight { private set; get; }
        public Dictionary<TSVector2, BlockBox> DictBlocks { private set; get; }
        public NineBlockBoxCollision(FP blockSize,FP worldWidth,FP worldHeight)
        {
            BlockSize = blockSize;
            WorldWidth = worldWidth;
            WorldHeight = worldHeight;
            DictBlocks = new Dictionary<TSVector2, BlockBox>();

            Init();
        }

        private void Init()
        {
            int wHalfCount = TSMath.Ceiling(WorldWidth / BlockSize / 2f).AsInt();
            int hHalfCount = TSMath.Ceiling(WorldHeight / BlockSize / 2f).AsInt();
            for(int w=-wHalfCount;w<wHalfCount;++w)
            {
                for(int h = -hHalfCount; h < hHalfCount; ++h)
                {
                    TSRect rect = new TSRect(BlockSize*w,BlockSize*h,BlockSize,BlockSize);
                    BlockBox box = new BlockBox(rect);
                    DictBlocks.Add(rect.min,box);
                }
            }
        }

        public void ForEachBlock(Action<BlockBox> action)
        {
            if(action!=null)
            {
                foreach(BlockBox box in DictBlocks.Values)
                {
                    action(box);
                }
            }
        }
        public void Add(TSVector2 position,uint entityId)
        {
            int iX = TSMath.Floor (position.x / BlockSize).AsInt();
            int iY = TSMath.Floor(position.y / BlockSize).AsInt();
            TSVector2 blockKey = new TSVector2(iX* BlockSize, iY* BlockSize);
            if (DictBlocks.ContainsKey(blockKey))
                DictBlocks[blockKey].Add(entityId);
        }
        public void Clear()
        {
            foreach(BlockBox box in DictBlocks.Values)
            {
                box.Clear();
            }
        }
        public bool CollisionDetection(TSVector2 position, Func<uint,bool> detectionRange)
        {
            int iX = TSMath.Floor(position.x / BlockSize).AsInt();
            int iY = TSMath.Floor(position.y / BlockSize).AsInt();
           
            TSVector2 ltBlock = new TSVector2((iX - 1) * BlockSize, (iY + 1) * BlockSize);
            TSVector2 ctBlock = new TSVector2(iX * BlockSize, (iY + 1) * BlockSize);
            TSVector2 rtBlock = new TSVector2((iX + 1) * BlockSize, (iY + 1) * BlockSize);

            TSVector2 lmBlock = new TSVector2((iX - 1) * BlockSize, iY * BlockSize);
            TSVector2 selfBlock = new TSVector2(iX * BlockSize, iY * BlockSize);
            TSVector2 rmBlock = new TSVector2((iX + 1) * BlockSize, iY * BlockSize);

            TSVector2 lbBlock = new TSVector2((iX - 1) * BlockSize, (iY - 1) * BlockSize);
            TSVector2 cbBlock = new TSVector2(iX * BlockSize, (iY - 1) * BlockSize);
            TSVector2 rbBlock = new TSVector2((iX + 1) * BlockSize, (iY - 1) * BlockSize);

            bool detectFlg = false;
            if (!detectFlg&&DictBlocks.ContainsKey(ltBlock))
                detectFlg = DictBlocks[ltBlock].ForEachIds(detectionRange);
            

            if (!detectFlg && DictBlocks.ContainsKey(ctBlock))
                detectFlg = DictBlocks[ctBlock].ForEachIds(detectionRange);

            if (!detectFlg && DictBlocks.ContainsKey(rtBlock))
                detectFlg = DictBlocks[rtBlock].ForEachIds(detectionRange);

            if (!detectFlg && DictBlocks.ContainsKey(lmBlock))
                detectFlg = DictBlocks[lmBlock].ForEachIds(detectionRange);

            if (!detectFlg && DictBlocks.ContainsKey(selfBlock))
                detectFlg = DictBlocks[selfBlock].ForEachIds(detectionRange);

            if (!detectFlg && DictBlocks.ContainsKey(rmBlock))
                detectFlg = DictBlocks[rmBlock].ForEachIds(detectionRange);

            if (!detectFlg && DictBlocks.ContainsKey(lbBlock))
                detectFlg = DictBlocks[lbBlock].ForEachIds(detectionRange);

            if (!detectFlg && DictBlocks.ContainsKey(cbBlock))
                detectFlg = DictBlocks[cbBlock].ForEachIds(detectionRange);

            if (!detectFlg && DictBlocks.ContainsKey(rbBlock))
                detectFlg = DictBlocks[rbBlock].ForEachIds(detectionRange);

            return detectFlg;
        }


    }
}
