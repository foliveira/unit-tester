using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;

namespace UNITester
{
    /// <summary>
    /// Encapsula um tipo, permitindo guardar informação sobre os seus métodos de teste e executá-los.
    /// </summary>
    internal class TypeWrapper
    {
        private Type _type;
        private BindingFlags _bindFlags;
        private IList<MethodInfo> _methods;
        private MethodInfo _startMethod;
        private MethodInfo _endMethod;
        private ClassReport _classReport;

        /// <summary>
        /// Cria um TypeWrapper
        /// </summary>
        /// <param name="t">O tipo a encapsular</param>
        public TypeWrapper(Type t)
        {
            _methods = new List<MethodInfo>();
            _classReport = new ClassReport(t);
            _type = t;
            _bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        }

        /// <summary>
        /// Adiciona os métodos marcados para teste à série de testes.
        /// </summary>
        public void Setup()
        {
            foreach (MethodInfo mi in _type.GetMethods(_bindFlags | BindingFlags.Static))
            {
                if(mi.IsDefined(typeof(TestMethodAttribute), true) && !isHeadMethod(mi))
                    _methods.Add(mi);
            }
        }

        /// <summary>
        /// Executa a série de testes associada ao tipo encapsulado.
        /// É garantida a execução do método marcado com StartMethodAttribute em primeiro lugar e do método
        /// marcado com EndMethodAttribute em último lugar, caso existam.
        /// </summary>
        /// <returns>Relatório da classe de teste. Contém informação sobre todos os métodos executados</returns>
        /// <exception cref="System.MethodAccessException" />
        /// <exception cref="System.MissingMethodException" />
        /// <exception cref="System.TypeInitializationException" />
        public IReport RunTestMethods()
        {
            if(_startMethod == null || _endMethod == null)
                SetHeadMethods();

            Object target = CreateTarget(_type);
            List<IReport> methodsReport = new List<IReport>();
            Stopwatch classWatch = new Stopwatch();

            classWatch.Start();
                if (_startMethod != null)
                    methodsReport.Add(CallTestMethod(target, _startMethod));

                foreach (MethodInfo mi in _methods)
                    methodsReport.Add(CallTestMethod(target, mi));

                if (_endMethod != null)
                    methodsReport.Add(CallTestMethod(target, _endMethod));
            classWatch.Stop();

            _classReport.ExecutionTime = (Int32)classWatch.ElapsedMilliseconds;
            _classReport.SubReports = methodsReport;

            return _classReport;
        }

        /// <summary>
        /// Tipo encapsulado
        /// </summary>
        public Type Type
        {
            get { return _type; }
        }

        /// <summary>
        /// Procura métodos marcados com StartMethodAttribute e EndMethodAttribute e adiciona-os à série
        /// para serem executados pela ordem correcta.
        /// É garantido que só existe um método marcado com cada um dos atributos.
        /// Só são considerados métodos que se encontram marcados, para além dos atributos em questão, com
        /// TestMethodAttribute.
        /// </summary>
        /// <exception cref="System.MethodAccessException" />
        private void SetHeadMethods()
        {
            foreach (MethodInfo mi in _type.GetMethods(_bindFlags | BindingFlags.Static))
            {
                if (mi.IsDefined(typeof(TestMethodAttribute), true))
                {
                    if (mi.IsDefined(typeof(StartMethodAttribute), true))
                    {
                        if (_startMethod != null)
                            throw new MethodAccessException("StartMethodAttribute repetido");

                        _startMethod = mi;
                    }
                    else if (mi.IsDefined(typeof(EndMethodAttribute), true))
                    {
                        if (_endMethod != null)
                            throw new MethodAccessException("EndMethodAttribute repetido");

                        _endMethod = mi;
                    }
                }
            }
            if ((_startMethod == null && _endMethod != null) || (_startMethod != null && _endMethod == null))
                throw new MethodAccessException("Não existe par de atributos Start/EndMethod");
        }

        /// <summary>
        /// Verifica se o método se encontra marcado com StartMethodAttribute ou EndMethodAttribute
        /// </summary>
        /// <param name="mi">Representante do método em questão</param>
        /// <returns><code>true</code> caso o método esteja marcado com um dos atributos</returns>
        private bool isHeadMethod(MethodInfo mi)
        {
            
            return mi.IsDefined(typeof(StartMethodAttribute), true) || 
                    mi.IsDefined(typeof(EndMethodAttribute), true);
        }

        /// <summary>
        /// Cria uma instância do tipo encapsulado por esta classe.
        /// Chama o primeiro construtor devolvido pela chamada a GetConstructors() de Type.
        /// </summary>
        /// <param name="type">Representante do tipo</param>
        /// <returns>Uma instância do tipo</returns>
        /// <exception cref="System.MissingMethodException" />
        /// <exception cref="System.TypeInitializationException" />
        private Object CreateTarget(Type type)
        {
            Object target = null;
            ConstructorInfo[] ci = type.GetConstructors(_bindFlags);

            if (ci.Length == 0)
                throw new MissingMethodException("Não existem construtores neste tipo.");

            foreach (ConstructorInfo c in ci)
            {
                Object[] pars = new Object[c.GetParameters().Length];
                try
                {
                    target = c.Invoke(pars);
                }
                catch (Exception) { }

                if (target != null)
                    return target;
            }
            
            throw new TypeInitializationException(_type.Name, null);
        }

        /// <summary>
        /// Chama um método de teste e realiza o relatório de execução deste.
        /// </summary>
        /// <param name="target">Instância sobre a qual o método deve ser evocado</param>
        /// <param name="mi">Representante do método</param>
        /// <returns>Um relatório com informação sobre a execução do método</returns>
        private MethodReport CallTestMethod(Object target, MethodInfo mi)
        {
            Object returnedValue = null;
            Stopwatch methodWatch = new Stopwatch();
            MethodReport methodReport = new MethodReport(mi);
            Object[] pars = new Object[mi.GetParameters().Length];

            try
            {
                methodWatch.Start();
                returnedValue = mi.Invoke(target, pars);
            }
            catch (Exception e)
            {
                methodReport.Throwed = e.InnerException.GetType().Name;
            }
            finally
            {
                methodWatch.Stop();
            }
            methodReport.ExecutionTime = (Int32)methodWatch.ElapsedMilliseconds;
            if(returnedValue != null)
                methodReport.Returned = returnedValue.ToString();

            return methodReport;
        }
    }
}
