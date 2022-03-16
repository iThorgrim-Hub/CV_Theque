using System.Reflection;
using System.Text;

namespace CV_Theque
{
    public sealed class DataGridViewBuilder : DataGridView
    {
        private static DataGridView? _dgv;
        private static List<CSV_Obj>? listObj;
        private static string? filename;
        private static Panel? parent;


        private static List<CSV_Obj> ListObj
        {
            get => listObj;
            set => listObj = value;
        }

        private static string Filename
        {
            get => filename;
            set => filename = value;
        }

        private static new Panel Parent
        {
            get => parent;
            set => parent = value;
        }

        private DataGridViewBuilder() 
        {
            // dgv builder
            _dgv = new DataGridView();

            _dgv.Name = "dgv";

            // dgv settings
            _dgv.AutoGenerateColumns = false;
            _dgv.AutoSize = false;
            _dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            _dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            _dgv.ScrollBars = ScrollBars.Both;
            _dgv.Visible = true;
            _dgv.TabIndex = 0;
            _dgv.Location = new Point(32, 18);

            this.SetDataGridView_Column();
            this.SetDataGridView_Row();

            this.SetControls();
        }

        private void SetControls()
        {
            if (Parent is not null)
            {
                _dgv.Size = Parent.ClientSize;
                _dgv.Dock = DockStyle.Right;
                Parent.Controls.Add(_dgv);
            }
        }

        public static DataGridView GetDgv(List<CSV_Obj> listObj, string filename, Panel parent)
        {
            if ( ListObj != listObj ) { ListObj = listObj; }
            if ( Filename != filename ) { Filename = filename; }
            if (Parent != parent) { Parent = parent; }

            if (_dgv == null)
            {
                _dgv = new DataGridViewBuilder();
            }
            return _dgv;
        }

        private void SetDataGridView_Column()
        {
            // We will parse our csv in order to instantiate our CSV_Obj class for each row, in order to have one object per row.
            // Then we will store the objects in a list.
            listObj = File.ReadAllLines(Filename, Encoding.GetEncoding("iso-8859-1"))
                                        .Skip(1)
                                        .Select(x => CSV_Obj.GenerateObj(x))
                                        .ToList();

            // If dgv is null then send error and stop application
            if (_dgv is null)
            {

            }
            else
            {

                // We use an iterative loop to browse CSV_Obj.methodDic which contains the names of the columns of our dgv
                foreach (KeyValuePair<int, CSV_Obj.Index> key in CSV_Obj.methodDic)
                {
                    // We initialize a column
                    DataGridViewColumn column = new DataGridViewTextBoxColumn();
                    // We add the name to our column
                    column.Name = key.Value.columnName;
                    // We add the column to our dgv
                    _dgv.Columns.Add(column);
                }
            }

        }
        private void SetDataGridView_Row()
        {
            // We use an iterative loop to browse listObj
            foreach (CSV_Obj obj in listObj)
            {
                // For each object in our list we will add a row and store the index of this row.
                int rowIndex = _dgv.Rows.Add();
                // We call the SetDataGridView_RowData function to populate the cells of our row.
                SetDataGridView_RowData(rowIndex, obj);
            }
        }

        private void SetDataGridView_RowData(int rowIndex, CSV_Obj obj)
        {

            // We get all the methods of the class CSV_Obj
            foreach (var method in typeof(CSV_Obj).GetMethods())
            {
                // We initialize mi which is of type MethodInfo that will sotcker the information of our method
                MethodInfo? mi = obj.GetType().GetMethod(method.Name.ToString());
                // We check that our dgv contains a column with the same name as our method
                if (_dgv.Columns.Contains(method.Name.ToString()))
                {
                    // We pass an array as argument, the contained argument is null because we just want to get a data and not assign a new value
                    // We check that the return of our method is of type List<string>

                    // If the return of our method call is not a list
                    if (mi.Invoke(obj, parameters: new string[1] { null }).GetType() != typeof(List<string>))
                    {
                        // Then we add the returned value in the corresponding cell
                        _dgv.Rows[rowIndex].Cells[method.Name.ToString()].Value = mi.Invoke(obj, parameters: new string[1] { null }).ToString();
                    }
                    // If the return of our method call is a list
                    else
                    {
                        // The return of our method call being an object and not a list we will cast our object to List<string>
                        List<string>? collection = (List<string>)mi.Invoke(obj, parameters: new string[1] { null });
                        // We get all the rows contained in the string array
                        foreach (string item in collection)
                        {
                            // For each row we will update the corresponding cell, its value will be equal to its value plus the recovered row
                            _dgv.Rows[rowIndex].Cells[method.Name.ToString()].Value = _dgv.Rows[rowIndex].Cells[method.Name.ToString()].Value + item + "\n";
                        }
                        // We update our preferences to display the entire cell
                        // In order to get the index of our column we will simply ask our dgv to return the index of the column corresponding to the name of our method
                        _dgv.Columns[_dgv.Columns[method.Name.ToString()].Index].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                    }
                }

            }
        }
    }
}
