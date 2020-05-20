using System;
using System.Data;
using System.IO;

namespace Task_3
{
    class Program
    {
        static void Main(string[] args)
        {
            DataTable tbl = new DataTable();
            StreamReader sr = new StreamReader("Data.txt");
            string line;
            line = sr.ReadLine();       //Создаём шапку
            foreach (string titl in line.Split("  "))
            {
                if (titl != "")
                {
                    tbl.Columns.Add(new DataColumn(titl.Replace(" ", "").Replace("_", " ")));
                }
            }
            while (!sr.EndOfStream)
            {
                line = sr.ReadLine();
                var cols = line.Split("  ");
                if (cols.Length <= 1)       //исключаем пустые строчки
                    continue;
                DataRow dr = tbl.NewRow();
                for (int cIndex = 0, tindex = 0; cIndex < cols.Length; cIndex++, tindex++)
                {
                    if (cols[cIndex].Replace(" ", "") == "")        //не учитываем лишние пробелы
                    {
                        tindex--;
                        continue;
                    }
                    dr[tindex] = cols[cIndex].Replace("_", " ");
                }
                tbl.Rows.Add(dr);
            }
            sr.Close();

            for (int i = 0; i < tbl.Rows.Count; i++)        //подписываем типы данных
            {
                for (int j = 1; j < tbl.Columns.Count; j++)
                {
                    int ibuf;
                    float fbuf;
                    char cbuf;
                    if (Int32.TryParse(tbl.Rows[i][j].ToString(), out ibuf))
                    {
                        tbl.Rows[i][j] = tbl.Rows[i][j] + "-int";
                        string e = tbl.Rows[i][j].ToString();
                    }
                    else if (float.TryParse(tbl.Rows[i][j].ToString(), out fbuf))
                    {
                        tbl.Rows[i][j] = tbl.Rows[i][j] + "-float";
                        string e = tbl.Rows[i][j].ToString();
                    }
                    else if (char.TryParse(tbl.Rows[i][j].ToString(), out cbuf))
                    {
                        tbl.Rows[i][j] = tbl.Rows[i][j] + "-char";
                        string e = tbl.Rows[i][j].ToString();
                    }
                    else
                        tbl.Rows[i][j] = tbl.Rows[i][j] + "-string";
                }
            }

            DataView dataView = new DataView(tbl);      //сортируем таблицу
            dataView.Sort = "№ ASC";
            DataTable sortedDT = dataView.ToTable();
            for (int i = 0; i < sortedDT.Rows.Count; i++)
            {
                for (int j = 0; j < sortedDT.Columns.Count; j++)
                {
                    Console.Write(sortedDT.Rows[i][j] + "   ");
                }
                Console.Write("\n");
            }
        }
    }
}
