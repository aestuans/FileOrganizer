﻿using System;
using System.Collections.Generic;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.IO;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Windows.Media.Animation;

namespace FileOrganizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>


    public partial class MainWindow : Window
    {
        private string[] gtargetDirectory;

        public static int count;
        private TextBlock[] folderArray;
        private TextBlock[] numberArray;
        private int numMoving = 0;
        public HashSet<string> acceptedformats = new HashSet<string>
        {
            ".jpg", ".jpeg", ".gif", ".png", ".bmp", ".gifv", ".svg",
            ".mp3", ".m4a", ".ogg", ".flac", ".aac", ".wav", ".wma",
            ".pdf", ".djv", ".djvu", ".xls", ".xlsx", ".htm", ".html", ".mht", ".mhtml", ".xhtml", ".txt", ".ppt", ".pptx", ".doc", ".docx",
            ".3gp", ".avi", ".divx", ".mov", ".mp4", ".mpg", ".mpeg4", ".wmv", ".flv", ".webm", ".mkv",
            ".c", ".cpp", ".py",
            ".7z", ".rar"
        };
        //public static bool gstopped = true;

        public static readonly DependencyProperty isStoppedProperty =
            DependencyProperty.Register("isStopped", typeof(bool), typeof(MainWindow));
        public bool isStopped
        {
            get { return (bool)GetValue(isStoppedProperty); }
            set { SetValue(isStoppedProperty, value); }
        }
        public MainWindow()
        {
            InitializeComponent();
            isStopped = true;
            TextBlock[] _folderArray = { Folder_1, Folder_2, Folder_3, Folder_4, Folder_5, Folder_6, Folder_7, Folder_8, Folder_9 };
            TextBlock[] _numberArray = { Num_1, Num_2, Num_3, Num_4, Num_5, Num_6, Num_7, Num_8, Num_9 };
            folderArray = _folderArray;
            numberArray = _numberArray;
            stopWatch = new System.Diagnostics.Stopwatch();
        }
        System.Diagnostics.Stopwatch stopWatch;
        private void moveFile(string source, string target)
        {
            if (File.Exists(source))
            {
                Tuple<string, string> t = new Tuple<string, string>(source, target);
                Thread moveThread = new Thread(new ParameterizedThreadStart(moveFileInNewThred));
                moveThread.Start(t);
            }
        }
        private void moveFileInNewThred(object data)
        {
            numMoving++;
            Application.Current.Dispatcher.Invoke(
                new Action(() => { TextBox_Copying.Text = numMoving.ToString() + " Files"; }));

            Tuple<string, string> t = (Tuple<string, string>)data;
            string source = t.Item1;
            string target = t.Item2;
            try
            {
                File.Move(source, target);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message.ToString(), "Error", MessageBoxButton.OK);
                appStop();
            }
            numMoving--;
            Application.Current.Dispatcher.Invoke(
                new Action(() => { TextBox_Copying.Text = numMoving.ToString() + " Files"; }));
        }
        private void onKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (stopWatch.IsRunning)
            {
                if (stopWatch.ElapsedMilliseconds < 500)
                    return;
                else
                {
                    stopWatch.Reset();
                    stopWatch.Start();
                }
            }
            else
                stopWatch.Start();

            if (!isStopped)
            {
                if (e.Key == Key.Add ||
                    (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) ||
                    (e.Key >= Key.D0 && e.Key <= Key.D9))
                {
                    SolidColorBrush animatedBrush = new SolidColorBrush(Colors.Aqua);
                    NameScope.SetNameScope(this, new NameScope());
                    this.RegisterName("animatedBrush", animatedBrush);

                    ColorAnimation blueColorAnimation =
                                new ColorAnimation(Colors.Aqua, Colors.CornflowerBlue, TimeSpan.FromSeconds(1));
                    ColorAnimation redColorAnimation =
                                new ColorAnimation(Colors.Orange, Colors.Red, TimeSpan.FromSeconds(1));

                    Storyboard.SetTargetName(blueColorAnimation, "animatedBrush");
                    Storyboard.SetTargetName(redColorAnimation, "animatedBrush");
                    Storyboard.SetTargetProperty(blueColorAnimation, new PropertyPath(SolidColorBrush.ColorProperty));
                    Storyboard.SetTargetProperty(redColorAnimation, new PropertyPath(SolidColorBrush.ColorProperty));

                    Storyboard storyBoard = new Storyboard();
                    storyBoard.Children.Add(blueColorAnimation);

                    string currentFilePath = FileProcessor.currentFile;
                    string currentFileName = System.IO.Path.GetFileName(currentFilePath);

                    if (e.Key == Key.NumPad0)
                    {
                        animatedBrush.Color = Colors.Orange;
                        storyBoard.Children.Clear();
                        storyBoard.Children.Add(redColorAnimation);
                        Rectangle_DEL.Fill = animatedBrush;
                        storyBoard.Begin(this);
                        File.Delete(currentFilePath);
                    }
                    else if (e.Key == Key.NumPad1 && gtargetDirectory.Length > 0)
                    {
                        Rectangle_1.Fill = animatedBrush;
                        storyBoard.Begin(this);
                        moveFile(currentFilePath, gtargetDirectory[0] + "\\" + currentFileName);
                    }
                    else if (e.Key == Key.NumPad2 && gtargetDirectory.Length > 1)
                    {
                        Rectangle_2.Fill = animatedBrush;
                        storyBoard.Begin(this);
                        moveFile(currentFilePath, gtargetDirectory[1] + "\\" + currentFileName);
                    }
                    else if (e.Key == Key.NumPad3 && gtargetDirectory.Length > 2)
                    {
                        Rectangle_3.Fill = animatedBrush;
                        storyBoard.Begin(this);
                        moveFile(currentFilePath, gtargetDirectory[2] + "\\" + currentFileName);
                    }
                    else if (e.Key == Key.NumPad4 && gtargetDirectory.Length > 3)
                    {
                        Rectangle_4.Fill = animatedBrush;
                        storyBoard.Begin(this);
                        moveFile(currentFilePath, gtargetDirectory[3] + "\\" + currentFileName);
                    }
                    else if (e.Key == Key.NumPad5 && gtargetDirectory.Length > 4)
                    {
                        Rectangle_5.Fill = animatedBrush;
                        storyBoard.Begin(this);
                        moveFile(currentFilePath, gtargetDirectory[4] + "\\" + currentFileName);
                    }
                    else if (e.Key == Key.NumPad6 && gtargetDirectory.Length > 5)
                    {
                        Rectangle_6.Fill = animatedBrush;
                        storyBoard.Begin(this);
                        moveFile(currentFilePath, gtargetDirectory[5] + "\\" + currentFileName);
                    }
                    else if (e.Key == Key.NumPad7 && gtargetDirectory.Length > 6)
                    {
                        Rectangle_7.Fill = animatedBrush;
                        storyBoard.Begin(this);
                        moveFile(currentFilePath, gtargetDirectory[6] + "\\" + currentFileName);
                    }
                    else if (e.Key == Key.NumPad8 && gtargetDirectory.Length > 7)
                    {
                        Rectangle_8.Fill = animatedBrush;
                        storyBoard.Begin(this);
                        moveFile(currentFilePath, gtargetDirectory[7] + "\\" + currentFileName);
                    }
                    else if (e.Key == Key.NumPad9 && gtargetDirectory.Length > 8)
                    {
                        Rectangle_9.Fill = animatedBrush;
                        storyBoard.Begin(this);
                        moveFile(currentFilePath, gtargetDirectory[8] + "\\" + currentFileName);
                    }

                    FileProcessor.processDirectory();
                }
            }
        }

        private void button_Source_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            CommonFileDialogResult result = dialog.ShowDialog();
            if (result == CommonFileDialogResult.Ok)
            {
                string sourceDirectory = dialog.FileName.ToString();
                FileInfo[] fileInfo = (new DirectoryInfo(sourceDirectory)).GetFiles();
                FileProcessor.setIndex(0);

                TextBlock_Source.Text = System.IO.Path.GetFileName(sourceDirectory);
                if (TextBlock_Source.Text == "")
                    TextBlock_Source.Text = sourceDirectory;
                TextBlock_NumFiles.Text = fileInfo.Length.ToString();

                List<string> fileEntries = new List<string>();
                for (int i = 0; i < fileInfo.Length; i++)
                {
                    string _file = fileInfo[i].FullName;
                    string extention = System.IO.Path.GetExtension(_file);
                    if (acceptedformats.Contains(extention) && !fileInfo[i].Attributes.HasFlag(FileAttributes.Hidden))
                        fileEntries.Add(_file);
                }
                FileProcessor.resetFileEntries(fileEntries);
            }
        }

        private void button_Target_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            dialog.Multiselect = true;
            CommonFileDialogResult result = dialog.ShowDialog();
            if (result == CommonFileDialogResult.Ok)
            {
                string[] targetDirectory = dialog.FileNames.ToArray<string>();
                gtargetDirectory = targetDirectory;

                for (int i = 0; i < folderArray.Length; i++)
                {
                    if (targetDirectory.Length > i)
                    {
                        folderArray[i].Text = " " + System.IO.Path.GetFileName(targetDirectory[i]) + " ";
                        if (folderArray[i].Text == "")
                            folderArray[i].Text = targetDirectory[i];
                        numberArray[i].Text = (i + 1).ToString();
                    }
                    else
                    {
                        folderArray[i].Text = "";
                        numberArray[i].Text = "";
                    }
                }
            }
        }

        private void button_StartStop_Click(object sender, RoutedEventArgs e)
        {
            if (isStopped)
            {
                appStart();
                FileProcessor.processDirectory();
            }
            else
            {
                appStop();
            }
        }
        public static void appStart()
        {
            ((MainWindow)Application.Current.MainWindow).isStopped = false;
            updateProcessingIndex(FileProcessor.getIndex() + 1);
        }
        public static void appStop()
        {
            ((MainWindow)Application.Current.MainWindow).isStopped = true;
            updateProcessingIndex("--");
            ((MainWindow)Application.Current.MainWindow).TextBox_CurrentFile.Text = "--";
        }
        public static void updateProcessingIndex(int num)
        {
            ((MainWindow)Application.Current.MainWindow).TextBlock_ProcessingIndex.Text = num.ToString()
                + "/" + FileProcessor.fileCount().ToString();
        }
        public static void updateProcessingIndex(string s)
        {
            ((MainWindow)Application.Current.MainWindow).TextBlock_ProcessingIndex.Text = s;
        }
    }


    public class BoolToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, System.Globalization.CultureInfo culture)
        {
            if ((bool)value)
                return "Start";
            else
                return "Stop";
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
