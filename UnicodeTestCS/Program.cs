using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace UnicodeTestCS
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                return;
            }
            Program p = new Program();
            FileStream fs = new FileStream(args[0], FileMode.Open);
            FileStream fsout = new FileStream(args[1], FileMode.Create);
            string strTxt = args[1];
            strTxt += ".txt";
            FileStream fsouttxt = new FileStream(strTxt, FileMode.Create);
            StreamReader sr = new StreamReader(fs);
            StreamWriter sw = new StreamWriter(fsout);
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
                //if (str.Contains("mText"))
                //{
                //    Console.WriteLine(str);
                //    string xxx = p.Unicode2Chinese(str); ;//str.Replace(@"\\u", @"\u");
                //}
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
            fs.Close();
            fsout.Close();
            fsouttxt.Close();
            //string abc = p.Unicode2Chinese("\\uff1a\\uFF1A\\uFF1A abc");
            //string abcd = "abc\\uFF1A\\uFF1Aabc";
            //string xx = abcd.Replace(@"\\u", @"\u");
        }
        //private string Unicode2Chinese(string strUnicode)
        //{
        //    string header = strUnicode.Substring(0, 2);
        //    bool bHeaderUnicode = false;
        //    if (header.Equals("\\u"))
        //    {
        //        bHeaderUnicode = true;
        //    }
        //    string[] splitString = new string[1];
        //    splitString[0] = "\\u";
        //    string[] unicodeArray = strUnicode.Split(splitString, StringSplitOptions.RemoveEmptyEntries);
        //    StringBuilder sb = new StringBuilder();

        //    foreach (string item in unicodeArray)
        //    {
        //        if (item.Length == 4)
        //        {
        //            byte[] codes = new byte[2];
        //            int code1, code2;
        //            code1 = Convert.ToInt32(item.Substring(0, 2), 16);
        //            code2 = Convert.ToInt32(item.Substring(2), 16);
        //            codes[0] = (byte)code2;//必须是小端在前
        //            codes[1] = (byte)code1;
        //            sb.Append(Encoding.Unicode.GetString(codes));
        //        }
        //        else
        //        {
        //            sb.Append(item);
        //        }
        //    }

        //    return sb.ToString();
        //}
    }
}
