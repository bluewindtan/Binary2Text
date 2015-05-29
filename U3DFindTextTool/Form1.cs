using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace U3DFindTextTool
{
	public partial class TextTool : Form
	{
		public TextTool()
		{
			InitializeComponent();
			string strText = "说明：" 
			+ Environment.NewLine + "该工具用来查找.unity或.prefab文件中的显示文本，找到之后，用一个字符串变量替换这个显示文本，同时将字符串变量与这个显示文本一一对应保存在.txt文件中。从而使得显示文本与Unity分离，从而更加方便实现多语言文本的配置。" 
			 + Environment.NewLine + Environment.NewLine + "用法：" 
			 + Environment.NewLine + "点击process按钮，选择一个目录，确定后开始处理，处理结束会弹出提示框。";
			textBox1.Text = strText;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			FolderBrowserDialog dlg = new FolderBrowserDialog();
			dlg.RootFolder = Environment.SpecialFolder.MyComputer;
			dlg.SelectedPath = Environment.CurrentDirectory;
			dlg.Description = "请选择目录";
			if (DialogResult.OK == dlg.ShowDialog())
			{
				string sDirectory = dlg.SelectedPath;
				ProcessDirectory(sDirectory);
				MessageBox.Show("Process finished！OK！");
			}
		}

		private void ProcessDirectory(string sDirectory)
		{
			// 判断是否目录
			if (Directory.Exists(sDirectory))
			{
				DirectoryInfo dirInfo = new DirectoryInfo(sDirectory);
				foreach (FileSystemInfo fsInfo in dirInfo.GetFileSystemInfos())
				{
					if (fsInfo is FileInfo)
					{
						FileInfo fi = fsInfo as FileInfo;
						FindText(fi.DirectoryName, fi.Name);
					}
					else if (fsInfo is DirectoryInfo)
					{
						ProcessDirectory(fsInfo.FullName);
					}
				}
			}
		}

		public static void FindText(string sDirectory, string sFile)
		{
			// 源文件
			FileStream fsource = new FileStream(sDirectory + "\\" + sFile, FileMode.Open);
			// 目标文件
			string targetDir = sDirectory + "\\target";
			if (!Directory.Exists(targetDir))
			{
				Directory.CreateDirectory(targetDir);
			}
			string targetFile = targetDir + "\\" + sFile;
			FileStream ftarget = new FileStream(targetFile, FileMode.Create);
			// 差异文本文件
			targetDir = sDirectory + "\\text";
			if (!Directory.Exists(targetDir))
			{
				Directory.CreateDirectory(targetDir);
			}
			string strTxt = targetDir + "\\" + sFile + ".txt";
			FileStream fsouttxt = new FileStream(strTxt, FileMode.Create);
			// Stream定义
			StreamReader sr = new StreamReader(fsource);
			StreamWriter sw = new StreamWriter(ftarget);
			StreamWriter swtxt = new StreamWriter(fsouttxt);
			byte bt1 = (byte)sr.Read();
			byte bt2 = (byte)sr.Read();
			byte bt3 = (byte)sr.Read();
			byte bt4 = (byte)sr.Read();
			byte bt5 = (byte)sr.Read();
			byte bt6 = (byte)sr.Read();
			bool bLabelLine = false;
			while (true)
			{
				if (bLabelLine)
				{
					if (bt1 == '\\' && bt2 == 'u')
					{
						byte[] codes = new byte[2];
						int code1, code2;
						string str1 = "";
						str1 += (char)bt3;
						str1 += (char)bt4;
						string str2 = "";
						str2 += (char)bt5;
						str2 += (char)bt6;
						code1 = Convert.ToInt32(str1, 16);
						code2 = Convert.ToInt32(str2, 16);
						codes[0] = (byte)code2;//必须是小端在前
						codes[1] = (byte)code1;
						string strUnicode = Encoding.Unicode.GetString(codes);
						sw.Write(strUnicode);
						swtxt.Write(strUnicode);

						bt1 = (byte)sr.Read();
						bt2 = (byte)sr.Read();
						bt3 = (byte)sr.Read();
						bt4 = (byte)sr.Read();
						bt5 = (byte)sr.Read();
						bt6 = (byte)sr.Read();
					}
					else
					{
						if (bt1 == '\n')
						{
							bLabelLine = false;
							swtxt.Write("\r");
						}
						swtxt.Write((char)bt1);
						sw.Write((char)bt1);
						bt1 = bt2;
						bt2 = bt3;
						bt3 = bt4;
						bt4 = bt5;
						bt5 = bt6;
						bt6 = (byte)sr.Read();
					}
				}
				else
				{
					if (bt1 == 'm' && bt2 == 'T' && bt3 == 'e' && bt4 == 'x' && bt5 == 't')
					{
						bLabelLine = true;
					}

					sw.Write((char)bt1);
					bt1 = bt2;
					bt2 = bt3;
					bt3 = bt4;
					bt4 = bt5;
					bt5 = bt6;
					bt6 = (byte)sr.Read();
				}

				if (sr.EndOfStream)
				{
					break;
				}
			}

			sw.Write((char)bt1);
			sw.Write((char)bt2);
			sw.Write((char)bt3);
			sw.Write((char)bt4);
			sw.Write((char)bt5);
			sw.Write((char)bt6);
			sr.Close();
			sw.Flush();
			sw.Close();
			swtxt.Flush();
			swtxt.Close();
			fsource.Close();
			ftarget.Close();
			fsouttxt.Close();
		}

	}
}