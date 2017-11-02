using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Timers;

namespace LEDTestSysterm
{
	public partial class PmacTest : Form
	{
		public bool movedZero=true;
		public bool isGetData;
		public bool moveComplete;
		public string curStatus,completeStatus;
		public RichTextBox rtbStatus;
		public Main mm;
		public ControlPmac pmac;
		public PmacCommandStruct pmacCommand;
		public double[] motionStatus;
		public System.Timers.Timer  collectDataTimer=new System.Timers.Timer(15);
		private const int jogStep = 50;
		public PmacTest()
		{
			InitializeComponent();
			this.ControlBox = false;
			collectDataTimer.Elapsed += new ElapsedEventHandler(collectData);
		}
		/// <summary>
		/// 下载程序并开始读数（判断命令字符串是否通过验证，不通过的话显示错误信息）
		/// </summary>
		/// <param name="errString"></param>
		private void checkAndStartThread()
		{
			pmacCommand = new PmacCommandStruct();
			if (pmacCommand.isValided)
			{
				ableGroupBox(false);
				downloadToPmac();
			}
			else
			{
				MessageBox.Show(pmac.pmacStatus+ "。请确认操作流程、参数设置是否正确", "警告！");
			}
		}
		/// <summary>
		/// 开始下载命令打开获取数据开关
		/// </summary>
		private void downloadToPmac()
		{
			bool success=false;
			try
			{
				switch (pmacCommand.dataType)
				{
					case CollectDataType.Both:
						collectDataTimer.Enabled = false;
						if (pmacCommand.moveCommand != "")
						success=pmac.doPmacCommand(pmacCommand.moveCommand);
						pmacCommand.moveCommand = "";
						if (!success)
						{
							moveComplete = false;
						}
						isGetData = true;
						collectDataTimer.Enabled = true;
						BeginInvoke(new showToRtb(showToRTB), pmac.pmacStatus);
						break;
					default:
						break;
				}
			}
			catch(Exception ee)
			{
					MessageBox.Show(ee.ToString() + "下载Pmac运动指令错误！", "警告！");
			}
		}
		public DateTime datenow;
		/// <summary>
		/// 采集数据
		/// </summary>
		/// <param name="source"></param>
		/// <param name="e"></param>
		public void collectData(object source,ElapsedEventArgs e)
		{
			if (isGetData)
			{
				if (datenow != DateTime.Now)
				{
					datenow = DateTime.Now;
					motionStatus = pmac.getPmacData(pmac.PmacCommand.flag,pmac.PmacCommand.flagLength);
					if (pmac.sttB._7 == "1") { }
					if (pmac.pmacRunComplete == 1)
					{
						isGetData = false;
					}
					BeginInvoke(new  addDataTo(addDataToUI),motionStatus);
				}
			}
			else
			{
				motionStatus = pmac.getPmacData(pmac.PmacCommand.flag, pmac.PmacCommand.flagLength);
				if (pmac.sttB._7 == "1") { }
				BeginInvoke(new addDataTo(addDataToUI), motionStatus);
				collectDataTimer.Enabled = false;
				stopCollectData();
			}
		}
		private void stopCollectData()
		{
			try
			{
				BeginInvoke(new ableBox(ableGroupBox), true);
				if (movedZero == false && pmac.pmacStatus.Contains("正在回零"))
					movedZero = true;
				pmac.pmacRunComplete = 1;
			}
			catch(Exception e)
			{
				MessageBox.Show(e.ToString(), "警告！");
			}
		}

