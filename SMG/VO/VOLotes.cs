using System;
using System.Collections;
using System.Xml.Serialization;


[Serializable]
public class VOLote
{
    private string _descricao;
    private string _valorEstimado;
    private string _valorRealizado;
    private string _prazo;
    private string _empresaVencedora;
    private string _tituloDataEventos;
    private DateTime _dataHoraHomologacao;
    private string _uf;
    private string _br;
    private string _kmInicio;
    private string _kmFim;
    private string _extensao;
    private string _regiao;

    public string Descricao
    {
        get
        {
            return _descricao;
        }

        set
        {
            _descricao = value;
        }
    }

    public string ValorEstimado
    {
        get
        {
            return _valorEstimado;
        }

        set
        {
            _valorEstimado = value;
        }
    }

    public string Prazo
    {
        get
        {
            return _prazo;
        }

        set
        {
            _prazo = value;
        }
    }

    public string ValorRealizado
    {
        get
        {
            return _valorRealizado;
        }

        set
        {
            _valorRealizado = value;
        }
    }

    public string EmpresaVencedora
    {
        get
        {
            return _empresaVencedora;
        }

        set
        {
            _empresaVencedora = value;
        }
    }

    public DateTime DataHoraHomologacao
    {
        get
        {
            return _dataHoraHomologacao;
        }

        set
        {
            _dataHoraHomologacao = value;
        }
    }

    public string Uf
    {
        get
        {
            return _uf;
        }

        set
        {
            _uf = value;
        }
    }

    public string Br
    {
        get
        {
            return _br;
        }

        set
        {
            _br = value;
        }
    }

    public string KmInicio
    {
        get
        {
            return _kmInicio;
        }

        set
        {
            _kmInicio = value;
        }
    }

    public string KmFim
    {
        get
        {
            return _kmFim;
        }

        set
        {
            _kmFim = value;
        }
    }

    public string Extensao
    {
        get
        {
            return _extensao;
        }

        set
        {
            _extensao = value;
        }
    }

    public string Regiao
    {
        get
        {
            return _regiao;
        }

        set
        {
            _regiao = value;
        }
    }

    public string TituloDataEventos
    {
        get
        {
            return _tituloDataEventos;
        }

        set
        {
            _tituloDataEventos = value;
        }
    }
}
