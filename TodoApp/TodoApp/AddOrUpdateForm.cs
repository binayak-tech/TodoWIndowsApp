using System;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace TodoApp
{
    public partial class AddOrUpdateForm : Form
    {
        public TodoItem TodoItem { get; set; }
        private DbManager _dbManager;
        private bool isEditing;
        public AddOrUpdateForm()
        {
            InitializeComponent();
            _dbManager = new DbManager();
            TodoItem = new TodoItem();
            txtSummary.Focus();
            cmbCategory.Items.AddRange(new string[] { "Open", "Closed", "In Progress" });
            isEditing = false;
            dtpDueDate.MinDate = DateTime.Now.Date;
        }

        public AddOrUpdateForm(TodoItem todoItem)
        {
            InitializeComponent();
            _dbManager = new DbManager();
            TodoItem = todoItem;
            dtpDueDate.MinDate = DateTime.Now.Date;

            txtSummary.Text = todoItem.Summary;
            txtDescription.Text = todoItem.Description;
            cmbCategory.Items.AddRange(new string[] { "Open", "Closed", "In Progress" });
            if (cmbCategory.Items.Contains(todoItem.Category))
            {
                cmbCategory.SelectedItem = todoItem.Category;
            }
            else
            {
                cmbCategory.SelectedIndex = -1;
            }

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
            dtpDueDate.Value = todoItem.DueDate;
            chkCompleted.Checked = TodoItem.Completed;

            Text = "Edit To Do Item";
            isEditing = true;
        }

        private void saveButton_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSummary.Text))
            {
                MessageBox.Show("Summary is a required fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSummary.Focus();
                return;
            }
            if (cmbCategory.SelectedItem == null) 
            {
                MessageBox.Show("Select a category.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbCategory.Focus();
                return;
            }
            if (!(rbHigh.Checked || rbMedium.Checked || rbLow.Checked))
            {
                MessageBox.Show("Please set a priority.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                rbHigh.Focus();
                return;
            }

            if(dtpDueDate.Value.Date < DateTime.Now.Date)
            {
                MessageBox.Show("Select a future date!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            TodoItem.Summary = txtSummary.Text;
            TodoItem.Description = txtDescription.Text;
            if (rbHigh.Checked)
                TodoItem.Priority = Priority.High;
            else if (rbMedium.Checked)
                TodoItem.Priority = Priority.Medium;
            else if (rbLow.Checked)
                TodoItem.Priority = Priority.Low;
            TodoItem.DueDate = dtpDueDate.Value.Date;
            TodoItem.Completed = chkCompleted.Checked;

            if (cmbCategory.SelectedItem != null)
            {
                TodoItem.Category = cmbCategory.SelectedItem.ToString();
            }


            SqlParameter[] parameters =
            {
               new SqlParameter("@Summary", TodoItem.Summary),
               new SqlParameter("@Description", TodoItem.Description),
               new SqlParameter("@Category", TodoItem.Category),
               new SqlParameter("@Priority", TodoItem.Priority.ToString()),
               new SqlParameter("@DueDate", TodoItem.DueDate),
               new SqlParameter("@Completed", TodoItem.Completed),
            };

            if (!isEditing)
            {
                string query = "INSERT INTO TodoItems (Summary, Description, Category, Priority, DueDate, Completed) VALUES (@Summary, @Description, @Category, @Priority, @DueDate, @Completed)";
                _dbManager.OpenConnection();
                _dbManager.ExecuteNonQuery(query, parameters);
                _dbManager.CloseConnection();
            }
            else
            {
                parameters = parameters.Append(new SqlParameter("@Id", TodoItem.Id)).ToArray();
                string query = "UPDATE TodoItems SET Summary = @Summary, Description = @Description, Category = @Category, Priority = @Priority, DueDate = @DueDate, Completed = @Completed WHERE Id=@Id";
                _dbManager.OpenConnection();
                _dbManager.ExecuteNonQuery(query, parameters);
                _dbManager.CloseConnection();
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
