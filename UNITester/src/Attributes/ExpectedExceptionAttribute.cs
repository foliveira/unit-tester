using System;

namespace UNITester
{
    /// <summary>
    /// Define qual a excepção que um método terá que lançar para ser considerado correctamente executado
    /// </summary>
    [CLSCompliant(true)]
    [AttributeUsage(AttributeTargets.Method)]
    public class ExpectedExceptionAttribute : Attribute
    { 
        private String _exception;

        /// <summary>
        /// Cria um novo ExpectedExceptionAttribute
        /// </summary>
        /// <param name="exception">O nome da excepção esperada</param>
        public ExpectedExceptionAttribute(String exception)
        {
            _exception = exception;
        }

        /// <summary>
        /// A excepção esperada
        /// </summary>
        public String Exception
        {
            get { return _exception; }
        }
    }
}
