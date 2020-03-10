using FI.AtividadeEntrevista.BLL;
using WebAtividadeEntrevista.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FI.AtividadeEntrevista.DML;
using static WebAtividadeEntrevista.Controllers.Utils;

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
            BoBeneficiarios boBen = new BoBeneficiarios();

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
                bool invalido = false;
                string msg = "";
                if (bo.VerificarExistencia(model.CPF))
                {
                    msg = string.Join(Environment.NewLine, string.Format("CPF:{0} já cadastrado!", model.CPF));
                    invalido = true;
                }
                if (!CpfCnpjUtils.IsValid(model.CPF))
                {
                    msg = string.Join(Environment.NewLine, string.Format("CPF:{0} invalido!", model.CPF));
                    invalido = true;
                }

                if (invalido)
                {
                    Response.StatusCode = 400;
                    return Json(msg);
                }

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
                    Telefone = model.Telefone
                });

                if (model.Benficiarios != null)
                {
                    foreach (var item in model.Benficiarios)
                    {
                        if (!CpfCnpjUtils.IsValid(item.CPF))
                        {
                            msg = string.Join(Environment.NewLine, string.Format("CPF:{0} invalido!", item.CPF));
                            invalido = true;
                        }
                        if (model.Benficiarios.Count(c => c.CPF == item.CPF) > 1)
                        {
                            msg = string.Join(Environment.NewLine, string.Format("CPF:{0} já cadastrado!", item.CPF));
                            invalido = true;
                        }
                        if (invalido)
                        {
                            Response.StatusCode = 400;
                            return Json(msg);
                        }

                        boBen.Incluir(new Benificiario()
                        {
                            CPF = item.CPF,
                            Nome = item.Nome,
                            IdCliente = model.Id
                        });
                    }
                }

                return Json("Cadastro efetuado com sucesso");
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
                    Benficiarios = new BoBeneficiarios().Consultar(id).Select(s => new BenificiarioModel
                    {
                        CPF = s.CPF,
                        Id = s.Id,
                        IdCliente = s.IdCliente,
                        Nome = s.Nome
                    }).ToList()
                };


            }

            return View(model);
        }

        [HttpPost]
        public JsonResult Alterar(ClienteModel model)
        {
            BoCliente bo = new BoCliente();
            BoBeneficiarios boBen = new BoBeneficiarios();

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
                var oCliente = bo.Consultar(model.Id);
                bool invalido = false;
                string msg = "";
                if (bo.VerificarExistencia(model.CPF) && oCliente.CPF != model.CPF)
                {
                    msg = string.Join(Environment.NewLine, string.Format("CPF:{0} já cadastrado!", model.CPF));
                    invalido = true;
                }
                if (!CpfCnpjUtils.IsValid(model.CPF))
                {
                    msg = string.Join(Environment.NewLine, string.Format("CPF:{0} invalido!", model.CPF));
                    invalido = true;
                }

                if (invalido)
                {
                    Response.StatusCode = 400;
                    return Json(msg);
                }

                bo.Alterar(new Cliente()
                {
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
                    CPF = model.CPF
                });

                var listaBeneficiadosExcluidos = new List<Benificiario>();
                var listaBeneficiados = boBen.Consultar(model.Id);

                if (model.Benficiarios != null)
                {
                    foreach (var item in model.Benficiarios)
                    {
                        if (!CpfCnpjUtils.IsValid(item.CPF))
                        {
                            msg = string.Join(Environment.NewLine, string.Format("CPF:{0} invalido!", item.CPF));
                            invalido = true;
                        }
                        if (model.Benficiarios.Count(c => c.CPF == item.CPF) > 1)
                        {
                            msg = string.Join(Environment.NewLine, string.Format("CPF:{0} já cadastrado!", item.CPF));
                            invalido = true;
                        }
                    }
                    if (invalido)
                    {
                        Response.StatusCode = 400;
                        return Json(msg);
                    }

                    listaBeneficiadosExcluidos = listaBeneficiados.Where(w => !model.Benficiarios.Select(s => s.CPF).Contains(w.CPF)).ToList();

                    foreach (var item in model.Benficiarios)
                    {
                        var oBeneficiado = listaBeneficiados.FirstOrDefault(w => w.CPF == item.CPF);

                        if (oBeneficiado == null)
                        {
                            boBen.Incluir(new Benificiario()
                            {
                                CPF = item.CPF,
                                Nome = item.Nome,
                                IdCliente = model.Id
                            });
                        }
                        else
                        {
                            oBeneficiado.CPF = item.CPF;
                            oBeneficiado.Nome = item.Nome;
                            oBeneficiado.IdCliente = model.Id;
                            boBen.Alterar(oBeneficiado);
                        }
                    }
                }
                else
                {
                    listaBeneficiadosExcluidos = listaBeneficiados;
                }
                foreach (var item in listaBeneficiadosExcluidos)
                {
                    boBen.Excluir(item.Id);
                }

                return Json("Cadastro alterado com sucesso");
            }
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