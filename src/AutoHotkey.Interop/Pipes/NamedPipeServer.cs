using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoHotkey.Interop.Pipes
{
    internal abstract class NamedPipeServer
    {
        private readonly string pipeName;
        private volatile NamedPipeServerStream serverStream;

        public bool IsClientConnected {
            get {
                if (serverStream == null)
                    return false;
                else
                    return serverStream.IsConnected;
            }
        }

        public NamedPipeServer(string pipeName) {
            this.pipeName = pipeName;
        }

        public void Start() {
            serverStream = MakeNamedPipeServerStream(pipeName);
            serverStream.BeginWaitForConnection(DoConnectionLoop, null);
        }

        private async void DoConnectionLoop(IAsyncResult result) {
            if (!result.IsCompleted) return;
            if (serverStream == null) return;

            //IOException = pipe is broken
            //ObjectDisposedException = cannot access closed pipe
            //OperationCanceledException - read was canceled

            //ACCEPT CLIENT CONNECTION
            try {
                serverStream.EndWaitForConnection(result);
            }
            catch (IOException) { RebuildNamedPipe(); return; }
            catch (ObjectDisposedException) { RebuildNamedPipe(); return; }
            catch (OperationCanceledException) { RebuildNamedPipe(); return; }

            while (IsClientConnected) {
                string clientMessage = null;
                string serverResponce = null;

                // READ FROM CLIENT
                if (serverStream == null) break;
                try {
                    clientMessage = await ReadClientMessage(serverStream);
                }
                catch (IOException) { RebuildNamedPipe(); return; }
                catch (ObjectDisposedException) { RebuildNamedPipe(); return; }
                catch (OperationCanceledException) { RebuildNamedPipe(); return; }

                //PROCESS CLIENT MESSAGE
                serverResponce = HandleClientMessage(clientMessage);



                //SEND RESPONCE BACK TO CLIENT
                if (serverStream == null) break;
                try {
                    await SendResponceToClient(serverStream, serverResponce);
                }
                catch (IOException) { RebuildNamedPipe(); return; }
                catch (ObjectDisposedException) { RebuildNamedPipe(); return; }
                catch (OperationCanceledException) { RebuildNamedPipe(); return; }
            }

            //client disconnected, relisten
            if (serverStream != null)
                serverStream.BeginWaitForConnection(DoConnectionLoop, null);
        }

        private void RebuildNamedPipe() {
            this.Shutdown();
            serverStream = MakeNamedPipeServerStream(pipeName);
            serverStream.BeginWaitForConnection(DoConnectionLoop, null);
        }

        protected abstract string HandleClientMessage(string clientMessage);

        private static async Task SendResponceToClient(NamedPipeServerStream stream, string serverResponce) {
            byte[] responceData = Encoding.Unicode.GetBytes(serverResponce);
            await stream.WriteAsync(responceData, 0, responceData.Length);
            await stream.FlushAsync();
            stream.WaitForPipeDrain();
        }

        private static async Task<string> ReadClientMessage(NamedPipeServerStream stream) {
            byte[] buffer = new byte[65535];
            int read = await stream.ReadAsync(buffer, 0, buffer.Length);
            string clientString = Encoding.Unicode.GetString(buffer, 0, read);
            return clientString;
        }

        public void Shutdown() {
            if (serverStream != null) {
                try { serverStream.Close(); } catch { }
                try { serverStream.Dispose(); } catch { }
                serverStream = null;
            }
        }


        private NamedPipeServerStream MakeNamedPipeServerStream(string pipeName) {
            return new System.IO.Pipes.NamedPipeServerStream(pipeName,
                            System.IO.Pipes.PipeDirection.InOut,
                            System.IO.Pipes.NamedPipeServerStream.MaxAllowedServerInstances,
                            System.IO.Pipes.PipeTransmissionMode.Byte,
                            System.IO.Pipes.PipeOptions.Asynchronous);
        }
    }
}