		 private delegate void addDataTo(double[] motionStatus);
		/// <summary>
		/// 将数据刷新至界面
		/// </summary>
		/// <param name="motionStatus"></param>
		private void addDataToUI(double[] motionStatus)
		{
			if (motionStatus != null)
			{
				tbXPosition.Text = motionStatus[0].ToString();
				tbYPosition.Text = motionStatus[2].ToString();
				tbZPosition.Text = motionStatus[4].ToString();
			}
		}
		delegate void showToRtb(string str);
		private void showToRTB(string str)
		{
			mm.addTextTotbStatus(DateTime.Now + "\t" + str);
		}
		private delegate void ableBox(bool b);
		private void ableGroupBox(bool b)
		{
			cbbGoTo.Enabled = b;
			cbbMotionSelect.Enabled = b;
			btnCLoseForm.Enabled = b;
			btnGoZero.Enabled = b;
			btnJogNegative.Enabled = b;
			btnJogPositive.Enabled = b;
		}
		#region 按钮事件
		/// <summary>
		/// 按下关闭窗口
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnCLoseForm_Click(object sender, EventArgs e)
		{
			//mm.addTextTotbStatus("sb");
			this.Hide();
		}
		/// <summary>
		/// 按下回零按钮
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnGoZero_Click(object sender, EventArgs e)
		{
			if (movedZero)
			{
				pmacCommand = new PmacCommandStruct();
				pmacCommand.isValided = false;
				try
				{
					switch (cbbMotionSelect.SelectedItem.ToString())
					{
						case "X轴":
							pmacCommand = pmac.getCommand(GoZero.X);
							curStatus = "正在进行X轴回零";
							completeStatus = "X轴回零完成";
							break;
						case "Y轴":
							pmacCommand = mm.pmac.getCommand(GoZero.Y);
							curStatus = "正在进行Y轴回零";
							completeStatus = "Y轴回零完成";
							break;
						case "Z轴":
							pmacCommand = mm.pmac.getCommand(GoZero.Z);
							curStatus = "正在进行Z轴回零";
							completeStatus = "Z轴回零完成";
							break;
						default:
							MessageBox.Show("参数输入错误！", "警告！");
							break;
					}
					checkAndStartThread();
				}
				catch(Exception ee)
				{
					MessageBox.Show("参数输入错误！" + ee.Message, "警告！");
				}
			}
			else
			{
				MessageBox.Show("请先寻零", "警告");
			}
		}
		/// <summary>
		/// 按下停止按钮
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnStopMotion_Click(object sender, EventArgs e)
		{
			pmacCommand = new PmacCommandStruct();
			pmacCommand.isValided = false;
			try
			{
				switch (cbbMotionSelect.SelectedItem.ToString())
				{
					case "X轴":
						pmacCommand = pmac.getCommand(MotionKill.X);
						curStatus = "正在进行X轴电机停止";
						completeStatus = "X轴电机已经停止";
						break;
					case "Y轴":
						pmacCommand = pmac.getCommand(MotionKill.Y);
						curStatus = "正在进行Y轴电机停止";
						completeStatus = "Y轴电机已经停止";
						break;
					case "Z轴":
						pmacCommand = pmac.getCommand(MotionKill.Z);
						curStatus = "正在进行Z轴电机停止";
						completeStatus = "Z轴电机已经停止";
						break;
					default:
						MessageBox.Show("参数输入错误！", "警告！");
						break;
				}
			}
			catch(Exception ee)
			{
				MessageBox.Show("参数输入错误！" + ee.Message, "警告！");
			}
		 }
		
