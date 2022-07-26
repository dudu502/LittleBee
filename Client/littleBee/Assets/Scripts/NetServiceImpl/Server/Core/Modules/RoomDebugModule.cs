using ServerDll.Service.Modules;
using Service.Core;
using Service.Event;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoomServer.Modules
{
    public class RoomDebugModule:DebugModule
    {
        public RoomDebugModule(BaseApplication app) : base(app)
        {

        }
        protected override void OnDebugQueryImpl(Dictionary<string, string> keyValuePairs, UnconnectedNetMessageEvt evt)
        {
            base.OnDebugQueryImpl(keyValuePairs, evt);
            if (keyValuePairs.TryGetValue("type", out string type))
            {
                switch (type)
                {
                    case "InitEntityId":
                        keyValuePairs.Add("result", ServerDll.Service.Modules.Service.GetModule<BattleModule>().Session.InitEntityId.ToString());
                        break;
                    default:
                        keyValuePairs.Add("error", "undefine query_type:" + type);
                        break;
                }
                ReplyDebugResult(keyValuePairs, evt);
            }
            else
            {
                keyValuePairs.Add("error", "undefine type field in request message");
                ReplyDebugResult(keyValuePairs, evt);
            }
        }
    }
}
