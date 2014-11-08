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
            string data = String.Format("DataSource={0}\\{1}", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "test.db");
            mSQLiteConn = new SQLiteConnection(data);
            mSQLiteConn.Open();
            CreateTable();
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

        private bool CreateTable()
        {
            string query = @"CREATE TABLE menu_info(
                            uid int primary key auto_increment, 
                            info text, 
                            file_path text, 
                            file_name text)";
            try
            {
                SQLiteCommand command = new SQLiteCommand(query, mSQLiteConn);
                command.ExecuteNonQueryAsync();
                
            }
            catch (SQLiteException ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                return false;
            }

            return true;
        }

        public async Task<string> GetMenuDataByName(string file_name)
        {
            string query = String.Format("SELECT * FROM menu_info WHERE {0}", file_name);
            try
            {
                SQLiteCommand command = new SQLiteCommand(query, mSQLiteConn);
                var reader = await command.ExecuteReaderAsync();

                return reader["info"].ToString();
            }
            catch (SQLiteException ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                return null;
            }
        }

        public async Task<string> GetMenuDataByPath(string file_path)
        {
            string query = String.Format("SELECT * FROM menu_info WHERE {0}", file_path);
            try
            {
                SQLiteCommand command = new SQLiteCommand(query, mSQLiteConn);
                var reader = await command.ExecuteReaderAsync();

                return reader["info"].ToString();
            }
            catch (SQLiteException ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                return null;
            }
        }

        public bool SetMenuData(Menu data)
        {
            string json = JsonConvert.SerializeObject(data);
            string query = @"";
            return true;
        }
    }
}
