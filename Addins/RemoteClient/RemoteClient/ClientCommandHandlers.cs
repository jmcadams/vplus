namespace RemoteClient
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using VixenPlus;

    internal static class ClientCommandHandlers
    {
        public static void ChannelCount(Socket socket, ClientContext clientContext)
        {
            socket.Send(new byte[] { 0x13 });
            Sockets.SendSocketInt32(socket, clientContext.ChannelCount);
            socket.Close();
        }

        public static void ChannelOff(Socket socket, ClientContext clientContext, int channelIndex)
        {
            if (clientContext.ExecutionInterface.ExecuteChannelOff(clientContext.ExecutionContextHandle, channelIndex))
            {
                socket.Send(new byte[] { 0x13 });
            }
            else
            {
                socket.Send(new byte[] { 20 });
            }
            socket.Close();
        }

        public static void ChannelOn(Socket socket, ClientContext clientContext, int channelIndex)
        {
            if (clientContext.ExecutionInterface.ExecuteChannelOn(clientContext.ExecutionContextHandle, channelIndex))
            {
                socket.Send(new byte[] { 0x13 });
            }
            else
            {
                socket.Send(new byte[] { 20 });
            }
            socket.Close();
        }

        public static void ClientName(Socket socket)
        {
            object obj2;
            if (Interfaces.Available.TryGetValue("ISystem", out obj2))
            {
                socket.Send(new byte[] { 0x13 });
                Sockets.SendSocketString(socket, ((ISystem) obj2).UserPreferences.GetString("ClientName"));
            }
            else
            {
                socket.Send(new byte[] { 20 });
            }
            socket.Close();
        }

        public static void CommitSequence(Socket socket, string fileName)
        {
            int num;
            try
            {
                num = Sockets.GetSocketInt32(socket);
            }
            catch
            {
                num = 0;
            }
            byte[] socketBytes = Sockets.GetSocketBytes(socket, num);
            string path = Path.Combine(Paths.SequencePath, fileName);
            try
            {
                System.IO.File.WriteAllBytes(path, socketBytes);
                socket.Send(new byte[] { 0x13 });
            }
            catch
            {
                socket.Send(new byte[] { 20 });
            }
            socket.Close();
        }

        public static void CurrentPosition(Socket socket, ClientContext clientContext)
        {
            socket.Send(new byte[] { 0x13 });
            Sockets.SendSocketInt32(socket, clientContext.ExecutionInterface.GetCurrentPosition(clientContext.ExecutionContextHandle));
            socket.Close();
        }

        public static void DownloadSequence(Socket socket, string fileName)
        {
            string path = Path.Combine(Paths.SequencePath, fileName);
            if (System.IO.File.Exists(path))
            {
                socket.Send(new byte[] { 0x13 });
                byte[] buffer = System.IO.File.ReadAllBytes(path);
                Sockets.SendSocketInt32(socket, buffer.Length);
                socket.Send(buffer);
            }
            else
            {
                socket.Send(new byte[] { 20 });
            }
            socket.Close();
        }

        private static string EnsureUniqueSequenceFileName(string fileName)
        {
            string str = string.Empty;
            int num = 2;
            fileName = Path.Combine(Paths.SequencePath, Path.GetFileName(fileName));
            while (System.IO.File.Exists(fileName + str))
            {
                str = " - " + num.ToString();
                num++;
            }
            return (fileName + str);
        }

        public static void Execute(Socket socket, ClientContext clientContext, int millisecondStart, int millsecondCount)
        {
            if (clientContext.ExecutionInterface.ExecutePlay(clientContext.ExecutionContextHandle, millisecondStart, millsecondCount))
            {
                socket.Send(new byte[] { 0x13 });
            }
            else
            {
                socket.Send(new byte[] { 20 });
            }
            socket.Close();
        }

        public static void ListLocal(Socket socket, ObjectType objectType, string fileSpec)
        {
            string path = null;
            string searchPattern = null;
            byte[] buffer = new byte[1];
            searchPattern = fileSpec;
            switch (objectType)
            {
                case ObjectType.Sequence:
                    path = Paths.SequencePath;
                    break;

                case ObjectType.Program:
                    path = Paths.ProgramPath;
                    break;

                default:
                    buffer[0] = 20;
                    socket.Send(buffer);
                    socket.Close();
                    return;
            }
            buffer[0] = 0x13;
            socket.Send(buffer);
            string[] files = Directory.GetFiles(path, searchPattern);
            buffer[0] = (byte) files.Length;
            socket.Send(buffer);
            foreach (string str3 in files)
            {
                Sockets.SendSocketString(socket, Path.GetFileName(str3));
            }
            socket.Close();
        }

        public static void Pause(Socket socket, ClientContext clientContext)
        {
            if (clientContext.ExecutionInterface.ExecutePause(clientContext.ExecutionContextHandle))
            {
                socket.Send(new byte[] { 0x13 });
            }
            else
            {
                socket.Send(new byte[] { 20 });
            }
            socket.Close();
        }

        public static void RetrieveLocal(Socket socket, ClientContext clientContext, ObjectType objectType, string objectFileName)
        {
            byte num = 20;
            try
            {
                string str;
                if (objectType == ObjectType.Program)
                {
                    str = Path.Combine(Paths.ProgramPath, objectFileName);
                    if (System.IO.File.Exists(str))
                    {
                        clientContext.ContextObject = new SequenceProgram(str);
                        num = 0x13;
                    }
                }
                else
                {
                    str = Path.Combine(Paths.SequencePath, objectFileName);
                    if (System.IO.File.Exists(str))
                    {
                        clientContext.ContextObject = new EventSequence(str);
                        num = 0x13;
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorLog.Log(exception.Message);
            }
            socket.Send(new byte[] { num });
            socket.Close();
        }

        public static void RetrieveRemote(Socket socket, IPAddress serverAddress, ClientContext clientContext, ObjectType objectType, string objectFileName)
        {
            if (objectType == ObjectType.Program)
            {
                clientContext.ContextObject = RetrieveRemoteProgram(objectFileName, serverAddress);
            }
            else
            {
                clientContext.ContextObject = RetrieveRemoteSequence(objectFileName, serverAddress);
            }
            socket.Send(new byte[] { (clientContext.ContextObject == null) ? ((byte) 20) : ((byte) 0x13) });
        }

        private static byte[] RetrieveRemoteObject(byte type, string name, Socket socket)
        {
            byte[] array = new byte[3 + name.Length];
            array[0] = 22;
            array[1] = type;
            array[2] = (byte) name.Length;
            Encoding.ASCII.GetBytes(name).CopyTo(array, 3);
            socket.Send(array);
            if (Sockets.GetSocketByte(socket) == 0x13)
            {
                int num2;
                int num = Sockets.GetSocketInt32(socket);
                byte[] buffer = new byte[num];
                int offset = 0;
                do
                {
                    num2 = socket.Receive(buffer, offset, num - offset, SocketFlags.None);
                    offset += num2;
                }
                while ((offset < num) && (num2 > 0));
                return buffer;
            }
            return null;
        }

        public static SequenceProgram RetrieveRemoteProgram(string fileName, IPAddress serverAddress)
        {
            TcpClient client = Sockets.ConnectTo(serverAddress, 0xa1b9);
            client.LingerState = new LingerOption(true, 5);
            byte[] buffer = null;
            try
            {
                buffer = RetrieveRemoteObject(1, fileName, client.Client);
                if (buffer == null)
                {
                    return null;
                }
                fileName = Path.Combine(Path.GetTempPath(), fileName);
                FileStream stream = new FileStream(fileName, FileMode.Create);
                stream.Write(buffer, 0, buffer.Length);
                stream.Flush();
                stream.Flush();
                stream.Close();
                stream.Dispose();
                SequenceProgram program = new SequenceProgram(fileName);
                System.IO.File.Delete(fileName);
                program.ClearSequences();
                int socketByte = Sockets.GetSocketByte(client.Client);
                while (socketByte-- > 0)
                {
                    string socketString = Sockets.GetSocketString(client.Client);
                    byte[] socketBytes = Sockets.GetSocketBytes(client.Client, Sockets.GetSocketInt32(client.Client));
                    string tempFileName = Path.GetTempFileName();
                    System.IO.File.WriteAllBytes(tempFileName, socketBytes);
                    EventSequence sequence = new EventSequence(tempFileName);
                    System.IO.File.Delete(tempFileName);
                    sequence.Name = socketString;
                    program.AddSequence(sequence);
                }
                return program;
            }
            catch
            {
            }
            finally
            {
                if (client.Connected)
                {
                    client.Close();
                }
            }
            return null;
        }

        public static EventSequence RetrieveRemoteSequence(string fileName, IPAddress serverAddress)
        {
            TcpClient client = Sockets.ConnectTo(serverAddress, 0xa1b9);
            client.LingerState = new LingerOption(true, 5);
            byte[] buffer = null;
            try
            {
                buffer = RetrieveRemoteObject(0, fileName, client.Client);
            }
            catch
            {
            }
            finally
            {
                if (client.Connected)
                {
                    client.Close();
                }
            }
            if (buffer == null)
            {
                return null;
            }
            fileName = Path.Combine(Path.GetTempPath(), fileName);
            FileStream stream = new FileStream(fileName, FileMode.Create);
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();
            stream.Flush();
            stream.Close();
            stream.Dispose();
            EventSequence sequence = new EventSequence(fileName);
            System.IO.File.Delete(fileName);
            return sequence;
        }

        public static void Stop(Socket socket, ClientContext clientContext)
        {
            if (clientContext.ExecutionInterface.ExecuteStop(clientContext.ExecutionContextHandle))
            {
                socket.Send(new byte[] { 0x13 });
            }
            else
            {
                socket.Send(new byte[] { 20 });
            }
            socket.Close();
        }

        public static void UpdateEventValues(Socket socket, ClientContext clientContext)
        {
            if (clientContext.ContextObject is EventSequence)
            {
                try
                {
                    int num3;
                    EventSequence contextObject = (EventSequence) clientContext.ContextObject;
                    int length = contextObject.EventValues.GetLength(0);
                    int num2 = contextObject.EventValues.GetLength(1);
                    try
                    {
                        num3 = Sockets.GetSocketInt32(socket);
                    }
                    catch
                    {
                        num3 = 0;
                    }
                    byte[] socketBytes = Sockets.GetSocketBytes(socket, num3);
                    if (num3 != (length * num2))
                    {
                        socket.Send(new byte[] { 20 });
                        ErrorLog.Log(string.Format("Array size mismatch: {0}x{1} vs {2}", length, num2, num3));
                        socket.Close();
                        return;
                    }
                    byte[,] buffer2 = new byte[length, num2];
                    int num5 = 0;
                    for (int i = 0; i < length; i++)
                    {
                        for (int j = 0; j < num2; j++)
                        {
                            buffer2[i, j] = socketBytes[num5++];
                        }
                    }
                    contextObject.EventValues = buffer2;
                    socket.Send(new byte[] { 0x13 });
                }
                catch (Exception exception)
                {
                    socket.Send(new byte[] { 20 });
                    ErrorLog.Log(exception.Message);
                }
            }
            else
            {
                socket.Send(new byte[] { 20 });
            }
            socket.Close();
        }
    }
}

