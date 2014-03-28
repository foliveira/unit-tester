using System;
using UNITester;

namespace TestStrongAssembly
{
    [TestClass("Outra classe de testes")]
    [MaximumTime(1500)]
    public class AnotherClass
    {
        [TestMethod("Método que cria um array de objectos")]
        [MaximumTime(1500)]
        public void CreateObjectArray()
        {
            Object[] objects = new Object[1024 * 1024];
            for (int i = 0; i < objects.Length; ++i)
                objects[i] = new Object();

            for (int i = 512 * 1024; i < objects.Length; ++i)
                objects[i].ToString();
        }
    }
}
