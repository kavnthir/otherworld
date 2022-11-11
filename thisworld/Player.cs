using LiteNetLib;
using LiteNetLib.Utils;
using Microsoft.Xna.Framework;
using ProtoBuf;

namespace thisworld {

	[ProtoContract]
    public class Player : Entity {

		[ProtoMember(1)]
        private float _playerSpeed;

		[ProtoMember(2)]
        public Vector2 Position;

		[ProtoMember(3)]
        public int peerID;

        public enum inputType { 
            None,
            Up,
            Left,
            Right,
            Down,
        }

        public Player(int peerID) {
            _playerSpeed = 2.5f;
            Position = new Vector2(0, 0);
            this.peerID = peerID;
        }

        public override void Update() {

        }

        public void UpdateClient(NetManager server) {
            NetPeer peer = server.GetPeerById(peerID);

            NetDataWriter writer = new NetDataWriter();
            writer.Put(Position.ToString());
            peer.Send(writer, DeliveryMethod.Unreliable);         
        }

        public void UpdatePosition(inputType input) {
            if(input == inputType.Up) {
                Position.Y -= _playerSpeed;
            }
            if (input == inputType.Left) {
                Position.X -= _playerSpeed;
            }
            if(input == inputType.Down) {
                Position.Y += _playerSpeed;
            }
            if (input == inputType.Right) {
                Position.X += _playerSpeed;
            }
        }
    }
}
