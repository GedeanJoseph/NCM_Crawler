using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using WebDataCrawlerTributacao.Entities;


namespace WebDataCrawlerTributacao.HtmlParser
{
    public class HtmlParserSubstituicaoTributaria
    {
        #region "Propriedades"

        private string htmlIFrameSubstituicaoTributaria;

        #endregion

        #region "Campos"

        public string HtmlIFrameSubstituicaoTributaria
        {
            get { return htmlIFrameSubstituicaoTributaria; }
            set { htmlIFrameSubstituicaoTributaria = value; }
        }

        #endregion

        #region "Contrutores"
        public HtmlParserSubstituicaoTributaria(string _htmlIframeSubstituicaotributaria)
        {
            this.HtmlIFrameSubstituicaoTributaria = _htmlIframeSubstituicaotributaria;
        }
        #endregion
        
        #region "Métodos Públicos"
        /// <summary>
        /// Controla as execuções dos métodos do parser para recuperar as informações das páginas.
        /// </summary>
        /// <returns></returns>
        public InformacaoSubstituicaoTributaria RetornaInformacoesSubstituicaoTributaria()
        { 
            InformacaoSubstituicaoTributaria substituicaoTributariaRecuperada = new InformacaoSubstituicaoTributaria();

            try
            {
                String[] TodasAsTabelasDaPagina = this.HtmlIFrameSubstituicaoTributaria.Split(new String[] { "<tbody>", "</tbody>" }, StringSplitOptions.RemoveEmptyEntries);//Recupera todas as tabelas da página para auxiliar os parser's

                this.ParseBaseLegalTributaria(ref substituicaoTributariaRecuperada);//Executa o parser para a Base legal            
                this.ParseValoresPercentuaisIvaStAliquota(ref substituicaoTributariaRecuperada);//Executa o parser para os 3 valores de IVA e Alíquota
                this.ParseBaseLegalIvaStAliquota(ref substituicaoTributariaRecuperada);//Executa o parser para os valores de Base Legal Iva-St e Base Legal Alíquota;
                this.ParseConveniosProtocolosSignatarios(ref substituicaoTributariaRecuperada, ref TodasAsTabelasDaPagina);//Executa o parser para os valores de convênios e protocolos.
                this.ParseBeneficiosFiscaisLegislacaoRelacionada(ref substituicaoTributariaRecuperada, ref TodasAsTabelasDaPagina);//Executa o parser para os valores de convênios e protocolos
                this.ParseObservacoesLegislacaoRelacionada(ref substituicaoTributariaRecuperada, ref TodasAsTabelasDaPagina);
                this.ParseIpiAliquota(ref substituicaoTributariaRecuperada, ref TodasAsTabelasDaPagina);
            }
            catch (Exception ex)
            {                
                throw ex;
            }
            

            return substituicaoTributariaRecuperada;
        }

        #endregion

        #region "Métodos Protegidos"

        /// <summary>
        /// Executa o parser da tag [Base Legal Substituição Tributária]
        /// </summary>
        /// <param name="_entideSubstituicaoTributaria"></param>
        private void ParseBaseLegalTributaria(ref InformacaoSubstituicaoTributaria _entideSubstituicaoTributaria)
        {
            Regex regexTagBaseLegal = new Regex("<td align=\"center\"><a href=\"[.]*.*</a></td>");
            Regex regexValorBaseLegal = new Regex("[Aa]rtigo ?[a-zA-Z0-9 -]*");
            Match matchTagBaseLegal = regexTagBaseLegal.Match(this.HtmlIFrameSubstituicaoTributaria);
            Match matchValorBaseLegal = regexValorBaseLegal.Match(matchTagBaseLegal.Value);

            _entideSubstituicaoTributaria.BaseLegalSubstituicaoTributaria = matchValorBaseLegal.Success ? matchValorBaseLegal.Value : "Não Identificado";
        }        

        /// <summary>
        /// Executa o parser das tag's dos 3 valores IVA-ST e Alíquota no Frame
        /// </summary>
        /// <param name="_entideSubstituicaoTributaria"></param>
        private void ParseValoresPercentuaisIvaStAliquota(ref InformacaoSubstituicaoTributaria _entideSubstituicaoTributaria)
        {
            Regex regexTagsIvaSt = new Regex("<tr align=\"center\" class=\"textos\">.*\n.*\n.*\n.*\n.*\n.*");
            Regex regexValoresIvaSt = new Regex("[0-9]{1,2},?[0-9]{0,2}");
            Match matchDasTables;
            MatchCollection matchValoresTrs;
            
            //Verifica os Matchs dos padrões no HTML
            matchDasTables = regexTagsIvaSt.Match(this.HtmlIFrameSubstituicaoTributaria);
            matchValoresTrs = regexValoresIvaSt.Matches(matchDasTables.Value);

            //atribui os valores caso encontrado
            _entideSubstituicaoTributaria.PctIVAST_Original = matchValoresTrs[0] != null ? Convert.ToDecimal(matchValoresTrs[0].Value) : 0;
            _entideSubstituicaoTributaria.PctIVAST_Ajustado_12_percent = matchValoresTrs[1] != null ? Convert.ToDecimal(matchValoresTrs[1].Value) : 0;
            _entideSubstituicaoTributaria.PctIVAST_Ajustado_4_percent = matchValoresTrs[2] != null ? Convert.ToDecimal(matchValoresTrs[2].Value) : 0;
            _entideSubstituicaoTributaria.PctAliquota = matchValoresTrs[3] != null ? Convert.ToDecimal(matchValoresTrs[3].Value) : 0;
        }

