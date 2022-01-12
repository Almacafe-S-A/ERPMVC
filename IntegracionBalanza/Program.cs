using AspNetCore.Http.Extensions;
using Hangfire;
using IntegracionBalanza.Utils;
using Microsoft.AspNetCore.Http;
//using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
//using Microsoft.IdentityModel.Protocols;
using MoreLinq;
using Newtonsoft.Json;
using NLog;
using NLog.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Timers;
using Topshelf;


namespace IntegracionBalanza
{

    public class MyService
    //public class  MyService : ServiceControl
    {
        static MyConfig moduleSettings = new MyConfig();
        static connectionStrings _cone = new connectionStrings();
        readonly Timer _timer = new Timer();
        readonly Timer _timersalidas = new Timer();
        readonly Timer _timerproductos = new Timer();
        private readonly Microsoft.Extensions.Logging.ILogger _logger;
        Runner runner;
        public MyService()
        {

            //    string currentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);

            var builder = new ConfigurationBuilder()
            // .SetBasePath(Directory.GetCurrentDirectory())
            .SetBasePath(System.AppDomain.CurrentDomain.BaseDirectory)
           .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
           .AddEnvironmentVariables();

            IConfigurationRoot configuration = builder.Build();

            var servicesProvider = Program.BuildDi(configuration);
            using (servicesProvider as IDisposable)
            {
                runner = servicesProvider.GetRequiredService<Runner>();
                runner.DoAction("Action1");
            }



            configuration.GetSection("AppSettings").Bind(moduleSettings);
            configuration.GetSection("connectionStrings").Bind(_cone);


            //TimeSpan ts = TimeSpan.FromTicks(moduleSettings.interval);
            //Int32 minutes = Convert.ToInt32(Math.Round(ts.TotalMinutes,0));

            //TimeSpan ts2 = TimeSpan.FromTicks(moduleSettings.interval2);
            //Int32 minutes2 = Convert.ToInt32(Math.Round(ts2.TotalMinutes, 0));

            //TimeSpan ts3 = TimeSpan.FromTicks(moduleSettings.interval3);
            //Int32 minutes3 = Convert.ToInt32(Math.Round(ts2.TotalMinutes, 0));

            //PeriodicTask
            //.Run(GuardarData, TimeSpan.FromSeconds(4))
            //.GetAwaiter()
            //.GetResult();

            // GuardarData();

            //TimeSpan _tEntradas = new TimeSpan(0, minutes, 0);           
            //var t1 = PeriodicTask.Run(GuardarData,_tEntradas);

            //TimeSpan _tSalidas = new TimeSpan(0, minutes2, 0);
            //var t2 = PeriodicTask.Run(GuardarSalidas, _tSalidas);

            //TimeSpan _tProductos = new TimeSpan(0, minutes3,0);
            //var t3 = PeriodicTask.Run(GuardarProductos, _tProductos);

            //TimeSpan _tEntradas = new TimeSpan(0, minutes, 0);
            //Action doWorkAction = new Action(GuardarData);
            //var t1 = PeriodicTask.Run(doWorkAction, _tEntradas);

            //  TimeSpan _tSalidas = new TimeSpan(moduleSettings.interval2);
            //  Action doWorkSalidas = new Action(GuardarSalidas);
            //var t2=    PeriodicTask.Run(doWorkSalidas, _tEntradas);


            //  TimeSpan _tProductos = new TimeSpan(moduleSettings.interval3);
            //  Action doWorkProductos = new Action(GuardarProductos);
            //var t3=   PeriodicTask.Run(doWorkProductos, _tProductos);

            //  t1.Wait();
            //t2.Wait();
            //t3.Wait();


            _timer = new Timer(moduleSettings.interval) { AutoReset = true };
            _timer.Elapsed += (sender, eventArgs) =>  GuardarData();

            _timerproductos = new Timer(moduleSettings.interval3) { AutoReset = true };
            _timerproductos.Elapsed += (sender, eventArgs) => GuardarProductos();

            _timersalidas = new Timer(moduleSettings.interval2) { AutoReset = true };
            _timersalidas.Elapsed += (sender, eventArgs) => GuardarSalidas();



        }

        //public void Start() {  GuardarData().Wait(); }
        //public void Stop() { _timer.Stop(); }

        private IDisposable _webapp;
        public void Start()
        {
            try
            {

                _timer.Start(); _timersalidas.Start();
                _timerproductos.Start();

            }
            catch (Exception ex)
            {
                runner.LogError($"Error al arrancar el servicio{ex.ToString()}");
                //_logger.LogError($"Error al arrancar el servicio{ex.ToString()}");
                Console.WriteLine(ex.ToString());
            }

        }
        public void Stop()
        {
            try
            {
               // _timer.Stop(); _timersalidas.Stop(); _timerproductos.Stop();
            }
            catch (Exception ex)
            {
                runner.LogError($"Error al arrancar el servicio{ex.ToString()}");
                //throw ex;
            }
        }

        //public void Start() { _timer.Start();  }
        //public void Stop() { _timer.Stop();  }


        //public  bool Start(HostControl hostControl)
        //{
        //    // Task.Run(()=>GuardarData()).Wait();
        //    //GlobalConfiguration.Configuration
        //    //   .UseColouredConsoleLogProvider()
        //    //   .UseSqlServerStorage("Data Source=DESKTOP-RFQ3R0I;Initial Catalog=ERP;User id=sa;password=sql20.15;");

        //    //using (var server = new BackgroundJobServer())
        //    //{
        //    //  //  MyService _miservicio = new MyService();
        //    //    //BackgroundJob.Enqueue(() => _miservicio.GuardarData());

        //    //    RecurringJob.AddOrUpdate(() =>GuardarData(), Cron.MinuteInterval(4));
        //    //    Console.WriteLine("Hangfire Server started. Press any key to exit...");
        //    //    //Console.ReadKey();
        //    //}


        //    return true;
        //}

        //public bool Stop(HostControl hostControl)
        //{
        //    Console.WriteLine("Stop");
        //    return true;
        //}

        OdbcConnection cnn = new OdbcConnection();
        private async Task<bool> Respuestas(List<Int64> listado)
        {
            try
            {
                if (listado.Count > 0)
                {
                    string connetionString = null;

                    connetionString = _cone.AccessConnectionString;

                    if (cnn.State == ConnectionState.Closed)
                    {
                        cnn = new OdbcConnection(connetionString);
                        cnn.Open();
                    }

                    //OdbcCommand command = new OdbcCommand("SELECT clave_e FROM boleto_ent  ", cnn);

                    string baseadress = moduleSettings.urlbase;

                    List<Int64> _noencontrados = new List<Int64>();

                    //ServicePointManager.SecurityProtocol = SecurityProtocolType.SystemDefault;
                    //ServicePointManager.ServerCertificateValidationCallback +=
                    //    (sender, cert, chain, sslPolicyErrors) => { return true; };


                    HttpClient _client = new HttpClient();
                    //ServicePointManager.ServerCertificateValidationCallback =
                    //delegate (object s, X509Certificate certificate,
                    //                X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

                    Stopwatch stopwatch = Stopwatch.StartNew();
                    runner.LogInformation($"Arranca comparación Entrada: {stopwatch.Elapsed}");
                    var resultlogin = await _client.PostAsJsonAsync(baseadress + "api/cuenta/login", new UserInfo { Email = moduleSettings.UserEmail, Password = moduleSettings.UserPassword });
                    if (resultlogin.IsSuccessStatusCode)
                    {
                        string webtoken = await (resultlogin.Content.ReadAsStringAsync());
                        UserToken _userToken = JsonConvert.DeserializeObject<UserToken>(webtoken);
                        _client = new HttpClient();
                        //      ServicePointManager.ServerCertificateValidationCallback =
                        //delegate (object s, X509Certificate certificate,
                        //                X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

                        _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _userToken.Token);
                        var result = await _client.PostAsJsonAsync(baseadress + "api/Boleto_Ent/GetBoleto_EntByClaveEList", listado);

                        string valorrespuesta = "";
                        if (result.IsSuccessStatusCode)
                        {
                            valorrespuesta = await (result.Content.ReadAsStringAsync());
                            _noencontrados = JsonConvert.DeserializeObject<List<Int64>>(valorrespuesta);

                        }
                    }

                    runner.LogInformation($"Termina comparación Entrada: {stopwatch.Elapsed}");


                    // List<Int64> noexistentes = listado.Except(_encontrados).ToList();

                    string listadoentradas = string.Join(",", _noencontrados);
                    Stopwatch stopwatch2 = Stopwatch.StartNew();
                    runner.LogInformation($"Inicia a consultar los no encuentrados en access Entrada: {stopwatch2.Elapsed}");
                    if (_noencontrados.Count > 0)
                    {
                        OdbcCommand command = new OdbcCommand("SELECT * FROM boleto_ent where clave_e in( " + listadoentradas + ") ", cnn);
                        Boleto_Ent _boleta_ent = new Boleto_Ent();
                        List<Boleto_Ent> _entradasnoexistentes = new List<Boleto_Ent>();
                        using (OdbcDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                _boleta_ent = new Boleto_Ent();
                                _boleta_ent.clave_e = Convert.ToInt64(reader.GetValue(0).ToString());
                                _boleta_ent.clave_C = reader.GetValue(1).ToString();
                                _boleta_ent.clave_o = reader.GetValue(2).ToString();
                                _boleta_ent.clave_p = reader.GetValue(3).ToString();
                                _boleta_ent.completo = Convert.ToBoolean(reader.GetValue(4).ToString());
                                _boleta_ent.fecha_e = Convert.ToDateTime(reader.GetValue(5).ToString());
                                _boleta_ent.hora_e = reader.GetValue(6).ToString();
                                _boleta_ent.placas = reader.GetValue(7).ToString();

                                _boleta_ent.conductor = reader.GetValue(8).ToString();
                                _boleta_ent.peso_e = Convert.ToInt32(reader.GetValue(9).ToString());
                                _boleta_ent.observa_e = reader.GetValue(10).ToString();
                                _boleta_ent.nombre_oe = reader.GetValue(11).ToString();
                                _boleta_ent.turno_oe = reader.GetValue(12).ToString();
                                _boleta_ent.unidad_e = reader.GetValue(13).ToString();

                                _boleta_ent.bascula_e = reader.GetValue(14).ToString();
                                _boleta_ent.t_entrada = Convert.ToInt32(reader.GetValue(15).ToString());
                                _boleta_ent.clave_u = reader.GetValue(16).ToString();

                                _entradasnoexistentes.Add(_boleta_ent);


                            }
                        }

                        runner.LogInformation($"Termina de agregar de access los no encontrados Entrada: {stopwatch2.Elapsed}");
                        //  ServicePointManager.ServerCertificateValidationCallback =
                        //delegate (object s, X509Certificate certificate,
                        //                X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                        runner.LogInformation($"Entradas No encontradas: {_entradasnoexistentes.Count.ToString()}");
                        //_logger.LogInformation($"Entradas No encontradas: {_entradasnoexistentes.Count.ToString()}");
                        Stopwatch stopwatch3 = Stopwatch.StartNew();
                        runner.LogInformation($"Arranca Insert Entrada: {stopwatch3.Elapsed}");
                        resultlogin = await _client.PostAsJsonAsync(baseadress + "api/cuenta/login", new UserInfo { Email = moduleSettings.UserEmail, Password = moduleSettings.UserPassword });
                        if (resultlogin.IsSuccessStatusCode)
                        {
                            string webtoken = await (resultlogin.Content.ReadAsStringAsync());
                            UserToken _userToken = JsonConvert.DeserializeObject<UserToken>(webtoken);
                            _client = new HttpClient();
                            //ServicePointManager.ServerCertificateValidationCallback =
                            //   delegate (object s, X509Certificate certificate,
                            //       X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _userToken.Token);

                            var result = await _client.PostAsJsonAsync(baseadress + "api/Boleto_Ent/GetBoleto_E_ByClassList", _entradasnoexistentes);
                            string valorrespuesta = "";
                            if (result.IsSuccessStatusCode)
                            {
                                valorrespuesta = await (result.Content.ReadAsStringAsync());
                                _noencontrados = JsonConvert.DeserializeObject<List<Int64>>(valorrespuesta);

                            }
                        }

                        runner.LogInformation($"Termina de insert  de entradas: {stopwatch3.Elapsed}");
                    }

                    // cnn.Dispose();
                    // cnn.Close();
                }
            }
            catch (Exception ex)
            {
                runner.LogError($"Error al arrancar el servicio{ex.ToString()}");
            }

