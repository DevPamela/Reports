using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Reports.Domain.Constants
{
    public class GoalsSatisfatory
    {
        [BsonId]
        [Display(Name = "Code")]
        public Guid Id { get; set; }

        [Column("Name")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Column("Name")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        //    public const string Adaptação = "Para {1} a adaptação foi muito tranquila, não chorou em nenhum momento, adaptou-se ao novo grupo," +
        //       " a nova sala e funcionários da escola, com naturalidade, sempre demonstrou muita alegria em estar no ambiente escolar.";

        //    public const string interação = "Foi possível perceber que {0} alun{0} interage com todos os colegas da turma. Tem um bom relacionamento com todos que convive no ambiente escolar, é muito prestativ{0} " +
        //        "com funcionarios da escola e colegas, é comum oferecer-se para auxiliar em diversas situações e mostra-se feliz em ajudar.";

        //    public const string comunicação = "{1} é uma criança que se comunica de maneira clara e objetiva, falando sobre suas necessidades, argumentando, questionando e expressando suas opiniões, tem um vocabulário muito rico. " +
        //        "Mostra-se curios{0}, muito comunicativ{0} e alegre. Sempre participa ativamente das rodas de conversa e é comum relatar fatos do seu dia a dia em casa.";

        //    public const string rodaDeConversa = "Nas rodas de conversa e leitura, demonstra-se concentrad{0} em ouvir os colegas e professores, participando ativamente e respeitando a vez de cada um.";

        //    public const string CoordenaçãoMotora = "{1} demonstrou grande avanço na coordenação motora fina, principalmente em atividades como desenho, pintura e construção com peças de encaixe.{0} alun{0} manipula objetos pequenos " +
        //        "com facilidade. No que diz respeito à coordenação motora grossa, se destaca em atividades físicas que envolvem movimentos amplos do corpo como correr, pular e em atividades que requerem equilíbrio e força.";

        //    public const string ResolucaoDeConflitos = "Em  situações de conflito, {0} alun{0} costuma usar o diálogo como principal estratégia e dificilmente solicita ajuda à professora, demonstrando iniciativa na resolução de conflitos.";

        //    public const string SilabicoSemValor = "Após observação atenta d{0} alun{0} em situações de escrita. É possível afirmar que, tem demonstrado interesse pela linguagem escrita. Já entende o sistema silábico e eegundo algumas atividades " +
        //        "coletivas, atividades de escrita espontânea e uma sondagem de palavras, {1} encontra - se na hipótese de escrita silábico com valor sonoro.";

        //    public const string SilabicoComValor = "Demonstra interesse pela linguagem escrita e já percebe que a  escrita representa a fala. Segundo algumas atividades coletivas, atividades de escrita espontânea e uma sondagem de palavras," +
        //        " {1} encontra - se na hipótese de escrita silábica com valor sonoro.";

        //    public const string SilabicoAlfabetico = "É possível perceber que {0} alun{0} passou a demonstrar interesse pela linguagem escrita e já percebe que a  escrita representa a fala. Segundo algumas atividades coletivas," +
        //        " atividades de escrita espontânea e uma sondagem de palavras, {1} encontra - se na hipótese de escrita silábico alfabético.";

        //    public bool ConstantExiste(Type tipo, string constantName)
        //    {
        //        FieldInfo[] fields = tipo.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

        //        foreach (FieldInfo field in fields)
        //        {
        //            if (field.IsLiteral && !field.IsInitOnly && field.Name == constantName)
        //            {
        //                return true;
        //            }
        //        }

        //        return false;
        //    }
        //    public string GetConst(Type type, string constantName)
        //    {
        //        FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);


        //        foreach (FieldInfo field in fields)
        //        {
        //            if (field.IsLiteral && !field.IsInitOnly && field.Name == constantName)
        //            {
        //                return field.GetValue(field.Name).ToString();
        //            }
        //        }
        //        return null;
        //    }
        //}
    }
}

