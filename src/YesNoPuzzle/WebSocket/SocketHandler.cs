using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace YesNoPuzzle.WebSocket
{
    public class SocketHandler 
    {
        public const int BufferSize = 4096;

        System.Net.WebSockets.WebSocket socket;

        private SocketHandler(System.Net.WebSockets.WebSocket socket)
        {
            this.socket = socket;
        }

        private async Task EchoLoop()
        {
            var buffer = new byte[BufferSize];
            var seg = new ArraySegment<byte>(buffer);

            while (this.socket.State == WebSocketState.Open)
            {
                var incoming = await this.socket.ReceiveAsync(seg, CancellationToken.None);
                var outgoing = new ArraySegment<byte>(buffer, 0, incoming.Count);
                await this.socket.SendAsync(outgoing, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

        private static async Task Acceptor(HttpContext hc, Func<Task> n)
        {
            if (!hc.WebSockets.IsWebSocketRequest)
                return;

            var socket = await hc.WebSockets.AcceptWebSocketAsync();
            var h = new SocketHandler(socket);
            await h.EchoLoop();
        }
        public static void Map(IApplicationBuilder app)
        {
            app.UseWebSockets();
            app.Use(SocketHandler.Acceptor);
        }
    }
}
