using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using FI.AtividadeEntrevista.DML;

/// <summary>
/// Sistema desenvolvido para processo seletivo função sistemas
/// Autor: Eduardo Felipe de Souza
/// Antes de executar limpe o cache do seu navegador com CTRL + F5
/// </summary>
/// 
namespace WebAtividadeEntrevista.Utils
{
    public static class ValidaDigitoCPF
    {
        /// <summary>
        /// Função para verificar se há cpfs repetidos na lista
        /// </summary>
        public static Boolean possuiCpfsRepetidos(List<Beneficiarios> beneficiarios)
        {
            List<string> cpfs = new List<string>();
            try
            {
                foreach (Beneficiarios benef in beneficiarios)
                    cpfs.Add(benef.CPFBeneficiario);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            if (cpfs.Count != cpfs.Distinct().Count())
                return true;
            else
                return false;
        }
        /// <summary>
        /// Informar um CPF completo para validação do digito verificador
        /// </summary>
        /// <param name="cpf">Int64 com o numero CPF completo com Digito</param>
        /// <returns>Boolean True/False onde True=Digito CPF Valido</returns>
        public static Boolean ValidaCPF(Int64 cpf)
        {
            return ValidaCPF(cpf.ToString("D11"));
        }

        /// <summary>
        /// Informar um CPF completo para validação do digito verificador
        /// </summary>
        /// <param name="cpf">string com o numero CPF completo com Digito</param>
        /// <returns>Boolean True/False onde True=Digito CPF Valido</returns>
        public static Boolean ValidaCPF(string cpf)
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;
            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");
            if (cpf.Length != 11)
                return false;
            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cpf.EndsWith(digito);
        }

        ///Tratar CPF
        public static Int64 Tratador(string cpf)
        {
            Regex pattern = new Regex("[.-]");
            string treatedCPF = pattern.Replace(cpf, "");
            return Int64.Parse(treatedCPF);
        }
        /// <summary>
        /// Calcula o Digito verificador de um CPF informado  
        /// </summary>
        /// <param name="cpf">int64 com o CPF contendo 9 digitos e sem o digito verificador</param>
        /// <returns>string com o digito calculado do CPF ou null caso o cpf informado for maior que 9 digitos</returns>
        public static string CalculaDigCPF(Int64 cpf)
        {
            return CalculaDigCPF(cpf.ToString("D9"));
        }

        /// <summary>
        /// Calcula o Digito verificador de um CPF informado  
        /// </summary>
        /// <param name="cpf">string com o CPF contendo 9 digitos e sem o digito verificador</param>
        public static string CalculaDigCPF(string cpf)
        {
            // Declara variaveis para uso
            string new_cpf = "";
            string digito = "";
            Int32 Aux1 = 0;
            Int32 Aux2 = 0;

            // Retira carcteres invalidos não numericos da string
            for (int i = 0; i < cpf.Length; i++)
            {
                if (isDigito(cpf.Substring(i, 1)))
                {
                    new_cpf += cpf.Substring(i, 1);
                }
            }

            // Ajusta o Tamanho do CPF para 9 digitos completando com zeros a esquerda
            new_cpf = Convert.ToInt64(new_cpf).ToString("D9");

            // Caso o tamanho do CPF informado for maior que 9 digitos retorna nulo
            if (new_cpf.Length > 9)
            {
                return null;
            }

            // Calcula o primeiro digito do CPF
            Aux1 = 0;

            for (int i = 0; i < new_cpf.Length; i++)
            {
                Aux1 += Convert.ToInt32(new_cpf.Substring(i, 1)) * (10 - i);
            }

            Aux2 = 11 - (Aux1 % 11);

            // Carrega o primeiro digito na variavel digito
            if (Aux2 > 9)
            {
                digito += "0";
            }
            else
            {
                digito += Aux2.ToString();
            }

            // Adiciona o primeiro digito ao final do CPF para calculo do segundo digito
            new_cpf += digito;

            // Calcula o segundo digito do CPF
            Aux1 = 0;

            for (int i = 0; i < new_cpf.Length; i++)
            {
                Aux1 += Convert.ToInt32(new_cpf.Substring(i, 1)) * (11 - i);
            }

            Aux2 = 11 - (Aux1 % 11);

            // Carrega o segundo digito na variavel digito
            if (Aux2 > 9)
            {
                digito += "0";
            }
            else
            {
                digito += Aux2.ToString();
            }

            return digito;
        }

        /// <summary>
        /// Verifica se um digito informado é um numero
        /// </summary>
        /// <param name="digito">string com um caracter para verificar se é um numero</param>
        /// <returns>Boolean True/False</returns>
        private static Boolean isDigito(string digito)
        {
            int n;
            return Int32.TryParse(digito, out n);
        }
    }
}