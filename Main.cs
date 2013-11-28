using System;
using System.Windows.Forms;

namespace SwailSharp
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Application.Run(new SwailShellWindow().Window);
		}
	}
}
