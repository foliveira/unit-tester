using System;
using System.Collections.Generic;

namespace UNITester
{
    /// <summary>
    /// Interface que define um relatório resultante da execução de um teste pela aplicação
    /// </summary>
    [CLSCompliant(true)]
    public interface IReport
    {
        /// <summary>
        /// Nome da classe
        /// </summary>
        String Class { get; }

        /// <summary>
        /// Nome do método
        /// </summary>
        String Method { get; }

        /// <summary>
        /// Descrição da classe
        /// </summary>
        String ClassDescription { get; }

        /// <summary>
        /// Descrição do método
        /// </summary>
        String MethodDescription { get; }

        /// <summary>
        /// Valor retornado pelo método
        /// </summary>
        String Returned { get; }

        /// <summary>
        /// Valor que o método deveria retornar
        /// </summary>
        String ShouldReturn { get; }

        /// <summary>
        /// Nome da excepção lançada pelo método
        /// </summary>
        String Throwed { get; }

        /// <summary>
        /// Nome da excepção que o método deveria lançar
        /// </summary>
        String ShouldThrow { get; }

        /// <summary>
        /// Tempo máximo para a execução de um teste, em millisegundos
        /// </summary>
        Int32 MaximumTime { get; }

        /// <summary>
        /// Tempo de execução de um teste, em millisegundos
        /// </summary>
        Int32 ExecutionTime { get; }

        /// <summary>
        /// Relatórios agregados à instância actual
        /// </summary>
        IList<IReport> SubReports { get; }
    }
}
