using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAtividadeEntrevista.Models
{
    /// <summary>
    /// Classe de Modelo de Cliente
    /// </summary>
    public class BenificiarioModel
    {
        public long Id { get; set; }

        /// <summary>
        /// CPF
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:###.###.###-##}")]
        public string CPF { get; set; }

        public string Nome { get; set; }

        public long IdCliente { get; set; }

    }    
}