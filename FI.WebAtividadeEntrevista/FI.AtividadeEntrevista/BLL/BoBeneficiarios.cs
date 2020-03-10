using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FI.AtividadeEntrevista.BLL
{
    public class BoBeneficiarios
    {
        /// <summary>
        /// Inclui um novo cliente
        /// </summary>
        /// <param name="cliente">Objeto de cliente</param>
        public long Incluir(DML.Benificiario beneficiario)
        {
            DAL.DaoBeneficiarios benef = new DAL.DaoBeneficiarios();
            return benef.Incluir(beneficiario);
        }

        /// <summary>
        /// Altera um cliente
        /// </summary>
        /// <param name="cliente">Objeto de cliente</param>
        public void Alterar(DML.Benificiario cliente)
        {
            DAL.DaoBeneficiarios benef = new DAL.DaoBeneficiarios();
            benef.Alterar(cliente);
        }

        /// <summary>
        /// Consulta o cliente pelo id
        /// </summary>
        /// <param name="id">id do cliente</param>
        /// <returns></returns>
        public List<DML.Benificiario> Consultar(long id)
        {
            DAL.DaoBeneficiarios benef = new DAL.DaoBeneficiarios();
            return benef.Consultar(id);
        }

        /// <summary>
        /// Excluir o cliente pelo id
        /// </summary>
        /// <param name="id">id do cliente</param>
        /// <returns></returns>
        public void Excluir(long id)
        {
            DAL.DaoBeneficiarios cli = new DAL.DaoBeneficiarios();
            cli.Excluir(id);
        }
    }
}
