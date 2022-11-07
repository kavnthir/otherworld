using System;
using LiteNetLib;
using System.Threading;

namespace otherworld
{
    public static class Program
    {
        
        static void Main()
        {
            // using (var game = new Surface())
            //    game.Run();

            EventBasedNetListener listener = new EventBasedNetListener();
            NetManager client = new NetManager(listener);
            client.Start();
            client.Connect("localhost", 7777 , "SomeConnectionKey");
            listener.NetworkReceiveEvent += (fromPeer, dataReader, deliveryMethod) =>
            {
                Console.WriteLine("We got: {0}", dataReader.GetString(100 /* max length of string */));
                dataReader.Recycle();
            };

            while (!Console.KeyAvailable) {
                client.PollEvents();
                Thread.Sleep(15);
            }

            client.Stop();
        }
    }
}
