using ServerDll.Service.Modules;
using Service.Core;
using Service.Event;
using System;
using System.Collections.Generic;
using System.Text;

namespace GateServer.Core.Modules
{
    public class GateDebugModule: DebugModule
    {
        public GateDebugModule(BaseApplication app) : base(app) { }

        protected override void OnDebugQueryImpl(Dictionary<string, string> keyValuePairs, UnconnectedNetMessageEvt evt)
        {
            base.OnDebugQueryImpl(keyValuePairs, evt);
            if(keyValuePairs.TryGetValue("type",out string type))
            {
                switch(type)
                {
                    case "users_count":
                        keyValuePairs.Add("result", "232323233");
                        break;
                    default:
                        keyValuePairs.Add("error","undefine query_type:"+type);
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
