using System;
using System.Collections;
using System.Xml.Serialization;


[Serializable]
public class VOCertame
{

    public const string CERTAME_ABERTO = "ABERTO";
    public const string CERTAME_SUSPENSO = "SUSPENSO";
    public const string CERTAME_SUSPENSO_TEMPORARIA = "SUSPENSO TEMP.";
    public const string CERTAME_EM_ANDAMENTO = "EM ANDAMENTO";
    public const string CERTAME_REALIZADOS_PENDENTES_RECURSO_ADJUDICACAO_HOMOLOGACAO = "REALIZADOS PENDENTES DE RECURSO ADJUDICAÇÃO HOMOLOGAÇÃO";
    public const string CERTAME_AGENDADOS = "PREGOES AGENDADOS";
    public const string CERTAME_ATAS = "PREGOES ATAS";


    public const string MODALIDADE_PREGAO_ELETRONICO = "Pregão Eletrônico";
    public const string MODALIDADE_PREGAO_ELETRONICO_SIGLA = "PE";
    public const string MODALIDADE_RDC_ELETRONICO = "RDC Eletrônico";


    private int _item;

    private DateTime _dataEdital;
    private DateTime _dataCertame;
    private DateTime _dataInicioProposta;
    private DateTime _dataFimProposta;
    private DateTime _dataBase;
    private string _orgao;
    private string _numeroProcesso;
    private string _uasg;
    private string _siglaModalidade;
    private string _numero;
    private string _cidade;
    private string _UF;
    private string _objeto;
    private double _valor;
    private double _valorDesconto;
    private string _status;
    private string _modalidade;
    private string _criterioJulgamento;
    private string _endereco;
    private string _telefone;
    private string _mensagens; // mensagens
    private string _historico; // histórico
    private string _sigla;
    private string _descricoes;
    private string _enderecoEdital;
    private string _id;
    private string _alertaNomeEmpresaEncontrado;
    private string _emailImpugnacao;
    private string _descricaoImpugnacao;
    private string _situacao;
    private string _informacoesCertame;
    private bool _downloadEdital;
    private bool _participando;
    private bool _acompanhamento;
    private bool _suspensoTemporariamente;
    private bool _abertoParaRecurso;
    private bool _atestadoOP;
    private bool _atestadoTEC;
    private bool _certameGanho;
    private bool _recursoImpetrado;
    private bool _novoItem;
    private bool _srp;
    private bool _icms;
    private bool _preferencial;
    private DateTime _dataAtualizacao;
    private IList _listTabArquivos;
    private IList _listTabLotes;

    public int Item
    {
        get
        {
            return _item;
        }

        set
        {
            _item = value;
        }
    }

    public string Cidade
    {
        get
        {
            return _cidade;
        }

        set
        {
            _cidade = value;
        }
    }

    public string UF
    {
        get
        {
            return _UF;
        }

        set
        {
            _UF = value;
        }
    }

    public string Orgao
    {
        get
        {
            return _orgao;
        }

        set
        {
            _orgao = value;
        }
    }

    public string Uasg
    {
        get
        {
            return _uasg;
        }

        set
        {
            _uasg = value;
        }
    }

    public string Modalidade
    {
        get
        {
            return _modalidade;
        }

        set
        {
            _modalidade = value;
        }
    }

    public string Numero
    {
        get
        {
            return _numero;
        }

        set
        {
            _numero = value;
        }
    }

    public double Valor
    {
        get
        {
            return _valor;
        }

        set
        {
            _valor = value;
        }
    }

    public string Descricoes
    {
        get
        {
            return _descricoes;
        }

        set
        {
            _descricoes = value;
        }
    }

    public bool Recurso
    {
        get
        {
            return RecursoImpetrado;
        }

        set
        {
            RecursoImpetrado = value;
        }
    }

    public bool AtestadoOP
    {
        get
        {
            return _atestadoOP;
        }

        set
        {
            _atestadoOP = value;
        }
    }

    public bool AtestadoTEC
    {
        get
        {
            return _atestadoTEC;
        }

        set
        {
            _atestadoTEC = value;
        }
    }

    public string Status
    {
        get
        {
            return _status;
        }

        set
        {
            _status = value;
        }
    }

    public bool Srp
    {
        get
        {
            return _srp;
        }

        set
        {
            _srp = value;
        }
    }

    public bool Icms
    {
        get
        {
            return _icms;
        }

        set
        {
            _icms = value;
        }
    }


    public string Mensagens
    {
        get
        {
            return _mensagens;
        }

        set
        {
            _mensagens = value;
        }
    }

    public string Objeto
    {
        get
        {
            return _objeto;
        }

        set
        {
            _objeto = value;
        }
    }

    public string Endereco
    {
        get
        {
            return _endereco;
        }

        set
        {
            _endereco = value;
        }
    }

    public string Telefone
    {
        get
        {
            return _telefone;
        }

        set
        {
            _telefone = value;
        }
    }

