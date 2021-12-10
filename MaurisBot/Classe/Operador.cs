using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MaurisBot.Classe
{
    static class Operador
    {
        // Chama o buscar e formatar para ter a lista de clientes obtida
        public static List<Cliente> Go(string diretorioRaiz)
        {
            return Formatar(Buscar(diretorioRaiz));
        }

        // Busca a partir do diretorio raiz todos os web.config existentes
        private static List<string> Buscar(string diretorioRaiz)
        {
            var Lista = new List<string>();
            string[] filePaths = Directory.GetFiles(diretorioRaiz, "web.config", SearchOption.AllDirectories);
            foreach (var item in filePaths)
            {
                Lista.Add(item);
            }
            return Lista;
        }
        // Recebe todos os caminhos de web.config, escolhe somente os que possuem ####_APW_####\server\apwebdispatcher\.net\
        // Abre o web.config de cada um e instancia a classe cliente com as informacoes encontradas
        private static List<Cliente> Formatar(List<string> lista)
        {
            var listaClientes = new List<Cliente>();
            foreach (var caminho in lista)
            {
                if (!caminho.Contains("\\Server\\ApWebDispatcher\\.net") || !caminho.Contains("_APW_"))
                    continue;
                using (var sr = new StreamReader(caminho))
                {
                    Regex oTED = new Regex(@"(?<=offlineTimeEventsDirectory="")[^""]*");
                    Regex uS = new Regex(@"(?<=userSessions="")[^""]*");
                    Regex uSD = new Regex(@"(?<=userStateDirectory="")[^""]*");
                    Regex uSsD = new Regex(@"(?<=userSessionsDirectory="")[^""]*");

                    var arquivo = sr.ReadToEnd();

                    Match matchOTED = oTED.Match(arquivo);
                    Match matchUS = uS.Match(arquivo);
                    Match matchUSD = uSD.Match(arquivo);
                    Match matchUSsD = uSsD.Match(arquivo);

                    var caminhoSplit = caminho.Split("APW_");
                    var clienteIndexFim = caminhoSplit[1].Split('\\')[0].Length;

                    var nome = caminhoSplit[1].Substring(0, clienteIndexFim);
                    var divisaoId = caminhoSplit[0].Split('C');

                    var id = int.Parse(divisaoId[divisaoId.Count() - 1].Substring(0, 3));
                    if(matchOTED.Value == @"c:\temp\marcacoes" ||
                        matchUS.Value == @"c:\temp\userSessions" ||
                        matchUSD.Value == @"c:\temp\userStates" ||
                        matchUSsD.Value == @"c:\temp\userSessions\")
                    {
                        var cliente = new Cliente(
                            id,
                            nome,
                            matchOTED.Value,
                            matchUS.Value,
                            matchUSD.Value,
                            matchUSsD.Value,
                            caminho);

                        listaClientes.Add(cliente);
                    }
                }
            }
            return listaClientes;
        }

        // Atualiza uma das listas de clientes, cria um arquivo backup do web.config, altera os valores e loga as alteracoes
        public static void Atualizar(List<Cliente> listaAntiga, List<Cliente> listaNova)
        {
            foreach (var cliente in listaNova)
            {
                var registroAntes = cliente.ToString();

                if (cliente.OfflineTimeEventsDirectory == @"c:\temp\marcacoes")
                {
                    cliente.OfflineTimeEventsDirectory = $@"c:\temp\marcacoes\{cliente.Nome.ToLower()}";
                    cliente.FoiAlterado = true;
                }
                    
                if (cliente.UserSessions == @"c:\temp\userSessions")
                {
                    cliente.UserSessions = $@"c:\temp\userSessions\{cliente.Nome.ToLower()}";
                    cliente.FoiAlterado = true;
                }

                if (cliente.UserStateDirectory == @"c:\temp\userStates")
                {
                    cliente.UserStateDirectory = $@"c:\temp\userStates\{cliente.Nome.ToLower()}";
                    cliente.FoiAlterado = true;
                }

                if (cliente.UserSessionsDirectory == @"c:\temp\userSessions")
                {
                    cliente.UserSessionsDirectory = $@"c:\temp\userSessions\{cliente.Nome.ToLower()}";
                    cliente.FoiAlterado = true;
                }

                if (cliente.FoiAlterado)
                {
                    var data = $"{DateTime.Now.Year}{DateTime.Now.Month}{DateTime.Now.Day}_{DateTime.Now.Hour}{DateTime.Now.Minute}";
                    var arquivoLog = $"Log_{data}.log";
                    var diretorioAtual = Directory.GetCurrentDirectory();
                    var registroDepois = cliente.ToString();
                    string arquivoAtual = cliente.Caminho;
                    string arquivoBackup = $"{arquivoAtual}_{data}";
                    try
                    {
                        File.Copy(arquivoAtual, arquivoBackup, true);
                    }
                    catch (IOException iox)
                    {
                        Console.WriteLine(iox.Message);
                    }
                    File.WriteAllText(arquivoAtual, File.ReadAllText(arquivoAtual)
                        .Replace("offlineTimeEventsDirectory=\"c:\\temp\\marcacoes\"", $"offlineTimeEventsDirectory=\"{cliente.OfflineTimeEventsDirectory}\"")
                        .Replace("userSessions=\"c:\\temp\\userSessions\"", $"userSessions=\"{cliente.UserSessions}\"")
                        .Replace("userStateDirectory=\"c:\\temp\\userStates\"", $"userStateDirectory=\"{cliente.UserStateDirectory}\"")
                        .Replace("userSessionsDirectory=\"c:\\temp\\userSessions\"", $"userSessionsDirectory=\"{cliente.UserSessionsDirectory}\""));

                    if (!Directory.Exists($@"{diretorioAtual}\Log"))
                    {
                        Directory.CreateDirectory($@"{diretorioAtual}\Log");
                    }

                    using (StreamWriter sw = File.AppendText($@"{diretorioAtual}\Log\{arquivoLog}"))
                    {
                        var dataFormatada = $"{ DateTime.Now.Day}/{ DateTime.Now.Month}/{ DateTime.Now.Year} {DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second}";
                        sw.WriteLine($"{dataFormatada} - {cliente.Nome} [{cliente.Id}]");
                        sw.WriteLine("Parametros anteriores:");
                        sw.WriteLine(registroAntes);
                        sw.WriteLine("\nParametros atuais:");
                        sw.WriteLine(registroDepois);
                        sw.WriteLine($"\nCaminho: {cliente.Caminho}");
                        sw.WriteLine($"Backup : {arquivoBackup}\n\n");
                    }

                }
                else
                {
                    continue;
                }
            }
        }
    }
}
