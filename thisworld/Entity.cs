using ProtoBuf;

namespace thisworld {

    [ProtoContract]
    [ProtoInclude(1, typeof(Player))]
    public abstract class Entity {
        public abstract void Update();
    }
}
