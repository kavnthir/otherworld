﻿using System;
using LiteNetLib;
using System.Threading;

namespace otherworld_server {
    public static class Program {
        static void Main() {

            Server server = new Server(9050, 10);
            server.Start();

        }
    }
}