    public DateTime DataEdital
    {
        get
        {
            return _dataEdital;
        }

        set
        {
            _dataEdital = value;
        }
    }

    public DateTime DataCertame
    {
        get
        {
            return _dataCertame;
        }

        set
        {
            _dataCertame = value;
        }
    }

    public string Historico
    {
        get
        {
            return _historico;
        }

        set
        {
            _historico = value;
        }
    }



    public bool RecursoImpetrado
    {
        get
        {
            return _recursoImpetrado;
        }

        set
        {
            _recursoImpetrado = value;
        }
    }

    public bool AbertoParaRecurso
    {
        get
        {
            return _abertoParaRecurso;
        }

        set
        {
            _abertoParaRecurso = value;
        }
    }


    public bool Acompanhamento
    {
        get
        {
            return _acompanhamento;
        }

        set
        {
            _acompanhamento = value;
        }
    }

    public bool DownloadEdital
    {
        get
        {
            return _downloadEdital;
        }

        set
        {
            _downloadEdital = value;
        }
    }

    public bool CertameGanho
    {
        get
        {
            return _certameGanho;
        }

        set
        {
            _certameGanho = value;
        }
    }

    public bool Participando
    {
        get
        {
            return _participando;
        }

        set
        {
            _participando = value;
        }
    }

    public string Sigla
    {
        get
        {
            return _sigla;
        }

        set
        {
            _sigla = value;
        }
    }

    public string SiglaModalidade
    {
        get
        {
            return _siglaModalidade;
        }

        set
        {
            _siglaModalidade = value;
        }
    }

    public string EnderecoEdital
    {
        get
        {
            return _enderecoEdital;
        }

        set
        {
            _enderecoEdital = value;
        }
    }

    public string Id
    {
        get
        {
            return _id;
        }

        set
        {
            _id = value;
        }
    }

    public bool NaoVencido
    {
        get
        {
            return _novoItem;
        }

        set
        {
            _novoItem = value;
        }
    }

    public string AlertaNomeEmpresaEncontrado
    {
        get
        {
            return _alertaNomeEmpresaEncontrado;
        }

        set
        {
            _alertaNomeEmpresaEncontrado = value;
        }
    }

    public bool SuspensoTemporariamente
    {
        get
        {
            return _suspensoTemporariamente;
        }

        set
        {
            _suspensoTemporariamente = value;
        }
    }

    public bool Preferencial
    {
        get
        {
            return _preferencial;
        }

        set
        {
            _preferencial = value;
        }
    }

    public string EmailImpugnacao
    {
        get
        {
            return _emailImpugnacao;
        }

        set
        {
            _emailImpugnacao = value;
        }
    }

    public string DescricaoImpugnacao
    {
        get
        {
            return _descricaoImpugnacao;
        }

        set
        {
            _descricaoImpugnacao = value;
        }
    }

    public DateTime DataAtualizacao
    {
        get
        {
            return _dataAtualizacao;
        }

        set
        {
            _dataAtualizacao = value;
        }
    }

    public double ValorDesconto
    {
        get
        {
            return _valorDesconto;
        }

        set
        {
            _valorDesconto = value;
        }
    }

    public DateTime DataInicioProposta
    {
        get
        {
            return _dataInicioProposta;
        }

        set
        {
            _dataInicioProposta = value;
        }
    }

    public DateTime DataFimProposta
    {
        get
        {
            return _dataFimProposta;
        }

        set
        {
            _dataFimProposta = value;
        }
    }

    public string Situacao
    {
        get
        {
            return _situacao;
        }

        set
        {
            _situacao = value;
        }
    }

    public string InformacoesCertame
    {
        get
        {
            return _informacoesCertame;
        }

        set
        {
            _informacoesCertame = value;
        }
    }

    public string CriterioJulgamento
    {
        get
        {
            return _criterioJulgamento;
        }

        set
        {
            _criterioJulgamento = value;
        }
    }

    public DateTime DataBase
    {
        get
        {
            return _dataBase;
        }

        set
        {
            _dataBase = value;
        }
    }

    public string NumeroProcesso
    {
        get
        {
            return _numeroProcesso;
        }

        set
        {
            _numeroProcesso = value;
        }
    }

    public IList ListTabArquivos
    {
        get
        {
            if (_listTabArquivos == null)
                _listTabArquivos = new ArrayList();
            return _listTabArquivos;
        }

        set
        {
            if (_listTabArquivos == null)
                _listTabArquivos = new ArrayList();
            _listTabArquivos = value;
        }
    }

    public IList ListTabLotes
    {
        get
        {
            if (_listTabLotes == null)
                _listTabLotes = new ArrayList();
            return _listTabLotes;
        }

        set
        {
            if (_listTabLotes == null)
                _listTabLotes = new ArrayList();
            _listTabLotes = value;
        }
    }
}
