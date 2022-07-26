using Evt;
using Net;
using Net.ServiceImpl;
using Service.Core;
using Service.Event;
using System;
using System.Collections.Generic;


namespace ServerDll.Service.Modules
{
    public class DebugModule : BaseModule
    {
        public DebugModule(BaseApplication app) : base(app)
        {
            EventMgr<RequestMessageId, UnconnectedNetMessageEvt>.AddListener(RequestMessageId.US_DebugQuery, OnDebugQuery);
        }
        void OnDebugQuery(UnconnectedNetMessageEvt evt)
        {
            using (ByteBuffer buffer = new ByteBuffer(evt.Content))
            {
                string queryStr = buffer.ReadString();
                BaseApplication.Logger.Log($"OnDebug Raw-Query-Information [{queryStr}] from [{evt.RemoteEndPoint.ToString()}]");
                try
                {
                    Dictionary<string, string> keyValuePairs = LitJson.JsonMapper.ToObject<Dictionary<string, string>>(queryStr);
                    OnDebugQueryImpl(keyValuePairs, evt);
                }
                catch(Exception ex)
                {
                    BaseApplication.Logger.LogWarning($"OnDebug Raw-Query Format Error."+ex.ToString());
                }                
            }
        }

        protected void ReplyDebugResult(Dictionary<string,string> result, UnconnectedNetMessageEvt evt)
        {
            string jsonResult = LitJson.JsonMapper.ToJson(result);
            PtMessagePackage ptMessagePackage = PtMessagePackage.Build((ushort)ResponseMessageId.US_DebugQuery, new ByteBuffer().WriteString(jsonResult).Getbuffer());
            evt.Reply(GetNetManager(),PtMessagePackage.Write(ptMessagePackage));
        }
        protected virtual void OnDebugQueryImpl(Dictionary<string, string> keyValuePairs, UnconnectedNetMessageEvt evt)
        {
            
        }
    }
}
