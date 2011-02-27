using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

// The bulk of your plugin's code belongs in this file.
namespace CSPluginTemplate
{
    class PluginCode
    {

        private string _output = "test";
        // 'Update', 'Update2', and 'GetString' all return data back to Rainmeter, depending on
        // if the Rainmeter measure wants a numeric value or a string/text data.
        //
        // The 'Instance' member contains all of the data necessary to read the INI file
        // passed to your plugin when this instance was first initialized.  Remember: your plugin
        // may be initialized multiple times.  For example, a plugin that reads the free space
        // of a hard drive may be called multiple times -- once for each installed hard drive.

        public UInt32 Update(Rainmeter.Settings.InstanceSettings Instance)
        {
            return 7;
        }

        public double Update2(Rainmeter.Settings.InstanceSettings Instance)
        {
            return 7.0;
        }

        public string GetString(Rainmeter.Settings.InstanceSettings Instance)
        {
            WriteDebug("ConfigName", Instance.ConfigName);
            WriteDebug("Variable:BatchFile", Instance.INI_Value("BatchFile") );

            var now = DateTime.Now;
            var lastAccess = Instance.GetVariable("LastAccess");
            var updateSpan = Convert.ToInt32(Instance.INI_Value("Update"));
            var wait = Convert.ToInt32(Instance.INI_Value("Update"));
            if (string.IsNullOrEmpty(lastAccess))
                wait = 1;
            else
            {
                TimeSpan span = now - Convert.ToDateTime(lastAccess);
                if (span.TotalMilliseconds > updateSpan)
                {
                    wait = 1;
                }
            }
            
            var file = Instance.INI_Value("BatchFile");
            System.Threading.Thread.Sleep(wait);
            Instance.SetVariable("LastAccess", DateTime.Now);
            return GetBatchString(file);

        }


        // 'ExecuteBang' is a way of Rainmeter telling your plugin to do something *right now*.
        // What it wants to do can be defined by the 'Command' parameter.
        public void ExecuteBang(Rainmeter.Settings.InstanceSettings Instance, string Command)
        {
            return;
        }

        private string GetBatchString(string path)
        {
            var p = new Process
            {
                StartInfo =
                {
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    FileName = path
                }
            };
            p.Start();
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            return output;
        }

        private void WriteDebug(string name, string value)
        {
            return;
            var w = new StreamWriter(@"c:\debug.out", true);
            var n = DateTime.Now;
            w.WriteLine(n.ToShortDateString() + " " + n.ToShortTimeString() + " Name:" + name +" Value:" + value);
            w.Close();
            w.Dispose();
        }
    }
}
