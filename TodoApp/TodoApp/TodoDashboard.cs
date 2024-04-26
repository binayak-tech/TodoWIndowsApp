using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace TodoApp
{
    public partial class TodoDashboard : Form
    {
        BindingList<TodoItem> myBind = new BindingList<TodoItem>();
        private DbManager _dbManager;

        public TodoDashboard()
        {
            InitializeComponent();
            _dbManager = new DbManager();
            LoadTodoItemsFromDatabase();
            dataGridView1.DataSource = myBind;
            dataGridView1.Refresh();
            dataGridView1.SelectionChanged += DataGridView1_SelectionChanged;

            dataGridView1.Columns[0].Width = (int)(dataGridView1.Width * 0.4);
            for (int i = 1; i < dataGridView1.Columns.Count; i++)
            {
                dataGridView1.Columns[i].Width = (int)(dataGridView1.Width * 0.15);
            }

        }

        private void LoadTodoItemsFromDatabase()
        {
            _dbManager.OpenConnection();
            DataTable dt = _dbManager.ExecuteQuery("SELECT * FROM TodoItems");
            _dbManager.CloseConnection();

            myBind.Clear();

            foreach (DataRow row in dt.Rows)
            {
                TodoItem todoItem = new TodoItem
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Summary = row["Summary"].ToString(),
                    Description = row["Description"].ToString(),
                    Priority = (Priority)Enum.Parse(typeof(Priority), row["Priority"].ToString()),
                    DueDate = Convert.ToDateTime(row["DueDate"]),
                    Completed = (bool)row["Completed"],
                    Category = row["Category"].ToString()
                };

                myBind.Add(todoItem);
            }
        }

        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0 && dataGridView1.SelectedRows[0].Index != -1)
            {
                TodoItem selectedTodo = dataGridView1.SelectedRows[0].DataBoundItem as TodoItem;
                DisplayTodoDetails(selectedTodo);
            }
            else
            {
                ClearTodoDetails();
            }
        }

        private void DisplayTodoDetails(TodoItem todoItem)
        {
            txtSummary.Text = todoItem.Summary;
            txtDescription.Text = todoItem.Description;
            dtpDueDate.Value = todoItem.DueDate;
            switch (todoItem.Priority)
            {
                case Priority.High:
                    rbHigh.Checked = true;
                    break;
                case Priority.Medium:
                    rbMedium.Checked = true;
                    break;
                case Priority.Low:
                    rbLow.Checked = true;
                    break;
            }
            chkCompleted.Checked = todoItem.Completed;
            cmbCategory.Text = todoItem.Category;
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            AddOrUpdateForm todoForm = new AddOrUpdateForm();
            if (todoForm.ShowDialog() == DialogResult.OK)
            {
                // Reload the data from the database
                LoadTodoItemsFromDatabase();
                dataGridView1.Refresh();
            }
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                TodoItem selectedTodo = dataGridView1.SelectedRows[0].DataBoundItem as TodoItem;
                AddOrUpdateForm todoForm = new AddOrUpdateForm(selectedTodo);
                if (todoForm.ShowDialog() == DialogResult.OK)
                {
                    // Reloading the data from the database
                    LoadTodoItemsFromDatabase();
                    dataGridView1.Refresh();
                }
            }
            else
                MessageBox.Show("Please select an item to edit.", "No Item Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            int selectedCount = dataGridView1.SelectedRows.Count;

            if (selectedCount > 0)
            {
                string message;
                if (selectedCount == 1)
                {
                    TodoItem selectedItem = dataGridView1.SelectedRows[0].DataBoundItem as TodoItem;
                    int length = Math.Min(selectedItem.Summary.Length, 20);
                    message = $"Are you sure you want to delete '{selectedItem.Summary.Substring(0,length)}'?";
                }
                else
                {
                    message = $"Are you sure you want to delete {selectedCount} items?";
                }

                DialogResult result = MessageBox.Show(message, "Confirm Delete", MessageBoxButtons.OKCancel);
                if (result == DialogResult.OK)
                {
                    foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                    {
                        TodoItem selectedTodo = row.DataBoundItem as TodoItem;
                        SqlParameter[] parameter = { new SqlParameter("@Id", selectedTodo.Id) };
                        _dbManager.OpenConnection();
                        _dbManager.ExecuteNonQuery("DELETE FROM TodoItems WHERE Id = @Id", parameter);
                        _dbManager.CloseConnection();
                    }

                    LoadTodoItemsFromDatabase();
                    dataGridView1.Refresh();
                    ClearTodoDetails();
                }
            }
            else
            {
                MessageBox.Show("Please select an item to delete.", "No Item Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ClearTodoDetails()
        {
            txtSummary.Clear();
            txtDescription.Clear();
            dtpDueDate.Value = DateTime.Now;
            rbHigh.Checked = false;
            rbMedium.Checked = false;
            rbLow.Checked = false;
            chkCompleted.Checked = false;
            cmbCategory.SelectedIndex = -1;
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "completedDataGridViewCheckBoxColumn")
            {
                if ((bool)e.Value)
                {
                    e.Value = "✔️";
                }
                else
                {
                    e.Value = "✖️";
                }
            }
        }

    }
}
