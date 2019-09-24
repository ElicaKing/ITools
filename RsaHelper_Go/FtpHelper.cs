using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace RsaHelper_Go
{
    public class FtpHelper
    {
        private static string FTPCONSTR = "ftp://59.202.26.4:21/";//FTP的服务器地址，格式为ftp://192.168.1.234:8021/。ip地址和端口换成自己的，这些建议写在配置文件中，方便修改
        private static string FTPUSERNAME = "hzjxwh";//FTP服务器的用户名
        private static string FTPPASSWORD = "hzjxwh@hzjxwh";//FTP服务器的密码
        /// <summary>
        /// 从ftp服务器下载文件的功能
        /// </summary>
        /// <param name="ftpfilepath">ftp下载的地址</param>
        /// <param name="filePath">存放到本地的路径</param>
        /// <param name="fileName">保存的文件名称</param>
        /// <returns></returns>
        public static bool Downloads(string ftpfilepath, string filePath, string fileName)
        {
            try
            {
                filePath = filePath.Replace("我的电脑\\", "");
                string onlyFileName = Path.GetFileName(fileName);
                string newFileName = filePath + onlyFileName;
                if (File.Exists(newFileName))
                {
                    //errorinfo = string.Format("本地文件{0}已存在,无法下载", newFileName);                   
                    File.Delete(newFileName);
                    //return false;
                }
                ftpfilepath = ftpfilepath.Replace("\\", "/");

                string url = FTPCONSTR + ftpfilepath;
                //Log.CaptureError("url", url);
                FtpWebRequest reqFtp = (FtpWebRequest)FtpWebRequest.Create(new Uri(url));
                reqFtp.UseBinary = true;
                reqFtp.KeepAlive = false;
                reqFtp.Credentials = new NetworkCredential(FTPUSERNAME, FTPPASSWORD);
                FtpWebResponse response = (FtpWebResponse)reqFtp.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];
                readCount = ftpStream.Read(buffer, 0, bufferSize);
                FileStream outputStream = new FileStream(newFileName, FileMode.Create);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }

                ftpStream.Close();
                outputStream.Close();
                response.Close();
                return true;
            }
            catch (Exception ex)
            {
                //Log.CaptureError("error", ex.Message);
                //errorinfo = string.Format("因{0},无法下载", ex.Message);
                return false;
            }
        }
        /// <summary>
        /// 读取文件目录下所有的文件名称，包括文件夹名称
        /// </summary>
        /// <param name="ftpAdd">传过来的文件夹路径</param>
        /// <returns>返回的文件或文件夹名称</returns>
        public static string[] GetFtpFileList(string ftpAdd)
        {
            //string url = FTPCONSTR + ftpAdd;
            FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(new Uri(ftpAdd));
            ftpRequest.UseBinary = true;
            ftpRequest.Credentials = new NetworkCredential(FTPUSERNAME, FTPPASSWORD);

            if (ftpRequest != null)
            {
                StringBuilder fileListBuilder = new StringBuilder();
                //ftpRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;//该方法可以得到文件名称的详细资源，包括修改时间、类型等这些属性
                ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory;//只得到文件或文件夹的名称
                try
                {
                    WebResponse ftpResponse = ftpRequest.GetResponse();
                    StreamReader ftpFileListReader = new StreamReader(ftpResponse.GetResponseStream(), Encoding.Default);

                    string line = ftpFileListReader.ReadLine();

                    while (line != null)
                    {
                        fileListBuilder.Append(line);
                        fileListBuilder.Append("@");
                        line = ftpFileListReader.ReadLine();
                    }
                    ftpFileListReader.Close();
                    ftpResponse.Close();
                    fileListBuilder.Remove(fileListBuilder.ToString().LastIndexOf("@"), 1);
                    return fileListBuilder.ToString().Split('@');//返回得到的数组
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 从ftp服务器下载文件的功能
        /// </summary>
        /// <param name="ftpfilepath">ftp下载的地址</param>
        /// <param name="filePath">存放到本地的路径</param>
        /// <param name="fileName">保存的文件名称</param>
        /// <returns></returns>
        public static bool Download(string ftpfilepath, string filePath, string fileName)
        {
            try
            {
                filePath = filePath.Replace("我的电脑\\", "");
                string onlyFileName = Path.GetFileName(fileName);
                string newFileName = filePath + onlyFileName;
                if (File.Exists(newFileName))
                {
                    //errorinfo = string.Format("本地文件{0}已存在,无法下载", newFileName);                   
                    File.Delete(newFileName);
                    //return false;
                }
                ftpfilepath = ftpfilepath.Replace("\\", "/");

                string url = FTPCONSTR + ftpfilepath;
                FtpWebRequest reqFtp = (FtpWebRequest)FtpWebRequest.Create(new Uri(url));
                reqFtp.UseBinary = true;
                reqFtp.KeepAlive = false;
                reqFtp.Credentials = new NetworkCredential(FTPUSERNAME, FTPPASSWORD);
                FtpWebResponse response = (FtpWebResponse)reqFtp.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];
                readCount = ftpStream.Read(buffer, 0, bufferSize);
                FileStream outputStream = new FileStream(newFileName, FileMode.Create);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }

                ftpStream.Close();
                outputStream.Close();
                response.Close();
                return true;
            }
            catch (Exception ex)
            {
                //Log.CaptureError("error", ex.Message);
                //errorinfo = string.Format("因{0},无法下载", ex.Message);
                return false;
            }
        }
        #region 从ftp服务器下载文件,调用此方法
        public void CheckFile()
        {
            List<dynamic> list = new List<dynamic>();
            //需要传递缴款单来源渠道编号
            string ftpFilePath = "";
                //Request.QueryString["ftpFilePath"] ?? "330000";
            string filePath = string.Concat(@"F:\ftpFile\" + ftpFilePath);
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);//创建完文件后必须关闭掉流
            }
            string fileName = string.Empty;
            //ftp下载文件的拼接地址
            string pathMore = string.Concat(FTPCONSTR + ftpFilePath);
            //获取所有文件的名称存数数组
            string[] arry = GetFtpFileList(pathMore);
            var count = arry.Length;
            for (int i = 0; i < count; i++)
            {
                fileName = arry[i].ToString();
                //截取文件命名规则中的日期字符串；
                dynamic fileDate = fileName.Substring(17, 8);
                list.Add(fileDate);
            }
            //将日期进行从小到大排序
            list.Sort();
            for (int j = 0; j < list.Count; j++)
            {
                //获取所有
                string filesPath = string.Concat(ftpFilePath + @"/" + fileName);
                if (arry[j].ToString().Contains(list[list.Count - 1]))
                {
                    Download(filesPath, filePath, fileName);
                }
            }
        }
        #endregion
    }
}
