using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SenthilKumarSelvaraj.Moe
{
    public class Test : INotifyPropertyChanged
    {
        public string Name { get; private set;}

        public Test(string name)
        {
            this.Name = name;
        }

        private Status status;
        public Status Status
        {
            get { return status; }
            set
            {
                status = value;
                RaisePropertyChanged("Status");
            }
        }

        private string errorMessage;
        public string ErrorMessage
        {
            get { return errorMessage; }
            set
            {
                errorMessage = value;
                RaisePropertyChanged("ErrorMessage");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public enum Status
    {
        None,
        Pending,
        Running,
        Passed,
        Failed
    }
}
