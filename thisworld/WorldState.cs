using System;
using System.Collections.Generic;
using ProtoBuf;

namespace thisworld {
	[ProtoContract]
    public class WorldState {

		[ProtoMember(1)]
        private List<Entity> _entities;

        public WorldState() {
            _entities = new List<Entity>();
        }

        public void Add(Entity entity) {
            _entities.Add(entity);
        }

        public string Export() {
            string state = "";

            foreach(var entity in _entities)
                state += entity.Export();

            return state;
        }
    }
}
