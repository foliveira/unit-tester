using System;

namespace UNITester
{
    /// <summary>
    /// Um método marcado com este atributo é garantidamente executado em primeiro lugar numa série de testes.
    /// Apenas poderá existir um método marcado com este atributo.
    /// No caso de um método definir este atributo, é necessário, nessa mesma classe, existir outro método marcado
    /// com EndMethodAttribute.
    /// Para o método ser executado é necessário marcá-lo com o TestMethodAttribute.
    /// </summary>
    /// <see cref="EndMethodAttribute"/>
    /// <seealso cref="TestMethodAttribute"/>
    [CLSCompliant(true)]
    [AttributeUsage(AttributeTargets.Method)]
    public class StartMethodAttribute : Attribute
    {
        /// <summary>
        /// Cria um novo StartMethodAttribute
        /// </summary>
        public StartMethodAttribute() { }
    }
}
