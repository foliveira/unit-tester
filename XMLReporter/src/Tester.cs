using System;
using System.Xml;
using System.Collections.Generic;
using UNITester;

namespace XMLReporter
{
    /// <summary>
    /// Cria um ficheiro XML contendo os resultados de uma série de testes, realizados sobre um assembly .NET
    /// </summary>
    [CLSCompliant(true)]
    class Tester
    {
        private String _assemblyName;
        private Unitester _unitester;
        private XmlDocument _xmlDoc;
        private IList<IReport> _reports;

        /// <summary>
        /// Cria uma nova instância de Unitester, para realizar os testes e um documento XML para 
        /// armazenar os resultados
        /// </summary>
        public Tester()
        {
            _unitester = new Unitester();
            _xmlDoc = new XmlDocument();
        }

        /// <summary>
        /// Lê o nome do assembly, passando-o ao objecto realizador dos testes.
        /// </summary>
        /// <returns><code>true</code> caso a inicialização seja bem sucedida</returns>
        public Boolean Init()
        {
            Console.Write("Introduza o nome (strong name ou caminho) do assembly que contém as classes de teste: ");
            _assemblyName = Console.ReadLine();

            try
            {
                _unitester.Setup(_assemblyName);
            }
            catch (Exception e)
            {
                Console.WriteLine("Não foi possível carregar o assembly.");
                Console.WriteLine("Mensagem de erro:");
                Console.WriteLine(e.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Corre uma série de testes sobre o assembly especificado
        /// </summary>
        public void RunTests()
        {
            _reports = _unitester.PerformTests();
        }

        /// <summary>
        /// Escreve os resultados dos testes para um ficheiro XML com o nome do assembly testado
        /// </summary>
        public void WriteResults()
        {
            _xmlDoc.AppendChild(_xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", "yes"));
            XmlNode root = _xmlDoc.CreateElement("UNITester");
            _xmlDoc.AppendChild(root);

            foreach (IReport r in _reports)
            {
                XmlNode classNode = WriteClassInfo(r);
                XmlNode methodsNode = _xmlDoc.CreateElement("methods");

                foreach(IReport m in r.SubReports)
                {
                    XmlNode method = WriteMethodInfo(m);
                    methodsNode.AppendChild(method);
                }

                classNode.AppendChild(methodsNode);
                root.AppendChild(classNode);
            }

            _xmlDoc.Save(_assemblyName + ".xml");
        }

        /// <summary>
        /// Escreve a informação de execução de um método para um nó XML
        /// </summary>
        /// <param name="m">Um relatório com o resultado dos testes ao método</param>
        /// <returns>O nó XML que contém a informação de execução do método</returns>
        private XmlNode WriteMethodInfo(IReport m)
        {
            XmlNode methodNode = _xmlDoc.CreateElement("method");
            XmlNode description = _xmlDoc.CreateElement("description");
            XmlNode returned = _xmlDoc.CreateElement("returned");
            XmlNode shouldReturn = _xmlDoc.CreateElement("shouldReturn");
            XmlNode throwed = _xmlDoc.CreateElement("throwed");
            XmlNode shouldThrow = _xmlDoc.CreateElement("shouldThrow");

            XmlAttribute name = _xmlDoc.CreateAttribute("name");
            XmlAttribute execTime = _xmlDoc.CreateAttribute("execTime");
            XmlAttribute maxTime = _xmlDoc.CreateAttribute("maxTime");

            name.Value = m.Method;
            execTime.Value = m.ExecutionTime.ToString();
            maxTime.Value = (m.MaximumTime == Int32.MaxValue) ? "Infinite" : m.MaximumTime.ToString();

            methodNode.Attributes.Append(name);
            methodNode.Attributes.Append(execTime);
            methodNode.Attributes.Append(maxTime);
            description.AppendChild(_xmlDoc.CreateTextNode(m.MethodDescription));
            methodNode.AppendChild(description);

            if (m.Returned != null)
            {
                returned.AppendChild(_xmlDoc.CreateTextNode(m.Returned));
                methodNode.AppendChild(returned);
            }
            if (m.ShouldReturn != null)
            {
                shouldReturn.AppendChild(_xmlDoc.CreateTextNode(m.ShouldReturn));
                methodNode.AppendChild(shouldReturn);
            }
            if (m.Throwed != null)
            {
                throwed.AppendChild(_xmlDoc.CreateTextNode(m.Throwed));
                methodNode.AppendChild(throwed);
            }
            if (m.ShouldThrow != null)
            {
                shouldThrow.AppendChild(_xmlDoc.CreateTextNode(m.ShouldThrow));
                methodNode.AppendChild(shouldThrow);
            }

            CheckMethodSuccess(m, methodNode);

            return methodNode;
        }

        /// <summary>
        /// Verifica se um método foi executado com sucesso.
        /// Para um método ser considerado como bem sucedido o seu tempo de execução não pode ter excedido o seu
        /// tempo máximo, no caso de alguma excepção ter sido lançada ou de ser lançada uma excepção não esperada
        /// e igualmente no caso do valor retornado ser diferente do que se encontra definido para o método
        /// </summary>
        /// <param name="m">O relatório de execução do método</param>
        /// <param name="methodNode">O nó XML que contém a informação sobre o teste ao método</param>
        private void CheckMethodSuccess(IReport r, XmlNode methodNode)
        {
            XmlAttribute success = _xmlDoc.CreateAttribute("success");
            XmlElement reason = _xmlDoc.CreateElement("failReason");

            Boolean time = r.ExecutionTime > r.MaximumTime;
            Boolean exception = (r.ShouldThrow == null) ? (r.Throwed != null) : !r.ShouldThrow.Equals(r.Throwed);
            Boolean returned = (r.ShouldReturn != null) && (!r.ShouldReturn.Equals(r.Returned));

            if (time || exception || returned)
            {
                success.Value = "no";

                if (time)
                {
                    reason.AppendChild(_xmlDoc.CreateTextNode("Tempo Máximo Excedido"));
                    methodNode.AppendChild(reason);
                }
                if (returned)
                {
                    reason.AppendChild(_xmlDoc.CreateTextNode("Retorno Discordante"));
                    methodNode.AppendChild(reason);
                }
                if (exception)
                {
                    if (r.ShouldThrow == null)
                        reason.AppendChild(_xmlDoc.CreateTextNode("Lançou Excepção"));
                    else
                        reason.AppendChild(_xmlDoc.CreateTextNode("Excepção Discordante"));
                    methodNode.AppendChild(reason);
                }
            }
            else
                success.Value = "yes";

            methodNode.Attributes.Append(success);
        }

        /// <summary>
        /// Escreve a informação de execução de uma classe para um nó XML
        /// </summary>
        /// <param name="m">Um relatório com o resultado dos testes á classe</param>
        /// <returns>O nó XML que contém a informação de execução da classe</returns>
        private XmlNode WriteClassInfo(IReport r)
        {
            XmlNode classNode = _xmlDoc.CreateElement("class");
            XmlNode description = _xmlDoc.CreateElement("description");

            XmlAttribute name = _xmlDoc.CreateAttribute("name");
            XmlAttribute execTime = _xmlDoc.CreateAttribute("execTime");
            XmlAttribute maxTime = _xmlDoc.CreateAttribute("maxTime");

            description.AppendChild(_xmlDoc.CreateTextNode(r.ClassDescription));
            name.Value = r.Class;
            execTime.Value = r.ExecutionTime.ToString();
            maxTime.Value = (r.MaximumTime == Int32.MaxValue) ? "Infinite" : r.MaximumTime.ToString();

            classNode.Attributes.Append(name);
            classNode.Attributes.Append(execTime);
            classNode.Attributes.Append(maxTime);
            classNode.AppendChild(description);

            CheckClassSuccess(r, classNode);

            return classNode;
        }

        /// <summary>
        /// Verifica se o teste executado sobre uma classe foi realizado com sucesso ou não.
        /// Um teste sobre uma classe tem sucesso, quando não excedeu o tempo máximo definido ou se a instanciação
        /// do tipo foi realizada com sucesso e todos os métodos marcados para teste foram carregados como tal.
        /// </summary>
        /// <param name="r">Um objecto que contém o relatório correspondente a uma classe</param>
        /// <param name="classNode">O nó que contém a informação de execução da classe</param>
        private void CheckClassSuccess(IReport r, XmlNode classNode)
        {
            XmlAttribute success = _xmlDoc.CreateAttribute("success");
            XmlElement reason = _xmlDoc.CreateElement("failReason");
            Boolean time = r.ExecutionTime > r.MaximumTime;
            Boolean exception = r.Throwed != null;

            if (time || exception)
            {
                success.Value = "no";

                if (time)
                {
                    reason.AppendChild(_xmlDoc.CreateTextNode("Tempo Máximo Excedido"));
                    classNode.AppendChild(reason);
                }
                if (exception)
                {
                    reason.AppendChild(_xmlDoc.CreateTextNode("Excepção Lançada: " + r.Throwed));
                    classNode.AppendChild(reason);
                }
            }
            else
                success.Value = "yes";

            classNode.Attributes.Append(success);
        }
    }
}
