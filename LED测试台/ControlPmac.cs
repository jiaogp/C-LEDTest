using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PCOMMSERVERLib;
namespace LEDTestSysterm
{
public 	class ControlPmac
	{
		// 实例化pmac
	    public PmacDeviceClass pmac;
		//Pmac命令
		public PmacCommandStruct PmacCommand;
		//m_nDevice>PMAC编号,与pmac通讯返回的整数值
		public  int m_nDevice, pStatus;
		//m_bDeviceOpen>是否打开设备
		public  bool m_bDeviceOpen;
		//Pmac当前状态,与Pmac通讯的返回字符串值
		public   string pmacStatus, pAnswer;
		//Pmac运行状态字
		public statusByte sttB = new statusByte();
		//计算位置、速度时使用，默认值都为96
		private int I107 = 96, I108 = 96, I109 = 96, I207 = 96, I208 = 96, I209 = 96,I307 = 96,I308 = 96,I309 = 96, I10 = 3421866;
		//Pmac运动程序完成标志
		public int pmacRunComplete;
		//Pmac设备状态是否可以下达命令
		private bool CheckDeviceStatus
		{
			get
			{
				if (m_bDeviceOpen)
				{
					if (sttB._2 != "1" && sttB._3 != "1" && sttB._3 != "1")
						return true;
					else
					{
						sttB._0 = "1";
						pmacStatus = "电机正在运行！";
					}
				}
				else
				{
					sttB._5 = "1";
					pmacStatus = "下位机未连接！";
				}
				return false;
			}
		}
		/// <summary>
		/// ControlPmac类构造函数
		/// </summary>
		public ControlPmac()
		{
			pmac = new PmacDeviceClass();
			pmacStatus = linkToPmac();
			sttB = getPmacStatus();
		}

		#region 与Pmac通讯的方法

