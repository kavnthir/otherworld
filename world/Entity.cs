using ProtoBuf;

namespace otherworld.world {

    [ProtoContract]
    [ProtoInclude(1, typeof(Player))]
    public abstract class Entity {
        public abstract void Update();
    }
}
