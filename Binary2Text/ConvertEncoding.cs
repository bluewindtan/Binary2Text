using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Binary2Text
{
	class ConvertEncoding
	{
		public enum FileEncoding
		{
			CN = 0,			// 中文简体
			CN_T,			// 中文繁体
			TH,				// 泰文 
			BR,				// 葡萄牙文
			KR,				// 韩文 

			MAX				// 枚举最大值
		}

		public static string[] m_EncoidingName = { 
											  "c中文简体",
											  "c中文繁体",
											  "t泰文",
											  "p葡萄牙文",
											  "k韩文",
									  };
		public static string[] m_EncoidingValue = { 
											   "936",
											   "950",
											   "874",
											   "28591",
											   "949",
									  };

		const string S_KEY_BINARY = "\\u";
		const string S_KEY_BLANK = "";
		const string S_KEY_SPLIT = ",";
		string S_ENCODING_NAME = m_EncoidingName[0]; // 默认
		System.Text.Encoding S_ENCODING = null;

		List<string> m_listRead = new List<string>();
		List<string> m_listWrite = new List<string>();

		public bool SetEncoding(string sEncoding)
		{
			bool bReturn = false;
			S_ENCODING = GetEncodingWithName(sEncoding);
			if (S_ENCODING != null)
			{
				bReturn = true;
				S_ENCODING_NAME = sEncoding;
			}


			return bReturn;
		}

		public string ConverBinary2Text(string strByte)
		{
			if (S_ENCODING == null)
			{
				SetEncoding(S_ENCODING_NAME);
			}
			// 先进行一些处理 
			string strTemp = strByte.Replace(S_KEY_BINARY, S_KEY_BLANK);
			strTemp = strByte.Replace(S_KEY_SPLIT, S_KEY_BLANK);
			if (strTemp.Length % 2 != 0)
			{
				strTemp += "20"; // 空格 
			}
			// 将文本转化为byte数组 
			byte[] arrByte = new byte[strTemp.Length / 2];
			for (int i = 0; i < arrByte.Length; i++)
			{
				try
				{
					// 每两个字符是一个 byte 
					arrByte[i] = byte.Parse(strTemp.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber);
				}
				catch
				{
					throw new ArgumentException("输入的不是有效的二进制编码");
				}
			}
			// 将byte数组转化为文本 
			string strText = S_ENCODING.GetString(arrByte);

			return strText;
		}

		public string ConverText2Binary(string strText)
		{
			if (S_ENCODING == null)
			{
				SetEncoding(S_ENCODING_NAME);
			}
			// 首先将文本转化为byte数组 
			byte[] arrByte = S_ENCODING.GetBytes(strText);
			// 将byte数组转化为文本 
			string strOutput = "";
			for (int i = 0; i < arrByte.Length; i++)
			{
				string strTemp = Convert.ToString(arrByte[i], 16);
				strOutput += strTemp;
				if (i != arrByte.Length - 1)
				{
					strOutput += S_KEY_SPLIT;
				}
			}

			return strOutput.ToUpper();
		}

		public string ConverBinary2Text_Unity3D(string strByte)
		{
			if (S_ENCODING == null)
			{
				SetEncoding(S_ENCODING_NAME);
			}
			// 先进行一些处理 
			string strTemp = strByte.Replace(S_KEY_BINARY, S_KEY_BLANK);
			strTemp = strByte.Replace(S_KEY_SPLIT, S_KEY_BLANK);
			if (strTemp.Length % 2 != 0)
			{
				strTemp += "20"; // 空格 
			}
			// 将文本转化为byte数组 
			byte[] arrByte = new byte[strTemp.Length / 2];
			for (int i = 0; i < arrByte.Length; i++)
			{
				try
				{
					// 每两个字符是一个 byte 
					arrByte[i] = byte.Parse(strTemp.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber);
				}
				catch
				{
					throw new ArgumentException("输入的不是有效的二进制编码");
				}
			}
			// 将byte数组转化为文本 
			string strText = S_ENCODING.GetString(arrByte);

			return strText;
		}


		public static System.Text.Encoding GetEncodingWithName(String strName)
		{
			Encoding encoding = null;
			try
			{
				encoding = Encoding.GetEncoding(strName);
			}
			catch
			{
				if (null == encoding)
				{
					encoding = Encoding.GetEncoding(Convert.ToInt32(strName));
				}
			}

			return encoding;
		}
	}
}