        /// <summary>
        /// Executa o parser das Tag's base Legal IVA-ST e Base  Legal Alíquota
        /// </summary>
        /// <param name="_entideSubstituicaoTributaria"></param>
        private void ParseBaseLegalIvaStAliquota(ref InformacaoSubstituicaoTributaria _entideSubstituicaoTributaria)
        {
            Regex regexTagsBaseLegalIvaStAliquota = new Regex("<td width=\"50%\"><a href=\".*(</a><a></a></td>|</a></td>|)*\n.*(</a><a></a></td>|</a></td>|)");
            Regex regexValoresBaseLegalIvaStAliquota = new  Regex("(?<=^|>)[^><\t]+?(?=<|$)");

            Match matchTagsBaseLegalIvaStAliquota = regexTagsBaseLegalIvaStAliquota.Match(this.HtmlIFrameSubstituicaoTributaria);
            MatchCollection matchValoresBaseLegalIvaStAliquota = regexValoresBaseLegalIvaStAliquota.Matches(matchTagsBaseLegalIvaStAliquota.Value);

            _entideSubstituicaoTributaria.BaseLegalIVAST = matchValoresBaseLegalIvaStAliquota[0] != null && matchValoresBaseLegalIvaStAliquota[0].Success ? matchValoresBaseLegalIvaStAliquota[0].Value : "";
            _entideSubstituicaoTributaria.BaseLegalAliquota = matchValoresBaseLegalIvaStAliquota[0] != null && matchValoresBaseLegalIvaStAliquota[1].Success ? matchValoresBaseLegalIvaStAliquota[1].Value : "";            

        }

        /// <summary>
        /// Executa o parser da tabela de Convênios, protocolos e Signatários.
        /// </summary>
        /// <param name="_entideSubstituicaoTributaria"></param>
        private void ParseConveniosProtocolosSignatarios(ref InformacaoSubstituicaoTributaria _entideSubstituicaoTributaria,ref String[] _todasTabelasDaPagina)
        {
            String tabelaConveniosProtocolos = _todasTabelasDaPagina[13].ToString();//Conforme a estrutura do site "index.com" a tabela [13] é a que possui as informações de "convênios e protocolos"
            Regex regexValoresConvProt = new Regex("(?<=^|>)(?<!<td width=\"50%\">)[^><\t\n]{3,}?(?=<|$)");
            Regex regexValoresSignatarios = new Regex("(?<=^|>)(?<!<td width=\"50%\">)[^><\t\n]{2}?(?=<|$)");

            MatchCollection matchValoresConvProts = regexValoresConvProt.Matches(tabelaConveniosProtocolos);
            MatchCollection matchValoresSignatarios = regexValoresSignatarios.Matches(tabelaConveniosProtocolos);

            int contadorMatches = 0;
            foreach (Match matchCorrente in matchValoresConvProts)
            {
                if (matchCorrente.Success && matchCorrente.Value != "")
                {
                    ConveniosProtocolosSignatarios novoConvenioProtocoloSignatarios = new ConveniosProtocolosSignatarios();
                    novoConvenioProtocoloSignatarios.ConveniosProtocolos = matchValoresConvProts[contadorMatches].Value;
                    novoConvenioProtocoloSignatarios.Signatarios = matchValoresSignatarios[contadorMatches].Value;

                    _entideSubstituicaoTributaria.ListaConveniosProtocolos.Add(novoConvenioProtocoloSignatarios);                    
                }

                contadorMatches = +1;//incrementa o contador para garantir que o convenios e os signatários serão tratados com o mesmo índice guia.
            }
                        
        }

