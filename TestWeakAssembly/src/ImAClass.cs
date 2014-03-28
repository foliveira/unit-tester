using System;
using UNITester;

namespace TestWeakAssembly
{
    [TestClass("Uma classe")]
    class ImAClass
    {
        Random _random;
        Byte[] _bytes;
        String _res;

        public ImAClass()
        {
            _random = new Random();
            _bytes = new Byte[256];
        }

        [TestMethod("Primeiro método a executar")]
        [StartMethod()]
        public void Start()
        {
            _random.NextBytes(_bytes);
        }

        [TestMethod("Aplica uma transformação a uma string com base em bytes aleatorios")]
        [MaximumTime(250)]
        private void Transform()
        {
            String s = "String a Transformar";
            System.Text.ASCIIEncoding ae = new System.Text.ASCIIEncoding();
            Byte[] sb = ae.GetBytes(s);

            for (int i = 0; i < sb.Length; ++i)
                sb[i] = (byte)(sb[i] ^ _bytes[i % _bytes.Length]);

            _res = ae.GetString(sb);
        }

        [TestMethod("Último método a executar")]
        [EndMethod()]
        public String End()
        {
            return _res;
        }
    }
}