            return true;
        }


        private async Task<bool> RespuestasSalidas(List<Int64> listado)
        {
            try
            {
                if (listado.Count > 0)
                {
                    string connetionString = null;
                    // OdbcConnection cnn;
                    connetionString = _cone.AccessConnectionString;

                    if (cnn.State == ConnectionState.Closed)
                    {
                        cnn = new OdbcConnection(connetionString);
                        cnn.Open();
                    }

                    string baseadress = moduleSettings.urlbase;

                    //ServicePointManager.SecurityProtocol = SecurityProtocolType.SystemDefault;
                    //ServicePointManager.ServerCertificateValidationCallback +=
                    //    (sender, cert, chain, sslPolicyErrors) => { return true; };
                    //ServicePointManager.ServerCertificateValidationCallback =
                    //     delegate (object s, X509Certificate certificate,
                    //                     X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };



                    List<Int64> _noencontrados = new List<Int64>();
                    HttpClient _client = new HttpClient();
                    var resultlogin = await _client.PostAsJsonAsync(baseadress + "api/cuenta/login", new UserInfo { Email = moduleSettings.UserEmail, Password = moduleSettings.UserPassword });
                    if (resultlogin.IsSuccessStatusCode)
                    {
                        string webtoken = await (resultlogin.Content.ReadAsStringAsync());
                        UserToken _userToken = JsonConvert.DeserializeObject<UserToken>(webtoken);
                        _client = new HttpClient();
                        //     ServicePointManager.ServerCertificateValidationCallback =
                        //delegate (object s, X509Certificate certificate,
                        //                X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                        _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _userToken.Token);
                        var result = await _client.PostAsJsonAsync(baseadress + "api/Boleto_Sal/GetBoleto_SalByClaveEList", listado);
                        string valorrespuesta = "";
                        if (result.IsSuccessStatusCode)
                        {
                            valorrespuesta = await (result.Content.ReadAsStringAsync());
                            _noencontrados = JsonConvert.DeserializeObject<List<Int64>>(valorrespuesta);

                        }
                    }


                    //  List<Int64> noexistentes = listado.Except(_encontrados).ToList();

                    string listadosalidas = string.Join(",", _noencontrados);

                    if (_noencontrados.Count > 0)
                    {
                        OdbcCommand command = new OdbcCommand("SELECT * FROM boleto_sal where clave_e in( " + listadosalidas + ") ", cnn);
                        Boleto_Sal _boleta_ent = new Boleto_Sal();
                        List<Boleto_Sal> _salidasnoexistentes = new List<Boleto_Sal>();
                        using (OdbcDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                _boleta_ent = new Boleto_Sal();
                                _boleta_ent.clave_e = Convert.ToInt64(reader.GetValue(0).ToString());
                                _boleta_ent.clave_o = reader.GetValue(1).ToString();
                                _boleta_ent.fecha_s = Convert.ToDateTime(reader.GetValue(2).ToString());
                                _boleta_ent.hora_s = reader.GetValue(3).ToString();
                                _boleta_ent.peso_s = Convert.ToDouble(reader.GetValue(4));
                                _boleta_ent.peso_n = Convert.ToDouble(reader.GetValue(5));
                                _boleta_ent.observa_s = reader.GetValue(6).ToString();
                                _boleta_ent.turno_s = reader.GetValue(7).ToString();
                                _boleta_ent.bascula_s = reader.GetValue(8).ToString();
                                _boleta_ent.s_manual = Convert.ToBoolean(reader.GetValue(9));


                                _salidasnoexistentes.Add(_boleta_ent);


                            }
                        }
                        //   _logger.LogInformation($"Salidas No encontradas: {_noencontrados.Count.ToString()}");
                        runner.LogInformation($"Salidas No encontradas: {_salidasnoexistentes.Count.ToString()}");
                        //  ServicePointManager.ServerCertificateValidationCallback =
                        //delegate (object s, X509Certificate certificate,
                        //                X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                        resultlogin = await _client.PostAsJsonAsync(baseadress + "api/cuenta/login", new UserInfo { Email = moduleSettings.UserEmail, Password = moduleSettings.UserPassword });
                        if (resultlogin.IsSuccessStatusCode)
                        {
                            string webtoken = await (resultlogin.Content.ReadAsStringAsync());
                            UserToken _userToken = JsonConvert.DeserializeObject<UserToken>(webtoken);
                            _client = new HttpClient();
                            //ServicePointManager.ServerCertificateValidationCallback =
                            //        delegate (object s, X509Certificate certificate,
                            //      X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _userToken.Token);
                            var result = await _client.PostAsJsonAsync(baseadress + "api/Boleto_Sal/GetBoleto_S_ByClassList", _salidasnoexistentes);
                            string valorrespuesta = "";
                            if (result.IsSuccessStatusCode)
                            {
                                valorrespuesta = await (result.Content.ReadAsStringAsync());
                                _noencontrados = JsonConvert.DeserializeObject<List<Int64>>(valorrespuesta);

                            }
                        }
                    }

                }
                // cnn.Dispose();
                //  cnn.Close();
            }
            catch (Exception ex)
            {
                runner.LogError($"Error al arrancar el servicio{ex.ToString()}");
                //_logger.LogError($"Error en metodo RespuestasSalidas(List<long> el servicio: {ex.ToString()}");
                //throw ex;
            }
       

            return true;
        }


        // public Action GuardarData()
        public  async Task  GuardarData()
        //  public async Task<bool> GuardarData()
        {
            string connetionString = null;
            //  OdbcConnection cnn;
            connetionString = _cone.AccessConnectionString;

            try
            {
                //await GuardarProductos();

                string baseadress = moduleSettings.urlbase;

                if (cnn.State == ConnectionState.Closed)
                {
                    cnn = new OdbcConnection(connetionString);
                    cnn.Open();
                }


                Int64 max = 0;
                HttpClient _client = new HttpClient();
                var resultlogin = await _client.PostAsJsonAsync(baseadress + "api/cuenta/login", new UserInfo { Email = moduleSettings.UserEmail, Password = moduleSettings.UserPassword });
                if (resultlogin.IsSuccessStatusCode)
                {
                    string webtoken = await (resultlogin.Content.ReadAsStringAsync());
                    UserToken _userToken = JsonConvert.DeserializeObject<UserToken>(webtoken);
                    _client = new HttpClient();
                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _userToken.Token);
                    var result2 = await _client.GetAsync(baseadress + "api/Boleto_Ent/GetBoleto_EntMax");
                    string valorrespuesta = "";
                    if (result2.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result2.Content.ReadAsStringAsync());
                        max = JsonConvert.DeserializeObject<Int64>(valorrespuesta);

                    }
                }

                //OdbcCommand command = new OdbcCommand("SELECT count(clave_e) FROM boleto_ent where clave_e > ? ", cnn);
                //command.Parameters.Add("@clave_e", OdbcType.Int).Value = max;
                //Int64 contarentradas = 0;
                //using (OdbcDataReader reader = command.ExecuteReader())
                //{
                //    while (reader.Read())
                //    {
                //        contarentradas = (Convert.ToInt64(reader.GetValue(0)));
                //    }

                //}



                //Int64 countapientradas = 0;
                // _client = new HttpClient();
                // resultlogin = await _client.PostAsJsonAsync(baseadress + "api/cuenta/login", new UserInfo { Email = moduleSettings.UserEmail, Password = moduleSettings.UserPassword });
                //if (resultlogin.IsSuccessStatusCode)
                //{
                //    string webtoken = await (resultlogin.Content.ReadAsStringAsync());
                //    UserToken _userToken = JsonConvert.DeserializeObject<UserToken>(webtoken);
                //    _client = new HttpClient();
                //    //     ServicePointManager.ServerCertificateValidationCallback =
                //    //delegate (object s, X509Certificate certificate,
                //    //                X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                //    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _userToken.Token);
                //    var result3 = await _client.GetAsync(baseadress + "api/Boleto_Ent/GetBoleto_EntCount");
                //    string valorrespuesta = "";
                //    if (result3.IsSuccessStatusCode)
                //    {
                //        valorrespuesta = await (result3.Content.ReadAsStringAsync());
                //        countapientradas = JsonConvert.DeserializeObject<Int64>(valorrespuesta);

                //    }
                //}


                //if (countapientradas != contarentradas)
                //{

                OdbcCommand command = new OdbcCommand("SELECT clave_e FROM boleto_ent where clave_e > ? ", cnn);
               // OdbcCommand command = new OdbcCommand("Select clave_e FROM  FROM boleto_sal where clave_e > ?  ", cnn);
                command.Parameters.Add("@clave_e", OdbcType.Int).Value = max;
                List<Int64> entradas = new List<Int64>();
                using (OdbcDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        entradas.Add(Convert.ToInt64(reader.GetValue(0)));
                    }
                    // Console.WriteLine("Connection Open ! ");
                    // OdbcConnection connection = new OdbcConnection("");
                    //command.Parameters.Add("@clave_e", OdbcType.Int).Value = _Boleto_Entp.clave_e;
                }

                double cantidad = entradas.Count / moduleSettings.NoTareas;
                if (cantidad == 0) { cantidad = 1; }
                Int32 round = Convert.ToInt32(Math.Round(cantidad, 0, MidpointRounding.AwayFromZero));
                var r = entradas.Batch(round);

                List<List<Int64>> _programaciones = new List<List<Int64>>();
                _programaciones = r.Select(s => s.ToList()).ToList();

                List<Int64> _programacion = new List<Int64>();
                _programacion = new List<Int64>();
                _programaciones.Add(_programacion); _programaciones.Add(_programacion);
                _programaciones.Add(_programacion); _programaciones.Add(_programacion);
                _programaciones.Add(_programacion); _programaciones.Add(_programacion);
                _programaciones.Add(_programacion); _programaciones.Add(_programacion);
                _programaciones.Add(_programacion); _programaciones.Add(_programacion);


                //var result1 = await Task.Run(() => Respuestas(_programaciones[0]));
                //var result2 = await Task.Run(() => Respuestas(_programaciones[1]));
                //var result3 = await Task.Run(() => Respuestas(_programaciones[2]));
                //var result4 = await Task.Run(() => Respuestas(_programaciones[3]));
                //var result5 = await Task.Run(() => Respuestas(_programaciones[4]));
                //var result6 = await Task.Run(() => Respuestas(_programaciones[5]));
                //var result7 = await Task.Run(() => Respuestas(_programaciones[6]));
                //var result8 = await Task.Run(() => Respuestas(_programaciones[7]));
                //var result9 = await Task.Run(() => Respuestas(_programaciones[8]));
                //var result10 = await Task.Run(() => Respuestas(_programaciones[9]));

                //Console.WriteLine($"Primer resultado:{result1}");

                // _programacion = _programacion.Take(500).ToList();
                // await Respuestas(entradas);

                //var programacion1 = await Task.Factory.StartNew(() => Respuestas(_programaciones[0]));
                //var programacion2 = await Task.Factory.StartNew(() => Respuestas(_programaciones[1]));
                //var programacion3 = await Task.Factory.StartNew(() => Respuestas(_programaciones[2]));
                //var programacion4 = await Task.Factory.StartNew(() => Respuestas(_programaciones[3]));
                //var programacion5 = await Task.Factory.StartNew(() => Respuestas(_programaciones[4]));
                //var programacion6 = await Task.Factory.StartNew(() => Respuestas(_programaciones[5]));
                //var programacion7 = await Task.Factory.StartNew(() => Respuestas(_programaciones[6]));
                //var programacion8 = await Task.Factory.StartNew(() => Respuestas(_programaciones[7]));
                //var programacion9 = await Task.Factory.StartNew(() => Respuestas(_programaciones[8]));
                //var programacion10 = await Task.Factory.StartNew(() => Respuestas(_programaciones[9]));

                var programacion1 = Task.Factory.StartNew(() => Respuestas(_programaciones[0]));
                var programacion2 = Task.Factory.StartNew(() => Respuestas(_programaciones[1]));
                var programacion3 = Task.Factory.StartNew(() => Respuestas(_programaciones[2]));
                var programacion4 = Task.Factory.StartNew(() => Respuestas(_programaciones[3]));
                var programacion5 = Task.Factory.StartNew(() => Respuestas(_programaciones[4]));
                var programacion6 = Task.Factory.StartNew(() => Respuestas(_programaciones[5]));
                var programacion7 = Task.Factory.StartNew(() => Respuestas(_programaciones[6]));
                var programacion8 = Task.Factory.StartNew(() => Respuestas(_programaciones[7]));
                var programacion9 = Task.Factory.StartNew(() => Respuestas(_programaciones[8]));
                var programacion10 = Task.Factory.StartNew(() => Respuestas(_programaciones[9]));

                var result = programacion1.Result;
                // var result2 = programacion5.Result;

                // await GuardarSalidas();

                //  cnn.Dispose();
                //  cnn.Close();


                //}//Comparar base de datos local con remota


            }
            catch (Exception ex)
            {
                runner.LogError($"Error al arrancar el servicio{ex.ToString()}");
                // _logger.LogError($"Error en metodo GuardarData el servicio: {ex.ToString()}");
            }

          //  return true;
        }


        // public async Task<Action> GuardarSalidas()
        public async Task GuardarSalidas()
        //public async Task<bool> GuardarSalidas()
        {

            string connetionString = null;
            // OdbcConnection cnn;
            connetionString = _cone.AccessConnectionString;

            try
            {
                if (cnn.State == ConnectionState.Closed)
                {
                    cnn = new OdbcConnection(connetionString);
                    cnn.Open();
                }
                string baseadress = moduleSettings.urlbase;

                Int64 max = 0;
                HttpClient _client = new HttpClient();
                var resultlogin = await _client.PostAsJsonAsync(baseadress + "api/cuenta/login", new UserInfo { Email = moduleSettings.UserEmail, Password = moduleSettings.UserPassword });
                if (resultlogin.IsSuccessStatusCode)
                {
                    string webtoken = await (resultlogin.Content.ReadAsStringAsync());
                    UserToken _userToken = JsonConvert.DeserializeObject<UserToken>(webtoken);
                    _client = new HttpClient();
                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _userToken.Token);
                    var result = await _client.GetAsync(baseadress + "api/Boleto_Sal/GetBoleto_SalMax");
                    string valorrespuesta = "";
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        max = JsonConvert.DeserializeObject<Int64>(valorrespuesta);

                    }
                }

                //OdbcCommand command = new OdbcCommand("SELECT count(clave_e) FROM boleto_sal where boleto_ent > ? ", cnn);
                //command.Parameters.Add("@clave_e", OdbcType.Int).Value = max;
                //Int64 contarsalidas = 0;
                //using (OdbcDataReader reader = command.ExecuteReader())
                //{
                //    while (reader.Read())
                //    {
                //        contarsalidas = (Convert.ToInt64(reader.GetValue(0)));
                //    }

                //}



                // command = new OdbcCommand("SELECT count(clave_e) FROM boleto_ent ", cnn);
                //Int64 contarsalidas = 0;
                //using (OdbcDataReader reader = command.ExecuteReader())
                //{
                //    while (reader.Read())
                //    {
                //        contarsalidas = (Convert.ToInt64(reader.GetValue(0)));
                //    }

                //}

                //Int64 countapientradas = 0;
                // _client = new HttpClient();
                // resultlogin = await _client.PostAsJsonAsync(baseadress + "api/cuenta/login", new UserInfo { Email = moduleSettings.UserEmail, Password = moduleSettings.UserPassword });
                //if (resultlogin.IsSuccessStatusCode)
                //{
                //    string webtoken = await (resultlogin.Content.ReadAsStringAsync());
                //    UserToken _userToken = JsonConvert.DeserializeObject<UserToken>(webtoken);
                //    _client = new HttpClient();
                //    //     ServicePointManager.ServerCertificateValidationCallback =
                //    //delegate (object s, X509Certificate certificate,
                //    //                X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                //    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _userToken.Token);
                //    var result = await _client.GetAsync(baseadress + "api/Boleto_Sal/GetBoleto_SalCount");
                //    string valorrespuesta = "";
                //    if (result.IsSuccessStatusCode)
                //    {
                //        valorrespuesta = await (result.Content.ReadAsStringAsync());
                //        countapientradas = JsonConvert.DeserializeObject<Int64>(valorrespuesta);

                //    }
                //}


                //if (contarsalidas != countapientradas)
                //{

                // OdbcCommand command = new OdbcCommand("Select clave_e FROM ( SELECT clave_e FROM boleto_sal where clave_e is not null ) as b where clave_e > ?  ", cnn);
                OdbcCommand command = new OdbcCommand("delete FROM boleto_sal where clave_e is  null ;  ", cnn);
                command.ExecuteNonQuery();

                 command = new OdbcCommand("Select clave_e FROM boleto_sal where clave_e > ?  ", cnn);
                 command.Parameters.Add("@clave_e", OdbcType.Int).Value = max;
                    List<Int64> salidas = new List<Int64>();
                    using (OdbcDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader.GetValue(0) != DBNull.Value)
                                salidas.Add(Convert.ToInt64(reader.GetValue(0)));
                        }

                    }


                    double cantidad = salidas.Count / moduleSettings.NoTareas ;
                   if (cantidad == 0) { cantidad = 1; }
                    Int32 round = Convert.ToInt32(Math.Round(cantidad, 0, MidpointRounding.AwayFromZero));
                    var r = salidas.Batch(round);

                    List<List<Int64>> _programaciones = new List<List<Int64>>();
                    _programaciones = r.Select(s => s.ToList()).ToList();

                    List<Int64> _programacion = new List<Int64>();
                    _programaciones = r.Select(s => s.ToList()).ToList();


                    _programacion = new List<Int64>();
                    _programaciones.Add(_programacion); _programaciones.Add(_programacion);
                    _programaciones.Add(_programacion); _programaciones.Add(_programacion);
                    _programaciones.Add(_programacion); _programaciones.Add(_programacion);
                    _programaciones.Add(_programacion); _programaciones.Add(_programacion);
                    _programaciones.Add(_programacion); _programaciones.Add(_programacion);

                    //var result11 = await Task.Run(() => RespuestasSalidas(_programaciones[0]));
                    //var result22 = await Task.Run(() => RespuestasSalidas(_programaciones[1]));
                    //var result33 = await Task.Run(() => RespuestasSalidas(_programaciones[2]));
                    //var result44 = await Task.Run(() => RespuestasSalidas(_programaciones[3]));
                    //var result55 = await Task.Run(() => RespuestasSalidas(_programaciones[4]));
                    //var result66 = await Task.Run(() => RespuestasSalidas(_programaciones[5]));
                    //var result77 = await Task.Run(() => RespuestasSalidas(_programaciones[6]));
                    //var result88 = await Task.Run(() => RespuestasSalidas(_programaciones[7]));
                    //var result99 = await Task.Run(() => RespuestasSalidas(_programaciones[8]));
                    //var result100 = await Task.Run(() => RespuestasSalidas(_programaciones[9]));

                    // salidas = salidas.Take(500).ToList();
                    // await RespuestasSalidas(salidas);

                    //var programacion11 = await Task.Factory.StartNew(() => RespuestasSalidas(_programaciones[0]));
                    //var programacion21 = await Task.Factory.StartNew(() => RespuestasSalidas(_programaciones[1]));
                    //var programacion31 = await Task.Factory.StartNew(() => RespuestasSalidas(_programaciones[2]));
                    //var programacion41 = await Task.Factory.StartNew(() => RespuestasSalidas(_programaciones[3]));
                    //var programacion51 = await Task.Factory.StartNew(() => RespuestasSalidas(_programaciones[4]));
                    //var programacion61 = await Task.Factory.StartNew(() => RespuestasSalidas(_programaciones[5]));
                    //var programacion71 = await Task.Factory.StartNew(() => RespuestasSalidas(_programaciones[6]));
                    //var programacion81 = await Task.Factory.StartNew(() => RespuestasSalidas(_programaciones[7]));
                    //var programacion91 = await Task.Factory.StartNew(() => RespuestasSalidas(_programaciones[8]));
                    //var programacion100 = await Task.Factory.StartNew(() => RespuestasSalidas(_programaciones[9]));

                    var programacion11 = Task.Factory.StartNew(() => RespuestasSalidas(_programaciones[0]));
                    var programacion21 = Task.Factory.StartNew(() => RespuestasSalidas(_programaciones[1]));
                    var programacion31 = Task.Factory.StartNew(() => RespuestasSalidas(_programaciones[2]));
                    var programacion41 = Task.Factory.StartNew(() => RespuestasSalidas(_programaciones[3]));
                    var programacion51 = Task.Factory.StartNew(() => RespuestasSalidas(_programaciones[4]));
                    var programacion61 = Task.Factory.StartNew(() => RespuestasSalidas(_programaciones[5]));
                    var programacion71 = Task.Factory.StartNew(() => RespuestasSalidas(_programaciones[6]));
                    var programacion81 = Task.Factory.StartNew(() => RespuestasSalidas(_programaciones[7]));
                    var programacion91 = Task.Factory.StartNew(() => RespuestasSalidas(_programaciones[8]));
                    var programacion100 = Task.Factory.StartNew(() => RespuestasSalidas(_programaciones[9]));

                    var result11 = programacion11.Result;
                    var result2 = programacion51.Result;

                    // cnn.Dispose();
                    // cnn.Close();
               // }
            }
            catch (Exception ex)
            {
                runner.LogError($"Error al arrancar el servicio{ex.ToString()}");
                //  _logger.LogError($"Error en metodo GuardarSalidas el servicio: {ex.ToString()}");
            }


          //  return true;
        }


        // public async  Task<Action> GuardarProductos()
        public async Task GuardarProductos()
        //public async Task<bool> GuardarProductos()
        {
            try
            {

                string connetionString = null;              
                connetionString = _cone.AccessConnectionString;
                if (cnn.State == ConnectionState.Closed)
                {
                    cnn = new OdbcConnection(connetionString);
                    cnn.Open();
                }


                string baseadress = moduleSettings.urlbase;

                Int64 contarproducto = 0;
                OdbcCommand  command = new OdbcCommand("SELECT count(*) FROM producto ", cnn);
                using (OdbcDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        contarproducto = Convert.ToInt64(reader.GetValue(0).ToString());
                    }
                }


                Int64 countapiproductos = 0;
                HttpClient _client = new HttpClient();
                var resultlogin = await _client.PostAsJsonAsync(baseadress + "api/cuenta/login", new UserInfo { Email = moduleSettings.UserEmail, Password = moduleSettings.UserPassword });
                if (resultlogin.IsSuccessStatusCode)
                {
                    string webtoken = await (resultlogin.Content.ReadAsStringAsync());
                    UserToken _userToken = JsonConvert.DeserializeObject<UserToken>(webtoken);
                    _client = new HttpClient();
                    //     ServicePointManager.ServerCertificateValidationCallback =
                    //delegate (object s, X509Certificate certificate,
                    //                X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _userToken.Token);
                    var result = await _client.GetAsync(baseadress + "api/SubProduct/GetSubProductByCodeCount");
                    string valorrespuesta = "";
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        countapiproductos = JsonConvert.DeserializeObject<Int64>(valorrespuesta);

                    }
                }

                if (countapiproductos != contarproducto)
                {

                    command = new OdbcCommand("SELECT * FROM producto ", cnn);
                    SubProduct _SubProduct = new SubProduct();
                    List<SubProduct> _SubProductlist = new List<SubProduct>();
                    using (OdbcDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            _SubProduct = new SubProduct();
                            _SubProduct.ProductCode = Convert.ToString(reader.GetValue(0).ToString());
                            _SubProduct.ProductName = reader.GetValue(1).ToString();
                            _SubProduct.Description = "Balanza";
                            _SubProduct.ProductTypeId = 2;
                            _SubProduct.ProductTypeName = "Clientes";
                            _SubProductlist.Add(_SubProduct);
                        }

                        _client = new HttpClient();
                        resultlogin = await _client.PostAsJsonAsync(baseadress + "api/cuenta/login", new UserInfo { Email = moduleSettings.UserEmail, Password = moduleSettings.UserPassword });
                        if (resultlogin.IsSuccessStatusCode)
                        {
                            string webtoken = await (resultlogin.Content.ReadAsStringAsync());
                            UserToken _userToken = JsonConvert.DeserializeObject<UserToken>(webtoken);
                            _client = new HttpClient();
                            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _userToken.Token);
                            var result = await _client.PostAsJsonAsync(baseadress + "api/SubProduct/InsertSubproduct_ClassList", _SubProductlist);
                            string valorrespuesta = "";
                            if (result.IsSuccessStatusCode)
                            {
                                valorrespuesta = await (result.Content.ReadAsStringAsync());
                                _SubProduct = JsonConvert.DeserializeObject<SubProduct>(valorrespuesta);
                                if (_SubProduct == null) { _SubProduct = new SubProduct(); }
                                //if(_SubProduct.SubproductId>0)
                                //{

                                //}
                            }
                        }
                    }
                }       

           
               



            }
            catch (Exception ex)
            {
                runner.LogError($"Error al arrancar el servicio{ex.ToString()}");
                // throw;
            }

         
           // return new Action<bool>;
          //  return true;
        }



    }






    class Program
    {
        private  static Microsoft.Extensions.Logging.ILogger _logger;
        static void Main(string[] args)
        {
            var logger = LogManager.GetCurrentClassLogger();
            try
            {

                var config = new ConfigurationBuilder()
                       // .SetBasePath(Directory.GetCurrentDirectory())
                  .SetBasePath(System.AppDomain.CurrentDomain.BaseDirectory)
                  .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                  .AddEnvironmentVariables()
                  .Build();

                var servicesProvider = BuildDi(config);
                using (servicesProvider as IDisposable)
                {
                    var runner = servicesProvider.GetRequiredService<Runner>();
                    runner.DoAction("Action1");

                
                }

               

                var rc = HostFactory.Run(x =>
                {
                    x.Service<MyService>(s => {
                        s.ConstructUsing(name => new MyService());
                        s.WhenStarted(tc => tc.Start());
                        s.WhenStopped(tc => tc.Stop());
                    });
                    // x.RunAsLocalSystem();
                    x.RunAsLocalSystem();

                  //  x.UseNLog();

                    x.SetServiceName("ErpWeb.IntegracionBalanzaFCH1");
                    x.SetDescription("ErpWeb.IntegracionBalanzaFCH1");
                    x.StartAutomatically();

                    //  x.RunAs(@"serviceAccountName", "********");
                    x.EnableServiceRecovery(r => r.RestartService(1));
                });

                var exitCode = (int)Convert.ChangeType(rc, rc.GetTypeCode());  //11
                Environment.ExitCode = exitCode;
            }
            catch (Exception ex)
            {
                logger.Error($"Error al arrancar el servicio{ex.ToString()}");
                // _logger.LogError($"Error en metodo Main el servicio: {ex.ToString()}");
                // throw ex; 
            }





           
        }


        public static IServiceProvider BuildDi(IConfiguration config)
        {
            return new ServiceCollection()
               .AddTransient<Runner>() // Runner is the custom class
               .AddLogging(loggingBuilder =>
               {
          // configure Logging with NLog
            loggingBuilder.ClearProviders();
                   loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                   loggingBuilder.AddNLog(config);
               })
               .BuildServiceProvider();
        }

        //private static void ConfigureService(HostConfigurator x, string serviceName)
        //{
        //    x.StartAutomatically();
        //    x.SetDisplayName(serviceName);
        //    x.SetServiceName(serviceName);

        //    x.RunAsPrompt();

        //    //x.UseNLog();

        //    x.EnableServiceRecovery(r =>
        //    {

        //        r.RestartService(0);
        //        r.OnCrashOnly();
        //        r.SetResetPeriod(1);
        //    });
        //}




    }
}
