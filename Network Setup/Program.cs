using System;
using System.Management;
using System.Text.RegularExpressions;

namespace Network_Setup
{
    class Program
    {


        static void Main(string[] args)
        {
            var query = new ObjectQuery("SELECT * FROM Win32_NetworkAdapter");

            using (var searcher = new ManagementObjectSearcher(query))
            {
                var queryCollection = searcher.Get();

                foreach (ManagementObject m in queryCollection)
                {
                    string name = m["Name"].ToString();

                    if (CleanupString(name).Contains("wifi"))
                    {

                        if (Boolean.Parse(m["NetEnabled"].ToString()))
                            m.InvokeMethod("Disable", null);
                        else
                            m.InvokeMethod("Enable", null);
                    }

                    else if (!IsVPN(name) && CleanupString(name).Contains("ethernet"))
                    {
                         if (Boolean.Parse(m["NetEnabled"].ToString()))
                            m.InvokeMethod("Disable", null);
                        else
                            m.InvokeMethod("Enable", null);

                    }

                }
            }

        }

        static string CleanupString(string str)
        {
            return Regex.Replace(str, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled).ToLower();
        }

        static bool IsVPN(string str)
        {
            if (str.ToUpper().Contains("VPN"))
                return true;
            else
                return false;
        }
    }
}