		//返回字符串数组
		private string[] strAnswer;
		//返回整数数组
		private int[] iAnswer;
		//采回的浮点类型数组
		private double[] dAnswer;
		//要采集的数，M162实际位置，M274平均实际速度，M137运行程序位，M139驱动器使能位，M121,M122极限输入状态
		private string collectData= "M162 M174 M262 M274 M362 M374 M137 M237 M337 M139 M239 M339 M4005 M121 M122 M221 M222 M321 M322";
		public  double xPosition, xVelocity,yPosition, yVelocity,zPosition, zVelocity;
		private double ctsToMM=1, ctsToMMVel=1;
		/// <summary>
		/// 连接Pmac
		/// </summary>
		/// <returns>Pmac状态</returns>
		public string linkToPmac()
		{
			try
			{
				if (m_bDeviceOpen)
				{
					pmac.Close(m_nDevice);
				}
				for (int i = 0; i < 8; i++)
				{
					pmac.Open(i, out m_bDeviceOpen);
					if (m_bDeviceOpen)
					{
						m_nDevice = i;
						i = 10;
					}
				}
				if (m_bDeviceOpen)
				{
					pmacStatus = "下位机通信正常";
				}
				else
				{
					pmacStatus = "下位机连接失败";
				}
				return pmacStatus;
			}
			catch(Exception e)
			{
				pmacStatus = "连接下位机时程序发生错误" + e.Message;
				return pmacStatus;
			}
		}
		/// <summary>
		/// 判断I变量值，返回Pmac状态
		/// </summary>
		/// <returns>Pmac状态</returns>
		public statusByte getPmacStatus()
		{
			statusByte sttByte = new statusByte();
			try
			{
				if (m_bDeviceOpen) {
					//10个I变量值是否正确----------M变量状态
					//I107,I108,I109比例因子，I10伺服中断时间，M121-Lim(正向极限)输入状态，M122+Lim(负向极限)输入状态，M137运行程序位，M139驱动器使能
					strAnswer = getOnlyAnswer("I107 I207 I307 I108 I208 I308 I109 I209 I309 I10 M121 M122 M221 M222 M321 M322 M137 M237 M337 M139 M239 M339 M4005").Split('\n','\r');
					iAnswer = Array.ConvertAll(strAnswer, new Converter<String, Int32>(strToInt32));
					if (iAnswer[0]!=I107|| iAnswer[1] != I207 || iAnswer[2] != I307 || iAnswer[3] != I108 || iAnswer[4] != I208 || iAnswer[5] != I308 ||
						iAnswer[6] != I109 || iAnswer[7] != I209 || iAnswer[8] != I309 || iAnswer[9] != I10 )
					{
						getOnlyAnswer("I107=96 I207=96 I307=96 I108=96 I208=96 I308=96 I109=96 I209=96 I309=96 I10=3421866 SAV ");
					}
					if (iAnswer[10] == 1 || iAnswer[11] == 1 || iAnswer[12] == 1 || iAnswer[13] == 1 || iAnswer[14] == 1 || iAnswer[15] == 1)
						sttByte._7 ="1";
					else sttByte._7 = "0";
					if (iAnswer[16] == 1)
						sttByte._2 = "1";
					else sttByte._2 = "0";
					if (iAnswer[17] == 1)
						sttByte._3 = "1";
					else sttByte._3 = "0";
					if (iAnswer[18] == 1)
						sttByte._4 = "1";
					else sttByte._3 = "0";
					if (iAnswer[19] == 0 || iAnswer[20] == 0 || iAnswer[21] == 0)
						sttByte._6= "1";
					else sttByte._3 = "0";
					if (iAnswer[22] == 0)
						sttByte._7 = "1";
					else sttByte._7 = "0";
				}
				else
				{
					sttByte._5 = "1";
					pmacStatus = "失去与下位机的连接！";
				}
			}
			catch
			{
				sttByte._1 = "1";
				pmacStatus = "获取Pmac状态字时程序出错";
			}
			return sttByte;
		}
		/// <summary>
		/// 采集Pmac数据
		/// </summary>
		/// <param name="flag">标志位</param>
		/// <param name="flagLength">标志位长度</param>
		/// <returns></returns>
		public double[] getPmacData(string flag,int flagLength)
		{
			sttB = new statusByte();
			if (m_bDeviceOpen)
			{
				try
				{//collectData= "M162 M174 M262 M274 M362 M374 M137 M237 M337 M139 M239 M339 M4005 M121 M122 M221 M222 M321 M322";
					strAnswer = getOnlyAnswer(collectData + flag).Split('\n', '\r');
					dAnswer = Array.ConvertAll(strAnswer, new Converter<string, double>(strToDouble));
					xPosition = Math.Round(dAnswer[0] * ctsToMM, 4);
					xVelocity = Math.Round(dAnswer[1] * ctsToMMVel, 4);
					yPosition = Math.Round(dAnswer[2] * ctsToMM, 4);
					yVelocity = Math.Round(dAnswer[3] * ctsToMMVel, 4);
					zPosition = Math.Round(dAnswer[4] * ctsToMM, 4);
					zVelocity = Math.Round(dAnswer[5] * ctsToMMVel, 4);
					if (strAnswer[9] =="1" || Math.Round(xVelocity, 2) !=0)
						sttB._2 = "1";
					else sttB._2 = "0";
					if (strAnswer[10] =="1" || Math.Round(yVelocity, 2) != 0)
						sttB._3 = "1";
					else sttB._3 = "0";
					if (strAnswer[11] =="1" || Math.Round(zVelocity, 2) != 0)
						sttB._4 = "1";
					else sttB._4 = "0";
					if (strAnswer[6] == "0" || strAnswer[7] == "0" || strAnswer[8] == "0")
						sttB._6 = "1";
					else sttB._6 = "0";
					if (strAnswer[12] == "1")
						sttB._7 = "1";
					else sttB._7 = "0";
					if (strAnswer[13] == "1" || strAnswer[14] == "1" || strAnswer[15] == "1" || strAnswer[16] == "1")
						sttB._0 = "1";
					else sttB._0 = "0";
					if (flagLength > 0)
					{
						for (int i = 0; i < flagLength; i++)
						{
							pmacRunComplete += (int)dAnswer[13 + i];
						}
						if (pmacRunComplete == flagLength && sttB._2 == "0" && sttB._3 == "0" && sttB._4 == "0")
						{
							pmacRunComplete = 1;
						}
						else
						{
							pmacRunComplete = 0;
						}
					}
					return new double[] { xPosition, xVelocity, yPosition, yVelocity, zPosition, zVelocity };
				}
				catch
				{
					sttB._1 = "1";
					pmacStatus = "执行下位机程序时程序发生错误";
					return null;
				}
			}
			else
			{
				sttB._5 = "1";
				pmacStatus = "失去与下位机的连接";
				return null;
			}
		}
		/// <summary>
		/// 只返回回应字符串的pmac通讯接口
		/// </summary>
		/// <param name="question">命令字符串</param>
		/// <returns>回回应字符串</returns>
		public string getOnlyAnswer(string question)
		{
			try
			{
				pmac.GetResponseEx(m_nDevice, question, true, out pAnswer, out pStatus);
				if (pStatus == 0)
				{
					sttB._5 = "1";
					m_bDeviceOpen = false;
				}
				return pAnswer;
			}
			catch (Exception e)
			{
				sttB._1 = "1";
				pmacStatus = "执行下位机程序时程序发生错误" + e.Message;
				return string.Empty;
			}
		}
		/// <summary>
		/// 下发Pmac在线指令,
		/// </summary>
		/// <returns>完成时返回Ture</returns>
		public bool doPmacCommand(string  pmacCommandString)
		{
			try
			{
				if (m_bDeviceOpen)
				{ getOnlyAnswer(pmacCommandString);
					return true;
				}
				else
				{
					sttB._5 = "1";
					return false;
				}
			}
			catch(Exception e)
			{
				sttB._1 = "1";
				pmacStatus = "执行下位机命令时程序出错" + e.Message + e.GetType().ToString();
				return false;
			}

		}
		/// <summary>
		/// 判断电机运动,改变Pmac状态字
		/// </summary>
		public void isMoving()
		{
			try
			{
				if (m_bDeviceOpen)
				{//M137程序运行位，M139驱动器使能位，M121,M122极限到位，M174平均实际速度
					strAnswer = getOnlyAnswer("M137 M237 M337 M139 M239 M339 M4005 M121 M122 M221 M222 M321 M322 M174 M274 M374").Split('\n', '\r');
					iAnswer = Array.ConvertAll(strAnswer,new Converter<String, Int32>(strToInt32));
					if (strAnswer[0] == "1" || Math.Round((Convert.ToDouble(strAnswer[13])), 2) != 0)
						sttB._2 = "1";
					else sttB._2 = "0";
					if (strAnswer[1] == "1" || Math.Round((Convert.ToDouble(strAnswer[14])), 2) != 0)
						sttB._3 = "1";
					else sttB._3 = "0";
					if (strAnswer[2] == "1" || Math.Round((Convert.ToDouble(strAnswer[15])), 2) != 0)
						sttB._2 = "1";
					else sttB._2 = "0";
					if (strAnswer[3] == "0" || strAnswer[4] == "0" || strAnswer[5] == "0")
						sttB._6 = "1";
					else sttB._6 = "0";
					if (strAnswer[6] == "0")
						sttB._7 = "1";
					else sttB._7 = "0";
					if (strAnswer[7] == "1" || strAnswer[8] == "1" || strAnswer[9] == "1" || strAnswer[10] == "1"||strAnswer[11] == "1"|| strAnswer[12] == "1")
						sttB._0 = "1";
					else sttB._0 = "0";
				}
				else
				{
					sttB._5 = "1";
					pmacStatus = "失去与下位机的连接！";
				}
			}
			catch
			{
				sttB._1 = "1";
				pmacStatus = "判断电机运动时程序出错";
			}
		}


