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
                            uid INTEGER primary key AUTOINCREMENT, 
                            info text, 
                            file_path text)";
            try
            {
                SQLiteCommand command = new SQLiteCommand(query, mSQLiteConn);
                command.ExecuteNonQuery();
            }
            catch (SQLiteException ex)
            {
                return false;
            }

            return true;
        }

        public SQLiteDataReader GetMenuDataByPath(string file_path)
        {
            string query = String.Format("SELECT * FROM menu_info WHERE file_path = \"{0}\"", file_path);
            try
            {
                SQLiteCommand command = new SQLiteCommand(query, mSQLiteConn);
                //var reader = await command.ExecuteReaderAsync();
                SQLiteDataReader reader = command.ExecuteReader();

                //if (reader.HasRows)
                //    return true;
                //else return false;
                return reader;
            }
            catch (SQLiteException ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                //return false;
                return null;
            }
        }

        public bool SetMenuData(List<Menu> data)
        {
            string json = JsonConvert.SerializeObject(data, Formatting.None, 
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

            try
            {
                string query = "INSERT INTO menu_info(info, file_path) VALUES(\"" + Util.Base64Encode(json) + "\", \"" + data[0].exePath + "\")";
                SQLiteCommand command = new SQLiteCommand(query, mSQLiteConn);
                command.ExecuteNonQuery();

                return true;
            }
            catch (SQLiteException ex)
            {
                return false;
            }
        }


    }
}
