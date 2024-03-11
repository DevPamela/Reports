/*
 versão gratuita ter uma o~ção de selecionar entre os objetivos disponíveis para geração de relatorio, garantindo qualidade de boa escrita e um paragrafo introdutório e um outro finalizando, só salva em pdf




 
 versão paga: acompanhamento do aluno e possibilidade de descrever os avanços do aluno, possibilidade de incluir timbre da escola e fotos do aluno já formatadas criar os proprios objetivos personalizados, salva pdf e word

api de autenticação
api de relatorio
api de planejamento
api de atividades
api de cadastro de usuario
so entrar no metodo de criação de relatório se não tiver feito nos ultimos tres meses, limite de 5 // plano gratuito
poder acrescentar o próprio paragrafo e colocar o nome do aluno.

proximos passos- 
criar cenarios para objetivos de alunos que não sao ruins e não sao bons 
criar validação, se não receber genero e outros valores
criar validação para se não receber primeiro e segundo paragrafo
rever o fluxo de informações 
refatorar para ter mais desempenho
Colocar os objetivos bonitinhos
estudar padrão orm
bolar um jeito de alterar os pronomes feminino e masculino 
ler lgpd ver o que fala sobre
estudar modelo mvc- abrir um form e o back recebe as infos
 
 
public string BuscarValor(string chave, bool validacao)
{
    string valor;

    if (validacao)
    {
        // Buscar valor na TabelaTrue com a chave fornecida
        valor = BuscarNoBanco<TabelaTrue>(chave).Valor;
    }
    else
    {
        // Buscar valor na TabelaFalse com a mesma chave
        valor = BuscarNoBanco<TabelaFalse>(chave).Valor;
    }

    return valor;
}

public class TabelaTrue
{
    public string Chave { get; set; }
    public string Valor { get; set; }
}

public class TabelaFalse
{
    public string Chave { get; set; }
    public string Valor { get; set; }
}

 
 
 
 */