		#endregion 与Pmac通讯的方法
		#region 数据转换方法
		/// <summary>
		/// 将string数组转换为double
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		private double strToDouble(string str)
		{
			if (str == "")
				return double.Parse("0");
			else
				return double.Parse(str);
		}
		/// <summary>
		/// 将string数组转换为int32
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		private  Int32 strToInt32(String str)
		{
			if (str == "")
				return Int32.Parse("0");
			else
			return Int32.Parse(str);
		}
		/// <summary>
		/// 将string数组转换为Long数组
		/// </summary>
		/// <param name="str">字符串数组中的元素</param>
		/// <returns></returns>
		private  Int64 strToInt64(String str)
		{
			if (str == "")
				return Int64.Parse("0");
			else
				return Int64.Parse(str);
		}
		#endregion 数据转换方法
		#region 生成命令字符方法
		/// <summary>
		/// 生成点动命令
		/// </summary>
		/// <param name="moveInch">轴</param>
		/// <param name="step">步数</param>
		/// <returns></returns>
		public PmacCommandStruct getCommand(MoveInching moveInch, int step)
		{
			PmacCommand = new PmacCommandStruct();
			PmacCommand.isValided = false;
			isMoving();
			try
			{
				if (CheckDeviceStatus)
					switch (moveInch)
					{
						case MoveInching.XPositive:
							if (strAnswer[7] == "0")
								PmacCommand = new PmacCommandStruct(true, "#1J+" + step.ToString(), "M133", 1, CollectDataType.Both);
							break;
						case MoveInching.XNegative:
							if (strAnswer[8] == "0")
								PmacCommand = new PmacCommandStruct(true, "#1J-" + step.ToString(), "M133", 1, CollectDataType.Both);
							break;
						case MoveInching.YPositive:
							if (strAnswer[9] == "0")
								PmacCommand = new PmacCommandStruct(true, "#2J+" + step.ToString(), "M233", 1, CollectDataType.Both);
							break;
						case MoveInching.YNegative:
							if (strAnswer[10] == "0")
								PmacCommand = new PmacCommandStruct(true, "#2J-" + step.ToString(), "M233", 1, CollectDataType.Both);
							break;
						case MoveInching.ZPositive:
							if (strAnswer[11] == "0")
								PmacCommand = new PmacCommandStruct(true, "#3J+" + step.ToString(), "M333", 1, CollectDataType.Both);
							break;
						case MoveInching.ZNegative:
							if (strAnswer[12] == "0")
								PmacCommand = new PmacCommandStruct(true, "#3J-" + step.ToString(), "M333", 1, CollectDataType.Both);
							break;
						default:
							sttB._0 = "1";
							sttB._7 = "1";
							pmacStatus = "电机已达到极限位置！";
							break;

					}
			}
			catch
			{
				sttB._1 = "1";
				pmacStatus = "生成运动命令时程序发生错误！";
			}
			return PmacCommand;
		}
		/// <summary>
		/// 生成回零命令
		/// </summary>
		/// <param name="gozero">回零轴</param>
		/// <returns></returns>
		public  PmacCommandStruct getCommand(GoZero gozero)
		{
			PmacCommand = new PmacCommandStruct();
			PmacCommand.isValided = false;
			isMoving();
			try
			{
				if (CheckDeviceStatus)
				{
					switch (gozero)
					{
						case GoZero.X:
							PmacCommand = new PmacCommandStruct(true,"P1=1","P13 M113",2,CollectDataType.Both);
							break;
						case GoZero.Y:
							PmacCommand = new PmacCommandStruct(true, "P2=1", "P23 M213", 2, CollectDataType.Both);
							break;
						case GoZero.Z:
							PmacCommand = new PmacCommandStruct(true, "P3=1", "P33 M313", 2, CollectDataType.Both);
							break;
						case GoZero.Both:
							PmacCommand = new PmacCommandStruct(true, "P11=1", "P113 M113 M213 M313", 4, CollectDataType.Both);
							break;
						default:
							sttB._0 = "1";
							pmacStatus = "生成命令参数错误";
							break;
					}
				 }
				else
				{
					sttB._5 = "1";
					pmacStatus = "下位机连接错误！";
				}
			}
			catch(Exception e)
			{
				sttB._1 = "1";
				pmacStatus = "生成运动命令时程序发生错误！"+e.Message;
			}
			return PmacCommand;
		}
		/// <summary>
		/// 生成停止轴命令
		/// </summary>
		/// <param name="motionKill"></param>
		/// <returns></returns>
		public PmacCommandStruct getCommand(MotionKill motionKill)
		{
			PmacCommand = new PmacCommandStruct();
			PmacCommand.isValided = false;
			isMoving();
			try
			{
				if (m_bDeviceOpen)//停止命令只要设备打开就可以生成
				{
					switch (motionKill)
					{
						case MotionKill.X:
							PmacCommand= new PmacCommandStruct(true, "A #1K ",  "M138", 1, CollectDataType.Both);
							break;
						case MotionKill.Y:
							PmacCommand = new PmacCommandStruct(true, "A #2K ", "M238", 1, CollectDataType.Both);
							break;
						case MotionKill.Z:
							PmacCommand = new PmacCommandStruct(true, "A #3K ", "M338", 1, CollectDataType.Both);
							break;
						case MotionKill.Both:
							PmacCommand = new PmacCommandStruct(true, "A #1K  #2K #3K ", "M138 M238 M338", 3, CollectDataType.Both);
							break;
						default:
							sttB._0 = "1";
							pmacStatus = "生成命令参数错误";
							break;
					}
				}
				else
				{
					sttB._5 = "1";
					pmacStatus = "下位机连接错误！";
				}
			}
			catch(Exception e)
			{
				sttB._1 = "1";
				pmacStatus = "生成运动命令时程序发生错误！" + e.Message;
			}
			return PmacCommand;
		}
		/// <summary>
		/// 生成运动到位置命令
		/// </summary>
		/// <param name="move"></param>
		/// <returns></returns>
		public PmacCommandStruct getCommand(Moving move)
		{
			PmacCommand = new PmacCommandStruct();
			PmacCommand.isValided = false;
			isMoving();
			try
			{
				if (CheckDeviceStatus)
				{//pmac连接正常并且电机不在运动
					switch (move)
					{
						case Moving.ToOne:
							PmacCommand = new PmacCommandStruct(true,"P4=1","P34 M133 M233 M333",4,CollectDataType.Both);
							break;
						case Moving.ToTwo:
							PmacCommand = new PmacCommandStruct(true, "P5=1", "P35 M133 M233 M333", 4, CollectDataType.Both);
							break;
						case Moving.ToThree:
							PmacCommand = new PmacCommandStruct(true, "P6=1", "P36 M133 M233 M333", 4, CollectDataType.Both);
							break;
						case Moving.ToFour:
							PmacCommand = new PmacCommandStruct(true, "P7=1", "P37 M133 M233 M333", 4, CollectDataType.Both);
							break;
						case Moving.ToFive:
							PmacCommand = new PmacCommandStruct(true, "P8=1", "P38 M133 M233 M333", 4, CollectDataType.Both);
							break;
						case Moving.ToSix:
							PmacCommand = new PmacCommandStruct(true, "P9=1", "P39 M133 M233 M333", 4, CollectDataType.Both);
							break;
						case Moving.ToSeven:
							PmacCommand = new PmacCommandStruct(true, "P50=1", "P60 M133 M233 M333", 4, CollectDataType.Both);
							break;
						case Moving.ToEight:
							PmacCommand = new PmacCommandStruct(true, "P51=1", "P61 M133 M233 M333", 4, CollectDataType.Both);
							break;
						case Moving.ToNine:
							PmacCommand = new PmacCommandStruct(true, "P52=1", "P62 M133 M233 M333", 4, CollectDataType.Both);
							break;
						case Moving.ToTen:
							PmacCommand = new PmacCommandStruct(true, "P53=1", "P63 M133 M233 M333", 4, CollectDataType.Both);
							break;
						case Moving.ToZero:
							PmacCommand = new PmacCommandStruct(true, "P54=1", "P64 M133 M233 M333", 4, CollectDataType.Both);
							break;
						default:
							sttB._0 = "1";
							pmacStatus = "生成命令参数错误";
							break;
					}
				}
				else
				{
					sttB._5 = "1";
					pmacStatus = "下位机连接错误！";
				}
			}
			catch(Exception e)
			{
				sttB._1 = "1";
				pmacStatus = "生成运动命令时程序发生错误！" + e.Message;
			}
			return PmacCommand;
		}
		/// <summary>
		/// 生成复位pmac命令
		/// </summary>
		/// <returns></returns>
		public PmacCommandStruct getCommand()
		{
			PmacCommand = new PmacCommandStruct();
			PmacCommand.isValided = false;
			isMoving();
			try
			{
				if (CheckDeviceStatus)
				{//pmac连接正常并且电机不在运动
					PmacCommand = new PmacCommandStruct(true, "P55=1", "P65", 1, CollectDataType.Both);
				}
				else
				{
					sttB._5 = "1";
					pmacStatus = "下位机连接错误！";
				}
			}
			catch
			{
				sttB._1 = "1";
				pmacStatus = "生成运动命令时程序发生错误！";
			}
			return PmacCommand;
		}

#endregion 生成命令字符方法

	}

