using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

namespace FrameWork_lz
{

    /// <summary>
    /// func: 文件流管理器
    /// 
    /// author: lz910608@gmail.com
    /// 
    /// time: 2019-1-29
    /// </summary>
    public class FileTools
    {
        /// <summary>
        /// 获取文件字节流
        /// </summary>
        /// <param name="fileUrl"></param>
        /// <returns></returns>
        public static byte[] GetFileByte (string fileUrl)
        {
            FileStream fs = new FileStream (fileUrl, FileMode.Open, FileAccess.Read);
            try {
                byte[] buffur = new byte[fs.Length];
                fs.Read (buffur, 0, (int)fs.Length);
                Debug.LogFormat ("video  buffur {0}", buffur.Length);
                return buffur;
            } catch (Exception ex) {
                Debug.LogError (ex.ToString ());
                return null;
            } finally {
                if (fs != null) {
                    Resources.UnloadUnusedAssets ();
                    //关闭资源
                    fs.Close ();
                    fs.Dispose ();
                }
            }
        }

        /// <summary>
        /// 创建新文件夹
        /// </summary>
        /// <param name="path"></param>
        public static void CreateOpenDirector (string path)
        {
            Directory.CreateDirectory (path);
        }

        /// <summary>
        /// 创建保存文本文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="value">保存文本内容</param>
        public static void CreateFile (string path, string value)
        {
         
            if (!File.Exists (path)) {
                Debug.LogFormat ("Current file save path is {0} value {1}", path, value);
                StreamWriter sw = new StreamWriter (path);      //  生成文件 
                sw.WriteLine (value);   // saveString 为你想存储的字符串  将其写入文本中
                sw.Close ();   //释放掉
                Resources.UnloadUnusedAssets ();
            } else {

                FileStream fs = new FileStream (path, FileMode.Append);
                StreamWriter sw = new StreamWriter (fs);
                sw.WriteLine (value);
                sw.Flush ();
                sw.Close ();
                fs.Close ();
            }
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="path">删除路径</param>
        public static void DelectFile (string path)
        {
            if (File.Exists (path)) {
                File.Delete (path);
            }
        }


        /// <summary>
        /// Reads the text line file.
        /// </summary>
        /// <returns>The text line file.</returns>
        /// <param name="path">Path.</param>
        public static string[] ReadStrLineFile (string path)
        {
            if (File.Exists (path)) {
                return  File.ReadAllLines (path);
            } else
                return null;
        }

        /// <summary>
        /// 读取文本内容
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ReadFileStr (string path)
        {
            string result;
            if (File.Exists (path)) {
                //文件读写流
                StreamReader sr = new StreamReader (path);
                //读取内容
                result = sr.ReadToEnd ();
                Debug.LogFormat ("reslut id {0}", result);
                sr.Close ();
                sr.Dispose ();
            } else {
                result = string.Empty;
            }

            return result;
        }

    }
}


