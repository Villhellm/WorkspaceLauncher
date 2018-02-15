using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkspaceLauncher.Models
{
	public class WindowsProgram
	{
		public string ProcessName { get; set; } = "";
		public string StartPath { get; set; } = "";
		public string Argument { get; set; } = "";
		public int WindowWidth { get; set; } = 0;
		public int WindowHeight { get; set; } = 0;
		public int XPos { get; set; } = 0;
		public int YPos { get; set; } = 0;
		public int Status { get; set; } = 1;

		public WindowsProgram(string ProcessName, string StartPath, string Argument, int WindowWidth, int WindowHeight, int XPos, int YPos, int Status)
		{
			this.ProcessName = ProcessName;
			this.StartPath = StartPath;
			this.Argument = Argument;
			this.WindowWidth = WindowWidth;
			this.WindowHeight = WindowHeight;
			this.XPos = XPos;
			this.YPos = YPos;
			this.Status = Status;
		}
	}
}
