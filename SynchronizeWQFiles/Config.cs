using SyntacticSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynchronizeWQFiles
{
    public class Config
    {
        public static string ExecutionTime { get { return ConfigSugar.GetAppString("ExecutionTime", "03:01:01"); } }
        public static string Sftp_Server { get { return ConfigSugar.GetAppString("Sftp_Server", string.Empty); } }
        public static string Sftp_Port { get { return ConfigSugar.GetAppString("Sftp_Port", string.Empty); } }
        public static string Sftp_UserName { get { return ConfigSugar.GetAppString("Sftp_UserName", string.Empty); } } 
        public static string Sftp_Password { get { return ConfigSugar.GetAppString("Sftp_Password", string.Empty); } }
        public static string Sftp_FileDirectory { get { return ConfigSugar.GetAppString("Sftp_FileDirectory", string.Empty); } }
        public static string Local_FileDirectory { get { return ConfigSugar.GetAppString("Local_FileDirectory", string.Empty); } }
        public static int SyncRangeTime { get { return ConfigSugar.GetAppInt("SyncRangeTime", 48); } }

        public static int DelHistoryImportfiles { get { return ConfigSugar.GetAppInt("DelHistoryImportfiles", 7); } }

    }
}
