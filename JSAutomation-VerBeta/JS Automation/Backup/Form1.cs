using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MultiSelectTreeview
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void button1_Click( object sender, EventArgs e )
		{
			List<TreeNode> list = new List<TreeNode>();
			list.Add(multiSelectTreeview1.Nodes[0]);
			list.Add(multiSelectTreeview1.Nodes[2]);
			list.Add(multiSelectTreeview1.Nodes[3]);

			multiSelectTreeview1.SelectedNodes = list;
		}

		private void button2_Click( object sender, EventArgs e )
		{
			multiSelectTreeview1.SelectedNodes = null;
		}
	}
}