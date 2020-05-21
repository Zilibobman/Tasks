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
                    uint uibuf;
                    float fbuf;
                    char cbuf;
                    byte bbuf;
                    sbyte sbbuf;
                    short shortbuf;
                    ushort ushortbuf;
                    ulong ulongbuf;
                    long longbuf;
                    if (byte.TryParse(tbl.Rows[i][j].ToString(), out bbuf))
                    {
                        tbl.Rows[i][j] = tbl.Rows[i][j] + "-byte";
                        string e = tbl.Rows[i][j].ToString();
                    }
                    else if (sbyte.TryParse(tbl.Rows[i][j].ToString(), out sbbuf))
                    {
                        tbl.Rows[i][j] = tbl.Rows[i][j] + "-sbyte";
                        string e = tbl.Rows[i][j].ToString();
                    }
                    else if (short.TryParse(tbl.Rows[i][j].ToString(), out shortbuf))
                    {
                        tbl.Rows[i][j] = tbl.Rows[i][j] + "-short";
                        string e = tbl.Rows[i][j].ToString();
                    }
                    else if (ushort.TryParse(tbl.Rows[i][j].ToString(), out ushortbuf))
                    {
                        tbl.Rows[i][j] = tbl.Rows[i][j] + "-ushort";
                        string e = tbl.Rows[i][j].ToString();
                    }
                    else if (Int32.TryParse(tbl.Rows[i][j].ToString(), out ibuf))
                    {
                        tbl.Rows[i][j] = tbl.Rows[i][j] + "-int";
                        string e = tbl.Rows[i][j].ToString();
                    }
                    else if (uint.TryParse(tbl.Rows[i][j].ToString(), out uibuf))
                    {
                        tbl.Rows[i][j] = tbl.Rows[i][j] + "-uint";
                        string e = tbl.Rows[i][j].ToString();
                    }
                    else if (long.TryParse(tbl.Rows[i][j].ToString(), out longbuf))
                    {
                        tbl.Rows[i][j] = tbl.Rows[i][j] + "-long";
                        string e = tbl.Rows[i][j].ToString();
                    }
                    else if (ulong.TryParse(tbl.Rows[i][j].ToString(), out ulongbuf))
                    {
                        tbl.Rows[i][j] = tbl.Rows[i][j] + "-ulong";
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
            for (int i = 0; i < tbl.Rows.Count; i++)
            {
                for (int n = tbl.Rows[i][0].ToString().Length; n < 25; n++)
                {
                    tbl.Rows[i][0] = tbl.Rows[i][0].ToString().Insert(0, "_");
                }
            }
            DataView dataView = new DataView(tbl);      //сортируем таблицу
            dataView.Sort = "№ ASC";
            DataTable sortedDT = dataView.ToTable();
            for (int i = 0; i < sortedDT.Rows.Count; i++)
            {
                    sortedDT.Rows[i][0] = sortedDT.Rows[i][0].ToString().Replace("_", "");
            }
            for (int i = 0; i < sortedDT.Columns.Count; i++)
            {
                Console.Write("{0,25}", sortedDT.Columns[i]);
            }
            Console.Write("\n");
            for (int i = 0; i < sortedDT.Rows.Count; i++)
            {
                for (int j = 0; j < sortedDT.Columns.Count; j++)
                {
                    Console.Write("{0,25}", sortedDT.Rows[i][j]);
                }
                Console.Write("\n");
            }
            Console.ReadLine();
        }
    }
}