using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FI.AtividadeEntrevista.DAL
{
    class DaoBeneficiarios : AcessoDados
    {
        /// <summary>
        /// Inclui um novo benificiario
        /// </summary>
        /// <param name="benificiario">Objeto de benificiario</param>
        internal long Incluir(DML.Benificiario benificiario)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("CPF", benificiario.CPF));
            parametros.Add(new System.Data.SqlClient.SqlParameter("Nome", benificiario.Nome));
            parametros.Add(new System.Data.SqlClient.SqlParameter("IDCLIENTE", benificiario.IdCliente));

            DataSet ds = base.Consultar("FI_SP_IncBenef", parametros);
            long ret = 0;
            if (ds.Tables[0].Rows.Count > 0)
                long.TryParse(ds.Tables[0].Rows[0][0].ToString(), out ret);
            return ret;
        }

        /// <summary>
        /// Consulta um benificiario
        /// </summary>
        /// <param name="benificiario">Objeto de benificiario</param>
        internal List<DML.Benificiario> Consultar(long idCliente)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("IDCLIENTE", idCliente));

            DataSet ds = base.Consultar("FI_SP_ConsBenef", parametros);
            List<DML.Benificiario> ben = Converter(ds);

            return ben;
        }

        /// <summary>
        /// Lista todos os benificiarios
        /// </summary>
        internal List<DML.Benificiario> Listar()
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("Id", 0));

            DataSet ds = base.Consultar("FI_SP_ConsBenef", parametros);
            List<DML.Benificiario> ben = Converter(ds);

            return ben;
        }

        /// <summary>
        /// Inclui um novo benificiario
        /// </summary>
        /// <param name="benificiario">Objeto de benificiario</param>
        internal void Alterar(DML.Benificiario benificiario)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("Nome", benificiario.Nome));
            parametros.Add(new System.Data.SqlClient.SqlParameter("CPF", benificiario.CPF));
            parametros.Add(new System.Data.SqlClient.SqlParameter("ID", benificiario.Id));

            base.Executar("FI_SP_AltBenef", parametros);
        }


        /// <summary>
        /// Excluir Benificiario
        /// </summary>
        /// <param name="benificiario">Objeto de benificiario</param>
        internal void Excluir(long id)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("Id", id));

            base.Executar("FI_SP_DelBenef", parametros);
        }

        private List<DML.Benificiario> Converter(DataSet ds)
        {
            List<DML.Benificiario> lista = new List<DML.Benificiario>();
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DML.Benificiario ben = new DML.Benificiario();
                    ben.Id = row.Field<long>("Id");
                    ben.CPF = row.Field<string>("CPF");
                    ben.Nome = row.Field<string>("Nome");
                    ben.IdCliente = row.Field<long>("IdCliente");
                    lista.Add(ben);
                }
            }

            return lista;
        }
    }
}
