using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Xml_Area
{
    class Program
    {
        private static Model1 db = new Model1();
        static void Main(string[] args)
        {
            task_3a();
            task_3bc();
            task_3def();
            task_3g();
            task_4a();
            task_4b();
            task_4c();
            task_4d();
            task_5a();
            task5_c();
        }

        static void task_3a()
        {

            /*Все зоны/участки которые принадлежат PavilionId = 1, 
             * при этом каждая зоны должна находиться в отдельном XML файле 
             * с наименованием PavilionId.*/


            DirectoryInfo dir = new DirectoryInfo("Pavilion");
            if (!dir.Exists)
                Directory.CreateDirectory("Pavilion");

            foreach (Area area in db.Area.Where(o => o.PavilionId == 1))
            {
                XElement xe = new XElement("Area",
                    new XElement("Name",
                        new XAttribute("AreaId", area.AreaId),
                    area.Name),
                    new XElement("FullName", area.FullName),
                    new XElement("IP", area.IP)
                    );
                xe.Save("Pavilion/PavilionId_" + area.AreaId + ".xml");
            }
        }

        static void task_3bc()
        {

            /* Используя Directory класс, создать папки с название 
             * зон/участков, в каждую папку выгрузить XML файл на основе
             *  данных их таблицы.
*/
            DirectoryInfo dir = new DirectoryInfo("Task2");
            if (!dir.Exists)
                Directory.CreateDirectory("Task2");

            foreach (Area area in db.Area.Where(o => o.ParentId == 0))
            {
                Directory.CreateDirectory("Task2/" + area.Name);
                XElement xe = new XElement("Area",
                     new XElement("Name",
                         new XAttribute("AreaId", area.AreaId),
                     area.Name),
                     new XElement("FullName", area.FullName),
                     new XElement("IP", area.IP)
                     );

                string path = string.Format("{0}/{1}/{2}.xml",
                    "Task2", area.Name, area.AreaId);

                xe.Save(path);
            }
        }

        static void task_3def()
        {

            /*Выгрузить из таблицы Timer, данные только для зон у которых есть IP 
             * адрес, при этом в XML файл должны войти следующие поля: UserId, 
             * Area Name (name из Талицы Area), DateStart*/

            XElement xe = new XElement("Timers");
            foreach (Timer timer in db.Timer.Where(o => o.AreaId != null && o.DateFinish != null))
            {
                XElement te =
                     new XElement("Timer",
                         new XAttribute("TimerId", timer.TimerId),
                     new XAttribute("UserId", timer.UserId),
                     new XAttribute("DateStart", timer.DateStart),
                     new XElement("AreaName", timer.AreaId)
                     );
                xe.Add(te);
            }
            xe.Save("TimerInfo.xml");

        }

        static void task_3g()
        {
            XNamespace nameSpace = "http://logbook.itstep.org/";

            XElement xe = new XElement("Areas");

            foreach (Area area in db.Area)
            {
                XElement x0 = new XElement(nameSpace + "Area",
                    new XElement(nameSpace + "Name", area.Name),
                    new XElement(nameSpace + "FullName", area.FullName),
                    new XElement(nameSpace + "IP", area.IP));
                xe.Add(x0);
            }
            xe.Save("Task6.xml");

        }

        static void task_4a()
        {
            /*a.	Вывести на экран поля UserId, AreaId, DocumentId из XML файла пункта F*/

            XDocument xd = XDocument.Load("TimerInfo.xml");

            foreach (XElement timer in xd.Element("Timers").Elements("Timer"))
            {
                Console.WriteLine(timer);
                //Console.WriteLine(timer.Attribute("TimerId").Value);
                //Console.WriteLine(timer.Attribute("UserId").Value);
                //Console.WriteLine(timer.Attribute("DateStart").Value);
            }
        }

        static void task_4b()
        {
            /*b.	Выгрузить все данные из XML пункта F, заменить при этом в XML файла DateFinish 
             * =текущая дата и сохранить данный XML файл с наименованием – «TimeChangeToday_10.27.2017»*/

            XDocument xd = XDocument.Load("TimerInfo.xml");

            foreach (XElement timer in xd.Element("Timers").Elements("Timer"))
            {
                timer.Attribute("DateStart").Value = DateTime.Now.ToString();
            }
            xd.Save("TimeChangeToday_10.27.2017.xml");
        }

        static void task_4c()
        {
            /*c.	Вывести на экран, данные из XML пункта С, из веток – AreaId, Name, FullName, IP*/

            DirectoryInfo dir = new DirectoryInfo("Task2");
            if (dir.Exists)
            {
                foreach (DirectoryInfo directory in dir.GetDirectories())
                {
                    foreach (FileInfo file in directory.GetFiles())
                    {
                        XDocument xd = XDocument.Load(file.FullName);
                        Console.WriteLine(xd);
                    }
                }
            }
        }

        static void task_4d()
        {
            /*d.	Выгрузить из XML файла (XML из пункта G) на экран, только те, у которых
             * UserId !=0,а так же нет даты завершения DateFinish =null.*/

            XDocument xd = XDocument.Load("Task6.xml");

            foreach (XElement area in xd.Element("Areas").Elements("Area"))
            {
                if (area.Element("UserId").Value != null && area.Element("DateFinish").Value != null)  // не выгружали эти данные
                {
                    Console.WriteLine(area);
                }
            }
        }

        static void task_5a()
        {
            DirectoryInfo dir = new DirectoryInfo("Task2");
            int q = 0;
            if (dir.Exists)
            {
                foreach (DirectoryInfo directory in dir.GetDirectories())
                {
                    foreach (FileInfo file in directory.GetFiles())
                    {
                        XDocument xd = XDocument.Load(file.FullName);
                        //if (xd.Element("Area").Element("DateFinish").Value == null)  // не считывали это поле
                        //    ++q;
                        if (xd.Element("Area").Element("IP").Value == "")
                            ++q;
                    }
                }
            }
            Console.WriteLine(q);
        }

        static void task5_c()
        {
            DirectoryInfo dir = new DirectoryInfo("Task2");
            int q = 0;
            if (dir.Exists)
            {
                foreach (DirectoryInfo directory in dir.GetDirectories())
                {
                    foreach (FileInfo file in directory.GetFiles())
                    {
                        XDocument xd = XDocument.Load(file.FullName);
                        //q += Int32.Parse(xd.Element("Area").Element("OrderExecution").Value);
                        q += Int32.Parse(xd.Element("Area").Element("Name").Attribute("AreaId").Value);
                    }
                }
                Console.WriteLine(q);
            }
        }
    }
}
