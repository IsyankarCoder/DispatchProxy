using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

internal class Program
{    private static async Task Main(string[] args)
    {
        var allClients = ClientProxy<IChatClient>.CreateDispatch((method, args) =>
        {

             Console.WriteLine($"İstemcilere gönderiliyor : {method}({args[0]})");
        });


        var hub =new ChatHub(new HubClients<IChatClient>(allClients));
        await hub.SendMessage(args[0]);
        
    }



    public interface IChatClient
    {
        Task VolkiTolkiMethod(string message);
        Task Budadegisikmethod(int a);
    }

    public class ClientProxy<T>:DispatchProxy
                        where T : class
    {
        private Action<string,object?[]> _send=null;

        public static T CreateDispatch(Action<string, object?[]> send)
        {
           var client = Create<T, ClientProxy<T>>();
                 ((ClientProxy<T>)(object)client)._send = send;
          return client;
        }

        protected override object? Invoke(MethodInfo? targetMethod,
                                          object?[]? args)
        {
            if(targetMethod is null)
             throw new InvalidOperationException("Çağrilan metod bulunamadi");

             _send(targetMethod.Name,args??[]);
             return Task.CompletedTask;
        }
    }


    public sealed class HubClients<T>
                        where T: class
    {
        public T All {get;}
        public HubClients(T all) => All=all;
    }


    public sealed class ChatHub
    {
        private readonly HubClients<IChatClient> _clients;
        public ChatHub(HubClients<IChatClient> clients)=>_clients=clients;

        public Task SendMessage(string message)=>
         _clients.All.Budadegisikmethod(10);
    }
}


