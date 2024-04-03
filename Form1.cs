using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Win32;

namespace WeChatLauncher
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // 设置窗体图标
            this.Icon = new Icon(".\\wx.ico");

            InitializeWeChatLauncher();
        }

        private void InitializeWeChatLauncher()
        {
            // 设置窗口标题和大小
            this.Text = "微信启动器";
            this.Width = 420; // 窗口宽度
            this.Height = 160; // 窗口高度调整
            this.FormBorderStyle = FormBorderStyle.FixedSingle; // 设置窗口边框样式为单一边框
            this.MaximizeBox = false; // 禁用最大化按钮

            // 微信路径标签
            Label pathLabel = new Label
            {
                Text = "微信路径：",
                Top = 10,
                Left = 10,
                Width = 80
            };
            Controls.Add(pathLabel);

            // 微信路径输入框
            TextBox pathTextBox = new TextBox
            {
                Width = 300,
                Top = pathLabel.Top,
                Left = pathLabel.Right, // 基于标签的右侧位置
                Text = GetWeChatInstallPath()
            };
            Controls.Add(pathTextBox);

            // 启动数量标签
            Label numberLabel = new Label
            {
                Text = "启动数量：",
                Top = pathTextBox.Bottom + 10,
                Left = 10,
                Width = 80
            };
            Controls.Add(numberLabel);

            // 启动数量输入框
            TextBox numberTextBox = new TextBox
            {
                Width = 300,
                Top = numberLabel.Top,
                Left = numberLabel.Right, // 基于标签的右侧位置
                Text = "2"
            };
            Controls.Add(numberTextBox);

            // 启动按钮
            Button launchButton = new Button
            {
                Width = 100,
                Height = 23,
                Top = numberTextBox.Bottom + 10, // 基于上一个控件的底部位置
                Left = 10,
                Text = "启动微信"
            };
            launchButton.Click += (sender, e) => LaunchWeChat(pathTextBox.Text, numberTextBox.Text);
            Controls.Add(launchButton);
        }


        private string GetWeChatInstallPath()
        {
            // 尝试从注册表获取微信安装路径
            string registryKeyPath = @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\WeChat";
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(registryKeyPath))
            {
                if (key != null)
                {
                    object installLocation = key.GetValue("InstallLocation");
                    if (installLocation != null)
                    {
                        // 确保路径以 \WeChat.exe 结尾
                        return installLocation.ToString().TrimEnd('\\') + @"\WeChat.exe";
                    }
                }
            }

            // 如果注册表中找不到，则返回默认路径
            return @"C:\Program Files\Tencent\WeChat\WeChat.exe";
        }

        private void LaunchWeChat(string path, string numberString)
        {
            if (int.TryParse(numberString, out int number))
            {
                for (int i = 0; i < number; i++)
                {
                    try
                    {
                        Process.Start(path);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"无法启动微信: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
                }
            }
            else
            {
                MessageBox.Show("请输入有效的数字。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
