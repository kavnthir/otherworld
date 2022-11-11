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
        public float X;

		[ProtoMember(3)]
        public float Y;

		[ProtoMember(4)]
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
            X = 0;
            Y = 0;
            this.peerID = peerID;
        }

        public override void Update() {

        }

        public void UpdateClient(NetManager server) {
            NetPeer peer = server.GetPeerById(peerID);

            NetDataWriter writer = new NetDataWriter();
            writer.Put(new Vector2(X,Y).ToString());
            peer.Send(writer, DeliveryMethod.Unreliable);         
        }

        public void UpdatePosition(inputType input) {
            if(input == inputType.Up) {
                Y -= _playerSpeed;
            }
            if (input == inputType.Left) {
                X -= _playerSpeed;
            }
            if(input == inputType.Down) {
                Y += _playerSpeed;
            }
            if (input == inputType.Right) {
                X += _playerSpeed;
            }
        }
    }
}
