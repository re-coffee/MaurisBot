using System;
using System.Collections.Generic;
using System.Text;

namespace MaurisBot.Classe
{
    class Cliente
    {
        public int Id { get; private set; }
        public string Nome { get; private set; }
        public string OfflineTimeEventsDirectory { get; set; }
        public string UserSessions { get; set; }
        public string UserStateDirectory { get; set; }
        public string UserSessionsDirectory { get; set; }
        public string Caminho { get; private set; }
        public bool FoiAlterado { get; set; }

        public Cliente(int id, string nome, string oTED, string uS, string uSD, string uSsD, string caminho)
        {
            Id = id;
            Nome = nome;
            OfflineTimeEventsDirectory = oTED == "" ? "nulo" : oTED;
            UserSessions = uS == "" ? "nulo" : uS;
            UserStateDirectory = uSD == "" ? "nulo" : uSD;
            UserSessionsDirectory = uSsD == "" ? "nulo" : uSsD;
            Caminho = caminho;
        }
        public Cliente() { }
        public override string ToString()
        {
            var log =
                $"  OfflineTimeEventsDirectory=\"{OfflineTimeEventsDirectory}\"\n" +
                $"  UserSessions=\"{UserSessions}\"\n" +
                $"  UserStateDirectory=\"{UserStateDirectory}\"\n" +
                $"  UserSessionsDirectory=\"{UserSessionsDirectory}\"";
            return log;
        }
    }
}
