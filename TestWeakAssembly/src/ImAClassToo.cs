using System;
using UNITester;

namespace TestWeakAssembly
{
    [TestClass("Uma outra classe")]
    class ImAClassToo
    {
        [TestMethod("Realizar um trabalho intensivo")]
        [MaximumTime(10000)]
        public void DoAnIntensiveJobHere()
        {
            Action a = ACallbackFunction;
            a();
        }

        private void ACallbackFunction()
        {
            WeakReference[] wr = new WeakReference[1024 * 1024];
            GC.AddMemoryPressure(1024*1024);

            for (int i = 0; i < wr.Length; ++i)
                wr[i] = new WeakReference(new Object());

            GC.KeepAlive(wr[24]);
            GC.Collect();
            GC.WaitForPendingFinalizers();

            for (int i = 512 * 1024; i < wr.Length; ++i)
            {
                if (wr[i].IsAlive)
                    wr[i].Target.GetHashCode();
            }

            GC.Collect();
        }
    }
}
