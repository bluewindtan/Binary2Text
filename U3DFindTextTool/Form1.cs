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
			string strText = "˵����" 
			+ Environment.NewLine + "�ù�����������.unity��.prefab�ļ��е���ʾ�ı����ҵ�֮����һ���ַ��������滻�����ʾ�ı���ͬʱ���ַ��������������ʾ�ı�һһ��Ӧ������.txt�ļ��С��Ӷ�ʹ����ʾ�ı���Unity���룬�Ӷ����ӷ���ʵ�ֶ������ı������á�" 
			 + Environment.NewLine + Environment.NewLine + "�÷���" 
			 + Environment.NewLine + "���process��ť��ѡ��һ��Ŀ¼��ȷ����ʼ������������ᵯ����ʾ��";
			textBox1.Text = strText;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			FolderBrowserDialog dlg = new FolderBrowserDialog();
			dlg.RootFolder = Environment.SpecialFolder.MyComputer;
			dlg.SelectedPath = Environment.CurrentDirectory;
			dlg.Description = "��ѡ��Ŀ¼";
			if (DialogResult.OK == dlg.ShowDialog())
			{
				string sDirectory = dlg.SelectedPath;
				ProcessDirectory(sDirectory);
				MessageBox.Show("Process finished��OK��");
			}
		}

		private void ProcessDirectory(string sDirectory)
		{
			// �ж��Ƿ�Ŀ¼
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
			// Դ�ļ�
			FileStream fsource = new FileStream(sDirectory + "\\" + sFile, FileMode.Open);
			// Ŀ���ļ�
			string targetDir = sDirectory + "\\target";
			if (!Directory.Exists(targetDir))
			{
				Directory.CreateDirectory(targetDir);
			}
			string targetFile = targetDir + "\\" + sFile;
			FileStream ftarget = new FileStream(targetFile, FileMode.Create);
			// �����ı��ļ�
			targetDir = sDirectory + "\\text";
			if (!Directory.Exists(targetDir))
			{
				Directory.CreateDirectory(targetDir);
			}
			string strTxt = targetDir + "\\" + sFile + ".txt";
			FileStream fsouttxt = new FileStream(strTxt, FileMode.Create);
			// Stream����
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
						codes[0] = (byte)code2;//������С����ǰ
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