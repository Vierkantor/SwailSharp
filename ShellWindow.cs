using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SwailSharp {
	public class ShellWindow : Form {
		IList<Label> Log;
		TextBox InputBox;
		
		public ShellWindow() : base() {
			this.DoubleBuffered = true;
			
			this.Log = new List<Label>();
			this.InputBox = new TextBox();
			this.InputBox.Location = new Point(25, 25);
			this.InputBox.KeyUp += (object o, KeyEventArgs ea) => {
				if (ea.KeyCode == Keys.Enter) {
					this.Input(this.InputBox.Text);
					this.InputBox.Clear();
				}
			};
			
			this.Controls.Add(InputBox);
		}
		
		private void AddLogLine(Label line) {
			line.Location = new Point(25, 50 + 25 * this.Log.Count);
			
			this.Log.Add(line);
			this.Controls.Add(line);
		}
		
		private void Input(string text) {
			Label line = new Label();
			line.Text = "> " + text;
			line.ForeColor = Color.Red;
			AddLogLine(line);
		}
	}
	
	// the Swail wrapper, should probably inherit from SwailWindow in the future
	public class SwailShellWindow : SwailObject {
		public ShellWindow Window;
		
		// do basically the same as a ShellWindow but wrap it
		public SwailShellWindow(string name) : base(name) {
			this.Window = new ShellWindow();
		}
	}
}

