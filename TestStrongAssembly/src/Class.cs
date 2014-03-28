using System;
using UNITester;

namespace TestStrongAssembly
{
    [TestClass("Uma classe de testes")]
    public class Class
    {
        static String _s;

        [TestMethod("Afecta uma variável estática")]
        public static void StaticMethodOne(String s)
        {
            _s = s;
        }

        [TestMethod("Manipula o valor de uma variável estática")]
        public void manipulateAStringValue()
        {
            _s = _s + "UNITester";
            _s = _s.GetHashCode().GetHashCode().GetHashCode().ToString();
            _s = _s.Substring(0, 1);
        }

        [TestMethod("Cria um grande número de objectos e realiza trabalho com estes")]
        [ExpectedException("OverflowException")]
        public void CreateObjects()
        {
            Int64 res = 0;
            Random[] randoms = new Random[2*1024*1024];

            for (int i = 0; i < randoms.Length; ++i)
            {
                randoms[i] = new Random();
                res += (randoms[i].Next() * 10);
            }

            if(res != 0)
                throw new OverflowException();
        }

        [TestMethod("Realiza uma collecção a nível do GC")]
        [Returns("PurgeMe")]
        [MaximumTime(9000)]
        public String PerformALotOfGCs()
        {
            String s = null;
            WeakReference[] wr = new WeakReference[4*1024*1024];

            for(int i = 0; i < wr.Length; ++i)
                wr[i] = new WeakReference("PurgeMe");

            GC.Collect();

            if(wr[2*1024*1024].IsAlive)
                s = (String)wr[2*1024*1024].Target;

            return s;
        }
    }
}
