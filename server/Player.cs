using LiteNetLib;
using LiteNetLib.Utils;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace otherworld_server {
    class Player : Entity {

        private float _playerSpeed;

        public Vector2 Position;
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
