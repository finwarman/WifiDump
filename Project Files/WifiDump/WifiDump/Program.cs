using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WifiDump
{
    class Program
    {
        static void Main(string[] args)
        {
            //Get string output for 'netsh wlan show profiles' (To get wlan SSIDs) -
            string Netsh_Output = NetshProcess("wlan show profiles");

            //Remove unneeded info from Netsh output -
            Netsh_Output = Netsh_Output.Remove(0, Netsh_Output.IndexOf("    All User Profile"));
            //Remove preceding text (leaving line-by-line SSID strings) -
            Netsh_Output = Netsh_Output.Replace("   All User Profile     : ", "");

            //Write formatted netsh output to consoles (Shows SSIDs) -
            //Console.Write(Netsh_Output);

            //Get root directory (drive) where executed, for saving dump -
            string driveLetter = Path.GetPathRoot(Environment.CurrentDirectory);

            File.WriteAllText(Environment.CurrentDirectory + "\\tempSSIDs.txt", Netsh_Output);

            string SSIDandPASS = " SSIDs and Passwords: (Filename format is Day-Month_Hour-Minute)\r\n [Pass of 1 is open, however may require web login]\r\n\r\n";

            foreach (string SSID in File.ReadAllLines(Environment.CurrentDirectory + "\\tempSSIDs.txt"))
            {
                try
                {
                    string profile = NetshProcess("wlan show profile name = " + SSID.Remove(0, 0) + " key = clear");

                    if (!(profile.Contains("is not found on the system.")))
                    {
                        SSIDandPASS += SSID;
                        SSIDandPASS += "\r\n";


                        //Console.WriteLine(profile);

                        profile = profile.Remove(0, profile.IndexOf("Key"));


                        string key = profile.Substring(profile.IndexOf("Key") + 24, profile.IndexOf("\r\n") - (profile.IndexOf("Key") + 24));

                        if (key == "1") { key = "Open. (May require Web login)"; };

                        SSIDandPASS += key;
                        SSIDandPASS += "\r\n\r\n";
                    }



                }

                catch
                {
                    SSIDandPASS += "\r\n Cannot get full network info. \"\r\n";
                }

                File.WriteAllText(Environment.CurrentDirectory + "\\SSID_PASS_DUMP_" + DateTime.Now.ToString("dd-MM_hh-mm") + ".txt", SSIDandPASS);

                File.Delete(Environment.CurrentDirectory + "\\tempSSIDs.txt");



            };

        }

        //Function to create process Netsh and return output as string
        public static string NetshProcess(string args)
        {
            Process p = new Process();

            p.StartInfo = new ProcessStartInfo("netsh", args);

            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.CreateNoWindow = true;

            p.Start();

            string output = p.StandardOutput.ReadToEnd();

            p.WaitForExit();

            return output;
        }
    }
}
