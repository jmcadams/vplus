namespace Utilities.FTP
{
    using Microsoft.VisualBasic;
    using Microsoft.VisualBasic.CompilerServices;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Runtime.InteropServices;
    using System.Text;

    [OptionText]
    public class FTPclient
    {
        private string _currentDirectory;
        private bool _enableSSL;
        private string _hostname;
        private bool _keepAlive;
        private string _lastDirectory;
        private string _password;
        private IWebProxy _proxy;
        private bool _usePassive;
        private string _username;

        public FTPclient()
        {
            this._lastDirectory = "";
            this._currentDirectory = "/";
            this._enableSSL = false;
            this._keepAlive = false;
            this._usePassive = false;
            this._proxy = null;
        }

        public FTPclient(string Hostname)
        {
            this._lastDirectory = "";
            this._currentDirectory = "/";
            this._enableSSL = false;
            this._keepAlive = false;
            this._usePassive = false;
            this._proxy = null;
            this._hostname = Hostname;
        }

        public FTPclient(string Hostname, string Username, string Password, bool KeepAlive = false)
        {
            this._lastDirectory = "";
            this._currentDirectory = "/";
            this._enableSSL = false;
            this._keepAlive = false;
            this._usePassive = false;
            this._proxy = null;
            this._hostname = Hostname;
            this._username = Username;
            this._password = Password;
        }

        private string AdjustDir(string path)
        {
            return (Conversions.ToString(Interaction.IIf(path.StartsWith("/"), "", "/")) + path);
        }

        public bool Download(string sourceFilename, FileInfo targetFI, bool PermitOverwrite = false)
        {
            string str;
            if (targetFI.Exists & !PermitOverwrite)
            {
                throw new ApplicationException("Target file already exists");
            }
            if (Operators.CompareString(sourceFilename.Trim(), "", true) == 0)
            {
                throw new ApplicationException("File not specified");
            }
            if (sourceFilename.Contains("/"))
            {
                str = this.AdjustDir(sourceFilename);
            }
            else
            {
                str = this.CurrentDirectory + sourceFilename;
            }
            string uRI = this.Hostname + str;
            FtpWebRequest request = this.GetRequest(uRI);
            request.Method = "RETR";
            request.UseBinary = true;
            using (FtpWebResponse response = (FtpWebResponse) request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    FileStream stream2 = targetFI.OpenWrite();
                    try
                    {
                        byte[] buffer = new byte[0x800];
                        int count = 0;
                        do
                        {
                            count = stream.Read(buffer, 0, buffer.Length);
                            stream2.Write(buffer, 0, count);
                        }
                        while (count != 0);
                        stream.Close();
                        stream2.Flush();
                        stream2.Close();
                    }
                    catch (Exception exception1)
                    {
                        ProjectData.SetProjectError(exception1);
                        Exception exception = exception1;
                        stream2.Close();
                        targetFI.Delete();
                        throw;
                        ProjectData.ClearProjectError();
                    }
                    finally
                    {
                        if (stream2 != null)
                        {
                            stream2.Dispose();
                        }
                    }
                    stream.Close();
                }
                response.Close();
            }
            return true;
        }

        public bool Download(string sourceFilename, string localFilename, bool PermitOverwrite = false)
        {
            FileInfo targetFI = new FileInfo(localFilename);
            return this.Download(sourceFilename, targetFI, PermitOverwrite);
        }

        public bool Download(FTPfileInfo file, FileInfo localFI, bool PermitOverwrite = false)
        {
            return this.Download(file.FullName, localFI, PermitOverwrite);
        }

        public bool Download(FTPfileInfo file, string localFilename, bool PermitOverwrite = false)
        {
            return this.Download(file.FullName, localFilename, PermitOverwrite);
        }

        public bool FtpCreateDirectory(string dirpath)
        {
            string uRI = this.Hostname + this.AdjustDir(dirpath);
            FtpWebRequest ftp = this.GetRequest(uRI);
            ftp.Method = "MKD";
            try
            {
                string stringResponse = this.GetStringResponse(ftp);
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                bool flag = false;
                ProjectData.ClearProjectError();
                return flag;
                ProjectData.ClearProjectError();
            }
            return true;
        }

        public bool FtpDelete(string filename)
        {
            string uRI = this.Hostname + this.GetFullPath(filename);
            FtpWebRequest ftp = this.GetRequest(uRI);
            ftp.Method = "DELE";
            try
            {
                string stringResponse = this.GetStringResponse(ftp);
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                bool flag = false;
                ProjectData.ClearProjectError();
                return flag;
                ProjectData.ClearProjectError();
            }
            return true;
        }

        public bool FtpDeleteDirectory(string dirpath)
        {
            string uRI = this.Hostname + this.AdjustDir(dirpath);
            FtpWebRequest ftp = this.GetRequest(uRI);
            ftp.Method = "RMD";
            try
            {
                string stringResponse = this.GetStringResponse(ftp);
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                bool flag = false;
                ProjectData.ClearProjectError();
                return flag;
                ProjectData.ClearProjectError();
            }
            return true;
        }

        public bool FtpDirectoryExists(string remoteDir)
        {
            bool flag;
            try
            {
                List<string> list = this.ListDirectory(remoteDir);
                flag = true;
            }
            catch (WebException exception1)
            {
                ProjectData.SetProjectError(exception1);
                WebException exception = exception1;
                if (exception.Message.Contains("550"))
                {
                    flag = false;
                    ProjectData.ClearProjectError();
                    return flag;
                }
                throw;
                ProjectData.ClearProjectError();
            }
            catch (Exception exception3)
            {
                ProjectData.SetProjectError(exception3);
                Exception exception2 = exception3;
                throw;
                ProjectData.ClearProjectError();
            }
            return flag;
        }

        public bool FtpFileExists(string filename)
        {
            bool flag;
            try
            {
                long fileSize = this.GetFileSize(filename);
                flag = true;
            }
            catch (WebException exception1)
            {
                ProjectData.SetProjectError(exception1);
                WebException exception = exception1;
                if (exception.Message.Contains("550"))
                {
                    flag = false;
                    ProjectData.ClearProjectError();
                    return flag;
                }
                throw;
                ProjectData.ClearProjectError();
            }
            catch (Exception exception2)
            {
                ProjectData.SetProjectError(exception2);
                throw;
                ProjectData.ClearProjectError();
            }
            return flag;
        }

        public bool FtpRename(string sourceFilename, string newName)
        {
            string fullPath = this.GetFullPath(sourceFilename);
            if (!this.FtpFileExists(fullPath))
            {
                throw new FileNotFoundException("File " + fullPath + " not found");
            }
            string left = this.GetFullPath(newName);
            if (Operators.CompareString(left, fullPath, true) == 0)
            {
                throw new ApplicationException("Source and target are the same");
            }
            if (this.FtpFileExists(left))
            {
                throw new ApplicationException("Target file " + left + " already exists");
            }
            string uRI = this.Hostname + fullPath;
            FtpWebRequest ftp = this.GetRequest(uRI);
            ftp.Method = "RENAME";
            ftp.RenameTo = left;
            try
            {
                string stringResponse = this.GetStringResponse(ftp);
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                bool flag = false;
                ProjectData.ClearProjectError();
                return flag;
                ProjectData.ClearProjectError();
            }
            return true;
        }

        private ICredentials GetCredentials()
        {
            return new NetworkCredential(this.Username, this.Password);
        }

        public DateTime GetDateTimestamp(string filename)
        {
            string str;
            if (filename.Contains("/"))
            {
                str = this.AdjustDir(filename);
            }
            else
            {
                str = this.CurrentDirectory + filename;
            }
            string uRI = this.Hostname + str;
            FtpWebRequest ftp = this.GetRequest(uRI);
            ftp.Method = "MDTM";
            return this.GetLastModified(ftp);
        }

        public DateTime GetDateTimestamp(FTPfileInfo file)
        {
            DateTime dateTimestamp = this.GetDateTimestamp(file.Filename);
            file.FileDateTime = dateTimestamp;
            return dateTimestamp;
        }

        private string GetDirectory(string directory = "")
        {
            string str2;
            if (Operators.CompareString(directory, "", true) == 0)
            {
                str2 = this.Hostname + this.CurrentDirectory;
                this._lastDirectory = this.CurrentDirectory;
                return str2;
            }
            if (!directory.StartsWith("/"))
            {
                throw new ApplicationException("Directory should start with /");
            }
            str2 = this.Hostname + directory;
            this._lastDirectory = directory;
            return str2;
        }

        public long GetFileSize(string filename)
        {
            string str;
            if (filename.Contains("/"))
            {
                str = this.AdjustDir(filename);
            }
            else
            {
                str = this.CurrentDirectory + filename;
            }
            string uRI = this.Hostname + str;
            FtpWebRequest ftp = this.GetRequest(uRI);
            ftp.Method = "SIZE";
            string stringResponse = this.GetStringResponse(ftp);
            return this.GetSize(ftp);
        }

        private string GetFullPath(string file)
        {
            if (file.Contains("/"))
            {
                return this.AdjustDir(file);
            }
            return (this.CurrentDirectory + file);
        }

        private DateTime GetLastModified(FtpWebRequest ftp)
        {
            DateTime lastModified;
            using (FtpWebResponse response = (FtpWebResponse) ftp.GetResponse())
            {
                lastModified = response.LastModified;
                response.Close();
            }
            return lastModified;
        }

        private FtpWebRequest GetRequest(string URI)
        {
            FtpWebRequest request2 = (FtpWebRequest) WebRequest.Create(URI);
            request2.Credentials = this.GetCredentials();
            request2.EnableSsl = this.EnableSSL;
            request2.KeepAlive = this.KeepAlive;
            request2.UsePassive = this.UsePassive;
            request2.Proxy = this.Proxy;
            return request2;
        }

        private long GetSize(FtpWebRequest ftp)
        {
            long contentLength;
            using (FtpWebResponse response = (FtpWebResponse) ftp.GetResponse())
            {
                contentLength = response.ContentLength;
                response.Close();
            }
            return contentLength;
        }

        private string GetStringResponse(FtpWebRequest ftp)
        {
            string str2 = "";
            using (FtpWebResponse response = (FtpWebResponse) ftp.GetResponse())
            {
                long contentLength = response.ContentLength;
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                    {
                        str2 = reader.ReadToEnd();
                        reader.Close();
                    }
                    stream.Close();
                }
                response.Close();
            }
            return str2;
        }

        public List<string> ListDirectory(string directory = "")
        {
            FtpWebRequest ftp = this.GetRequest(this.GetDirectory(directory));
            ftp.Method = "NLST";
            string str = this.GetStringResponse(ftp).Replace("\r\n", "\r").TrimEnd(new char[] { '\r' });
            List<string> list2 = new List<string>();
            list2.AddRange(str.Split(new char[] { '\r' }));
            return list2;
        }

        public FTPdirectory ListDirectoryDetail(string directory = "", bool doDateTimeStamp = false)
        {
            FtpWebRequest ftp = this.GetRequest(this.GetDirectory(directory));
            ftp.Method = "LIST";
            FTPdirectory pdirectory = new FTPdirectory(this.GetStringResponse(ftp).Replace("\r\n", "\r").TrimEnd(new char[] { '\r' }), this._lastDirectory);
            if (doDateTimeStamp)
            {
                foreach (FTPfileInfo info in pdirectory)
                {
                    info.FileDateTime = this.GetDateTimestamp(info);
                }
            }
            return pdirectory;
        }

        public bool Upload(FileInfo fi, string targetFilename = "")
        {
            string str;
            if (Operators.CompareString(targetFilename.Trim(), "", true) == 0)
            {
                str = this.CurrentDirectory + fi.Name;
            }
            else if (targetFilename.Contains("/"))
            {
                str = this.AdjustDir(targetFilename);
            }
            else
            {
                str = this.CurrentDirectory + targetFilename;
            }
            string uRI = this.Hostname + str;
            FtpWebRequest request = this.GetRequest(uRI);
            request.Method = "STOR";
            request.UseBinary = true;
            request.ContentLength = fi.Length;
            byte[] array = new byte[0x800];
            using (FileStream stream = fi.OpenRead())
            {
                try
                {
                    using (Stream stream2 = request.GetRequestStream())
                    {
                        int num;
                        do
                        {
                            num = stream.Read(array, 0, 0x800);
                            stream2.Write(array, 0, num);
                        }
                        while (num >= 0x800);
                        stream2.Close();
                    }
                }
                catch (Exception exception1)
                {
                    ProjectData.SetProjectError(exception1);
                    Exception exception = exception1;
                    ProjectData.ClearProjectError();
                }
                finally
                {
                    stream.Close();
                }
            }
            request = null;
            return true;
        }

        public bool Upload(string localFilename, string targetFilename = "")
        {
            if (!System.IO.File.Exists(localFilename))
            {
                throw new ApplicationException("File " + localFilename + " not found");
            }
            FileInfo fi = new FileInfo(localFilename);
            return this.Upload(fi, targetFilename);
        }

        public string CurrentDirectory
        {
            get
            {
                return (this._currentDirectory + Conversions.ToString(Interaction.IIf(this._currentDirectory.EndsWith("/"), "", "/")));
            }
            set
            {
                if (!value.StartsWith("/"))
                {
                    throw new ApplicationException("Directory should start with /");
                }
                this._currentDirectory = value;
            }
        }

        public bool EnableSSL
        {
            get
            {
                return this._enableSSL;
            }
            set
            {
                this._enableSSL = value;
            }
        }

        public string Hostname
        {
            get
            {
                if (this._hostname.StartsWith("ftp://"))
                {
                    return this._hostname;
                }
                return ("ftp://" + this._hostname);
            }
            set
            {
                this._hostname = value;
            }
        }

        public bool KeepAlive
        {
            get
            {
                return this._keepAlive;
            }
            set
            {
                this._keepAlive = value;
            }
        }

        public string Password
        {
            get
            {
                return this._password;
            }
            set
            {
                this._password = value;
            }
        }

        public IWebProxy Proxy
        {
            get
            {
                return this._proxy;
            }
            set
            {
                this._proxy = value;
            }
        }

        public bool UsePassive
        {
            get
            {
                return this._usePassive;
            }
            set
            {
                this._usePassive = value;
            }
        }

        public string Username
        {
            get
            {
                return Conversions.ToString(Interaction.IIf(Operators.CompareString(this._username, "", true) == 0, "anonymous", this._username));
            }
            set
            {
                this._username = value;
            }
        }
    }
}