	#region 状态字类
	/// <summary>
	/// 状态字
	/// </summary>
	public class statusByte
	{
		/// <summary>
		/// 参数有误,或未回零,或达到极限位置=1
		/// </summary>
		public string _0 = "0";
		/// <summary>
		/// 程序出错=1
		/// </summary>
		public string _1 = "0";
		/// <summary>
		/// X轴在运行=1
		/// </summary>
		public string _2 = "0";
		/// <summary>
		/// Y轴在运行=1
		/// </summary>
		public string _3 = "0";
		/// <summary>
		/// Z轴在运行=1
		/// </summary>
		public string _4 = "0";
		/// <summary>
		/// 控制卡未上电或连接失败=1
		/// </summary>
		public string _5 = "0";
		/// <summary>
		/// 驱动器未使能=1
		/// </summary>
		public string _6 = "0";
		/// <summary>
		/// 拍下急停=1
		/// </summary>
		public string _7 = "0";
		/// <summary>
		/// 状态字（只读）
		/// </summary>
		public byte stByte
		{
			get
			{
				return Convert.ToByte(_7 + _6 + _5 + _4 + _3 + _2 + _1 + _0, 2);
			}
		}
	}
	#endregion
	/// <summary>
	/// Pmac命令结构体
	/// </summary>
	public struct PmacCommandStruct
	{
		public bool isValided;
		public string moveCommand;
		public string flag;
		public int flagLength;
		 public CollectDataType dataType;
		/// <summary>
		/// Pmac命令
		/// </summary>
		/// <param name="isValid">命令可用性</param>
		/// <param name="moveCommand">运动命令</param>
		/// <param name="flag"> 标志位</param>
		/// <param name="flagLength">标志位长度</param>
		/// <param name="dataType">采集数据类型</param>
		public PmacCommandStruct(bool isValid,string moveCommand, string flag, int flagLength, CollectDataType dataType)
		{
			this.isValided = isValid;
			this.moveCommand = moveCommand;
			this.flag = flag;
			this.flagLength = flagLength;
			this.dataType = dataType;
		}
	}
	
