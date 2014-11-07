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
        private ManagementEventWatcher mProcessWatcher;

        public ProcessOpenEvent()
        {
            WqlEventQuery query = new WqlEventQuery("Select * From __InstanceCreationEvent Within 2 Where TargetInstance Isa 'Win32_Process'");
            mProcessWatcher = new ManagementEventWatcher(query);
            mProcessWatcher.EventArrived += new EventArrivedEventHandler(watcher_EventArrived);
        }

        public void watcher_EventArrived(object sender, EventArrivedEventArgs e)
        {
            ManagementBaseObject obj = (ManagementBaseObject)e.NewEvent["TargetInstance"];
            //TODO
            //obj["Name"] == 응용프로그램 이름
            //응용프로그램 이름으로 sqlite3에서 select한 뒤 menu가 올라가있는 인스턴스 불러와서 load
        }
    }
}
