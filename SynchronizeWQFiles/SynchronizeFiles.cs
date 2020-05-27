using SyntacticSugar;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Mango.Core.Log;
namespace SynchronizeWQFiles
{
    public class SynchronizeFiles
    {

        private static Thread thread;
        public static void SynchronizeFilesData()
        {
            thread = new Thread(Synchronize);
            thread.IsBackground = true;
            thread.Start();
        }

        public static void Synchronize()
        {
            while (true)
            {
                string time = DateTime.Now.ToString("HH:mm:ss");
                if (time == Config.ExecutionTime)
                {
                    LogService.AddLog(LogMode.FileLog, LogEvent.Debug, new Exception("开始执行获取sftp服务器文件..."));
                    SftpSynchronize();
                    LogService.AddLog(LogMode.FileLog, LogEvent.Debug, new Exception("执行结束获取sftp服务器文件。"));
                    LogService.AddLog(LogMode.FileLog, LogEvent.Debug, new Exception("开始执行删除过期文件（默认7天）..."));
                    SynGetFileDay();
                    LogService.AddLog(LogMode.FileLog, LogEvent.Debug, new Exception("执行结束删除过期文件（默认7天）..."));
                }
                Thread.Sleep(1000);
            }
        }

        public static void SftpSynchronize()
        {
            try
            {
                LogService.AddLog(LogMode.FileLog, LogEvent.Debug, new Exception("初始化连接Sftp..."));
                SFTPHelper sftp = new SFTPHelper(Config.Sftp_Server, Config.Sftp_Port, Config.Sftp_UserName, Config.Sftp_Password);
                LogService.AddLog(LogMode.FileLog, LogEvent.Debug, new Exception("开始获取Sftp服务器文件列表..."));
                List<string> files = sftp.GetFileList(Config.Sftp_FileDirectory, "zip");
                LogService.AddLog(LogMode.FileLog, LogEvent.Debug, new Exception(string.Format("SFTP服务器共有{0}个文件", files.Count)));
                int index = 1;
                foreach (var file in files)
                {
                    LogService.AddLog(LogMode.FileLog, LogEvent.Debug, new Exception(string.Format("开始下载SFTP第{0}个文件{1}", index, file)));
                    string sftpFilePath = Config.Sftp_FileDirectory + "/" + file;
                    string loaclFilePath = Path.Combine(Config.Local_FileDirectory, file);
                    if (File.Exists(loaclFilePath))
                    {
                        File.Delete(loaclFilePath);
                    }
                    sftp.Get(sftpFilePath, loaclFilePath);
                    LogService.AddLog(LogMode.FileLog, LogEvent.Debug, new Exception(string.Format("结束下载SFTP第{0}个文件{1}", index, file)));
                    index++;
                }
            }
            catch (Exception ex)
            {
                LogService.AddLog(LogMode.FileLog, LogEvent.Error, ex);
            }
        }

        public static void About()
        {
            thread.Abort();
        }

        public static void SynGetFileDay()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(Config.Local_FileDirectory);
            FileInfo[] files = directoryInfo.GetFiles("*.zip");
            try
            {
                foreach (var file in files)
                {
                    if (file.CreationTime.AddDays(Config.DelHistoryImportfiles) < DateTime.Now)
                    {
                        LogService.AddLog(LogMode.FileLog, LogEvent.Debug, new Exception(string.Format("开始删除文件{0}", file)));
                        file.Delete();
                        LogService.AddLog(LogMode.FileLog, LogEvent.Debug, new Exception(string.Format("结束删除文件{0}", file)));
                    }
                }
            }
            catch (Exception ex)
            {
                LogService.AddLog(LogMode.FileLog, LogEvent.Error, ex);
            }
        }

    }
}
