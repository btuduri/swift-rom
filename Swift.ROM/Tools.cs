using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace Swift.ROM
{
	class Tools
	{

		#region GetFileSHA1 取回文件SHA1编码
		/// <summary>
		/// 取回文件SHA1编码
		/// </summary>
		/// <param name="fileName">文件路径</param>
		/// <returns>SHA1编码</returns>
		public static string GetFileSHA1(string fileName)
		{
			FileStream fs;
			string sha1 = "";

			try
			{
				fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
			}
			catch
			{
				return null;
			}

			SHA1 sha1m = new SHA1Managed();
			byte[] result = sha1m.ComputeHash(fs);

			fs.Close();

			foreach (byte b in result)
				sha1 += b.ToString("X2");

			return sha1;
		}
		#endregion

		public static void Start(string fileName)
		{
			Process proc = new Process();
			proc.StartInfo.FileName = fileName;
			proc.Start();
		}

		#region unZIP 解压压缩文件
		/// <summary>
		/// 解压压缩文件
		/// </summary>
		/// <param name="rarFile">压缩文件名称</param>
		/// <param name="extPath">展开路径</param>
		public static void unZIP(string zipFile, string extPath)
		{
			Debug.WriteLine("开始解压..." + zipFile);
			Process proc = new Process();

			switch (Path.GetExtension(zipFile).ToUpper())
			{ 
				case ".RAR":
					//判断解压程序是否存在
					if (!File.Exists(Application.StartupPath + @"\UnRAR.exe"))
					{
						MessageBox.Show("Not Found --> UnRAR.exe");
						return;
					}
					
					proc.StartInfo.FileName = Application.StartupPath + @"\UnRAR.exe";
					proc.StartInfo.Arguments = "e -y -p- \"" + zipFile + "\" \"" + extPath + "\"";
					
					break;
				case ".ZIP":
				case ".7Z":
					//判断解压程序是否存在
					if (!File.Exists(Application.StartupPath + @"\7za.exe"))
					{
						MessageBox.Show("Not Found --> 7za.exe");
						return;
					}

					proc.StartInfo.FileName = Application.StartupPath + @"\7za.exe";
					proc.StartInfo.Arguments = "e -y \"" + zipFile + "\" -o\"" + extPath + "\"";
					
					break;
				default:
					MessageBox.Show("Not Support!");
					return;
			}

			proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			Debug.WriteLine(proc.StartInfo.Arguments);
			proc.Start();
			Debug.WriteLine("等待解压结束...");
			proc.WaitForExit();
			Debug.WriteLine("解压结束.ExitCode="+proc.ExitCode);
		}
		#endregion

		public static Image GetImage(string fileName)
		{
			////判断文件是否存在
			//String fn=Application.StartupPath;
			//if (File.Exists(fn + fileName))
			//    fn += fileName;
			//else
			//{
			//    fn = Application.StartupPath + @"\2008";
			//    if (File.Exists(fn + fileName))
			//        fn += fileName;
			//    else
			//    {
			//        fn = Application.StartupPath + @"\2008now";
			//        if (File.Exists(fn + fileName))
			//            fn += fileName;
			//        else
			//            return null;
			//    }
			//}

			//fileName = fn;

			byte[] bytes = File.ReadAllBytes(fileName);
			MemoryStream ms = new MemoryStream(bytes);

			return Image.FromStream(ms);
		}

		//public static String GetImageFileName(string fileName)
		//{
		//    String fn = Application.StartupPath;
		//    if (File.Exists(fn + fileName))
		//        return fn + fileName;
		//    else
		//    {
		//        fn = Application.StartupPath + @"\2008";
		//        if (File.Exists(fn + fileName))
		//            return fn + fileName;
		//        else
		//        {
		//            fn = Application.StartupPath + @"\2008now";
		//            if (File.Exists(fn + fileName))
		//                return fn + fileName;
		//            else
		//                return null;
		//        }
		//    }
		//}
	}
}
