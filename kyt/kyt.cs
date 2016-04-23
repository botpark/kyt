using System;
using System.ServiceProcess;

namespace kyt
{
    public partial class kyt : ServiceBase
    {
        public kyt()
        {
            InitializeComponent();
        }

        public void OnDebug()
        {
            OnStart(null);
        }

        protected override void OnStart(string[] args)
        {
        }

        protected override void OnStop()
        {
        }
    }
}
