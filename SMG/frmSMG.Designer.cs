using System.Globalization;

namespace SMG
{
    partial class frmSMG
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnMonitoramento = new System.Windows.Forms.Button();
            this.btnExportar = new System.Windows.Forms.Button();
            this.cmbTipoPesquisa = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txbCertameNumero = new System.Windows.Forms.TextBox();
            this.txbUasg = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dgvLicitacoes = new System.Windows.Forms.DataGridView();
            this.btnUploadDb = new System.Windows.Forms.Button();
            this.vOCertameBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.numeroProcessoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.orgaoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.uasgDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.modalidadeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.numeroDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.objetoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.valorDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mensagensDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.valorDescontoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataCertameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataInicioPropostaDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataFimPropostaDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.situacaoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.informacoesCertameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLicitacoes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.vOCertameBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // btnMonitoramento
            // 
            this.btnMonitoramento.Location = new System.Drawing.Point(983, 458);
            this.btnMonitoramento.Name = "btnMonitoramento";
            this.btnMonitoramento.Size = new System.Drawing.Size(146, 23);
            this.btnMonitoramento.TabIndex = 0;
            this.btnMonitoramento.Text = "Realizar Monitoramento";
            this.btnMonitoramento.UseVisualStyleBackColor = true;
            this.btnMonitoramento.Click += new System.EventHandler(this.btnMonitoramento_Click);
            // 
            // btnExportar
            // 
            this.btnExportar.Location = new System.Drawing.Point(867, 458);
            this.btnExportar.Name = "btnExportar";
            this.btnExportar.Size = new System.Drawing.Size(110, 23);
            this.btnExportar.TabIndex = 1;
            this.btnExportar.Text = "Exportar Excel";
            this.btnExportar.UseVisualStyleBackColor = true;
            this.btnExportar.Click += new System.EventHandler(this.btnExportar_Click);
            // 
            // cmbTipoPesquisa
            // 
            this.cmbTipoPesquisa.FormattingEnabled = true;
            this.cmbTipoPesquisa.Items.AddRange(new object[] {
            "Pregões Agendados",
            "Pregões Em Andamento",
            "Realizados, Pendentes de Recurso/Adjudicação/Homologação",
            "Pregões Revogados, Anulados ou Abandonados",
            "Pregões Atas",
            "Pregões Internacionais com Recurso do BID ou BIRD"});
            this.cmbTipoPesquisa.Location = new System.Drawing.Point(35, 45);
            this.cmbTipoPesquisa.Name = "cmbTipoPesquisa";
            this.cmbTipoPesquisa.Size = new System.Drawing.Size(302, 21);
            this.cmbTipoPesquisa.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(392, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Número do certame:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(32, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Tipo do certame:";
            // 
            // txbCertameNumero
            // 
            this.txbCertameNumero.Location = new System.Drawing.Point(395, 46);
            this.txbCertameNumero.Name = "txbCertameNumero";
            this.txbCertameNumero.Size = new System.Drawing.Size(142, 20);
            this.txbCertameNumero.TabIndex = 5;
            this.txbCertameNumero.Text = "0372/2018";
            // 
            // txbUasg
            // 
            this.txbUasg.Location = new System.Drawing.Point(596, 46);
            this.txbUasg.Name = "txbUasg";
            this.txbUasg.Size = new System.Drawing.Size(142, 20);
            this.txbUasg.TabIndex = 7;
            this.txbUasg.Text = "393031";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(593, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "UASG:";
            // 
            // dgvLicitacoes
            // 
            this.dgvLicitacoes.AutoGenerateColumns = false;
            this.dgvLicitacoes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLicitacoes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.numeroProcessoDataGridViewTextBoxColumn,
            this.orgaoDataGridViewTextBoxColumn,
            this.uasgDataGridViewTextBoxColumn,
            this.modalidadeDataGridViewTextBoxColumn,
            this.numeroDataGridViewTextBoxColumn,
            this.objetoDataGridViewTextBoxColumn,
            this.valorDataGridViewTextBoxColumn,
            this.statusDataGridViewTextBoxColumn,
            this.mensagensDataGridViewTextBoxColumn,
            this.valorDescontoDataGridViewTextBoxColumn,
            this.dataCertameDataGridViewTextBoxColumn,
            this.dataInicioPropostaDataGridViewTextBoxColumn,
            this.dataFimPropostaDataGridViewTextBoxColumn,
            this.situacaoDataGridViewTextBoxColumn,
            this.informacoesCertameDataGridViewTextBoxColumn});
            this.dgvLicitacoes.DataSource = this.vOCertameBindingSource;
            this.dgvLicitacoes.Location = new System.Drawing.Point(35, 96);
            this.dgvLicitacoes.Name = "dgvLicitacoes";
            this.dgvLicitacoes.Size = new System.Drawing.Size(1094, 356);
            this.dgvLicitacoes.TabIndex = 8;
            this.dgvLicitacoes.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvLicitacoes_ColumnHeaderMouseClick);
            this.dgvLicitacoes.Columns[6].DefaultCellStyle.Format = "c";
            this.dgvLicitacoes.Columns[9].DefaultCellStyle.Format = "c";
            

