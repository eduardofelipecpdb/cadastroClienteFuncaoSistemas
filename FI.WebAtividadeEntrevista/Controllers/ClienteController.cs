using FI.AtividadeEntrevista.BLL;
using WebAtividadeEntrevista.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FI.AtividadeEntrevista.DML;
using WebAtividadeEntrevista.Utils;
using System.Text.RegularExpressions;
/// <summary>
/// Sistema desenvolvido para processo seletivo função sistemas
/// Autor: Eduardo Felipe de Souza
/// Antes de executar limpe o cache do seu navegador com CTRL + F5
/// </summary>
namespace WebAtividadeEntrevista.Controllers
{
    public class ClienteController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Incluir()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Incluir(ClienteModel model)
        {
            BoCliente bo = new BoCliente();

            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {
                if (ValidaDigitoCPF.possuiCpfsRepetidos(model.beneficiarios))
                {
                    Response.StatusCode = 400;
                    return Json(string.Join(Environment.NewLine, "Há um ou mais CPFs iguais na lista de beneficiarios"));
                }

                try
                {
                    foreach (Beneficiarios novoBenef in model.beneficiarios)
                        if (ValidaDigitoCPF.ValidaCPF(ValidaDigitoCPF.Tratador(novoBenef.CPFBeneficiario)).Equals(false))
                        {
                            Response.StatusCode = 400;
                            return Json(string.Join(Environment.NewLine, "CPF do beneficiário " + novoBenef.NomeBeneficiario + " é inválido"));
                        }
                }
                catch(Exception e)
                {
                    Console.WriteLine("Cliente não possui beneficiarios");
                }
                if (ValidaDigitoCPF.ValidaCPF(ValidaDigitoCPF.Tratador(model.CPF)).Equals(true))
                {
                    if (bo.VerificarExistencia(model.CPF).Equals(false))
                    {
                        model.Id = bo.Incluir(new Cliente()
                        {
                            
                            CPF = model.CPF,
                            CEP = model.CEP,
                            Cidade = model.Cidade,
                            Email = model.Email,
                            Estado = model.Estado,
                            Logradouro = model.Logradouro,
                            Nacionalidade = model.Nacionalidade,
                            Nome = model.Nome,
                            Sobrenome = model.Sobrenome,
                            Telefone = model.Telefone,
                            beneficiarios = model.beneficiarios
                        });

                        return Json("Cadastro efetuado com sucesso");
                    }
                    else
                    {
                        Response.StatusCode = 400;
                        return Json(string.Join(Environment.NewLine, "CPF já cadastrado"));
                    }
                }
                else
                {
                    Response.StatusCode = 400;
                    return Json(string.Join(Environment.NewLine, "CPF inválido"));
                }
            }
        }

        [HttpPost]
        public JsonResult Alterar(ClienteModel model)
        {
            BoCliente bo = new BoCliente();

            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {
                if (ValidaDigitoCPF.possuiCpfsRepetidos(model.beneficiarios))
                {
                    Response.StatusCode = 400;
                    return Json(string.Join(Environment.NewLine, "Há um ou mais CPFs iguais na lista de beneficiarios"));
                }

                try
                {
                    foreach (Beneficiarios novoBenef in model.beneficiarios)
                        if (ValidaDigitoCPF.ValidaCPF(ValidaDigitoCPF.Tratador(novoBenef.CPFBeneficiario)).Equals(false))
                        {
                            Response.StatusCode = 400;
                            return Json(string.Join(Environment.NewLine, "CPF do beneficiário " + novoBenef.NomeBeneficiario + " é inválido"));
                        }
                }
                catch(Exception e) 
                {
                    Console.WriteLine("Cliente não possui beneficiários");
                }

                if (ValidaDigitoCPF.ValidaCPF(ValidaDigitoCPF.Tratador(model.CPF)).Equals(true))
                {
                    bo.Alterar(new Cliente()
                    {
                        CPF = model.CPF,
                        Id = model.Id,
                        CEP = model.CEP,
                        Cidade = model.Cidade,
                        Email = model.Email,
                        Estado = model.Estado,
                        Logradouro = model.Logradouro,
                        Nacionalidade = model.Nacionalidade,
                        Nome = model.Nome,
                        Sobrenome = model.Sobrenome,
                        Telefone = model.Telefone,
                        beneficiarios = model.beneficiarios
                    });

                    return Json("Cadastro alterado com sucesso");
                }
                else
                {
                    Response.StatusCode = 400;
                    return Json(string.Join(Environment.NewLine, "CPF do cliente é inválido"));
                }
            }
        }

        [HttpGet]
        public ActionResult Alterar(long id)
        {
            BoCliente bo = new BoCliente();
            Cliente cliente = bo.Consultar(id);
            Models.ClienteModel model = null;

            if (cliente != null)
            {
                model = new ClienteModel()
                {
                    Id = cliente.Id,
                    CEP = cliente.CEP,
                    Cidade = cliente.Cidade,
                    Email = cliente.Email,
                    Estado = cliente.Estado,
                    Logradouro = cliente.Logradouro,
                    Nacionalidade = cliente.Nacionalidade,
                    Nome = cliente.Nome,
                    Sobrenome = cliente.Sobrenome,
                    Telefone = cliente.Telefone,
                    CPF = cliente.CPF,
                    beneficiarios = cliente.beneficiarios
                };
            }

            return View(model);
        }

        [HttpPost]
        public JsonResult ClienteList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                int qtd = 0;
                string campo = string.Empty;
                string crescente = string.Empty;
                string[] array = jtSorting.Split(' ');

                if (array.Length > 0)
                    campo = array[0];

                if (array.Length > 1)
                    crescente = array[1];

                List<Cliente> clientes = new BoCliente().Pesquisa(jtStartIndex, jtPageSize, campo, crescente.Equals("ASC", StringComparison.InvariantCultureIgnoreCase), out qtd);

                //Return result to jTable
                return Json(new { Result = "OK", Records = clientes, TotalRecordCount = qtd });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
    }
}