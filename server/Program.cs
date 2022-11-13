
namespace otherworld.server {
    public static class Program {
        static void Main() {
            Server server = new Server(9050, 10);
            server.Start();

            while(true) {
                server.Update();
            }
        }
    }
}
