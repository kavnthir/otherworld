using System;
using System.Collections.Generic;

namespace thisworld {
    public class WorldState {

        private List<Entity> _entities;

        public WorldState() {
            _entities = new List<Entity>();
        }

        public string Export() {
            string state = "";

            foreach(var entity in _entities)
                state += entity.Export();

            return state;
        }
    }
}
