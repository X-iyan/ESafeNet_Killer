using System;

using System.Diagnostics;
using System.IO;

using System.Windows;

using System.Windows.Input;
using IWshRuntimeLibrary;
using Microsoft.WindowsAPICodePack.Dialogs;
using Newtonsoft.Json;
using File = System.IO.File;

//using Microsoft.Win32;
namespace DeCodeApp
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private AllTypes allTypes;
        void Decode(string[] args)
        {
            #region 程序A

            //Console.WriteLine("请输入路径");
            //string path = Console.ReadLine();
            //DirectoryInfo root = new DirectoryInfo(path);
            //Console.WriteLine("请输入文件格式");
            //string ext = "*." + Console.ReadLine();
            //FileInfo[] files = root.GetFiles(ext,SearchOption.AllDirectories);
            //foreach (var item in files)
            //{
            //    byte[] bt = getImageByte(item.FullName);
            //    FileStream fs = new FileStream(item.Directory + "/" + item.Name + ".exe", FileMode.Create);
            //    fs.Write(bt, 0, bt.Length);
            //    fs.Close();
            //}

            //Process pro = Process.Start(@"..\netcoreapp3.2\ConsoleApp1.exe", $"\"{path}\"");//打开程序B

            #endregion

            #region 程序B

           

            #endregion
        }

        


        public MainWindow()
        {
            InitializeComponent();
            var json = File.ReadAllText("./config.json");
            allTypes = JsonConvert.DeserializeObject<AllTypes>(json);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tb1.Text))
            {
                return;
            }

            if (!Directory.Exists(tb1.Text))
            {
                return;
            }
            string path = tb1.Text;
            if (!File.Exists(tb2.Text))
            {
                throw new Exception($"{tb2.Text} 不存在");
            }
            ProcessStartInfo info = new ProcessStartInfo(tb2.Text);
            info.Arguments = $"\"{path}\" \"{cb1.Text}\"";
            Process.Start(info);
            //            DirectoryInfo root = new DirectoryInfo(path);
            //            string ext = "*." + ((ComboBoxItem)cb1.SelectedItem).Content;
            //            FileInfo[] files = root.GetFiles(ext, SearchOption.AllDirectories);
            //            foreach (var item in files)
            //            {
            //                byte[] bt = getImageByte(item.FullName);
            //                FileStream fs = new FileStream(item.Directory + "/" + item.Name + ".exe", FileMode.Create);
            //                fs.Write(bt, 0, bt.Length);
            //                fs.Close();
            //            }

        }

        private void ButtonAuto_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tb1.Text))
            {
                return;
            }

            if (!Directory.Exists(tb1.Text))
            {
                return;
            }
            string path = tb1.Text;
            if (!File.Exists(tb2.Text))
            {
                throw new Exception($"{tb2.Text} 不存在");
            }

            tb_Notice.Text = "";
            foreach (var fileTypeClass in allTypes.fTypes)
            {
                foreach (var decodeType in fileTypeClass.decodeTypes)
                {
                    DirectoryInfo root = new DirectoryInfo(path);
                    //FileInfo[] files = root.GetFiles("*"+ decodeType, SearchOption.AllDirectories);
                    /*foreach (var fileInfo in files)
                    {
                        ProcessStartInfo info = new ProcessStartInfo(fileTypeClass.strFile);
                        info.Arguments = $"\"{fileInfo.FullName}\"";
                        Process p =Process.Start(info);
                        p.WaitForExit(5000);
                        tb_Notice.Text += ("\r\n+"+fileInfo.Name);
                    }*/
                    ProcessStartInfo info = new ProcessStartInfo(fileTypeClass.strFile);
                    info.WindowStyle = ProcessWindowStyle.Hidden;
                    info.Arguments = $"\"{path}\" \"{decodeType}\"";
                    Process p = Process.Start(info);
                    p.WaitForExit(500);
                    //p.StandardInput.Write("\r\n");
                    tb_Notice.Text += ("\r\n+" + decodeType);
                }
            }
            Button_Click_1(null, null);
            MessageBox.Show("已完成！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            //            DirectoryInfo root = new DirectoryInfo(path);
            //            string ext = "*." + ((ComboBoxItem)cb1.SelectedItem).Content;
            //            FileInfo[] files = root.GetFiles(ext, SearchOption.AllDirectories);
            //            foreach (var item in files)
            //            {
            //                byte[] bt = getImageByte(item.FullName);
            //                FileStream fs = new FileStream(item.Directory + "/" + item.Name + ".exe", FileMode.Create);
            //                fs.Write(bt, 0, bt.Length);
            //                fs.Close();
            //            }

        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tb1.Text))
            {
                return;
            }

            if (!Directory.Exists(tb1.Text))
            {
                return;
            }
            string path = tb1.Text;
            ProcessStartInfo info = new ProcessStartInfo(@".\remover.exe");
            info.WindowStyle = ProcessWindowStyle.Hidden;
            info.Arguments = $"\"{path}\"";
            Process.Start(info);
        }

        private void tb1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //            FolderDialog openFileDialog = new FolderBrowserDialog();
            //            DialogResult result = openFileDialog.ShowDialog();
            //            if (result == System.Windows.Forms.DialogResult.OK)
            //            {
            //                tb1.Text = openFileDialog.SelectedPath;
            //            }
            var dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            CommonFileDialogResult result = dialog.ShowDialog();
            if (result == CommonFileDialogResult.Ok)
            {
                tb1.Text = dialog.FileName;
            }
        }

        private void tb2_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var dialog = new CommonOpenFileDialog();
            CommonFileDialogResult result = dialog.ShowDialog();
            if (result == CommonFileDialogResult.Ok)
            {
                tb2.Text = dialog.FileName;
            }
        }

        private void ComboBoxItem_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void tb1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Link;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }

        }

        private void tb1_Drop(object sender, DragEventArgs e)
        {
            var filename = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            if (filename.EndsWith("lnk"))
            {
                WshShell shell = new WshShell();
                IWshShortcut iWshShortcut = (IWshShortcut) shell.CreateShortcut(filename);
                filename = iWshShortcut.TargetPath;
            }

            tb1.Text = filename;
        }
    }
}
