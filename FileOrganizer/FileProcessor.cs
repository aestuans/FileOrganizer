using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;


namespace FileOrganizer
{
    public class FileProcessor
    {
        private static List<string> gfileEntries = new List<string> { };
        private static int gindex = 0;
        public static string currentFile = "";
        public static int fileCount()
        {
            return gfileEntries.Count();
        }
        public static void addToFileEntries(string s)
        {
            gfileEntries.Add(s);
        }
        public static void resetFileEntries(List<string> l)
        {
            gfileEntries = l;
        }
        public static void setIndex(int i)
        {
            gindex = i;
        }
        public static int getIndex()
        {
            return gindex;
        }
        public static void processDirectory()
        {
            if (gfileEntries == null)
            {
                MainWindow.appStop();
                gindex = 0;
                return;
            }
            if (gindex == gfileEntries.Count())
            {
                MainWindow.appStop();
                gindex = 0;
            }
            if (((MainWindow)Application.Current.MainWindow).isStopped)
                return;

            try
            {
                processFile(gfileEntries[gindex]);

            }
            catch (Exception e)
            {
                // if the process wasn't able to start, stop the program.
                // The progress won't be removed, if the user clicks start, it will try to open the same process again.
                MainWindow.appStop();
                return;
            }

            currentFile = gfileEntries[gindex];
            ((MainWindow)Application.Current.MainWindow).TextBox_CurrentFile.Text = System.IO.Path.GetFileName(gfileEntries[gindex]);
            gindex++;
            MainWindow.updateProcessingIndex(gindex);

        }
        public static void processFile(string path)
        {
            try
            {
                openFileWithDefaultApp(path);
            }
            catch (Exception e)
            {
                throw;
            }
        }
        private static void openFileWithDefaultApp(string path)
        {
            System.Diagnostics.Process myProcess = new System.Diagnostics.Process();
            myProcess.StartInfo.FileName = path;
            try
            {
                myProcess.Start();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message.ToString(), "Error in external process", MessageBoxButton.OK);
                throw;
            }
        }
    }
}
