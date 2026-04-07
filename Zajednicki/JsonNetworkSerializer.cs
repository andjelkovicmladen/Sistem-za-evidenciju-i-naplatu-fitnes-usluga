using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Zajednicki
{
    public class JsonNetworkSerializer
    {
        private readonly Socket socket;

        public JsonNetworkSerializer(Socket socket)
        {
            this.socket = socket;
        }

        public void Send<T>(T obj)
        {
            string json = JsonSerializer.Serialize(obj);
            byte[] data = Encoding.UTF8.GetBytes(json);
            byte[] header = BitConverter.GetBytes(data.Length);
            socket.Send(header);
            socket.Send(data);
        }

        public T Recieve<T>()
        {
            byte[] header = new byte[4];
            ReceiveAll(header);
            int length = BitConverter.ToInt32(header, 0);
            byte[] data = new byte[length];
            ReceiveAll(data);
            string json = Encoding.UTF8.GetString(data);
            return JsonSerializer.Deserialize<T>(json);
        }

        private void ReceiveAll(byte[] buffer)
        {
            int offset = 0;
            while (offset < buffer.Length)
            {
                int received = socket.Receive(buffer, offset, buffer.Length - offset, SocketFlags.None);
                if (received == 0) throw new Exception("Veza prekinuta");
                offset += received;
            }
        }
    }
}
