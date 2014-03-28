using System;

namespace UNITester
{
    /// <summary>
    /// Atributo que define se um método é para ser executado numa série de testes
    /// </summary>
    [CLSCompliant(true)]
    [AttributeUsage(AttributeTargets.Method)]
    public class TestMethodAttribute : Attribute
    {
        private String _description;

        /// <summary>
        /// Cria uma instância de TestMethodAttribute
        /// </summary>
        /// <param name="description">Descrição associada ao método</param>
        public TestMethodAttribute(String description)
        {
            _description = description;
        }

        /// <summary>
        /// A descrição do método de teste
        /// </summary>
        public String Description
        {
            get { return _description; }
        }
    }
}
