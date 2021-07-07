using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using WebDataCrawlerTributacao.Entities;
using WebDataCrawlerTributacao.HtmlParser;

namespace WebDataCrawlerTributacao.BL
{
    public class AliquotasInterestaduaisBL
    {
        public List<AliquotaInterestaduaisOrigemDestino> RetornaAliquotasInterestaduais()
        {
            List<AliquotaInterestaduaisOrigemDestino> origemDestinoAliquotaInterestaduais = new List<AliquotaInterestaduaisOrigemDestino>();

            String htmlDaPagina = "";

            using (StreamReader reader = new StreamReader(@"C:\Regex\OrigemDestinoAliquotasInterestaduais\..   ECONET Editora   .._files\tab_icms-interestaduais.htm", Encoding.ASCII))
            {
                htmlDaPagina = reader.ReadToEnd();
            }

            HtmlParserTabelaAliquotasInterestaduaiss parserTabela = new HtmlParserTabelaAliquotasInterestaduaiss(htmlDaPagina);
            origemDestinoAliquotaInterestaduais = parserTabela.RetornaAliquotasInterestaduaisOrigemDestino();

            return origemDestinoAliquotaInterestaduais;
        }
        
    }
}

