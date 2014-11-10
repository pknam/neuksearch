using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

            string exePath = obj["ExecutablePath"].ToString();
            int pid = int.Parse(obj["ProcessId"].ToString());
            
            IntPtr hwnd = Pid2Hwnd(pid);




            if (MenuCrawler.crawl(hwnd, exePath))
            {
                MessageBox.Show("add");
            }
            else
            {
                MessageBox.Show("no menu");
            }
            


            //TODO
            //obj["Name"] == 응용프로그램 이름
            //obj["ProcessId"] == process id
            //obj["CommandLine"] == 응용프로그램 full path
            //응용프로그램 이름으로 sqlite3에서 select한 뒤 menu가 올라가있는 인스턴스 불러와서 load
            //menu가 들어있는 singleton불러와서 쓰면됨
            
            /*
            if(sqlite contains exePath)
            {
                load sqlite data to MenuManager.Instance.MenuSet;
            }
            else
            {
                crawl menu of obj;
                save data to sqlite;
            }
            */


            
        }

        private IntPtr Pid2Hwnd(int pid)
        {
            System.Diagnostics.Process[] processes = System.Diagnostics.Process.GetProcesses();

            foreach (var process in processes)
            {
                if (process.Id == pid)
                {
                    return process.MainWindowHandle;
                }

                
            }

            return new IntPtr(0);
        }
    }
}
