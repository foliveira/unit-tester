using System;
using System.Collections.Generic;

namespace UNITester
{
    /// <summary>
    /// Relatório de uma classe
    /// </summary>
    internal class ClassReport : IReport
    {
        private IList<IReport> _methodReports;
        private Type _type;
        private Int32 _executionTime;
        private String _description;
        private String _throwed;

        /// <summary>
        /// Cria um novo ClassReport
        /// </summary>
        /// <param name="type">Tipo associado ao relatório</param>
        /// <param name="throwed">Nome da excepção lançada</param>
        public ClassReport(Type type, String throwed)
            : this(type)
        {
            _throwed = throwed;
        }

        /// <summary>
        /// Cria um novo ClassReport
        /// </summary>
        /// <param name="type">Tipo associado ao relatório</param>
        public ClassReport(Type type) 
            : this(type, Int32.MaxValue) {}

        /// <summary>
        /// Cria um novo ClassReport
        /// </summary>
        /// <param name="type">Tipo associado ao relatório</param>
        /// <param name="time">Tempo de execução do conjunto de testes da classe</param>
        public ClassReport(Type type, Int32 time)
        {
            _methodReports = new List<IReport>();
            _type = type;
            _executionTime = time;
            _description = ((TestClassAttribute)Attribute.GetCustomAttribute(_type, typeof(TestClassAttribute))).Description;
        }

        /// <summary>
        /// Nome da classe
        /// </summary>
        public String Class
        {
            get { return _type.Name; }
        }

        /// <summary>
        /// Não implementado
        /// </summary>
        public String Method
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Descrição da classe
        /// </summary>
        public String ClassDescription
        {
            get { return _description; }
            internal set { _description = value; }
        }

        /// <summary>
        /// Não implementado
        /// </summary>
        public String MethodDescription
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Não implementado
        /// </summary>
        public String Returned
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Não implementado
        /// </summary>
        public String ShouldReturn
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Excepção lançada
        /// </summary>
        public String Throwed
        {
            get { return _throwed; }
        }

        /// <summary>
        /// Não implementado
        /// </summary>
        public String ShouldThrow
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Tempo máximo de execução do conjunto de testes de uma classe
        /// </summary>
        public Int32 MaximumTime
        {
            get 
            { 
                Int32 time = Int32.MaxValue;
                Attribute at = Attribute.GetCustomAttribute(_type, typeof(MaximumTimeAttribute));
                MaximumTimeAttribute mt = at as MaximumTimeAttribute;

                if(mt != null)
                    time = mt.Time;

                return time;
            }
        }

        /// <summary>
        /// Tempo de execução do conjunto de testes de uma classe
        /// </summary>
        public Int32 ExecutionTime
        {
            get { return _executionTime; }
            internal set { _executionTime = value; }
        }

        /// <summary>
        /// Lista de relatórios dos testes associados aos métodos de teste da classe
        /// </summary>
        public IList<IReport> SubReports
        {
            get 
            {
                IList<IReport> list = _methodReports;
                return list;
            }
            internal set
            {
                ((List<IReport>)_methodReports).AddRange(value);
            }
        }

    }
}
