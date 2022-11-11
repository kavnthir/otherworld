using System;
using System.Collections.Generic;
using System.IO;
using ProtoBuf;

namespace thisworld {

	[ProtoContract]
    public class WorldState {

		[ProtoMember(1)]
        public List<Entity> Entities;

        public WorldState() {
            Entities = new List<Entity>();
        }

        public void Add(Entity entity) {
            Entities.Add(entity);
        }

        public byte[] Export() {
            MemoryStream ms = new MemoryStream();
            Serializer.Serialize(ms, this);
            return ms.ToArray();
        }
    }
}
