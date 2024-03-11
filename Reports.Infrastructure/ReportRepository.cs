namespace Reports.Infrastructure
{
    public class ReportRepository
    {
        private readonly Dictionary<string, string> _objetivos = new Dictionary<string, string>();
        
        public ReportRepository()
        {
            _objetivos["Adaptação"] = "Para {1} a adaptação foi muito tranquila, não chorou em nenhum momento, adaptou-se ao novo grupo, a nova sala, funcionários da escola e " +
                " com naturalidade, sempre demonstrou muita alegria em estar no ambiente escolar.";
            _objetivos["Resolução de Conflitos"] = "Em  situações de conflito, {1} costuma usar o diálogo como principal estratégia  e dificilmente solicita ajuda à professora," +
                " demonstrando iniciativa na resolução de conflitos. ";
            _objetivos["Coordenação motora"] = "{1} demonstrou grande avanço na coordenação motora fina, principalmente em atividades como desenho, pintura e construção de com peças de encaixe." +
         "{0} alun{0} manipula objetos pequenos com facilidade.No que diz respeito à coordenação motora grossa, se destaca em atividades físicas que envolvem movimentos amplos do corpo como correr," +
         "pular e em atividades que requerem equilíbrio e força.";

            _objetivos["comunicação oral"] = "{1} é uma criança que se comunica de maneira clara e objetiva, falando sobre suas necessidades, argumentando," +
                "questionando e expressando suas opiniões, tem um vocabulário muito rico. Mostra-se curios{0}, " +
                "muito comunicativ{0} e alegre. Sempre participa ativamente das rodas de conversa e é comum relatar fatos do seu dia a dia em casa.";
            _objetivos["Ouvir os colegas"] = "Nas rodas de conversa e leitura, demonstra-se concentrad{0} em ouvir os colegas e professora.";
            _objetivos["interação"] = "{1} interage com todos os colegas da turma. Tem um bom relacionamento com todos que convive no ambiente escolar, é muito prestativ{0}" +
                "com funcionarios da escola e colegas, é comum oferecer-se para auxiliar em diversas situações e mostra-se feliz em ajudar.";
            _objetivos["hipotese de escrita"] = "Demonstra interesse pela linguagem escrita e já percebe que a  escrita representa a fala." +
                " Segundo algumas atividades coletivas," +
         " atividades de escrita espontânea e uma sondagem de palavras, {1} encontra - se na hipótese de escrita silábica com valor sonoro.";
            _objetivos["hipotese de escrita - SA"] = "É possível perceber que {0} alun{0} passou a demonstrar interesse pela linguagem escrita e já percebe que a  escrita representa a fala." +
               " Segundo algumas atividades coletivas," +
            " atividades de escrita espontânea e uma sondagem de palavras, {1} encontra - se na hipótese de escrita silábico alfabético.";
            _objetivos["hipotese de escrita - SV"] = "Após observação atenta d{0} alun{0} em situações de escrita. É possível afirmar que, tem demonstrado interesse pela linguagem escrita. Já entende o sistema silábico" +
              " Segundo algumas atividades coletivas," +
       " atividades de escrita espontânea e uma sondagem de palavras, {1} encontra - se na hipótese de escrita silábico com valor sonoro.";
        }
        /* {1} demonstrou grande avanço na coordenação motora fina, principalmente em atividades como desenho, pintura e construção de com peças de encaixe. 
         * {0} alun{0} manipula objetos pequenos com facilidade. No que diz respeito à coordenação motora grossa,
         *  se destaca em atividades físicas que envolvem movimentos amplos do corpo como correr,
         * pular e em atividades que requerem equilíbrio e força.
         matematica
        */

        public string? GetItem(string chave)
        {
             return _objetivos.TryGetValue(chave, out var valor) ? valor : null;
        }

        public bool CheckIItemExists(string chave)
        {
            return _objetivos.ContainsKey(chave);
        }
    }
}