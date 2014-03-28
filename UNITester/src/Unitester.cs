using System;
using System.Collections.Generic;
using System.Reflection;

namespace UNITester
{
    /// <summary>
    /// Representa uma fábrica de testes.
    /// Carrega o assembly e os tipos marcados para teste, juntamente com os seus métodos e inicia os testes.
    /// Retorna uma lista de relatórios com os resultados dos testes.
    /// </summary>
    [CLSCompliant(true)]
    public class Unitester
    {
        private Assembly _asm;
        private IList<TypeWrapper> _types;

        /// <summary>
        /// Cria um Unitester
        /// </summary>
        public Unitester()
        {
            _types = new List<TypeWrapper>();
        }

        /// <summary>
        /// Carrega o assembly e todos os seus tipos marcados para testes.
        /// </summary>
        /// <param name="assemblyName">Nome do assembly a carregar</param>
        /// <exception cref="System.ArgumentNullException" />
        /// <exception cref="System.ArgumentException" />
        /// <exception cref="System.IO.FileNotFoundException" />
        /// <exception cref="System.IO.FileLoadException" />
        /// <exception cref="System.BadImageFormatException" />
        public void Setup(String assemblyName)
        {
            _asm = Assembly.Load(assemblyName);
            _types.Clear();

            foreach (Type t in _asm.GetTypes())
            {
                TypeWrapper tw;

                if (t.IsDefined(typeof(TestClassAttribute), true))
                {
                    tw = new TypeWrapper(t);
                    tw.Setup();
                    _types.Add(tw);
                }
            }           
        }

        /// <summary>
        /// Realiza os testes sobre os métodos marcados para teste
        /// </summary>
        /// <returns>Lista com relatórios de todos os testes efectuados</returns>
        public IList<IReport> PerformTests()
        {
            IList<IReport> reports = new List<IReport>();
            
            foreach (TypeWrapper tw in _types)
            {
                try
                {
                    reports.Add(tw.RunTestMethods());
                }
                catch (Exception e)
                {
                    reports.Add(new ClassReport(tw.Type, e.GetType().Name));
                }
            }

            return reports;
        }
    }
}
