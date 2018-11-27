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
    public partial class Form1 : Form
    {


        public Form1()
        {
            InitializeComponent();
        }

        private void btnMonitoramento_Click(object sender, EventArgs e)
        {
            BusinessService bs = new BusinessService();
            bs.Operation = BusinessService.OPRATION_READ;

            bs.ListObject.Add(Navegar.MODALIDADE_PREGAO);
            bs = realizarPesquisa(bs);

            if (bs.Result == BusinessService.RESULT_ERROR)
            {
                MessageBox.Show("Erro", "Alerta");
            }
        }

        BusinessService realizarPesquisa(BusinessService _bs)
        {
            BusinessService bsCertamesPesquisadoAgora = new BusinessService();
            Navegar navegar = new Navegar();

            BusinessService bsIO = new BusinessService();
            bsIO.ListObject.Add(VOCertame.CERTAME_AGENDADOS);
            // Intervalo de captura das informações
            bsIO.ListObject.Add(4);
            IOObjetos io = new IOObjetos();

            Boolean recoveryCertamesComprasNet = io.lastChange(bsIO);

            BusinessService bsCertamesBancoDeDados = io.Load(bsIO);

            BusinessService bsCertamesAtualizados = new BusinessService();

            if (recoveryCertamesComprasNet)
            {
                bsCertamesPesquisadoAgora = navegar.realizarPesquisalistaPregaoFiltro(_bs);

                // Verificar última pesquisa
                // Recuperar certames antigos


                // Mesclar certames - Parte 1 - Banco de dados e Comprasnet
                // bsCertamesBancoDeDados
                // bsCertamesPesquisadoAgora

                // Verifica ultima atualização e se preciso salva no banco de dados

                bsIO = new BusinessService();
                bsIO.ListObject.Add(VOCertame.CERTAME_AGENDADOS);
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
            bsIO.ListObject.Add(bsCertamesAtualizados.ListObject);
            bsCertamesPesquisadoAgora = io.Save(bsIO);

            return _bs;
        }


    }
}
