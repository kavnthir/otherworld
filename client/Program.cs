using System;
using LiteNetLib;
using System.Threading;

namespace otherworld {
    public static class Program {
        static void Main() {
            // using (var game = new Surface())
            //     game.Run();

            Client client = new Client("localhost", 9050, "pass");
            client.Start();

        }
    }
}
