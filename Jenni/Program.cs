using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jenni
{
    class Program
    {
        private static Random rara = new Random();

        static void Main(string[] args)
        {
            // convention: args[0], template @ ../args[0]-template

            if (args.Length < 1)
                throw new Exception("need target dir, then convention is: args[0], template @ ../args[0]-template");

            var dirTarget = Path.Combine(Directory.GetCurrentDirectory(), args[0]);
            var dirTemplate = Path.Combine(Directory.GetCurrentDirectory(), $"{args[0]}-template");

            Debug.WriteLine($"target folder {dirTarget}");
            Debug.WriteLine($"template folder {dirTemplate}");

            var filesTemplate = Directory.GetFiles(dirTemplate, "*", SearchOption.AllDirectories);

            const int nSteps = 2;
            const int daysSpan = 600;
            var dates = MakeDates(TimeSpan.FromDays(daysSpan), nSteps);
            foreach (var date in dates)
            {
                Debug.WriteLine(">>> " + date);
                AdminRun($@"
date {date.Date.ToString("yyyy-MM-dd")}
time {date.TimeOfDay.ToString("hh\\:mm\\:ss")}");
            }
            AdminRun($@"
start ms-settings:dateandtime");

            AdminCleanup();

            //var index = 0;
            //foreach (var page in filesTemplate.Paginazu(85))
            //{
            //    Debug.WriteLine($"======= page {index} ========");
            //    foreach (var filename in page)
            //    {
            //        Debug.WriteLine($"  {filename}");
            //    }
            //    index++;
            //}
        }

        private static DateTime[] MakeDates(TimeSpan timespan, int n)
        {
            var nRandKeypoints = n / 2;
            var nRandKeypointsSample = n / 4;
            var list = new List<DateTime>();
            for (int i = 0; i < nRandKeypoints; i++)
                list.Add(DateTime.Now.Add(-TimeSpan.FromMilliseconds(timespan.TotalMilliseconds * rara.NextDouble())));
            for (int i = nRandKeypoints; i < n; i++)
                list.Add(list[rara.Next(nRandKeypointsSample)].Add(TimeSpan.FromSeconds(rara.Next(3000, 48*3600))));
            return list.OrderBy(x => x).ToArray();
        }

        private static void AdminRun(string batCode)
        {
            var tmpBat = BatFilename();
            File.WriteAllText(tmpBat, batCode);
            Process.Start(new ProcessStartInfo(tmpBat)
            {
                CreateNoWindow = true,
                ErrorDialog = false,
                UseShellExecute = false,
                RedirectStandardError = true,
                Verb = "runas"
            }).WaitForExit();
        }

        private static void AdminCleanup()
        {
            File.Delete(BatFilename());
        }

        private static string BatFilename()
        {
            return Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "ubliabl\\camel-with-67-humps.bat"
            );
        }

        


    }
}
