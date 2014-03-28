using System;
using UNITester;

namespace TestStrongAssembly
{
    [TestClass("Mais uma classe de teste")]
    public class LookAnotherClass
    {
        Object o;

        [TestMethod("Primeiro método a correr")]
        [StartMethod()]
        public void ImFirst()
        {
            o = new Exception();
        }

        [TestMethod("Cria um novo objecto string")]
        public void DoSomethingInBetween()
        {
            char[] c = {'n', 'o', 't', ' ', 'e','x','c','e','p','t','i','o','n'};
            o = new String(c);
        }

        [TestMethod("Último método a correr")]
        [EndMethod()]
        [Returns("not exception")]
        public Object ImLast()
        {
            if (o.GetType().Equals(typeof(Exception)))
                throw new Exception();

            return o;
        }
    }
}
