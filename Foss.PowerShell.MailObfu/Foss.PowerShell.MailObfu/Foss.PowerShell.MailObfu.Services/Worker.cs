using Foss.PowerShell.MailObfu.Core;

namespace DesensitizeMailService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        private readonly string watchDir = @"C:\Users\mochen\Documents\EmailWorkspace";
        private readonly string outDir = @"C:\Users\mochen\Documents\EmailWorkspace\Output";

        private readonly FileSystemWatcher _watcher;
        private readonly MailDesensitizer _mailDes;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            _mailDes = new();

            if (!Directory.Exists(outDir))
            {
                Directory.CreateDirectory(outDir);
            }

            _watcher = new FileSystemWatcher(watchDir);
            _watcher.Changed += FileChanged;

            _watcher.Filter = "*.txt";
            _watcher.EnableRaisingEvents = true;
            _watcher.IncludeSubdirectories = false;

        }

        private void FileChanged(object sender, FileSystemEventArgs e)
        {
            _logger.LogInformation(new EventId(1, "FileChanged"), "File changed at: {0}", e.FullPath);

            using (FileStream fs = File.OpenRead(e.FullPath))
            using (StreamReader sr = new StreamReader(fs))
            {
                var t = sr.ReadToEndAsync();

                string content = t.Result;
                _logger.LogInformation(new EventId(2, "FileRead"),
                    content);

                string processed = _mailDes.ObfuscateMail(content);
                _logger.LogInformation(new EventId(3, "mailDesensitized"), processed);
            }


        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}