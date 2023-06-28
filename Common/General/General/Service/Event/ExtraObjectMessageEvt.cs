using System;
using System.Collections.Generic;
using System.Text;

namespace Synchronize.Game.Lockstep.Service.Event
{
    public class ExtraObjectMessageEvt
    {
        public object ExtraObj { private set; get; }
        public byte[] Content { private set; get; }
        public ExtraObjectMessageEvt(object obj, byte[] content)
        {
            ExtraObj = obj;
            Content = content;
        }
    }
}