            // 
            // btnUploadDb
            // 
            this.btnUploadDb.Location = new System.Drawing.Point(751, 458);
            this.btnUploadDb.Name = "btnUploadDb";
            this.btnUploadDb.Size = new System.Drawing.Size(110, 23);
            this.btnUploadDb.TabIndex = 9;
            this.btnUploadDb.Text = "Upload DB";
            this.btnUploadDb.UseVisualStyleBackColor = true;
            this.btnUploadDb.Click += new System.EventHandler(this.btnUploadDb_Click);
            // 
            // vOCertameBindingSource
            // 
            this.vOCertameBindingSource.DataSource = typeof(VOCertame);
            // 
            // numeroProcessoDataGridViewTextBoxColumn
            // 
            this.numeroProcessoDataGridViewTextBoxColumn.DataPropertyName = "NumeroProcesso";
            this.numeroProcessoDataGridViewTextBoxColumn.HeaderText = "NumeroProcesso";
            this.numeroProcessoDataGridViewTextBoxColumn.Name = "numeroProcessoDataGridViewTextBoxColumn";
            // 
            // orgaoDataGridViewTextBoxColumn
            // 
            this.orgaoDataGridViewTextBoxColumn.DataPropertyName = "Orgao";
            this.orgaoDataGridViewTextBoxColumn.HeaderText = "Orgao";
            this.orgaoDataGridViewTextBoxColumn.Name = "orgaoDataGridViewTextBoxColumn";
            // 
            // uasgDataGridViewTextBoxColumn
            // 
            this.uasgDataGridViewTextBoxColumn.DataPropertyName = "Uasg";
            this.uasgDataGridViewTextBoxColumn.HeaderText = "Uasg";
            this.uasgDataGridViewTextBoxColumn.Name = "uasgDataGridViewTextBoxColumn";
            // 
            // modalidadeDataGridViewTextBoxColumn
            // 
            this.modalidadeDataGridViewTextBoxColumn.DataPropertyName = "Modalidade";
            this.modalidadeDataGridViewTextBoxColumn.HeaderText = "Modalidade";
            this.modalidadeDataGridViewTextBoxColumn.Name = "modalidadeDataGridViewTextBoxColumn";
            // 
            // numeroDataGridViewTextBoxColumn
            // 
            this.numeroDataGridViewTextBoxColumn.DataPropertyName = "Numero";
            this.numeroDataGridViewTextBoxColumn.HeaderText = "Numero";
            this.numeroDataGridViewTextBoxColumn.Name = "numeroDataGridViewTextBoxColumn";
            // 
            // objetoDataGridViewTextBoxColumn
            // 
            this.objetoDataGridViewTextBoxColumn.DataPropertyName = "Objeto";
            this.objetoDataGridViewTextBoxColumn.HeaderText = "Objeto";
            this.objetoDataGridViewTextBoxColumn.Name = "objetoDataGridViewTextBoxColumn";
            // 
            // valorDataGridViewTextBoxColumn
            // 
            this.valorDataGridViewTextBoxColumn.DataPropertyName = "Valor";
            this.valorDataGridViewTextBoxColumn.HeaderText = "Valor";
            this.valorDataGridViewTextBoxColumn.Name = "valorDataGridViewTextBoxColumn";
            // 
            // statusDataGridViewTextBoxColumn
            // 
            this.statusDataGridViewTextBoxColumn.DataPropertyName = "Status";
            this.statusDataGridViewTextBoxColumn.HeaderText = "Status";
            this.statusDataGridViewTextBoxColumn.Name = "statusDataGridViewTextBoxColumn";
            // 
            // mensagensDataGridViewTextBoxColumn
            // 
            this.mensagensDataGridViewTextBoxColumn.DataPropertyName = "Mensagens";
            this.mensagensDataGridViewTextBoxColumn.HeaderText = "Mensagens";
            this.mensagensDataGridViewTextBoxColumn.Name = "mensagensDataGridViewTextBoxColumn";
            // 
            // valorDescontoDataGridViewTextBoxColumn
            // 
            this.valorDescontoDataGridViewTextBoxColumn.DataPropertyName = "ValorDesconto";
            this.valorDescontoDataGridViewTextBoxColumn.HeaderText = "ValorDesconto";
            this.valorDescontoDataGridViewTextBoxColumn.Name = "valorDescontoDataGridViewTextBoxColumn";
            // 
            // dataCertameDataGridViewTextBoxColumn
            // 
            this.dataCertameDataGridViewTextBoxColumn.DataPropertyName = "DataCertame";
            this.dataCertameDataGridViewTextBoxColumn.HeaderText = "DataCertame";
            this.dataCertameDataGridViewTextBoxColumn.Name = "dataCertameDataGridViewTextBoxColumn";
            // 
            // dataInicioPropostaDataGridViewTextBoxColumn
            // 
            this.dataInicioPropostaDataGridViewTextBoxColumn.DataPropertyName = "DataInicioProposta";
            this.dataInicioPropostaDataGridViewTextBoxColumn.HeaderText = "DataInicioProposta";
            this.dataInicioPropostaDataGridViewTextBoxColumn.Name = "dataInicioPropostaDataGridViewTextBoxColumn";
            // 
            // dataFimPropostaDataGridViewTextBoxColumn
            // 
            this.dataFimPropostaDataGridViewTextBoxColumn.DataPropertyName = "DataFimProposta";
            this.dataFimPropostaDataGridViewTextBoxColumn.HeaderText = "DataFimProposta";
            this.dataFimPropostaDataGridViewTextBoxColumn.Name = "dataFimPropostaDataGridViewTextBoxColumn";
            // 
            // situacaoDataGridViewTextBoxColumn
            // 
            this.situacaoDataGridViewTextBoxColumn.DataPropertyName = "Situacao";
            this.situacaoDataGridViewTextBoxColumn.HeaderText = "Situacao";
            this.situacaoDataGridViewTextBoxColumn.Name = "situacaoDataGridViewTextBoxColumn";
            // 
            // informacoesCertameDataGridViewTextBoxColumn
            // 
            this.informacoesCertameDataGridViewTextBoxColumn.DataPropertyName = "InformacoesCertame";
            this.informacoesCertameDataGridViewTextBoxColumn.HeaderText = "InformacoesCertame";
            this.informacoesCertameDataGridViewTextBoxColumn.Name = "informacoesCertameDataGridViewTextBoxColumn";
            // 
            // frmSMG
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1174, 493);
            this.Controls.Add(this.btnUploadDb);
            this.Controls.Add(this.dgvLicitacoes);
            this.Controls.Add(this.txbUasg);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txbCertameNumero);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbTipoPesquisa);
            this.Controls.Add(this.btnExportar);
            this.Controls.Add(this.btnMonitoramento);
            this.Name = "frmSMG";
            this.Text = "SMG - Sistema de Monitoramento Gerencial";
            this.Load += new System.EventHandler(this.frmSMG_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLicitacoes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.vOCertameBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnMonitoramento;
        private System.Windows.Forms.Button btnExportar;
        private System.Windows.Forms.ComboBox cmbTipoPesquisa;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txbCertameNumero;
        private System.Windows.Forms.TextBox txbUasg;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView dgvLicitacoes;
        private System.Windows.Forms.BindingSource vOCertameBindingSource;
        private System.Windows.Forms.Button btnUploadDb;
        private System.Windows.Forms.DataGridViewTextBoxColumn numeroProcessoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn orgaoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn uasgDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn modalidadeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn numeroDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn objetoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn valorDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn statusDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn mensagensDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn valorDescontoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataCertameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataInicioPropostaDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataFimPropostaDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn situacaoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn informacoesCertameDataGridViewTextBoxColumn;
    }
}

