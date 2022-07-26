using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueSync;
namespace Components.Unity
{
    public class Transform3D : AbstractComponent
    {
        private TSVector _localPosition;
        private TSVector _position;
        public override AbstractComponent Clone()
        {
            throw new NotImplementedException();
        }

        public override void CopyFrom(AbstractComponent component)
        {
            throw new NotImplementedException();
        }

        public override AbstractComponent Deserialize(byte[] bytes)
        {
            throw new NotImplementedException();
        }

        public override byte[] Serialize()
        {
            throw new NotImplementedException();
        }
    }
}
