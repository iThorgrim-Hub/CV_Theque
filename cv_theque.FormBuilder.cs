namespace CV_Theque
{
    public partial class FormBuilder : Form
    {
        private DataGridView dgv;
        private Panel mainMenuPanel;
        private Panel mainPanel;

        private List<CSV_Obj> listObj;
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // FormBuilder
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(800, 450);
            this.Name = "CV_Thèque";
            this.Text = this.Name;
            this.Load += new EventHandler(this.FormBuilder_Load);
            this.ResumeLayout(false);
        }

        public void FormBuilder_Load(object? sender, EventArgs? e)
        {
            // To the loading of our form

            // We call the construction of our dgv
            this.mainPanel = PanelBuilder.GetPanel("MainPanel", this);
            this.mainMenuPanel = PanelBuilder.GetPanel("MainMenu", this);

            this.dgv = DataGridViewBuilder.GetDgv(listObj, "./hrdata.csv", this.mainPanel);
        }

        public FormBuilder()
        {
            InitializeComponent();
        }

        //
        // Obj Generator methods
        //
    }
}
