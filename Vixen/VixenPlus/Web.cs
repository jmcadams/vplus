namespace Vixen
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;
    using Utilities.FTP;

	[Obsolete]
    internal class Web : IDisposable
    {
        private WebClient m_client = null;

        public event EventHandler DownloadComplete;

        public event DownloadProgressEventHandler DownloadProgress;

        public Web()
        {
            this.m_client = new WebClient();
            this.m_client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            this.m_client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(this.m_client_DownloadProgressChanged);
            this.m_client.DownloadFileCompleted += new AsyncCompletedEventHandler(this.m_client_DownloadFileCompleted);
        }

        private void BreakFTPPath(string fileUri, out string host, out string directory)
        {
            if (!Uri.IsWellFormedUriString(fileUri, UriKind.Absolute))
            {
                throw new Exception("Invalid resource identifier.");
            }
            Uri uri = new Uri(fileUri);
            host = string.Format("{0}://{1}", uri.Scheme, uri.Host);
            string absolutePath = uri.AbsolutePath;
            directory = string.Format("{0}{1}", absolutePath.StartsWith("/") ? "" : "/", absolutePath);
            string fileName = Path.GetFileName(absolutePath);
            directory = directory.Remove(directory.Length - fileName.Length);
        }

        public void Dispose()
        {
            if (this.m_client != null)
            {
                this.m_client.Dispose();
            }
        }

        public void DownloadFile(string sourceUri, string destFilePath)
        {
            this.DownloadFile(sourceUri, destFilePath, false);
        }

        public void DownloadFile(string sourceUri, string destFilePath, bool synchronously)
        {
            if (!Uri.IsWellFormedUriString(sourceUri, UriKind.Absolute))
            {
                throw new Exception("Invalid resource identifier.");
            }
            this.m_client.DownloadFileAsync(new Uri(sourceUri), destFilePath);
            if (synchronously)
            {
                while (this.m_client.IsBusy)
                {
                    Thread.Sleep(50);
                }
            }
        }

        private FTPclient FTPLogin(string uri, string userName, string password)
        {
            string str;
            string str2;
            this.BreakFTPPath(uri, out str, out str2);
            FTPclient pclient = new FTPclient(str, userName, password, true);
            pclient.CurrentDirectory = str2;
            return pclient;
        }

        private void m_client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            this.OnDownloadComplete();
        }

        private void m_client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            this.OnDownloadProgress(e.BytesReceived, e.TotalBytesToReceive);
        }

        protected void OnDownloadComplete()
        {
            if (this.DownloadComplete != null)
            {
                this.DownloadComplete(this, new EventArgs());
            }
        }

        protected void OnDownloadProgress(long bytesComplete, long bytesTotal)
        {
            if (this.DownloadProgress != null)
            {
                this.DownloadProgress(bytesComplete, bytesTotal);
            }
        }

        public void UploadFile(string destHost, string destFileName, string sourceFilePath, string userName, string password)
        {
            this.UploadFile(destHost, destFileName, sourceFilePath, userName, password, false);
        }

        public void UploadFile(string destHost, string destFileName, string sourceFilePath, string userName, string password, bool synchronously)
        {
            if (destHost.Contains("://"))
            {
                destHost = destHost.Remove(0, destHost.IndexOf("://") + 3);
            }
            destHost = "ftp://" + destHost;
            Uri uri = new Uri(new Uri(destHost), destFileName);
            if (!Uri.IsWellFormedUriString(uri.AbsoluteUri, UriKind.Absolute))
            {
                throw new Exception("Invalid resource identifier.");
            }
            this.FTPLogin(uri.AbsoluteUri, userName, password).Upload(sourceFilePath, destFileName);
        }

        public delegate void DownloadProgressEventHandler(long bytesComplete, long bytesTotal);
    }
}

