using System;
using System.Collections.Generic;
using System.Text;

namespace Synchronize.Game.Lockstep.Misc
{
    public enum EntityType : byte
    {
        Star = 1,
        UFO = 2,
        Player = 3,
        Item = 4,
        Asteroid = 5,
        Bullet = 6,
        BackgroudCamera = 7,
        BrokenPiece = 8,
    }

    public enum UserState:byte
    {
        None=0,
        EnteredRoom=1,
        BeReadyToEnterScene=2,

        Re_EnteredRoom =3,
        Re_BeReadyToEnterScene=4,
    }

    public enum RoomPlayerType:byte
    {
        Empty = 0,
        Player = 1,
        EasyComputer = 2,
        CrazyComputer = 3,
        Closed = 4,
    }

}
