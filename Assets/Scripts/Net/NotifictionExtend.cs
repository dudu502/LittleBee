using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net
{
    public static class NotifictionExtend
    {
        public static byte[] GetBytes(this Notify.Notification note)
        {
            return note.Params[0] as byte[];
        }

        public static Message GetMessage(this Notify.Notification note)
        {
            return note.Params[1] as Message;
        }
    }
}
