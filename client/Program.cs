using System;
using LiteNetLib;
using System.Threading;

namespace otherworld {
    public static class Program {
        static void Main() {
            using (var game = new Otherworld())
                game.Run();
        }
    }
}
