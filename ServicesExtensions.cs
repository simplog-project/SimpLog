using SimpleLog.Services.FileServices;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.IO;

namespace SimpleLog
{
    public static class ServicesExtensions
    {
        /// <summary>
        /// SimpLog is a library for log files.
        /// </summary>
        public static async Task SimpLog()
        {
            Thread backhroundThread = new Thread(BufferSave) { IsBackground = true };
            backhroundThread.Start();
        }

        static async void BufferSave()
        {
            var token = new CancellationTokenSource();

            CancellationToken stoppingToken;

            while (!token.IsCancellationRequested)
            {
                //  If the configuration file was not set
                if (!File.Exists(Environment.CurrentDirectory + "\\simplog.json"))
                    await Task.Delay(TimeSpan.FromSeconds(10));

                await new FileService().SaveMessageIntoLogFile();

                await Task.Delay(TimeSpan.FromSeconds(5));
            }
        }

    }
}
