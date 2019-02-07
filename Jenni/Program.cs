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
                throw new Exception("need template dir as first argument");

            var dirTemplate = Path.Combine(Directory.GetCurrentDirectory(), args[0]);
            Debug.WriteLine($"template folder: {dirTemplate}");

            var filesTemplate = Directory.GetFiles(dirTemplate, "*", SearchOption.AllDirectories);

            const int nSteps = 2;
            const int daysSpan = 600;
            var amts = MakeAmounts(nSteps, Math.Min(22, filesTemplate.Length / 9), filesTemplate.Length);
            var dates = MakeDates(TimeSpan.FromDays(daysSpan), nSteps);
            for (int i = 0; i < dates.Length; i++)
            {
                var date = dates[i];
                var amt = amts[i];
                Debug.WriteLine(">>> " + date);

                var iFile = 0;
                foreach (var filefinal in filesTemplate)
                {
                    var destfname = filefinal.Replace(dirTemplate, Directory.GetCurrentDirectory());
                    var fileContents = File.ReadAllText(filefinal);

                    // TODO: alter

                    if (!Directory.Exists(Path.GetDirectoryName(destfname)))
                        Directory.CreateDirectory(Path.GetDirectoryName(destfname));
                    File.WriteAllText(destfname, fileContents);
                    iFile++;
                    if (iFile >= amt)
                        break;
                }

                var message = rara.NextDouble() < 0.3
                    ? "wip"
                    : rara.NextDouble() < 0.3
                    ? "fix..."
                    : rara.NextDouble() < 0.2
                    ? "broken!.."
                    : "...";
                if (rara.NextDouble() < 0.6)
                    message = "TN - " + message;

//                AdminRun($@"
//date {date.Date.ToString("yyyy-MM-dd")}
//time {date.TimeOfDay.ToString("hh\\:mm\\:ss")}
//git add .
//git commit -m ""wippppp - {date}""");

//                AdminRun($@"
//git add .
//git commit -a -m ""{message}""");

                Debug.WriteLine($@"
git add .
git commit -a -m ""{message}""");
            }

            AdminRun($@"start ms-settings:dateandtime");

            foreach (var filefinal in filesTemplate)
            {
                var destfname = filefinal.Replace(dirTemplate, Directory.GetCurrentDirectory());
                if (!Directory.Exists(Path.GetDirectoryName(destfname)))
                    Directory.CreateDirectory(Path.GetDirectoryName(destfname));
                File.Copy(filefinal, destfname, true);
            }

//            AdminRun($@"
//git add .
//git commit -m ""...""");

            AdminCleanup();
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

        private static int[] MakeAmounts(int n, int min, int max)
        {
            var list = new List<int>();
            for (int i = 0; i < n; i++)
                list.Add(rara.Next(min, max));
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
