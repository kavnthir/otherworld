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
        public Player() {
            _playerSpeed = 2.5f;
            X = 0;
            Y = 0;
            this.peerID = -1;
        }

        public Player(int peerID) {
            _playerSpeed = 2.5f;
            X = 0;
            Y = 0;
            this.peerID = peerID;
        }

        public override void Update() {

        }

        public void UpdatePosition(ClientInputState input) {
            if (input.MoveUp)
                Y -= _playerSpeed;
            if (input.MoveLeft)
                X -= _playerSpeed;
            if (input.MoveDown)
                Y += _playerSpeed;
            if (input.MoveRight)
                X += _playerSpeed;
        }
    }
}
