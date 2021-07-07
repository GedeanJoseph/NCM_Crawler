using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.IO;
using WebDataCrawlerTributacao.Entities;


namespace WebDataCrawlerTributacao.BL
{
    /// <summary>
    /// Classe estática que fornece status de conexão, cookies de acesso e efetua login no site www.econet.com.br
    /// </summary>
    public static class EconetLoginBL
    {
        #region "Propriedades"
        private static CookieContainer cookiesLogin = null;
        private static Boolean logadoSucesso = false;
        #endregion

        #region "Campos"
        public static CookieContainer CookiesLogin
        {
            get { return EconetLoginBL.cookiesLogin; }
            set { EconetLoginBL.cookiesLogin = value; }
        }
        public static Boolean LogadoSucesso
        {
            get { return EconetLoginBL.logadoSucesso; }
            set { EconetLoginBL.logadoSucesso = value; }
        }
        #endregion

        #region "Métodos"
        /// <summary>
        /// Efetua o login no site utilizando os dados de login informados ou como padrão os dados log 
        /// </summary>
        /// <param name="_log"></param>
        /// <param name="_sen"></param>
        /// <returns>Retorna os as informações de Cookies para que sejam utilizados em outras solicitações e requisicões</returns>
        public static CookieContainer LoginEconetEditora(String _log = "FTP24518", String _sen = "comgoli")
        {
            #region "Configuração do cookie inicial"
            CookieContainer cookieContainer = new CookieContainer();
            Cookie cookieInicial = new Cookie();
            cookieInicial.Name = "PHPSESSID";
            cookieInicial.Comment = "";
            cookieInicial.CommentUri = null;
            cookieInicial.Discard = false;
            cookieInicial.Domain = "www2.econeteditora.com.br";
            cookieInicial.Expires = DateTime.MinValue;
            cookieInicial.Expired = false;
            cookieInicial.HttpOnly = false;
            cookieInicial.Path = "/";
            cookieInicial.Port = "";
            cookieInicial.Secure = false;
            cookieInicial.Value = "ucei1j65grgrr4scpfeljhtmj2";
            cookieContainer.Add(cookieInicial);
            #endregion

            #region "Executa o Post da página de login"
            Uri urlEconet = new Uri("http://www2.econeteditora.com.br/user/ver_log.asp");
            HttpWebRequest http = (HttpWebRequest)HttpWebRequest.Create(urlEconet);
            http.Method = "POST";            
            http.ContentType = "application/x-www-form-urlencoded";
            http.CookieContainer = cookieContainer;//atribui o cookie inicial
            http.Timeout = 22000;
            http.KeepAlive = true;
            http.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            http.ReadWriteTimeout = 32000;
            http.Referer = "http://www2.econeteditora.com.br/user/ver_log.asp";
            http.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/32.0.1700.76 Safari/537.36";//Se identifica como sendo um acesso via browser.
            Stream streamRequesteconet = http.GetRequestStream();
            FormUtility form = new FormUtility(streamRequesteconet, null);
            form.Add("Log", _log);
            form.Add("Sen", _sen);
            form.Add("Pag", "logged.php");
            form.Add("Pag", "");
            form.Complete();
            streamRequesteconet.Close();
            HttpWebResponse response = (HttpWebResponse)http.GetResponse();
            Stream streamResponse = response.GetResponseStream();

            #endregion

            #region "Executa o get da página inicial logada"
            Uri urlLogado = new Uri("http://www2.econeteditora.com.br/index.asp?url=inicial.php");
            HttpWebRequest requestHttp = (HttpWebRequest)HttpWebRequest.Create(urlLogado);
            requestHttp.CookieContainer = cookieContainer;

            HttpWebResponse ResponseHttpLogado = (HttpWebResponse)requestHttp.GetResponse();
            Stream streamResponseLogado = ResponseHttpLogado.GetResponseStream();
            StreamReader readerLogado = new StreamReader(streamResponseLogado, Encoding.GetEncoding("iso-8859-1"));
            #endregion

            #region "Validao o sucesso do login"
            
            Regex regexSucessologin = new Regex("(?=&nbsp;Login Efetuado com Sucesso[.])"); //Executa a leitura da página de retorno procurando a confirmação de login com sucesso.  
            if (regexSucessologin.Match(readerLogado.ReadToEnd()).Success)
            { 
                LogadoSucesso = true;
                CookiesLogin = cookieContainer;                
            }
            else
            {
                LogadoSucesso = false;
                CookiesLogin = null;
            }
            #endregion

            //retorna o container com as informações de cookie caso a ER acima tenha sucesso, senão retorna Nulo, falha no login
            return CookiesLogin;
        }
        #endregion
    }
}
