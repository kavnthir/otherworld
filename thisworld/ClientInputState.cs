using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using LiteNetLib;
using ProtoBuf;

namespace thisworld {

    [ProtoContract]
    public class ClientInputState {

        [ProtoMember(1)]
        public bool MoveUp;
        [ProtoMember(2)]
        public bool MoveLeft;
        [ProtoMember(3)]
        public bool MoveRight;
        [ProtoMember(4)]
        public bool MoveDown;

        public ClientInputState() {
            MoveUp = false;
            MoveLeft = false;
            MoveRight = false;
            MoveDown = false;
        }

        public ClientInputState(NetPacketReader reader) {
            byte[] InputEvent = new byte[reader.AvailableBytes];
            reader.GetBytes(InputEvent,reader.AvailableBytes);
            MemoryStream ms = new MemoryStream(InputEvent);
            ClientInputState input = Serializer.Deserialize<ClientInputState>(ms);
            MoveUp = input.MoveUp;
            MoveLeft = input.MoveLeft;
            MoveRight = input.MoveRight;
            MoveDown = input.MoveDown;
        }

        public MemoryStream Export() {
            MemoryStream ms = new MemoryStream();
            Serializer.Serialize(ms, this);
            return ms;
        }

        public static bool operator ==(ClientInputState lhs, ClientInputState rhs) {
            bool ret = true;
            ret &= lhs.MoveUp == rhs.MoveUp;
            ret &= lhs.MoveLeft == rhs.MoveLeft;
            ret &= lhs.MoveRight == rhs.MoveRight;
            ret &= lhs.MoveDown == rhs.MoveDown;
            return ret;
        }
        public static bool operator !=(ClientInputState lhs, ClientInputState rhs) {
            return !(lhs == rhs);
        }
    }
}
