using CommandLine;
using rapidyank.Rapid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rapidyank
{
    [Verb("test", HelpText = "Test if connections are working")]
    public class OptTest
    {
        [Option("webservice", HelpText = "Test if connection to web service is working")]
        public bool WebService { get; set; }

        [Option("database", HelpText = "Test if connection to database is working")]
        public bool Database { get; set; }
    }

    public class CmdTest
    {
        private OptTest o = null;

        public CmdTest(OptTest o)
        {
            this.o = o;
        }

        public void Execute()
        {
            if (o.WebService)
            {
                Logger.Info($"Connecting to web service {Config.Json.BaseUrl} ...");
                var client = new Api(Config.Json.BaseUrl);

                var login = client.Login(Config.Json.Username, Config.Json.Password);
                Logger.Info($"Login result: {login}");

                var checkloggedon = client.CheckLoggedOn();
                Logger.Info($"CheckLoggedOn result: {checkloggedon}");

                var cnldata = client.GetCurCnlData(Config.Json.Channels.First()[0]);
                Logger.Info($"GetCurCnlData result: {cnldata.Val}");

                var cnldataexts = client.GetCurCnlDataExt(string.Join(',', Config.Json.Channels.Select(c => c[0])), string.Join(',', Config.Json.Channels.Select(c => c[1])), 0);
                Logger.Info("CURRENT: " + string.Join(' ', cnldataexts.Select(c => $"[{c.CnlNum}: {c.TextWithUnit}]")));

                var hourcnldata = client.GetHourCnlData(2017, 11, 11, 0, 23, string.Join(',', Config.Json.Channels.Select(c => c[0])), string.Join(',', Config.Json.Channels.Select(c => c[1])), 0, false, "");
                foreach (var hcd in hourcnldata.Data)
                {
                    var values = string.Join(' ', hcd.CnlDataExtArr.Select(c => $"[{c.CnlNum}: {c.TextWithUnit}]"));
                    Logger.Info($"HOUR: {hcd.Hour} {values}");
                }

                for (int i = 101; i < 104; i++)
                {
                    var viewstamp = client.GetViewStamp(i);
                    Logger.Info($"View: {i} => Stamp: {viewstamp}");
                }
            }
        }
    }
}
