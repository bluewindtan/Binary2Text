using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Binary2Text
{
	public partial class Form1 : Form
	{
		ConvertEncoding conEncode = null;
		public Form1()
		{
			InitializeComponent();
		}
		private void Form1_Load(object sender, EventArgs e)
		{
			// 构建数据源 
			DataTable dataT1 = new DataTable();
			dataT1.Columns.Add("name");
			dataT1.Columns.Add("value");
			DataTable dataT2 = new DataTable();
			dataT2.Columns.Add("name");
			dataT2.Columns.Add("value");
			for (int i = 0; i < (int)ConvertEncoding.FileEncoding.MAX; i++)
			{
				dataT1.Rows.Add(new string[] { ConvertEncoding.m_EncoidingName[i], ConvertEncoding.m_EncoidingValue[i] });
				dataT2.Rows.Add(new string[] { ConvertEncoding.m_EncoidingName[i], ConvertEncoding.m_EncoidingValue[i] });
			}

			// 绑定下拉框
			_InitComboBox(comboBox1, dataT1);

			conEncode = new ConvertEncoding();
		}

		private void _InitComboBox(ComboBox cb, DataTable dt)
		{
			cb.DataSource = dt;
			cb.DisplayMember = "name";
			cb.ValueMember = "value";
			cb.AutoCompleteSource = AutoCompleteSource.ListItems;
			cb.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			string strText = textBox1.Text;
			if (strText.Length == 0)
			{
				MessageBox.Show("请输入想转换的内容！");
				return;
			}
			conEncode.SetEncoding(comboBox1.SelectedValue.ToString());
			if (checkBox1.Checked)
			{

			}
			else
			{
				textBox2.Text = conEncode.ConverBinary2Text(strText);
			}
		}

		private void button2_Click(object sender, EventArgs e)
		{
			string strText = textBox2.Text;
			if (strText.Length == 0)
			{
				MessageBox.Show("请输入想转换的内容！");
				return;
			}
			conEncode.SetEncoding(comboBox1.SelectedValue.ToString());
			textBox1.Text = conEncode.ConverText2Binary(strText);

		}

	}
}
