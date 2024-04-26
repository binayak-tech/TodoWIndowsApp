using System;
using System.Windows.Forms;
using TodoApp;

namespace TodoDetails
{
    public partial class AddOrUpdateTodo : Form
    {
        public TodoItem TodoItem { get; set; }
        public AddOrUpdateTodo()
        {
            InitializeComponent();
            TodoItem = new TodoItem();
            txtSummary.Focus();
            cmbCategory.Items.AddRange(new string[] { "Open", "Closed", "In Progress" });
        }

        public AddOrUpdateTodo(TodoItem todoItem)
        {
            InitializeComponent();
            TodoItem = todoItem;
            txtSummary.Text = todoItem.Summary;
            txtDescription.Text = todoItem.Description;
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
            chkCompleted.Checked = todoItem.Completed;
            cmbCategory.SelectedItem = todoItem.Category;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            TodoItem.Summary = txtSummary.Text;
            TodoItem.Description = txtDescription.Text;
            if(rbHigh.Checked)
                TodoItem.Priority = Priority.High;
            else if (rbMedium.Checked) 
                TodoItem.Priority = Priority.Medium;
            else if (rbLow.Checked)
                TodoItem.Priority = Priority.Low;
            TodoItem.DueDate = dtpDueDate.Value;
            TodoItem.Completed = chkCompleted.Checked;
            TodoItem.Category = cmbCategory.SelectedItem.ToString();

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
