using System;

namespace UNITester
{
    /// <summary>
    /// Um método marcado com este atributo é garantidamente executado em último lugar numa série de testes.
    /// Apenas poderá existir um método marcado com este atributo.
    /// No caso de um método definir este atributo, é necessário, nessa mesma classe, existir outro método marcado
    /// com StartMethodAttribute.
    /// Para o método ser executado é necessário marcá-lo com o TestMethodAttribute.
    /// </summary>
    /// <see cref="StartMethodAttribute"/>
    /// <seealso cref="TestMethodAttribute"/>
    [CLSCompliant(true)]
    [AttributeUsage(AttributeTargets.Method)]
    public class EndMethodAttribute : Attribute
    {
        /// <summary>
        /// Cria um novo EndMethodAttribute
        /// </summary>
        public EndMethodAttribute() { }
    }
}
