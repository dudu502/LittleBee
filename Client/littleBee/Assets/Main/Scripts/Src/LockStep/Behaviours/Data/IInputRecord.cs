using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synchronize.Game.Lockstep.Behaviours.Data
{
    public interface IInputRecord
    {
        
        uint EntityId { set; get; }
        /// <summary>
        /// FrameCommand
        /// </summary>
        /// <returns></returns>
        int GetRecordType();

        bool IsDirty(IInputRecord record);

        void CopyFrom(IInputRecord record);

    }
}
