
# LittleBee
A little bee Advanced Version
![preview](./social-preview.jpg)

[[Watch playing the video（Youtube）]](https://youtu.be/pD_egZciNhY)

[[Watch replaying the video （Youtube）]](https://youtu.be/jZ31cVKSKfo)

[[Watch playing the video（bilibili）]](https://www.bilibili.com/video/BV1tW4y127j9/?vd_source=3a6e61a8026ee10f9fd0a18f24c332ca)

[[Watch replaying the video（bilibili）]](https://www.bilibili.com/video/BV19g411y7jw/?vd_source=3a6e61a8026ee10f9fd0a18f24c332ca)


## 帧同步与状态同步
* 帧同步和状态同步都是一种能让多个客户端在相对同一时刻表现一致的方法，当然只有外观表现一致还远远不够，或者说还不够精确，这时候还需要在数据层面也要保持一致，用数据的变化来驱动外在表现的变化。这点不管是帧同步或状态同步都是一样的。
* 关于数据驱动显示的话题，这里有一个反面事例，我想起了在若干年以前我曾经听一个同事说他一个俄罗斯方块游戏的开发经历，他分享了在开发过程中就方块与方块间图像碰撞检测的心得，并以此彰显开发在这个游戏的不易，以及自己的小聪明。
* 帧同步和状态同步没有孰优孰劣，都是很好的同步方案，但同时也都有自己的局限。像星际争霸，魔兽争霸等老牌RTS游戏都采用的帧同步模式。而传奇，奇迹等RPG游戏都采用的状态同步模式，这些大作都是成功的典范。讨论哪一种方式碾压另一种没有意义，具体还是要看需求场景。

### 优缺点
* 以下列举的优缺点只是一般情况下：

|    		| 帧同步  | 状态同步  |
|  ----  | ----  |----  |
| 一致性  | <font color=#219167>设计层面决定了必然一致</font>|<font color=#219167>可以保证一致性</font> |
| 玩家数 | <font color=#bd2158>对多玩家支持有限</font> |<font color=#219167>多玩家有优势</font> |
|跨平台|<font color=#bd2158>需要考虑浮点运算的一致性</font> |<font color=#219167>由于主要的计算都在服务器，因此不存在跨平台问题</font> |
|防作弊|<font color=#bd2158>容易作弊，但是可以优化</font> |<font color=#219167>可以很好的防作弊 </font> |
|断线重连|<font color=#bd2158>实现起来比较难，但不是不能</font> |<font color=#219167>只需要重新发送一次信息即可，好实现</font> |
|回放需求|<font color=#219167>能完美实现</font> |<font color=#bd2158>无法实现</font> |
|暂停游戏|<font color=#219167>好实现</font> |<font color=#bd2158>不好实现</font> |
|网络传输量|<font color=#219167>比较小</font> |<font color=#bd2158>比较大</font> |
|开发难度|<font color=#bd2158>相对复杂</font> |<font color=#219167>相对简单</font> |
|RTS类游戏|<font color=#219167>适合</font> |<font color=#bd2158>不适合</font> |
|格斗类游戏|<font color=#219167>适合</font> |<font color=#bd2158>不适合</font> |
|MOBA类游戏|<font color=#219167>适合</font> |<font color=#bd2158>不适合</font> |
|MMO类游戏|<font color=#bd2158>不适合</font> |<font color=#219167>适合</font> |

### 帧同步需要克服的难点
* 浮点计算一致性，我使用的是TrueSync浮点运算库。可以保证浮点数在不同设备上运算一致。
* 逻辑帧的驱动，我需要不同设备在启动后经过一段时间后所有的设备都能保持一致的帧数，这就需要在每次tick的时候通过DateTime校准。
* 客户端的逻辑帧只在逻辑帧结束时发送用户的手动命令给逻辑帧服务器，逻辑帧服务器在收到客户端发来的命令后将命令逻辑帧设置为当前服务器的逻辑帧编号，这是为了广播给所有客户端做准备。
* 客户端收到服务器发来的逻辑帧命令后，根据客户端自身的逻辑帧编号进行回滚并重新计算整个游戏世界，因此客户端需要对过往的N帧历史数据进行快照保存。
* 断线重连是一个相对复杂的功能，这需要验证断线玩家的身份以及断线时刻的进度。这部分可以分两步由浅入深实现，简单一点的第一步是断线的玩家重新进入游戏世界需要根据服务器提供的历史逻辑帧命令从头计算至当前游戏进度，因此会让玩家在进入游戏世界的时候等待比较长的时间，这个时间会随着游戏进度的深入而增加，体验不够人性化。第二种方法就是客户端每隔一段时间保存游戏世界快照到文件，让需要重新进入游戏的玩家就近载入一段快照并且从快照定义的逻辑帧往后计算至当前游戏进度，这个方法可以节省很多等待时间，体验也更好。
* 作弊问题在帧同步游戏中比较难避免，因为所有的数据以及运算都在客户端进行，理论上客户端里的数据可以被任意修改，甚至包括修改自己客户端里其他玩家的数据。导致游戏出现不同步现象。关于防作弊的思路，我想到的方法是，每个客户端可以每隔N帧发送给服务器一次当前游戏世界快照的校验码，服务器收到后判断是否一致性，这个方法虽然可以发现作弊现象但是具体是那个客户端作弊就不好判断了，只能通知所有客户端这件作弊事件，等待玩家做决定。

## 帧同步与ECS的结合
了解了帧同步开发过程中需要克服的难点，我们接下来就要考虑选用一种比较好的实现方式，或者说是一种开发框架。由于帧同步开发非常需要数据和表现分离，分离到什么程度呢？就是数据计算部分甚至可以放在一个单独的线程里。这样编写逻辑的好处还可以让服务器运行以达到快速复盘游戏的功能，能做到这种程度的分离我想只有ECS了。帧同步加上ECS绝对是完美搭档。
### ECS说明
首先要介绍一下ECS，ECS并非一种全新的技术，也不是Unity首先提出来的。这种名词的出现非常早，而近几年突然火爆，是因为暴雪的《守望先锋》。《守望先锋》的服务器和客户端框架完全基于ECS构建，在游戏机制、网络、渲染方面都有非常出色的表现。坦白地说ECS不像一种设计模式，我们以前用的设计模式都是在面向对象设计下谈论的，ECS都不是面向对象。Unity也有ECS，其实Unity本身的组件也是一种ECS，只不过还不够纯粹。ECS特别适合做Gameplay。关于ECS的变种也有很多，我这里也是稍微做了一些修改的ECS。
* ECS中的E代表Entity，不过也可以不需要，因为E表示的是一个唯一物体，完全可以用int来搞定。
* C代表Component，这个Component和Unity里的Component不一样，这里的Component用来存储数据，这是一个没有具体方法的类型，主要表示属性，当然如果有一些简单的方法如ToString ，或者对自身数据的处理我想也可以。
* S代表的是System，在这里只有方法，用于修改Component属性。
* 当然还可以加上R，R代表的是Renderer，Renderer只读取感兴趣的Component并负责显示正确的行为。E-C-S这三部分在线程里运行，R这部分在主线程运行，如此最大限度的提升性能。

## 网络通信

这里推荐[RevenantX/LiteNetLib](https://github.com/RevenantX/LiteNetLib)，这个库很强大并且用法很简洁，它提供了可靠UDP传输，这正是我想要的。
网络通信的数据协议可以选择的有很多，我这里使用的是自制二进制流协议，主要实现的功能是序列化与反序列化，结构体内的字段支持可选。
就像这个PtRoom结构：
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
这是一个基于帧同步的Unity工程

## Common
一些工具：Pt结构体生成工具，Excel2Json生成工具，General库项目，ServerDll库项目
## Design
设计文档：大纲设计文档，原型设计文档，配置表
## Server
这是一个服务集合项目，包括WebServer，GateServer，RoomServer等


## Star History

[![Star History Chart](https://api.star-history.com/svg?repos=dudu502/LittleBee&type=Timeline)](https://star-history.com/#dudu502/LittleBee&Timeline)