		/// <summary>
		/// 按下正向点动按钮
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnJogPositive_Click(object sender, EventArgs e)
		{
			
			if (movedZero)
			{
				try
				{
					pmacCommand = new PmacCommandStruct();
					pmacCommand.isValided = false;
					switch (cbbMotionSelect.SelectedItem.ToString())
					{
						case "X轴":
							pmacCommand = mm.pmac.getCommand(MoveInching.XPositive,jogStep);
							curStatus = "正在进行X轴正向点动";
							completeStatus = "X轴正向点动完成";
							break;
						case "Y轴":
							pmacCommand = mm.pmac.getCommand(MoveInching.YPositive, jogStep);
						    curStatus = "正在进行Y轴正向点动";
							completeStatus = "Y轴正向点动完成";
							break;
						case "Z轴":
							pmacCommand = mm.pmac.getCommand(MoveInching.ZPositive ,jogStep);
							curStatus = "正在进行Z轴正向点动";
							completeStatus = "Z轴正向点动完成";
							break;
						default:
							MessageBox.Show("参数输入错误！", "警告！");
							break;
					}
					checkAndStartThread();
				}
				catch(Exception ee)
				{
					MessageBox.Show("参数输入错误！"+ee.Message, "警告！");
				}
			}
			else
			{
				MessageBox.Show("请先寻零", "警告");
			}

		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cbbGoTo_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (movedZero)
			{ 
				try
				{
					switch (cbbGoTo.SelectedItem.ToString())
						{
					case "LED1":
						pmac.PmacCommand = pmac.getCommand(Moving.ToOne);
						curStatus = "正在运动到LED1";
						completeStatus = "运动到LED1完成";
						break;
					case "LED2":
						pmac.PmacCommand = pmac.getCommand(Moving.ToTwo);
						curStatus = "正在运动到LED2";
						completeStatus = "运动到LED2完成";
						break;
					case "LED3":
						pmac.PmacCommand = pmac.getCommand(Moving.ToThree);
						curStatus = "正在运动到LED3";
						completeStatus = "运动到LED3完成";
						break;
					case "LED4":
						pmac.PmacCommand = pmac.getCommand(Moving.ToFour);
						curStatus = "正在运动到LED4";
						completeStatus = "运动到LED4完成";
						break;
					case "LED5":
						pmac.PmacCommand = pmac.getCommand(Moving.ToFive);
						curStatus = "正在运动到LED5";
						completeStatus = "运动到LED5完成";
						break;
					case "LED6":
						pmac.PmacCommand = pmac.getCommand(Moving.ToSix);
						curStatus = "正在运动到LED6";
						completeStatus = "运动到LED6完成";
						break;
					case "LED7":
						pmac.PmacCommand = pmac.getCommand(Moving.ToSeven);
						curStatus = "正在运动到LED7";
						completeStatus = "运动到LED7完成";
						break;
					case "LED8":
						pmac.PmacCommand = pmac.getCommand(Moving.ToEight);
						curStatus = "正在运动到LED8";
						completeStatus = "运动到LED8完成";
						break;
					case "LED9":
						pmac.PmacCommand = pmac.getCommand(Moving.ToNine);
						curStatus = "正在运动到LED9";
						completeStatus = "运动到LED9完成";
						break;
					case "LED10":
						pmac.PmacCommand = pmac.getCommand(Moving.ToTen);
						curStatus = "正在运动到LED10";
						completeStatus = "运动到LED10完成";
						break;
					case "起始位置":
						pmac.PmacCommand = pmac.getCommand(Moving.ToZero);
						curStatus = "正在运动到起始位置";
						completeStatus = "运动到起始位置完成";
						break;
					default:
						MessageBox.Show("参数输入错误！", "警告！");
						break;
					}
				checkAndStartThread();
				}
				catch(Exception ee)
				{
				MessageBox.Show("参数输入错误！" + ee.Message, "警告！");
				}
			}
			else
			{
				MessageBox.Show("请先寻零", "警告");
			}
		}

		/// <summary>
		/// 按下负向点动
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnJogNegative_Click(object sender, EventArgs e)
		{
			if (movedZero)
			{
				try
				{
					pmacCommand = new PmacCommandStruct();
					pmacCommand.isValided = false;
					switch (cbbMotionSelect.SelectedItem.ToString())
					{
						case "X轴":
							pmacCommand = mm.pmac.getCommand(MoveInching.XNegative, jogStep);
							curStatus = "正在进行X轴负向点动";
							completeStatus = "X轴负向点动完成";
							break;
						case "Y轴":
							pmacCommand = mm.pmac.getCommand(MoveInching.YNegative, jogStep);
							curStatus = "正在进行Y轴负向点动";
							completeStatus = "Y轴负向点动完成";
							break;
						case "Z轴":
							pmacCommand = mm.pmac.getCommand(MoveInching.ZNegative, jogStep);
							curStatus = "正在进行Z轴负向点动";
							completeStatus = "Z轴负向点动完成";
							break;
						default:
							MessageBox.Show("参数输入错误！", "警告！");
							break;
					}
					checkAndStartThread();
				}
				catch(Exception ee)
				{
					MessageBox.Show("参数输入错误！" + ee.Message, "警告！");
				}
			}
			else
			{
				MessageBox.Show("请先寻零", "警告");
			}
		}
		#endregion
	}
}
