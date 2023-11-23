# LittleBee

“A little bee Advanced Version” is a frame synchronization game example that synchronizes hundreds of objects and thousands of states in the game. The game background is a shooting game under a planetary system.
![preview](./social-preview.jpg)

[[Watch playing the video（Youtube）]](https://youtu.be/pD_egZciNhY)

[[Watch replaying the video （Youtube）]](https://youtu.be/jZ31cVKSKfo)

[[Watch playing the video（bilibili）]](https://www.bilibili.com/video/BV1tW4y127j9/?vd_source=3a6e61a8026ee10f9fd0a18f24c332ca)

[[Watch replaying the video（bilibili）]](https://www.bilibili.com/video/BV19g411y7jw/?vd_source=3a6e61a8026ee10f9fd0a18f24c332ca)

https://github.com/dudu502/LittleBee/assets/36216406/3d1534a9-3c1c-483f-85dd-413151163f5e

https://github.com/dudu502/LittleBee/assets/36216406/ad711712-359e-47e4-87f9-dc47d3e3ac5a

## Frame synchronization and state synchronization

* Both frame synchronization and state synchronization are methods that allow multiple clients to perform consistently at relatively the same time. Of course, it is far from enough to have only consistent appearance, or rather, it is not precise enough. At this time, it is necessary to maintain consistency at the data level and use the change of data to drive the change of external performance. This point is the same for both frame synchronization and state synchronization.
* Regarding the topic of data-driven display, here is a negative example. I remembered that several years ago, I heard a colleague talk about his experience in developing a Tetris game. He shared his insights on image collision detection between blocks during development, and used this to demonstrate the difficulty of development in this game, as well as his own cleverness.
* There is no superiority or inferiority between frame synchronization and state synchronization, both are good synchronization schemes, but they also have their own limitations. Old-school RTS games like StarCraft and Warcraft use frame synchronization. RPG games like Legend and Miracle use state synchronization. These masterpieces are all successful examples. It is meaningless to discuss which method is better than the other. Specific scenarios still depend on the demand.

### The advantages and disadvantages

* The advantages and disadvantages listed below are just general:
  
  |                             | Frame synchronization                                                                    | State synchronization                                                                                                |
  | --------------------------- | ---------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------------------------------- |
  | Consistency                 | <font color=#219167>Consistency is determined by the design</font>                       | <font color=#219167>Consistency can be ensured</font>                                                                |
  | Number of players           | <font color=#bd2158>Support for multiple players is limited</font>                       | <font color=#219167>Multiple players have an advantage</font>                                                        |
  | Cross-platform              | <font color=#bd2158>Need to consider the consistency of floating-point operations</font> | <font color=#219167>Since most of the calculations are done on the server, there is no cross-platform problem</font> |
  | Anti-cheating               | <font color=#bd2158>Easy to cheat, but can be optimized</font>                           | <font color=#219167>Can prevent cheating well</font>                                                                 |
  | Reconnection                | <font color=#bd2158>Relatively difficult to implement, but not impossible</font>         | <font color=#219167>Only need to resend the information once, easy to implement</font>                               |
  | Replay requirement          | <font color=#219167>Can be perfectly implemented</font>                                  | <font color=#bd2158>Cannot be implemented</font>                                                                     |
  | Pause game                  | <font color=#219167>Easy to implement</font>                                             | <font color=#bd2158>Difficult to implement</font>                                                                    |
  | Network transmission volume | <font color=#219167>Relatively small</font>                                              | <font color=#bd2158>Relatively large</font>                                                                          |
  | Development difficulty      | <font color=#bd2158>Relatively complex</font>                                            | <font color=#219167>Relatively simple</font>                                                                         |
  | RTS games                   | <font color=#219167>Suitable</font>                                                      | <font color=#bd2158>Not suitable</font>                                                                              |
  | Fighting games              | <font color=#219167>Suitable</font>                                                      | <font color=#bd2158>Not suitable</font>                                                                              |
  | MOBA games                  | <font color=#219167>Suitable</font>                                                      | <font color=#bd2158>Not suitable</font>                                                                              |
  | MMO games                   | <font color=#bd2158>Not suitable</font>                                                  | <font color=#219167>Suitable</font>                                                                                  |

### The difficulties that need to be overcome in frame synchronization are:

* Floating-point calculation consistency. I use the TrueSync floating-point operation library, which can ensure that floating-point numbers are calculated consistently on different devices.
* Driving the logical frame. I need different devices to maintain a consistent frame rate after a period of time after startup, which requires calibration through DateTime at each tick.
* The client’s logical frame only sends the user’s manual command to the logical frame server when the logical frame ends. After the logical frame server receives the command sent by the client, it sets the command logical frame to the current server’s logical frame number, which is to prepare for broadcasting to all clients.
* After the client receives the logical frame command sent by the server, it rolls back and recalculates the entire game world based on its own logical frame number. Therefore, the client needs to save snapshots of N frames of historical data.
* Reconnection is a relatively complex function, which requires verifying the identity of the disconnected player and the progress at the time of disconnection. This part can be implemented step by step from shallow to deep. The simpler first step is that the disconnected player needs to calculate from the historical logical frame command provided by the server to the current game progress when re-entering the game world. Therefore, it will make the player wait for a relatively long time when entering the game world, and this time will increase as the game progresses, which is not very user-friendly. The second method is that the client saves a snapshot of the game world every once in a while to a file, so that players who need to re-enter the game can load a snapshot nearby and calculate from the logical frame defined by the snapshot to the current game progress. This method can save a lot of waiting time and provide a better experience.
* Cheating is difficult to avoid in frame synchronization games because all data and operations are performed on the client. In theory, the data in the client can be arbitrarily modified, including modifying the data of other players in their own client. This causes the game to appear out of sync. Regarding the idea of anti-cheating, the method I thought of is that each client can send the checksum of the current game world snapshot to the server every N frames. After receiving it, the server judges whether it is consistent. Although this method can find cheating, it is difficult to determine which client is cheating. Only all clients can be notified of this cheating event and wait for the player to make a decision.

## Frame synchronization and ECS combination

After understanding the difficulties that need to be overcome in frame synchronization development, we need to consider choosing a good implementation method or a development framework. Since frame synchronization development requires data and presentation separation, to what extent? The data calculation part can even be placed in a separate thread. The benefit of writing logic in this way is that it can also allow the server to run to achieve the function of quickly replaying the game. I think only ECS can achieve this level of separation. Frame synchronization plus ECS is definitely a perfect match.

### ECS Explanation

First of all, let me introduce ECS. ECS is not a completely new technology, nor was it first proposed by Unity. The appearance of this term is very early, but it suddenly became popular in recent years because of Blizzard’s “Overwatch”. The server and client framework of “Overwatch” are completely built on ECS, and they have performed very well in game mechanics, network, and rendering. To be honest, ECS is not like a design pattern. The design patterns we used before were discussed under object-oriented design, and ECS is not object-oriented. Unity also has ECS. In fact, Unity’s components themselves are a kind of ECS, but they are not pure enough. ECS is especially suitable for gameplay. There are many variations of ECS, and I have made some modifications here.

* The E in ECS stands for Entity, but it is not necessary because E represents a unique object, which can be easily handled with int.
* C stands for Component. This Component is different from the Component in Unity. The Component here is used to store data. It is a type without specific methods, mainly representing attributes. Of course, if there are some simple methods such as ToString, or processing of its own data, I think it is also possible.
* S stands for System. Here, there are only methods used to modify Component attributes.
* Of course, R can also be added. R stands for Renderer. Renderer only reads interested Components and is responsible for displaying the correct behavior. E-C-S runs in threads, and R runs in the main thread, thereby maximizing performance.

## Network Communications

I recommend [RevenantX/LiteNetLib](https://github.com/RevenantX/LiteNetLib/releases). This library is powerful and easy to use. It provides reliable UDP transmission, which is exactly what I want. There are many data protocols for network communication to choose from. I use a self-made binary stream protocol here, which mainly implements serialization and deserialization. The fields inside the structure are optional. Just like this PtRoom structure:

```csharp
//Template auto generator:[AutoGenPt] v1.0
//Creation time:2021/1/28 16:43:48
using System;
using System.Collections;
using System.Collections.Generic;
namespace Net.Pt
{
public class PtRoom
{
    public byte __tag__ { get;private set;}

    public uint RoomId{ get;private set;}
    public byte Status{ get;private set;}
    public uint MapId{ get;private set;}
    public string RoomOwnerUserId{ get;private set;}
    public byte MaxPlayerCount{ get;private set;}
    public List<PtRoomPlayer> Players{ get;private set;}

    public PtRoom SetRoomId(uint value){RoomId=value; __tag__|=1; return this;}
    public PtRoom SetStatus(byte value){Status=value; __tag__|=2; return this;}
    public PtRoom SetMapId(uint value){MapId=value; __tag__|=4; return this;}
    public PtRoom SetRoomOwnerUserId(string value){RoomOwnerUserId=value; __tag__|=8; return this;}
    public PtRoom SetMaxPlayerCount(byte value){MaxPlayerCount=value; __tag__|=16; return this;}
    public PtRoom SetPlayers(List<PtRoomPlayer> value){Players=value; __tag__|=32; return this;}

    public bool HasRoomId(){return (__tag__&1)==1;}
    public bool HasStatus(){return (__tag__&2)==2;}
    public bool HasMapId(){return (__tag__&4)==4;}
    public bool HasRoomOwnerUserId(){return (__tag__&8)==8;}
    public bool HasMaxPlayerCount(){return (__tag__&16)==16;}
    public bool HasPlayers(){return (__tag__&32)==32;}

    public static byte[] Write(PtRoom data)
    {
        using(ByteBuffer buffer = new ByteBuffer())
        {
            buffer.WriteByte(data.__tag__);
            if(data.HasRoomId())buffer.WriteUInt32(data.RoomId);
            if(data.HasStatus())buffer.WriteByte(data.Status);
            if(data.HasMapId())buffer.WriteUInt32(data.MapId);
            if(data.HasRoomOwnerUserId())buffer.WriteString(data.RoomOwnerUserId);
            if(data.HasMaxPlayerCount())buffer.WriteByte(data.MaxPlayerCount);
            if(data.HasPlayers())buffer.WriteCollection(data.Players,(element)=>PtRoomPlayer.Write(element));

            return buffer.Getbuffer();
        }
    }

    public static PtRoom Read(byte[] bytes)
    {
        using(ByteBuffer buffer = new ByteBuffer(bytes))
        {
            PtRoom data = new PtRoom();
            data.__tag__ = buffer.ReadByte();
            if(data.HasRoomId())data.RoomId = buffer.ReadUInt32();
            if(data.HasStatus())data.Status = buffer.ReadByte();
            if(data.HasMapId())data.MapId = buffer.ReadUInt32();
            if(data.HasRoomOwnerUserId())data.RoomOwnerUserId = buffer.ReadString();
            if(data.HasMaxPlayerCount())data.MaxPlayerCount = buffer.ReadByte();
            if(data.HasPlayers())data.Players = buffer.ReadCollection( (rBytes)=>PtRoomPlayer.Read(rBytes) );

            return data;
        }       
    }
}
}
```

## Client

### littleBee

This is a Unity project based on frame synchronization.

## Common

Some tools: Pt structure generation tool, Excel2Json generation tool, General library project, ServerDll library project.

## Design

Design documents: outline design documents, prototype design documents, configuration tables.

## Server

This is a service collection project, including WebServer, GateServer, RoomServer, etc.

## Star History

[![Star History Chart](https://api.star-history.com/svg?repos=dudu502/LittleBee&type=Date)](https://star-history.com/#dudu502/LittleBee&Date)
