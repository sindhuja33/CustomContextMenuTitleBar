using System.Runtime.InteropServices;

namespace CustomTitleBarContextMenu
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hwnd, int msg, int wParam, int lParam);

        public const int SC_MOVE = 0xF010;
        public const Int32 WM_SYSCOMMAND = 0x0112;
        public const int SC_SIZE = 0xF000;
        public const Int32 WM_NCRBUTTONDOWN = 0x00A4;
        public const Int32 HTCAPTION = 0x0084;

        public Form1()
        {
            InitializeComponent();
        }
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_NCRBUTTONDOWN:
                    var pos = new Point(m.LParam.ToInt32());
                    onTitlebar_Click(pos);
                    m.Result = new IntPtr(1);
                    break;
                default:
                    base.WndProc(ref m);
                    break;

            }

        }
        private void onTitlebar_Click(Point pos)
        {
            CreateContextMenu(pos);
        }
        private void CreateContextMenu(Point pos)
        {
            ContextMenuStrip menustrip = new ContextMenuStrip();

            ToolStripMenuItem menuItem = new ToolStripMenuItem("Restore");
            menuItem.Name = "Restore";
            menuItem.Click += MenuItem_Click;
            menustrip.Items.Add(menuItem);


            ToolStripMenuItem menuItem1 = new ToolStripMenuItem("Move");
            menuItem1.Name = "Move";
            menuItem1.Click += MenuItem_Click;
            menustrip.Items.Add(menuItem1);

            ToolStripMenuItem menuItem2 = new ToolStripMenuItem("Size");
            menuItem2.Name = "Size";
            menuItem2.Click += MenuItem_Click;
            menustrip.Items.Add(menuItem2);

            ToolStripMenuItem menuItem3 = new ToolStripMenuItem("Minimize");
            menuItem3.Name = "Minimize";
            menuItem3.Click += MenuItem_Click;
            menustrip.Items.Add(menuItem3);


            ToolStripMenuItem menuItem4 = new ToolStripMenuItem("Maximize");
            menuItem4.Name = "Maximize";
            menuItem4.Click += MenuItem_Click;
            menustrip.Items.Add(menuItem4);

            ToolStripMenuItem menuItem5 = new ToolStripMenuItem("Close      Alt+F4");
            menuItem5.Name = "Close";
            menuItem5.Click += MenuItem_Click;
            menustrip.Items.Add(menuItem5);
            if (WindowState == FormWindowState.Minimized)
            {
                menustrip.Items[1].Enabled = false;//Disable Move
                menustrip.Items[2].Enabled = false;//Disable Size
                menustrip.Items[3].Enabled = false;//Disable Minimize

            }
            if (WindowState == FormWindowState.Maximized)
            {
                menustrip.Items[1].Enabled = false;//Disable Move
                menustrip.Items[2].Enabled = false;//Disable Size
                menustrip.Items[4].Enabled = false;//Disable Maximize

            }
            if (WindowState == FormWindowState.Minimized || WindowState == FormWindowState.Maximized)
            {
                menustrip.Items[0].Enabled = true;//if the window is in minimized or maximize mode then enable restore option
            }
            else
            {
                menustrip.Items[0].Enabled = false;
            }
            menustrip.Show(pos);

        }

        private void MenuItem_Click(object? sender, EventArgs e)
        {
            if (sender != null)
            {
                ToolStripMenuItem menuItem = (ToolStripMenuItem)sender;

                switch (menuItem.Name)
                {
                    case "Close":
                        this.Close();
                        break;
                    case "Maximize":
                        WindowState = FormWindowState.Maximized;
                        break;
                    case "Minimize":
                        WindowState = FormWindowState.Minimized;
                        break;
                    case "Restore":
                        WindowState = FormWindowState.Normal;
                        break;
                    case "Move":
                        SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE, 0);
                        break;
                    case "Size":
                        SendMessage(this.Handle, WM_SYSCOMMAND, SC_SIZE, 0);
                        break;
                    default:
                        break;
                }
            }
        }

    }
}