using System.Reflection;
using System.Text;

namespace CV_Theque
{
    public partial class Form1 : Form
    {
        private List<CSV_Obj> myList = new List<CSV_Obj>();

        public void GenerateDataGridView()
        {
            dataGridView1 = new DataGridView();

            dataGridView1.Location = new Point(366, 12);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(422, 426);
            dataGridView1.TabIndex = 1;

            Controls.Add(dataGridView1);
        }

        public Form1()
        {
            GenerateDataGridView();
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            if (!File.Exists("./hrdata.csv"))
            {

            }
            else
            {
                List<CSV_Obj> ListObj = File.ReadAllLines("./hrdata.csv", Encoding.GetEncoding("iso-8859-1"))
                                            .Skip(1)
                                            .Select(x => CSV_Obj.GenerateObj(x))
                                            .ToList();

                string[] arr = new string[1] { null };


                foreach (KeyValuePair<int, string> col in CSV_Obj.methodDic)
                {
                    DataGridViewColumn column = new DataGridViewTextBoxColumn();
                    column.Name = col.Value.ToString();
                    dataGridView1.Columns.Add(column);
                }

                foreach (CSV_Obj obj in ListObj)
                {
                    // On ajoute une ligne à notre dataGridView
                    var RowIndex = dataGridView1.Rows.Add();
                    // On récupère tout les methodes de la class CSV_Obj
                    foreach (var method in typeof(CSV_Obj).GetMethods())
                    {
                        // On initialise mi qui est de type MethodInfo qui va sotcker les information de notre method
                        MethodInfo? mi = obj.GetType().GetMethod(method.Name.ToString());
                        // Si la colonne du même nom de la method existe
                        if (dataGridView1.Columns.Contains(method.Name.ToString()))
                        {
                            string value = "";
                            // Si la valeur retourné est de type List<string>
                            if (mi.Invoke(obj, arr).GetType() == typeof(List<string>))
                            {
                                // Je cast mon objet en List<string>
                                List<string> collection = (List<string>)mi.Invoke(obj, arr);
                                foreach (string item in collection)
                                {
                                    dataGridView1.Rows[RowIndex].Cells[method.Name.ToString()].Value = dataGridView1.Rows[RowIndex].Cells[method.Name.ToString()].Value + item + "\n";
                                }
                                dataGridView1.Columns[dataGridView1.Columns[method.Name.ToString()].Index].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                            }
                            else
                            {
                                dataGridView1.Rows[RowIndex].Cells[method.Name.ToString()].Value = mi.Invoke(obj, arr).ToString();
                            }
                        }

                    }
                }

                dataGridView1.AutoGenerateColumns = false;
                dataGridView1.AutoSize = false;
                dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dataGridView1.ScrollBars = ScrollBars.Both;
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private DataGridView dataGridView1;
    }

}



