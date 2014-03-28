using System;
using System.Reflection;
using System.Collections.Generic;
using UNITester;

namespace ConsoleReporter
{
    /// <summary>
    /// Apresenta os resultados de uma série de testes, realizados sobre um assembly .NET, na consola.
    /// </summary>
    [CLSCompliant(true)]
    class Tester
    {
        private String _assemblyName;
        private Unitester _unitester;
        private IList<IReport> _reports;

        /// <summary>
        /// Cria uma nova instância de Unitester, para realizar os testes.
        /// </summary>
        public Tester()
        {
            _unitester = new Unitester();
        }

        /// <summary>
        /// Lê o nome do assembly, passando-o ao objecto realizador dos testes.
        /// </summary>
        /// <returns><code>true</code> caso a inicialização seja bem sucedida</returns>
        public Boolean Init()
        {
            Console.Write("Introduza o nome (strong name ou caminho) do assembly que contém as classes de teste: ");
            _assemblyName = Console.ReadLine();

            try
            {
                _unitester.Setup(_assemblyName);
            }
            catch (Exception e)
            {
                Console.WriteLine("Não foi possível carregar o assembly.");
                Console.WriteLine("Mensagem de erro:"); 
                Console.WriteLine(e.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Corre uma série de testes sobre o assembly especificado
        /// </summary>
        public void RunTests()
        {
            _reports = _unitester.PerformTests();
        }

        /// <summary>
        /// Exibe os resultados, formatados, na consola
        /// </summary>
        public void ShowResults()
        {
            Console.WriteLine("\nResultado do testes para {0}", _assemblyName);

            foreach (IReport r in _reports)
            {
                Console.WriteLine("--------------------------------------------------");
                Console.WriteLine("Classe: {0}", r.Class);
                Console.WriteLine("Descrição: {0}", r.ClassDescription);
                Console.WriteLine("Tempo de Execução: {0}ms", r.ExecutionTime);
                Console.Write("Resultado: "); CheckClassSucess(r);

                Console.WriteLine("\nResultado para métodos:");
                Console.WriteLine("-------------------------");
                foreach (IReport m in r.SubReports)
                {
                    Console.WriteLine("Método: {0}", m.Method);
                    Console.WriteLine("Descrição: {0}", m.MethodDescription);
                    Console.WriteLine("Tempo de Execução: {0}ms", m.ExecutionTime);
                    if (m.Returned != null)
                        Console.WriteLine("Retorno: {0}", m.Returned);
                    Console.Write("Resultado: "); CheckMethodSucess(m);
                    Console.WriteLine("\n");
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Verifica se o teste executado sobre uma classe foi realizado com sucesso ou não.
        /// Um teste sobre uma classe tem sucesso, quando não excedeu o tempo máximo definido ou se a instanciação
        /// do tipo foi realizada com sucesso e todos os métodos marcados para teste foram carregados como tal.
        /// </summary>
        /// <param name="r">Um objecto que contém o relatório correspondente a uma classe</param>
        private void CheckClassSucess(IReport r)
        {
            Boolean time = r.ExecutionTime > r.MaximumTime;
            Boolean exception = r.Throwed != null;

            if (time || exception)
            {
                Console.WriteLine("Falhou");
                Console.WriteLine("Razão:");

                if (time)
                    Console.WriteLine("\tTempo Máximo Excedido [Demorou: {0}ms / Máximo: {1}ms]", r.ExecutionTime,
                                                                                                  r.MaximumTime);
                if (exception)
                    Console.WriteLine("\tExcepção Lançada [Nome: {0}]", r.Throwed);
            }
            else
                Console.WriteLine("Sucesso");
        }

        /// <summary>
        /// Verifica se um método foi executado com sucesso.
        /// Para um método ser considerado como bem sucedido o seu tempo de execução não pode ter excedido o seu
        /// tempo máximo, no caso de alguma excepção ter sido lançada ou de ser lançada uma excepção não esperada
        /// e igualmente no caso do valor retornado ser diferente do que se encontra definido para o método
        /// </summary>
        /// <param name="r">O relatório de execução do método</param>
        private void CheckMethodSucess(IReport r)
        {
            Boolean time = r.ExecutionTime > r.MaximumTime;
            Boolean exception = (r.ShouldThrow == null) ? (r.Throwed != null) : !r.ShouldThrow.Equals(r.Throwed);
            Boolean returned = (r.ShouldReturn != null) && (!r.ShouldReturn.Equals(r.Returned));

            if (time || exception || returned)
            {
                Console.WriteLine("Falhou");
                Console.WriteLine("Razão:");

                if(time)
                    Console.WriteLine("\tTempo Máximo Excedido [Demorou: {0}ms / Máximo: {1}ms]",
                                                                    r.ExecutionTime, r.MaximumTime);
                if (returned)
                    Console.WriteLine("\tRetorno Discordante [Retornou: {0} / Devia Retornar: {1}]",
                                                                            r.Returned, r.ShouldReturn);
                if (exception)
                {
                    if(r.ShouldThrow == null)
                        Console.WriteLine("\tLançou Excepção [Nome: {0}]", r.Throwed);
                    else
                        Console.WriteLine("\tExcepção Discordante [Lançou: {0} / Devia Lançar: {1}]",
                                (r.Throwed == null ? "<Nenhuma Excepção>" : r.Throwed), r.ShouldThrow);
                }
            }
            else
                Console.WriteLine("Sucesso");
        }
    }
}
