using System;

namespace UNITester
{
    /// <summary>
    /// Define o tempo máximo para a execução de um método ou classe (totalidade de métodos numa classe)
    /// </summary>
    [CLSCompliant(true)]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class MaximumTimeAttribute : Attribute
    {
        private Int32 _time;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="time">O tempo máximo de execução, em millisegundos</param>
        public MaximumTimeAttribute(Int32 time)
        {
            _time = time;
        }

        /// <summary>
        /// O tempo máximo de execução
        /// </summary>
        public Int32 Time
        {
            get { return _time; }
        }
    }
}
