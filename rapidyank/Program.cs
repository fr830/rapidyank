using System;
using RestSharp;
using System.Net;
using Newtonsoft.Json;
using System.Linq;
using System.Text;
using System.IO;
using CommandLine;

namespace rapidyank
{
    public class Program
    {
        public static int Main(string[] args)
        {
            try
            {
                new ArgumentException();
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                Config.Json = JsonConvert.DeserializeObject<Config>(File.ReadAllText("appsettings.json"));
                var parsed = Parser.Default.ParseArguments(args, typeof(OptTest), typeof(OptTransfer));
                parsed.WithParsed<OptTest>(o => 
                {
                    var cmd = new CmdTest(o);
                    cmd.Execute();
                });
                parsed.WithParsed<OptTransfer>(o => 
                {
                    var cmd = new CmdTransfer(o);
                    cmd.Execute();
                });
                
                return (int)ExitCode.Ok;
            }
            catch (FatalException exc)
            {
                Logger.Fatal(exc, exc.Message);
                return (int)exc.ExitCode;
            }
            catch (Exception exc)
            {
                Logger.Fatal(exc, exc.Message);
                return (int)ExitCode.UnknownError;
            }
            finally
            {
                if (System.Diagnostics.Debugger.IsAttached)
                    Console.ReadLine();
            }
        }
    }
}
