using System;

namespace XMLReporter
{
    /// <summary>
    /// Classe de entrada da aplicação.
    /// </summary>
    [CLSCompliant(true)]
    class Program
    {
        /// <summary>
        /// Ponto de entrada da aplicação
        /// </summary>
        /// <param name="args">Argumentos da linha de comandos</param>
        static void Main(string[] args)
        {
            Tester prog = new Tester();

            if (!prog.Init())
                return;

            prog.RunTests();
            prog.WriteResults();
        }
    }
}
