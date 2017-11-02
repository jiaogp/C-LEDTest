using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LEDTestSysterm
{
	public delegate void addTextTotb(string texte);
	public partial class Main : Form
	{  //声明 ControlPmac实例
		public  ControlPmac pmac ;
		//声明 PmacTest 窗体类
		public PmacTest pmacTest;
		
	
		public Main()
		{
			InitializeComponent();
			//产生ControlPmac实例
			pmac = new ControlPmac();
			pmacTest = new PmacTest();
			addTextTotbStatus(pmac.linkToPmac());
			//将main中对象引至PmacTest
			linkToPmacTest();
		}
		/// <summary>
		/// 将main中对象引至PmacTest
		/// </summary>
		private void linkToPmacTest()
		{
			//产生PmacTest窗口实例
			pmacTest = new PmacTest();
			//将Main中的rtbStatus赋给PmacTest中的rtbStatus
			pmacTest.rtbStatus = this.rtbStatus;
			//将Main中的pmac赋给PmacTest中的pmac
			pmacTest.pmac = pmac;
			//将Main赋给PmacTest中的mm
			pmacTest.mm = this;
		}
		
		/// <summary>
		/// 将文本添加至文本框
		/// </summary>
		/// <param name="texte"></param>
		public void addTextTotbStatus(string texte)
		{
			rtbStatus.Text += texte + "\n";
			rtbStatus.SelectionStart = rtbStatus.TextLength;
			rtbStatus.ScrollToCaret();
		}
		/// <summary>
		/// 按下Pmac测试按钮
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnPmacTest_Click(object sender, EventArgs e)
		{
			pmacTest.Show();
			addTextTotbStatus("sb");
		}
	}
}
