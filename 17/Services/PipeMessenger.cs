using System;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace CRMApp.Services
{
    public class PipeMessage
    {
        public string Type { get; set; } = string.Empty;   // "NewOrder", "Chat", "Notification"
        public string Sender { get; set; } = string.Empty;
        public string Payload { get; set; } = string.Empty;
        public DateTime SentAt { get; set; } = DateTime.Now;
    }

    /// <summary>
    /// Обмен данными между запущенными экземплярами CRMApp через Named Pipe.
    /// </summary>
    public class PipeMessenger : IDisposable
    {
        private const string PipeName = "CRMApp_IPC_Pipe";
        private CancellationTokenSource? _cts;
        private Task? _listenerTask;

        public event Action<PipeMessage>? MessageReceived;

        // ── Запуск сервера (слушаем входящие сообщения) ─────────────────────
        public void StartListening()
        {
            _cts = new CancellationTokenSource();
            _listenerTask = Task.Run(() => ListenLoop(_cts.Token));
        }

        private async Task ListenLoop(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                try
                {
                    using var server = new NamedPipeServerStream(
                        PipeName,
                        PipeDirection.In,
                        NamedPipeServerStream.MaxAllowedServerInstances,
                        PipeTransmissionMode.Byte,
                        PipeOptions.Asynchronous);

                    await server.WaitForConnectionAsync(ct);

                    using var reader = new StreamReader(server, Encoding.UTF8);
                    var json = await reader.ReadToEndAsync(ct);

                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        var msg = JsonSerializer.Deserialize<PipeMessage>(json);
                        if (msg != null)
                            MessageReceived?.Invoke(msg);
                    }
                }
                catch (OperationCanceledException) { break; }
                catch { /* pipe reset — continue */ }
            }
        }

        // ── Отправка сообщения другим экземплярам ───────────────────────────
        public static void Send(PipeMessage message)
        {
            try
            {
                using var client = new NamedPipeClientStream(".", PipeName, PipeDirection.Out);
                client.Connect(500); // timeout 500ms

                var json = JsonSerializer.Serialize(message);
                using var writer = new StreamWriter(client, Encoding.UTF8);
                writer.Write(json);
                writer.Flush();
            }
            catch
            {
                // Если нет получателя — тихо игнорируем
            }
        }

        public void Dispose()
        {
            _cts?.Cancel();
            _cts?.Dispose();
        }
    }
}
