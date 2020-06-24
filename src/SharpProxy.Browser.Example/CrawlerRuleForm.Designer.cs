namespace SharpProxy.Browser.Example
{
    partial class CrawlerRuleForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CrawlerRuleForm));
            this.label1 = new System.Windows.Forms.Label();
            this.cbxBookName = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtAuthor = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSaveBookRules = new System.Windows.Forms.Button();
            this.cbxSelectedRule = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtChapterRootPath = new System.Windows.Forms.TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label5 = new System.Windows.Forms.Label();
            this.cbxChapterTitleType = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtNextPagePath = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtStartUrl = new System.Windows.Forms.TextBox();
            this.cbxRuleName = new System.Windows.Forms.ComboBox();
            this.txtChapterTitlePath = new System.Windows.Forms.TextBox();
            this.btnSaveRules = new System.Windows.Forms.Button();
            this.btnImportRules = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.txtAwaitTime = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(51, 69);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "书名";
            // 
            // cbxBookName
            // 
            this.cbxBookName.FormattingEnabled = true;
            this.cbxBookName.Location = new System.Drawing.Point(97, 61);
            this.cbxBookName.Name = "cbxBookName";
            this.cbxBookName.Size = new System.Drawing.Size(127, 20);
            this.cbxBookName.TabIndex = 1;
            this.cbxBookName.SelectedIndexChanged += new System.EventHandler(this.cbxBookName_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(283, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "作者";
            // 
            // txtAuthor
            // 
            this.txtAuthor.Location = new System.Drawing.Point(342, 60);
            this.txtAuthor.Name = "txtAuthor";
            this.txtAuthor.Size = new System.Drawing.Size(227, 21);
            this.txtAuthor.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(38, 124);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "使用规则";
            // 
            // btnSaveBookRules
            // 
            this.btnSaveBookRules.Location = new System.Drawing.Point(646, 121);
            this.btnSaveBookRules.Name = "btnSaveBookRules";
            this.btnSaveBookRules.Size = new System.Drawing.Size(75, 23);
            this.btnSaveBookRules.TabIndex = 6;
            this.btnSaveBookRules.Text = "保存配置";
            this.btnSaveBookRules.UseVisualStyleBackColor = true;
            this.btnSaveBookRules.Click += new System.EventHandler(this.btnSaveBookRules_Click);
            // 
            // cbxSelectedRule
            // 
            this.cbxSelectedRule.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxSelectedRule.FormattingEnabled = true;
            this.cbxSelectedRule.Location = new System.Drawing.Point(97, 121);
            this.cbxSelectedRule.Name = "cbxSelectedRule";
            this.cbxSelectedRule.Size = new System.Drawing.Size(127, 20);
            this.cbxSelectedRule.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(259, 258);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 8;
            this.label4.Text = "主节点ID";
            this.toolTip1.SetToolTip(this.label4, "在主窗口中按F12, 选取章节内容的主节点, 找到ID, 复制到此处, 也可以使用xpath, Chrome可以复制xpath路径");
            // 
            // txtChapterRootPath
            // 
            this.txtChapterRootPath.Location = new System.Drawing.Point(342, 254);
            this.txtChapterRootPath.Name = "txtChapterRootPath";
            this.txtChapterRootPath.Size = new System.Drawing.Size(227, 21);
            this.txtChapterRootPath.TabIndex = 9;
            this.toolTip1.SetToolTip(this.txtChapterRootPath, "在主窗口中按F12, 选取章节内容的主节点, 找到ID, 复制到此处, 也可以使用xpath, Chrome可以复制xpath路径");
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 301);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 12);
            this.label5.TabIndex = 10;
            this.label5.Text = "章节标题节点";
            this.toolTip1.SetToolTip(this.label5, "章节标题节点的class, xpath, id都行");
            // 
            // cbxChapterTitleType
            // 
            this.cbxChapterTitleType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxChapterTitleType.FormattingEnabled = true;
            this.cbxChapterTitleType.Items.AddRange(new object[] {
            "Id",
            "xpath",
            "class"});
            this.cbxChapterTitleType.Location = new System.Drawing.Point(97, 298);
            this.cbxChapterTitleType.Name = "cbxChapterTitleType";
            this.cbxChapterTitleType.Size = new System.Drawing.Size(127, 20);
            this.cbxChapterTitleType.TabIndex = 12;
            this.toolTip1.SetToolTip(this.cbxChapterTitleType, "章节标题节点的class, xpath, id都行");
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(38, 350);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 13;
            this.label6.Text = "翻页按钮";
            this.toolTip1.SetToolTip(this.label6, "翻页按钮的 JS pxth");
            // 
            // txtNextPagePath
            // 
            this.txtNextPagePath.Location = new System.Drawing.Point(97, 341);
            this.txtNextPagePath.Name = "txtNextPagePath";
            this.txtNextPagePath.Size = new System.Drawing.Size(127, 21);
            this.txtNextPagePath.TabIndex = 14;
            this.toolTip1.SetToolTip(this.txtNextPagePath, "翻页按钮的 JS pxth");
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(38, 258);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 15;
            this.label7.Text = "规则名称";
            this.toolTip1.SetToolTip(this.label7, "建议使用网站url");
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(265, 124);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(47, 12);
            this.label8.TabIndex = 18;
            this.label8.Text = "起始Url";
            this.toolTip1.SetToolTip(this.label8, "从哪一章节开始就用哪一章节的url");
            // 
            // txtStartUrl
            // 
            this.txtStartUrl.Location = new System.Drawing.Point(342, 121);
            this.txtStartUrl.Name = "txtStartUrl";
            this.txtStartUrl.Size = new System.Drawing.Size(227, 21);
            this.txtStartUrl.TabIndex = 19;
            this.toolTip1.SetToolTip(this.txtStartUrl, "从哪一章节开始就用哪一章节的url");
            // 
            // cbxRuleName
            // 
            this.cbxRuleName.FormattingEnabled = true;
            this.cbxRuleName.Location = new System.Drawing.Point(97, 255);
            this.cbxRuleName.Name = "cbxRuleName";
            this.cbxRuleName.Size = new System.Drawing.Size(127, 20);
            this.cbxRuleName.TabIndex = 20;
            this.toolTip1.SetToolTip(this.cbxRuleName, "建议使用网站url");
            this.cbxRuleName.SelectedIndexChanged += new System.EventHandler(this.cbxRuleName_SelectedIndexChanged);
            // 
            // txtChapterTitlePath
            // 
            this.txtChapterTitlePath.Location = new System.Drawing.Point(230, 297);
            this.txtChapterTitlePath.Name = "txtChapterTitlePath";
            this.txtChapterTitlePath.Size = new System.Drawing.Size(339, 21);
            this.txtChapterTitlePath.TabIndex = 11;
            // 
            // btnSaveRules
            // 
            this.btnSaveRules.Location = new System.Drawing.Point(342, 401);
            this.btnSaveRules.Name = "btnSaveRules";
            this.btnSaveRules.Size = new System.Drawing.Size(75, 23);
            this.btnSaveRules.TabIndex = 17;
            this.btnSaveRules.Text = "保存规则";
            this.btnSaveRules.UseVisualStyleBackColor = true;
            this.btnSaveRules.Click += new System.EventHandler(this.btnSaveRules_Click);
            // 
            // btnImportRules
            // 
            this.btnImportRules.Location = new System.Drawing.Point(97, 401);
            this.btnImportRules.Name = "btnImportRules";
            this.btnImportRules.Size = new System.Drawing.Size(75, 23);
            this.btnImportRules.TabIndex = 21;
            this.btnImportRules.Text = "导入规则";
            this.btnImportRules.UseVisualStyleBackColor = true;
            this.btnImportRules.Click += new System.EventHandler(this.btnImportRules_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(283, 353);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 12);
            this.label9.TabIndex = 22;
            this.label9.Text = "采集延时";
            this.toolTip1.SetToolTip(this.label9, "防止加载网页过快被检测为机器人, 单位: 秒");
            // 
            // txtAwaitTime
            // 
            this.txtAwaitTime.Location = new System.Drawing.Point(342, 347);
            this.txtAwaitTime.Name = "txtAwaitTime";
            this.txtAwaitTime.Size = new System.Drawing.Size(227, 21);
            this.txtAwaitTime.TabIndex = 23;
            this.txtAwaitTime.Text = "1";
            this.toolTip1.SetToolTip(this.txtAwaitTime, "防止加载网页过快被检测为机器人, 单位: 秒");
            // 
            // CrawlerRuleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(861, 547);
            this.Controls.Add(this.txtAwaitTime);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.btnImportRules);
            this.Controls.Add(this.cbxRuleName);
            this.Controls.Add(this.txtStartUrl);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.btnSaveRules);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtNextPagePath);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cbxChapterTitleType);
            this.Controls.Add(this.txtChapterTitlePath);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtChapterRootPath);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cbxSelectedRule);
            this.Controls.Add(this.btnSaveBookRules);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtAuthor);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbxBookName);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CrawlerRuleForm";
            this.Text = "编辑爬虫规则";
            this.Load += new System.EventHandler(this.CrawlerRuleForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbxBookName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtAuthor;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnSaveBookRules;
        private System.Windows.Forms.ComboBox cbxSelectedRule;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtChapterRootPath;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtChapterTitlePath;
        private System.Windows.Forms.ComboBox cbxChapterTitleType;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtNextPagePath;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnSaveRules;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtStartUrl;
        private System.Windows.Forms.ComboBox cbxRuleName;
        private System.Windows.Forms.Button btnImportRules;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtAwaitTime;
    }
}