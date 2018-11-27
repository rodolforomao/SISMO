using BusinessSelenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Persistencia;
using System.Collections;

namespace SMG
{
    public partial class frmSMG : Form
    {
        #region WebSite

        const String WEBSITE_PREGOES_AGENDADOS = "http://comprasnet.gov.br/acesso.asp?url=/livre/Pregao/lista_pregao_filtro.asp?Opc=0";
        const String WEBSITE_PREGOES_EM_ANDAMENTO = "http://comprasnet.gov.br/acesso.asp?url=/livre/Pregao/lista_pregao_filtro.asp?Opc=2";
        const String WEBSITE_PREGOES_ATAS = "http://comprasnet.gov.br/acesso.asp?url=/livre/Pregao/ata0.asp";
        const String WEBSITE_PREGOES_REALIZADOS_PENDENTES_RECURSO_ADJUDICACAO_HOMOLOGACAO = "http://comprasnet.gov.br/acesso.asp?url=/livre/Pregao/lista_pregao_filtro.asp?Opc=1";

        #endregion

        public frmSMG()
        {
            InitializeComponent();
        }

        private void btnMonitoramento_Click(object sender, EventArgs e)
        {
            BusinessService bs = new BusinessService();
            bs.Operation = BusinessService.OPRATION_READ;

            bs.ListObject.Add(Navegar.MODALIDADE_PREGAO);
            VOCertame voCertame = new VOCertame();
            voCertame.Numero = txbCertameNumero.Text;
            voCertame.Uasg = txbUasg.Text;

            if(cmbTipoPesquisa.SelectedItem != null && !String.IsNullOrEmpty(cmbTipoPesquisa.SelectedItem.ToString()))
            {
                if (cmbTipoPesquisa.SelectedItem.ToString().ToUpper().Contains("AGENDADOS"))
                    bs.ListObject.Add(VOCertame.CERTAME_AGENDADOS);
                else if (cmbTipoPesquisa.SelectedItem.ToString().ToUpper().Contains("ANDAMENTO"))
                    bs.ListObject.Add(VOCertame.CERTAME_EM_ANDAMENTO);
                else if (cmbTipoPesquisa.SelectedItem.ToString().ToUpper().Contains("REALIZADOS"))
                    bs.ListObject.Add(VOCertame.CERTAME_REALIZADOS_PENDENTES_RECURSO_ADJUDICACAO_HOMOLOGACAO);
                else if (cmbTipoPesquisa.SelectedItem.ToString().ToUpper().Contains("ATAS"))
                    bs.ListObject.Add(VOCertame.CERTAME_ATAS);

                bs.ListObject.Add(voCertame);

                bs = realizarPesquisaPregões(bs);
            }
            else
            {
                MessageBox.Show("Selecione o tipo de modalidade", "Alerta");
            }

            if (bs.Result == BusinessService.RESULT_ERROR)
            {
                MessageBox.Show("Erro", "Alerta");
            }
        }

