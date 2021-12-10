using MaurisBot.Classe;
using System;
using System.IO;

namespace MaurisBot
{
    class Program
    {
        static void Main(string[] args)
        {
            
            var listaClientesOld = Operador.Go(args[0]);
            var listaClientesNew = listaClientesOld;
            Operador.Atualizar(listaClientesOld, listaClientesNew);

        }
    }
}
