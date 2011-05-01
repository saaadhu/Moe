using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnvDTE;
using System.ComponentModel;
using EnvDTE80;

namespace SenthilKumarSelvaraj.Moe
{
    class Moe
    {
        DTE dte;
        public Moe(DTE dte)
        {
            this.dte = dte;
            MoeEngine.Instance.ProgressChanged += new EventHandler<System.ComponentModel.ProgressChangedEventArgs>(ProgressChanged);
            TracepointManager.Initialize((DTE2)dte);
        }

        public void Start()
        {
            TracepointManager.SetupAllTracepoints();
            dte.Debugger.Go(false);
        }

        void ProgressChanged(object state, ProgressChangedEventArgs p)
        {
            if (p.ProgressPercentage == 100)
            {
                dte.Debugger.Stop(true);
            }
        }

    }
}