        /// <summary>
        /// Efetua o parser dos valores de benefícios fiscais
        /// </summary>
        /// <param name="_entideSubstituicaoTributaria"></param>
        private void ParseBeneficiosFiscaisLegislacaoRelacionada(ref InformacaoSubstituicaoTributaria _entideSubstituicaoTributaria, ref String[] _todasTabelasDaPagina)
        {
            String tabelaBeneficiosFiscais = _todasTabelasDaPagina[15].ToString();//Conforme a estrutura do site "index.com" a tabela [15] que possui as informações 
            Regex regexValorBeneficiosFiscais = new Regex("(?<=<td style=\"text-align:justify\">)[^><\t\r\n]+?(?=<|$)");
            Regex regexValorLegislacaoRelacionada = new Regex("(?<=<a href=.+?>)[^><\t\r\n\f\v]+?(?=<|$)");

            MatchCollection matchValorBeneficiosFiscais = regexValorBeneficiosFiscais.Matches(tabelaBeneficiosFiscais);
            MatchCollection matchValorLegislacaoRelacionada = regexValorLegislacaoRelacionada.Matches(tabelaBeneficiosFiscais);

            foreach (Match matchCorrente in matchValorBeneficiosFiscais)
            {
                _entideSubstituicaoTributaria.ListaBeneficiosFiscais.Add(matchCorrente.Value);
            }

            foreach (Match matchCorrente in matchValorLegislacaoRelacionada)
            {
                _entideSubstituicaoTributaria.ListaBeneficiosFiscaisLegislacaoRelacionada.Add(matchCorrente.Value);
            }
            

        }

        /// <summary>
        /// Parser para recuperar as informações do Campo Observações e da legislação relacionada com as Observações;
        /// </summary>
        /// <param name="_entideSubstituicaoTributaria"></param>
        /// <param name="_todasTabelasDaPagina"></param>
        private void ParseObservacoesLegislacaoRelacionada(ref InformacaoSubstituicaoTributaria _entideSubstituicaoTributaria, ref String[] _todasTabelasDaPagina)
        {
            String tabelaBeneficiosFiscais = _todasTabelasDaPagina[17].ToString();//Conforme a estrutura do site "index.com" a tabela [15] que possui as informações 
            Regex regexValorBeneficiosFiscais = new Regex("(?<=<td style=\"text-align:justify\">)[^><\t\r\n]+?(?=<|$)");
            Regex regexValorLegislacaoRelacionada = new Regex("(?<=<a href=.+?>)[^><\t\r\n\f\v]+?(?=<|$)");

            MatchCollection matchValorBeneficiosFiscais = regexValorBeneficiosFiscais.Matches(tabelaBeneficiosFiscais);
            MatchCollection matchValorLegislacaoRelacionada = regexValorLegislacaoRelacionada.Matches(tabelaBeneficiosFiscais);

            foreach (Match matchCorrente in matchValorBeneficiosFiscais)
            {
                _entideSubstituicaoTributaria.ListaObservacoes.Add(matchCorrente.Value);
            }

            foreach (Match matchCorrente in matchValorLegislacaoRelacionada)
            {
                _entideSubstituicaoTributaria.ListaObservacoesLegiscaoRelacionada.Add(matchCorrente.Value);
            }
            
        }

        /// <summary>
        /// Parser para recuperar a informação de IPI e dentro do meno superior "IPI"
        /// </summary>
        /// <param name="_entideSubstituicaoTributaria"></param>
        /// <param name="_todasTabelasDaPagina"></param>
        private void ParseIpiAliquota(ref InformacaoSubstituicaoTributaria _entideSubstituicaoTributaria, ref String[] _todasTabelasDaPagina)
        {
            String tabelaBeneficiosFiscais = _todasTabelasDaPagina[19].ToString();//Conforme a estrutura do site "index.com" a tabela [15] que possui as informações 
            String[] linhasTabela = tabelaBeneficiosFiscais.Split(new String[] { "<tr class=\"textos\" align=\"center\">"},StringSplitOptions.None);
            Regex regexValoresNcmAliquota = new Regex("(?<=<td>)[0-9.]+?(?=</td>)");
            Regex regexValorDescricao = new Regex("(?<=<td style=\"text-align:justify\">)[^><]+?(?=</td>)");

            
            foreach (string linhaCorrente in linhasTabela)
            {
                if (!linhaCorrente.Contains("NCM")) //verifica se a linha corrente não é a header da table.
                {
                    IpiNcmAliquota novoIpiAliquota = new IpiNcmAliquota();

                    MatchCollection matchValoresNcmaliquota = regexValoresNcmAliquota.Matches(linhaCorrente);
                    Match matchDescricao = regexValorDescricao.Match(linhaCorrente);

                    novoIpiAliquota.Descricao = matchDescricao.Value;
                    novoIpiAliquota.Ncm = matchValoresNcmaliquota[0].Value;
                    novoIpiAliquota.Aliquota = matchValoresNcmaliquota.Count > 1? Convert.ToDecimal(matchValoresNcmaliquota[1].Value) : 0;

                    _entideSubstituicaoTributaria.ListaIpiNcmAliquita.Add(novoIpiAliquota);
                }                
            }
        }


        #endregion
    }
}