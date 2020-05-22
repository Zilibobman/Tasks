using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Office.Interop.Excel;
using System.IO;
using System.Xml.Linq;

namespace Task_4
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Clear();
            Application app = new Application();        //загружаем исходную таблицу
            Workbook workbook = app.Workbooks.Open("D:/Netology/Job/Tasks/Task_4/Task_4/ФайлСИсходнымиДанными.xls");
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

            System.Data.DataTable strucData = new System.Data.DataTable();      //редактируем её для удобства
            strucData.Columns.Add("Код счёта бюджетного учёта", typeof(string));
            strucData.Columns.Add("Номер банковского (лицевого) счета", typeof(string));
            strucData.Columns.Add("Остаток средств на начало года на счёте", typeof(string));
            strucData.Columns.Add("Средства в пути на начало года", typeof(string));
            strucData.Columns.Add("Остаток средств на счете на конец года", typeof(string));
            strucData.Columns.Add("Средства в пути на отчетную дату (в рублях)", typeof(string));
            for (row = 4; row <= ((dt.Rows.Count / dt.Columns.Count) - 1); row++)
            {
                strucData.Rows.Add();
                strucData.Rows[row - 4][0] = "1" + dt.Rows[row][1].ToString().Remove(dt.Rows[row][1].ToString().Length - 3, 3) + "000";
                strucData.Rows[row - 4][1] = dt.Rows[row][0].ToString();
                for (int r1 = 2; r1 < strucData.Columns.Count; r1++)
                {
                    strucData.Rows[row - 4][r1] = dt.Rows[row][r1].ToString();
                }
            }
            DataView dataView = new DataView(strucData);      //сортируем таблицу
            dataView.Sort = "Код счёта бюджетного учёта";
            System.Data.DataTable sortData = dataView.ToTable();
            app.Quit();


            XDocument xdoc = new XDocument(new XDeclaration("1.0", Encoding.GetEncoding("windows-1251").WebName, ""));      //записываем в xml
            XElement RootXml = new XElement("RootXml");

            XElement SchemaVersion = new XElement("SchemaVersion");
            XAttribute Number = new XAttribute("Number", "2");
            SchemaVersion.Add(Number);

            XElement Period = new XElement("Period");
            XAttribute Date = new XAttribute("Date", "2014-02-06");
            Period.Add(Date);

            XElement Source = new XElement("Source");
            XAttribute ClassCode = new XAttribute("ClassCode", "ДМС");
            XAttribute Code = new XAttribute("Code", "819");
            Source.Add(Code);
            Source.Add(ClassCode);

            XElement Form = new XElement("Form");
            XAttribute Code1 = new XAttribute("Code", "178");
            XAttribute Name1 = new XAttribute("Name", "Счета в кредитных организациях");
            XAttribute Status = new XAttribute("Status", "0");
            Form.Add(Status);
            Form.Add(Name1);
            Form.Add(Code1);

            for (int i = 1; i < sortData.Columns.Count; i++)
            {
                XElement Column = new XElement("Column");
                XAttribute Num = new XAttribute("Num", i.ToString());
                XAttribute Name = new XAttribute("Name", sortData.Columns[i].ToString());
                Column.Add(Name);
                Column.Add(Num);
                Form.Add(Column);
            }

            String CurentDoc = "";//sortData.Rows[0][0].ToString();
            XElement Doc = new XElement("Document");
            XAttribute PL = new XAttribute("ПлСч11", CurentDoc);
            for (int i = 0, numStr = 1; i < sortData.Rows.Count; i++, numStr++)
            {
                string str = numStr.ToString();
                for (int s = str.Length; s < 3; s++)
                {
                    str = str.Insert(0, "0");
                }
                string nextDoc = sortData.Rows[i][0].ToString();
                if (CurentDoc == "")
                {
                    CurentDoc = nextDoc;
                    PL.Value = nextDoc;
                    Doc.Add(PL);
                }
                if (CurentDoc == nextDoc)
                {
                    XElement Data = new XElement("Data");
                    XAttribute strData = new XAttribute("СТРОКА", str);
                    for (int j = 1; j < sortData.Columns.Count; j++)
                    {
                        XElement Px = new XElement("Px");
                        XAttribute Num = new XAttribute("Num", j.ToString());
                        XAttribute Value = new XAttribute("Value", sortData.Rows[i][j].ToString());
                        Px.Add(Value);
                        Px.Add(Num);
                        Data.Add(Px);
                    }
                    Data.Add(strData);
                    Doc.Add(Data);
                    if (i == sortData.Rows.Count - 1)
                    {
                        Data = new XElement("Data");
                        strData = new XAttribute("СТРОКА", "960");
                        for (int j = 2; j < sortData.Columns.Count; j++)
                        {
                            XElement Px = new XElement("Px");
                            XAttribute Num = new XAttribute("Num", j.ToString());
                            float val = 0;
                            foreach (XElement px in Doc.Elements("Data").Elements("Px"))
                            {
                                if (px.Attribute("Num").Value.ToString() == j.ToString())
                                {
                                    val += float.Parse(px.Attribute("Value").Value.ToString());
                                }
                            }
                            XAttribute Value = new XAttribute("Value", val.ToString());
                            Px.Add(Value);
                            Px.Add(Num);
                            Data.Add(Px);
                        }
                        Data.Add(strData);
                        Doc.Add(Data);
                        Form.Add(Doc);
                    }
                }
                else
                {
                    XElement Data = new XElement("Data");
                    XAttribute strData = new XAttribute("СТРОКА", "960");
                    for (int j = 2; j < sortData.Columns.Count; j++)
                    {
                        XElement Px = new XElement("Px");
                        XAttribute Num = new XAttribute("Num", j.ToString());
                        float val = 0;
                        foreach (XElement px in Doc.Elements("Data").Elements("Px"))
                        {
                            if (px.Attribute("Num").Value.ToString() == j.ToString())
                            {
                                val += float.Parse(px.Attribute("Value").Value.ToString());
                            }
                        }
                        XAttribute Value = new XAttribute("Value", val.ToString());
                        Px.Add(Value);
                        Px.Add(Num);
                        Data.Add(Px);
                    }
                    Data.Add(strData);
                    Doc.Add(Data);
                    Form.Add(Doc);
                    numStr = 0;
                    i--;
                    Doc = new XElement("Document");
                    PL = new XAttribute("ПлСч11", nextDoc);
                    CurentDoc = nextDoc;
                    Doc.Add(PL);
                }
            }
            RootXml.Add(SchemaVersion);
            SchemaVersion.Add(Period);
            Period.Add(Source);
            Source.Add(Form);
            xdoc.Add(RootXml);

            xdoc.Save("ФайлРезультат.xml");     //сохраняем

            for (int i = 0; i < sortData.Columns.Count; i++)
            {
                Console.Write("{0,35}", sortData.Columns[i].ToString());
            }
            Console.Write("\n");
            for (int i = 0; i < sortData.Rows.Count; i++)
            {
                for (int j = 0; j < sortData.Columns.Count; j++)
                {
                    Console.Write("{0,35}", sortData.Rows[i][j].ToString());
                }
                Console.Write("\n");
            }
            Console.ReadKey();
        }
    }
}

