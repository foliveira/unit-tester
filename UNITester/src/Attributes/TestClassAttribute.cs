using System;

namespace UNITester
{
    /// <summary>
    /// Define se uma classe marcada com este atributo contém métodos para adicionar a uma série de testes
    /// </summary>
    [CLSCompliant(true)]
    [AttributeUsage(AttributeTargets.Class)]
    public class TestClassAttribute : Attribute
    {
        private String _description;

        /// <summary>
        /// Cria uma instância de TestClassAttribute
        /// </summary>
        /// <param name="description">A descrição a atribuir à classe</param>
        public TestClassAttribute(String description) 
        {
            _description = description;
        }

        /// <summary>
        /// A descrição da classe
        /// </summary>
        public String Description
        {
            get { return _description;  }
        }
    }
}
