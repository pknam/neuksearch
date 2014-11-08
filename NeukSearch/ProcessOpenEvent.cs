using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeukSearch
{
    using System.Management;
    class ProcessOpenEvent
    {
        private static ProcessOpenEvent instance;
        private ManagementEventWatcher mProcessWatcher;

        private ProcessOpenEvent()
        {
            WqlEventQuery query = new WqlEventQuery("Select * From __InstanceCreationEvent Within 2 Where TargetInstance Isa 'Win32_Process'");
            mProcessWatcher = new ManagementEventWatcher(query);
            mProcessWatcher.EventArrived += new EventArrivedEventHandler(watcher_EventArrived);
        }

        public static ProcessOpenEvent _instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ProcessOpenEvent();
                }

                return instance;
            }
        }

        public void run()
        {
            mProcessWatcher.Start();
        }

        public void watcher_EventArrived(object sender, EventArrivedEventArgs e)
        {
            ManagementBaseObject obj = (ManagementBaseObject)e.NewEvent["TargetInstance"];
            System.Windows.Forms.MessageBox.Show(obj["CommandLine"].ToString());
            //TODO
            //obj["Name"] == 응용프로그램 이름
            //obj["ProcessId"] == process id
            //obj["CommandLine"] == 응용프로그램 full path
            //응용프로그램 이름으로 sqlite3에서 select한 뒤 menu가 올라가있는 인스턴스 불러와서 load
            //menu가 들어있는 singleton불러와서 쓰면됨
        }
    }
}
