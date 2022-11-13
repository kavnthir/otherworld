using System;
using System.Collections.Generic;
using System.IO;
using LiteNetLib;
using ProtoBuf;

namespace otherworld.world {

	[ProtoContract]
    public class WorldState {

		[ProtoMember(1)]
        public List<Entity> Entities;

        public WorldState() {
            Entities = new List<Entity>();
        }
        public WorldState(NetPacketReader reader) {
            byte[] StateEvent = new byte[reader.AvailableBytes];
            reader.GetBytes(StateEvent,reader.AvailableBytes);
            MemoryStream ms = new MemoryStream(StateEvent);
            WorldState state = Serializer.Deserialize<WorldState>(ms);
            Entities = state.Entities;
        }
        public byte[] Export() {
            MemoryStream ms = new MemoryStream();
            Serializer.Serialize(ms, this);
            return ms.ToArray();
        }
        public void Add(Entity entity) {
            Entities.Add(entity);
        }

    }
}
