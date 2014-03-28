using System;
using System.Collections.Generic;
using System.Reflection;

namespace UNITester
{
    /// <summary>
    /// Relatório de um método
    /// </summary>
    internal class MethodReport : IReport
    {
        private MethodInfo _methodInfo;
        private String _throwedException;
        private String _returnedValue;
        private Int32 _executionTime;

        /// <summary>
        /// Cria um MethodReport
        /// </summary>
        /// <param name="mi">Representante do método</param>
        public MethodReport(MethodInfo mi) : this(mi, null, null, Int32.MaxValue) { }
        
        /// <summary>
        /// Cria um MethodReport
        /// </summary>
        /// <param name="mi">Representante do método</param>
        /// <param name="throwed">Nome da excepção lançada</param>
        /// <param name="returned">Representação do valor retornado pelo método</param>
        /// <param name="time">Tempo de execução do método</param>
        public MethodReport(MethodInfo mi, String throwed, String returned, Int32 time)
        {
            _methodInfo = mi;
            _throwedException = throwed;
            _returnedValue = returned;
            _executionTime = time;
        }

        /// <summary>
        /// Não implementado
        /// </summary>
        public String Class
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Nome do método
        /// </summary>
        public String Method
        {
            get { return _methodInfo.Name; }
        }

        /// <summary>
        /// Não implementado
        /// </summary>
        public String ClassDescription
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Descrição do método
        /// </summary>
        public String MethodDescription
        {
            get 
            {
                return ((TestMethodAttribute)Attribute.GetCustomAttribute(_methodInfo, typeof(TestMethodAttribute))).Description;
            }
        }

        /// <summary>
        /// Valor retornado pelo método
        /// </summary>
        public String Returned
        {
            get { return _returnedValue; }
            internal set { _returnedValue = value; }
        }

        /// <summary>
        /// Valor que deverá ser retornado pelo método
        /// </summary>
        public String ShouldReturn
        {
            get
            {
                String value = null;
                Attribute at = Attribute.GetCustomAttribute(_methodInfo, typeof(ReturnsAttribute));
                ReturnsAttribute ra = at as ReturnsAttribute;

                if (ra != null)
                    value = ra.Value;

                return value;
            }
        }

        /// <summary>
        /// Nome da excepção lançada
        /// </summary>
        public String Throwed
        {
            get { return _throwedException; }
            internal set { _throwedException = value; }
        }

        /// <summary>
        /// Nome da excepção que deveria ser lançada
        /// </summary>
        public String ShouldThrow
        {
            get 
            {
                String value = null;
                Attribute at = Attribute.GetCustomAttribute(_methodInfo, typeof(ExpectedExceptionAttribute));
                ExpectedExceptionAttribute ra = at as ExpectedExceptionAttribute;

                if (ra != null)
                    value = ra.Exception;

                return value;
            }
        }

        /// <summary>
        /// Tempo máximo de execução do método
        /// </summary>
        public Int32 MaximumTime
        {
            get 
            {
                Int32 time = Int32.MaxValue;
                Attribute at = Attribute.GetCustomAttribute(_methodInfo, typeof(MaximumTimeAttribute));
                MaximumTimeAttribute mt = at as MaximumTimeAttribute;

                if (mt != null)
                    time = mt.Time;

                return time;
            }
        }

        /// <summary>
        /// Tempo de execução do método
        /// </summary>
        public Int32 ExecutionTime
        {
            get { return _executionTime; }
            internal set { _executionTime = value; }
        }

        /// <summary>
        /// Não implementado
        /// </summary>
        public IList<IReport> SubReports
        {
            get { throw new NotImplementedException(); }
        }
    }
}