        BusinessService realizarPesquisaPregões(BusinessService _bs)
        {
            BusinessService bsCertamesPesquisadoAgora = new BusinessService();
            Navegar navegar = new Navegar();

            String modalidade = _bs.ListObject[0].ToString();

            BusinessService bsIO = new BusinessService();
            bsIO.ListObject.Add(modalidade);
            // Intervalo de captura das informações
            bsIO.ListObject.Add(1);
            IOObjetos io = new IOObjetos();

            Boolean recoveryCertamesComprasNet = io.lastChange(bsIO);
            //Boolean recoveryCertamesComprasNet = true;

            BusinessService bsCertamesBancoDeDados = io.Load(bsIO);

            BusinessService bsCertamesAtualizados = new BusinessService();

            if (recoveryCertamesComprasNet)
            {
                
                String statusDoCertame = _bs.ListObject[1].ToString();
                VOCertame voCertame = (VOCertame)_bs.ListObject[2];
                _bs = new BusinessService();

                bsIO = new BusinessService();

                if (statusDoCertame.Contains(VOCertame.CERTAME_AGENDADOS))
                {
                    _bs.ListObject.Add(WEBSITE_PREGOES_AGENDADOS);
                    _bs.ListObject.Add(voCertame);
                    _bs.ListObject.Add(VOCertame.CERTAME_AGENDADOS);
                    bsCertamesPesquisadoAgora = navegar.realizarPesquisalistaPregaoFiltro(_bs);

                    bsIO.ListObject.Add(VOCertame.CERTAME_AGENDADOS);
                }
                else if (statusDoCertame.Contains(VOCertame.CERTAME_EM_ANDAMENTO))
                {
                    _bs.ListObject.Add(WEBSITE_PREGOES_EM_ANDAMENTO);
                    _bs.ListObject.Add(voCertame);
                    _bs.ListObject.Add(VOCertame.CERTAME_EM_ANDAMENTO);
                    bsCertamesPesquisadoAgora = navegar.realizarPesquisalistaPregaoEmAndamento(_bs);

                    bsIO.ListObject.Add(VOCertame.CERTAME_EM_ANDAMENTO);
                }
                else if (statusDoCertame.Contains(VOCertame.CERTAME_REALIZADOS_PENDENTES_RECURSO_ADJUDICACAO_HOMOLOGACAO))
                {
                    _bs.ListObject.Add(WEBSITE_PREGOES_REALIZADOS_PENDENTES_RECURSO_ADJUDICACAO_HOMOLOGACAO);
                    _bs.ListObject.Add(voCertame);
                    _bs.ListObject.Add(VOCertame.CERTAME_REALIZADOS_PENDENTES_RECURSO_ADJUDICACAO_HOMOLOGACAO);
                    bsCertamesPesquisadoAgora = navegar.realizarPesquisalistaPregaoRealizadosPendentesdeRecursoAdjudicaçãoHomologação(_bs);

                    bsIO.ListObject.Add(VOCertame.CERTAME_EM_ANDAMENTO);
                }
                else if (statusDoCertame.Contains(VOCertame.CERTAME_ATAS))
                {
                    _bs.ListObject.Add(WEBSITE_PREGOES_ATAS);
                    _bs.ListObject.Add(voCertame);
                    _bs.ListObject.Add(VOCertame.CERTAME_ATAS);
                    bsCertamesPesquisadoAgora = navegar.realizarPesquisalistaPregaoAtaFiltro(_bs);

                    bsIO.ListObject.Add(VOCertame.CERTAME_ATAS);
                }

                if (bsCertamesPesquisadoAgora.ListObject.Count > 0)
                {
                    dgvLicitacoes.DataSource = bsCertamesPesquisadoAgora.ListObject;
                    dgvLicitacoes.Refresh();
                }
                
                bsIO.ListObject.Add(bsCertamesPesquisadoAgora.ListObject);
                bsCertamesPesquisadoAgora = io.Save(bsIO);
                bsIO.ListObject.RemoveAt(0);
            }

            if (bsCertamesPesquisadoAgora.ListObject.Count > 0)
                bsCertamesAtualizados.ListObject = (IList)bsCertamesPesquisadoAgora.ListObject[0];
            else
                bsCertamesAtualizados.ListObject = (IList)bsCertamesBancoDeDados.ListObject;

            // Atualizar informações com infromações do DNIT
            BusinessService bsCertamesPesquisadoDnit = navegar.realizarPesquisaDNIT(bsCertamesAtualizados);

            // Mesclar certames - Parte 2 - Banco de dados atualizado e DNIT
            // bsCertamesPesquisadoDnit
            // bsCertamesAtualizados

            // Salvar no banco de dados
            bsIO = new BusinessService();
            bsIO.ListObject.Add(VOCertame.CERTAME_AGENDADOS);
            bsIO.ListObject.Add(bsCertamesPesquisadoDnit.ListObject);
            bsCertamesPesquisadoAgora = io.Save(bsIO);

            return _bs;
        }


        private void btnExportar_Click(object sender, EventArgs e)
        {
            BusinessService bsIO = new BusinessService();
            bsIO.ListObject.Add(VOCertame.CERTAME_AGENDADOS);
            IOObjetos io = new IOObjetos();
            BusinessService bsCertamesBancoDeDados = io.Load(bsIO);

            ExportarCertames exportar = new ExportarCertames();

            BusinessService bsExportacao = new BusinessService();
            bsExportacao.ListObject.Add("teste.xlsx");
            bsExportacao.ListObject.Add(bsCertamesBancoDeDados.ListObject);

            bsExportacao = exportar.exportarcertames(bsExportacao);

            if (bsExportacao.Result == BusinessService.RESULT_SUCESS)
            {
                MessageBox.Show("Exportação feita com sucesso.", "Alerta");
            }
            else
            {
                MessageBox.Show(bsExportacao.Message[0].ToString(), "Erro");
            }

        }

        private void frmSMG_Load(object sender, EventArgs e)
        {
            cmbTipoPesquisa.SelectedIndex = 2;
        }

        private void btnUploadDb_Click(object sender, EventArgs e)
        {
            IOObjetos io = new IOObjetos();
            BusinessService bs = new BusinessService();

            bs.ListObject.Add(VOCertame.CERTAME_AGENDADOS);
            bs = io.Load(bs);

            dgvLicitacoes.DataSource = bs.ListObject;
        }

        Boolean order = false;
        private void dgvLicitacoes_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int columnSelected = e.ColumnIndex;
            switch(columnSelected)
            {
                case 0:
                    {
                        IOObjetos io = new IOObjetos();
                        BusinessService bs = new BusinessService();
                        bs.ListObject.Add(VOCertame.CERTAME_AGENDADOS);
                        bs = io.Load(bs);
                        IList listaOrdenada = new ArrayList();

                        if (order)
                        {
                            foreach (VOCertame objVo in bs.ListObject)
                            {
                                listaOrdenada.Insert(0, objVo);
                            }
                        }
                        else
                            listaOrdenada = bs.ListObject;

                        dgvLicitacoes.DataSource = listaOrdenada;

                        order = !order;
                    }
                    break;
            }
        }
    }
}
