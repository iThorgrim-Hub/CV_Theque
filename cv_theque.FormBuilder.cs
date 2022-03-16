using System.Reflection;
using System.Text;

namespace CV_Theque
{
    public partial class FormBuilder : Form
    {
        private DataGridView dgv;
        private List<CSV_Obj> ListObj;
        private void InitializeComponent()
        {
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(800, 450);

            this.Name = "Form";
            this.Text = this.Name;

            this.Load += new EventHandler(this.FormBuilder_Load);
            ResumeLayout(false);
        }

        public void SetDataGridView()
        {
            // dgv builder
            dgv = new DataGridView();

            dgv.Name = "dgv";
            dgv.Size = new Size(800, 450);
            dgv.TabIndex = 1;

            // dgv settings
            dgv.AutoGenerateColumns = false;
            dgv.AutoSize = false;
            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgv.ScrollBars = ScrollBars.Both;

            Controls.Add(dgv);
        }
        public void SetDataGridView_Column(string filename)
        {
            // We will parse our csv in order to instantiate our CSV_Obj class for each row, in order to have one object per row.
            // Then we will store the objects in a list.
            ListObj = File.ReadAllLines(filename, Encoding.GetEncoding("iso-8859-1"))
                                        .Skip(1)
                                        .Select(x => CSV_Obj.GenerateObj(x))
                                        .ToList();

            // If dgv is null then send error and stop application
            if (dgv is null)
            {

            }
            else
            {
                // We use an iterative loop to browse CSV_Obj.methodDic which contains the names of the columns of our dgv
                foreach (KeyValuePair<int, string> col in CSV_Obj.methodDic)
                {
                    // We initialize a column
                    DataGridViewColumn column = new DataGridViewTextBoxColumn();
                    // We add the name to our column
                    column.Name = col.Value.ToString();
                    // We add the column to our dgv
                    dgv.Columns.Add(column);
                }

            }

        }

        public void SetDataGridView_RowData(int rowIndex, CSV_Obj obj)
        {

            // We get all the methods of the class CSV_Obj
            foreach (var method in typeof(CSV_Obj).GetMethods())
            {
                // We initialize mi which is of type MethodInfo that will sotcker the information of our method
                MethodInfo? mi = obj.GetType().GetMethod(method.Name.ToString());
                // We check that our dgv contains a column with the same name as our method
                if (dgv.Columns.Contains(method.Name.ToString()))
                {
                    // We pass an array as argument, the contained argument is null because we just want to get a data and not assign a new value
                    // We check that the return of our method is of type List<string>

                    // If the return of our method call is not a list
                    if (mi.Invoke(obj, parameters: new string[1] { null }).GetType() != typeof(List<string>))
                    {
                        // Then we add the returned value in the corresponding cell
                        dgv.Rows[rowIndex].Cells[method.Name.ToString()].Value = mi.Invoke(obj, new string[1] { null }).ToString();
                    }
                    // If the return of our method call is a list
                    else
                    {
                        // The return of our method call being an object and not a list we will cast our object to List<string>
                        List<string>? collection = (List<string>)mi.Invoke(obj, new string[1] { null });
                        // We get all the rows contained in the string array
                        foreach (string item in collection)
                        {
                            // For each row we will update the corresponding cell, its value will be equal to its value plus the recovered row
                            dgv.Rows[rowIndex].Cells[method.Name.ToString()].Value = dgv.Rows[rowIndex].Cells[method.Name.ToString()].Value + item + "\n";
                        }
                        // We update our preferences to display the entire cell
                        // In order to get the index of our column we will simply ask our dgv to return the index of the column corresponding to the name of our method
                        dgv.Columns[dgv.Columns[method.Name.ToString()].Index].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                    }
                }

            }
        }

        public void SetDataGridView_Row()
        {
            // We use an iterative loop to browse ListObj
            foreach (CSV_Obj obj in ListObj)
            {
                // For each object in our list we will add a row and store the index of this row.
                int rowIndex = dgv.Rows.Add();
                // We call the SetDataGridView_RowData function to populate the cells of our row.
                SetDataGridView_RowData(rowIndex, obj);
            }
        }

        public void FormBuilder_Load(object? sender, EventArgs? e)
        {
            // To the loading of our form

            // We call the construction of our dgv
            SetDataGridView();
            // We call the implementation of the columns
            SetDataGridView_Column("./hrdata.csv");
            // We call the implementation of the rows
            SetDataGridView_Row();
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