	#region 自定义数据类型
	/// <summary>
	/// 采集数据
	/// </summary>
	public enum CollectDataType
	{   /// <summary>
		/// 运动数据
		// </summary>
		MotionData,
		/// <summary>
		/// 标志位数据
		/// </summary>
		FlagData,
		/// <summary>
		/// 全部数据
		/// </summary>
		Both,
	}
	/// <summary>
	/// 回零
	/// </summary>
	public enum GoZero
	{   /// <summary>
		/// 回零X轴
		/// </summary>
		X,
		/// <summary>
		/// 回零Y轴
		/// </summary>
		Y,
		/// <summary>
		/// 回零Z轴
		/// </summary>
		Z,
		/// <summary>
		/// 回零全部轴
		/// </summary>
		Both,
	}
	/// <summary>
	/// 运动到某点
	/// </summary>
	public enum Moving
	{   /// <summary>
	    /// 移动至点1
	    /// </summary>
		ToOne,
		/// <summary>
		/// 移动至点2
		/// </summary>
		ToTwo,
		/// <summary>
		/// 移动至点3
		/// </summary>
		ToThree,
		/// <summary>
		/// 移动至点4
		/// </summary>
		ToFour,
		/// <summary>
		/// 移动至点5
		/// </summary>
		ToFive,
		/// <summary>
		/// 移动至点6
		/// </summary>
		ToSix,
		/// <summary>
		/// 移动至点7
		/// </summary>
		ToSeven,
		/// <summary>
		/// 移动至点8
		/// </summary>
		ToEight,
		/// <summary>
		/// 移动至点9
		/// </summary>
		ToNine,
		/// <summary>
		/// 移动至点10
		/// </summary>
		ToTen,
		/// <summary>
		/// 移动至点0
		/// </summary>
		ToZero,
	}
	/// <summary>
	/// 停止轴
	/// </summary>
	public enum MotionKill
	{   /// <summary>
	    /// 停止X轴
	    /// </summary>
		X,
		/// <summary>
		/// 停止Y轴
		/// </summary>
		Y,
		/// <summary>
		/// 停止Z轴
		/// </summary>
		Z,
		/// <summary>
		/// 停止全部轴
		/// </summary>
		Both,
	}
	/// <summary>
	/// 点动类型
	/// </summary>
	public enum MoveInching
	{
		/// <summary>
		/// X正向点动
		/// </summary>
		XPositive,
		/// <summary>
		/// X负向点动
		/// </summary>
		XNegative,
		/// <summary>
		/// Y正向点动
		/// </summary>
		YPositive,
		/// <summary>
		/// Y负向点动
		/// </summary>
		YNegative,
		/// <summary>
		/// Z正向点动
		/// </summary>
		ZPositive,
		/// <summary>
		/// Z负向点动
		/// </summary>
		ZNegative,
	}
	#endregion
}
