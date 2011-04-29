using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;

namespace SenthilKumarSelvaraj.Moe
{
    /// <summary>
    /// Interaction logic for MyControl.xaml
    /// </summary>
    public partial class MyControl : UserControl, INotifyPropertyChanged
    {
        MoeEngine moeEngine = new MoeEngine();
        public MyControl()
        {
            InitializeComponent();
            RegisterForMoeEvents();
            moeEngine.ProgressChanged += ProgressChanged;
            ProgressBarColor = Colors.LimeGreen;
        }
         
        public ObservableCollection<Test> Tests
        {
            get { return moeEngine.Tests; }
        }

        private OutputWindowEvents outputWindowEvents;

        private void RegisterForMoeEvents()
        {
            var dte = (DTE2)Package.GetGlobalService(typeof (DTE));
            outputWindowEvents = dte.Events.OutputWindowEvents["Debug"];
            outputWindowEvents.PaneUpdated += new _dispOutputWindowEvents_PaneUpdatedEventHandler(DebugPaneUpdated);
            outputWindowEvents.PaneClearing += new _dispOutputWindowEvents_PaneClearingEventHandler(DebugPaneCleared);
        }

        private string lastContent = "";

        private void DebugPaneCleared(OutputWindowPane pane)
        {
            lastContent = string.Empty;
        }

        private void DebugPaneUpdated(OutputWindowPane pane)
        {
            var selection = pane.TextDocument.Selection;
            selection.SelectAll();
            var text = selection.Text;

            if (text.Length == lastContent.Length)
            {
                return;
            }

            var newText = text.Substring(lastContent.Length);

            foreach(var line in newText.Split(new [] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries))
            {
                moeEngine.ProcessLine(line);        
            }

            lastContent = text;
        }

        private Color progressBarColor;
        public Color ProgressBarColor
        {
            get { return progressBarColor; }
            set { progressBarColor = value; RaisePropertyChanged("ProgressBarColor"); }
        }

        private int progressPercentage;
        public int ProgressPercentage
        {
            get { return progressPercentage; }
            set
            {
                progressPercentage = value;
                RaisePropertyChanged("ProgressPercentage");
            }
        }


        private void ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressPercentage = e.ProgressPercentage;

            if (Tests.Any(t => t.Status == Status.Failed))
                ProgressBarColor = Colors.Red;
        }

        void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}