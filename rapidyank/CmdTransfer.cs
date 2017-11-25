using CommandLine;
using rapidyank.Rapid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using rapidyank.Db;

namespace rapidyank
{
    [Verb("transfer", HelpText = "Transfer information from web service to database")]
    public class OptTransfer
    {

    }

    public class CmdTransfer
    {
        private OptTransfer o = null;
        private Api Client { get; set; }
        private IDbConnection Db { get; set; }
        public int Errores { get; set; }

        public CmdTransfer(OptTransfer o)
        {
            this.o = o;
        }

        public void Execute()
        {
            Client = new Api(Config.Json.BaseUrl);
            Db = new SqlConnection(Config.Json.ConnectionString);

            var timer = new Timer();
            timer.Interval = Config.Json.IntervalSeconds * 1000;
            timer.AutoReset = true;
            timer.Elapsed += Timer_Elapsed;
            Logger.Info("Primer ejecución ...");
            Timer_Elapsed(timer, null);
            Logger.Info("Iniciando Timer ...");
            timer.Enabled = true;

            if (Console.ReadKey().Key == ConsoleKey.Q)
                return;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Logger.Info("Timer action start ...");
            // obtener valores del SCADA
            var readings = new List<reading>();
            try
            {
                if (!Client.CheckLoggedOn())
                {
                    if (!Client.Login(Config.Json.Username, Config.Json.Password))
                        throw new FatalException("Invalid user or password", exitCode: ExitCode.BadConfig);
                    Logger.Info("Login");
                }
                if (Config.Json.Channels.Any())
                {
                    Logger.Info("Obteniendo valores por canal del servidor SCADA ...");
                    var time = GetDateTime();
                    var datas = Client.GetCurCnlDataExt(
                        string.Join(',', Config.Json.Channels.Select(c => c[0])),
                        string.Join(',', Config.Json.Channels.Select(c => c[1])),
                        0);
                    foreach (var data in datas)
                        readings.Add(GetReading(data, time));
                }
                foreach (var view in Config.Json.Views)
                {
                    Logger.Info("Obteniendo valores por vista del servidor SCADA ...");
                    var time = GetDateTime();
                    readings.AddRange(Client.GetCurCnlDataExt(string.Empty, string.Empty, view)
                        .Select(d => GetReading(d, time)));
                }
            }
            catch (FatalException)
            {
                throw;
            }
            catch (Exception exc)
            {
                Errores++;
                if (Errores > Config.Json.MaxCommErrors)
                    throw new FatalException($"Se sobrepasó el límite de errores", innerException: exc, exitCode: ExitCode.CommunicationError);
                Logger.Error(exc, "Error al intentar obtener valores del SCADA");
                return;
            }

            // guardar en base de datos
            IDbTransaction tran = null;
            try
            {
                if (Db.State != ConnectionState.Open)
                {
                    Logger.Info($"Abriendo conexión a la base de datos ...");
                    Db.Open();
                }
                Logger.Info($"Iniciando transacción ...");
                tran = Db.BeginTransaction();
                Logger.Info($"Insertando registros ...");
                var count = Insert(readings, tran);
                Logger.Info($"Se insertaron {count} registros en la tabla readings, haciendo commit");
                tran.Commit();
            }
            catch (Exception exc)
            {
                if (tran != null)
                {
                    Logger.Info($"Haciendo rollback ...");
                    tran.Rollback();
                }

                Errores++;
                if (Errores > Config.Json.MaxCommErrors)
                    throw new FatalException($"Se sobrepasó el límite de errores", innerException: exc, exitCode: ExitCode.CommunicationError);
                Logger.Error(exc, "Error al intentar guardar valores en la base de datos");
                return;
            }
            finally
            {
                if (Db != null && Db.State == ConnectionState.Open)
                {
                    Logger.Info($"Cerrando conexión a la base de datos ...");
                    Db.Close();
                }
            }
        }

        private DateTime GetDateTime()
        {
            var time = DateTime.Now;
            return new DateTime(time.Ticks - (time.Ticks % TimeSpan.TicksPerSecond), time.Kind);
            //return new DateTime(time.Year, time.Month, time.Day, time.Hour, time.Minute, (int)Math.Round(time.Second + time.Millisecond / 1000D));
        }

        private reading GetReading(CnlDataExt data, DateTime time)
        {
            return new reading
            {
                channel = data.CnlNum,
                date = time,
                value = data.Val,
                status = data.Stat,
                text = data.Text,
                textandunit = data.TextWithUnit,
                color = data.Color
            };
        }

        private int Insert(IEnumerable<reading> readings, IDbTransaction tran)
        {
            return
                Db.Execute(
                    "INSERT INTO reading (channel, date, value, status, text, textandunit, color) " +
                    "VALUES (@channel, @date, @value, @status, @text, @textandunit, @color);",
                    param: readings,
                    transaction: tran,
                    commandTimeout: 15,
                    commandType: CommandType.Text);
        }
    }
}
