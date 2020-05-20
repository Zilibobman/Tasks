using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Office.Interop.Excel;
using System.IO;

namespace Task_4
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Clear();
            Application app = new Application();
            Workbook workbook = app.Workbooks.Open("D:/ProjectsC#/Новая папка/ConsoleApp1/ConsoleApp1/ФайлСИсходнымиДанными.xls");
            var worksheet = workbook.Sheets[1];
            Range range = worksheet.UsedRange;

            object[,] data = range.Value2;

            int cCnt = range.Columns.Count;
            int rCnt = range.Rows.Count;

            int row;
            int col;

            for (col = 1; col <= cCnt; col++)
            {
                dt.Columns.Add(col.ToString(), typeof(string));

                for (row = 1; row <= rCnt; row++)
                {
                    dt.Rows.Add();
                    dt.Rows[row - 1][col - 1] = data[row, col];
                }
            }

            System.IO.StringWriter writer = new System.IO.StringWriter();
            dt.WriteXml(writer, XmlWriteMode.WriteSchema, true);
            StreamWriter f = new StreamWriter("test.txt");
            f.WriteLine(writer);
            f.Close();
            writer.Close();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    Console.Write("{0,35}",dt.Rows[i][j].ToString());
                }
                Console.Write("\n");
            }
            Console.ReadKey();
        }
    }
}

