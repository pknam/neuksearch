using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Runtime.InteropServices;
using Newtonsoft.Json;

namespace NeukSearch
{
    class MenuSqliteHelper
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        [PreserveSig]
        public static extern uint GetModuleFileName
            ([In]IntPtr hModule, [Out] StringBuilder lpFilename, [In][MarshalAs(UnmanagedType.U4)]int nSize);

        private static MenuSqliteHelper instance;
        private SQLiteConnection mSQLiteConn;
        private String mDataSource;

        private MenuSqliteHelper() {
            mDataSource = @"Data Source=test.sql";
            mSQLiteConn = new SQLiteConnection();
        }

        public static MenuSqliteHelper _instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MenuSqliteHelper();
                }

                return instance;
            }
        }

        private string GetFilePath(IntPtr hwnd)
        {
            StringBuilder fileName = new StringBuilder(255);
            GetModuleFileName(hwnd, fileName, fileName.Capacity);

            return fileName.ToString();
        }

        public bool GetMenuData()
        {
            return true;
        }

        public bool SetMenuData(Menu data)
        {
            string json = JsonConvert.SerializeObject(data);
            return true;
        }
    }
}
