using System;

namespace UNITester
{
    /// <summary>
    /// Um método marcado com este atributo define que espera que o retorno coincide com o valor definido.
    /// </summary>
    [CLSCompliant(true)]
    [AttributeUsage(AttributeTargets.Method)]
    public class ReturnsAttribute : Attribute
    {
        private String _value;

        /// <summary>
        /// Cria um novo ReturnsAttribute
        /// </summary>
        /// <param name="value">O valor que o método tem que retornar</param>
        public ReturnsAttribute(String value)
        {
            _value = value;
        }

        /// <summary>
        /// O valor a retornar pelo método
        /// </summary>
        public String Value
        {
            get { return _value; }
        }
    }
}
