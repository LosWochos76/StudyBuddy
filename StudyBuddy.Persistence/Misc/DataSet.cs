using System;
using System.Collections.Generic;
using Npgsql;

namespace StudyBuddy.Persistence
{
    public class DataSet
    {
        private Dictionary<string,int> column_names = new Dictionary<string,int>();
        private List<object[]> rows = new List<object[]>();

        public void FillFromReader(NpgsqlDataReader reader)
        {
            ReadColumnNames(reader);
            ReadValues(reader);
        }

        private void ReadColumnNames(NpgsqlDataReader reader)
        {
            column_names.Clear();
            for (var i = 0; i < reader.FieldCount; i++)
                column_names.Add(reader.GetName(i), i);
        }

        private void ReadValues(NpgsqlDataReader reader)
        {
            rows.Clear();
            while (reader.Read())
            {
                var row = new object[reader.FieldCount];
                reader.GetValues(row);
                rows.Add(row);
            }
        }

        public int RowCount
        {
            get { return rows.Count; }
        }

        public bool HasRows
        {
            get { return RowCount > 0; }
        }

        private int ColumnIndex(string column_name)
        {
            if (!column_names.ContainsKey(column_name))
                return -1;
            else
                return column_names[column_name];
        }

        public bool HasColumn(string column_name)
        {
            return ColumnIndex(column_name) >= 0;
        }

        public object GetValue(int row, string column_name)
        {
            int column_index = ColumnIndex(column_name);
            if (column_index < 0 || row < 0 || row > RowCount)
                return null;

            return rows[row][column_index];
        }

        public int GetInt(int row, string column_name)
        {
            try
            {
                var value = GetValue(row, column_name);
                return value == null ? 0 : Convert.ToInt32(value);
            }
            catch
            {
                return 0;
            }
        }

        public string GetString(int row, string column_name)
        {
            try
            {
                var value = GetValue(row, column_name);
                return value == null ? string.Empty : value.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        public bool GetBool(int row, string column_name)
        {
            try
            {
                var value = GetValue(row, column_name);
                return value == null ? false : Convert.ToBoolean(value);
            }
            catch
            {
                return false;
            }
        }

        public DateTime GetDateTime(int row, string column_name)
        {
            try
            {
                var value = GetValue(row, column_name);
                return value == null ? default(DateTime) : Convert.ToDateTime(value);
            }
            catch
            {
                return default(DateTime);
            }
        }

        public double GetDouble(int row, string column_name)
        {
            try
            {
                var value = GetValue(row, column_name);
                return value == null ? 0 : Convert.ToDouble(value);
            }
            catch
            {
                return 0;
            }
        }

        public long GetLong(int row, string column_name)
        {
            try
            {
                var value = GetValue(row, column_name);
                return value == null ? 0 : Convert.ToInt64(value);
            }
            catch
            {
                return 0;
            }
        }

        public byte[] GetByteArray(int row, string column_name)
        {
            try
            {
                var value = GetValue(row, column_name);
                if (value == null)
                    return new byte[0];

                return (byte[])value;
            }
            catch
            {
                return new byte[0];
            }
        }
    }
}

