using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TodoApp.DTO;
using TodoApp.Entity;

namespace TodoApp
{
    /// <summary>
    /// This form is responsible for both adding and updating a Todo Item.
    /// </summary>
    public partial class AddOrUpdateView : Form
    {
        //private readonly DBManagerCodeFirst _dbManager = DBManagerCodeFirst.Instance;

        private readonly List<Category> _categoryList;
        private readonly List<Priority> _priorityList;
        public TodoItemDTO TodoItemDTO { get; set; }

        /// <summary>
        /// Initializes a new instance of the AddOrUpdateForm class.
        /// </summary>
        /// <param name="todoItem">The TodoItemDTO to edit. If null, a new TodoItem will be created.</param>
        /// <param name="categoryList">The list of categories to populate in the category ComboBox.</param>
        /// <param name="priorityList">The list of priorities to populate in the priority RadioButton group.</param>
        /// <param name="isEditing">Flag indicating whether the form is used for editing an existing item.</param>
        public AddOrUpdateView(TodoItemDTO todoItem = null, List<Category> categoryList = null, List<Priority> priorityList = null, bool isEditing = false)
        {
            InitializeComponent();
            _categoryList = categoryList ?? new List<Category>();
            _priorityList = priorityList ?? new List<Priority>();
            TodoItemDTO = todoItem ?? new TodoItemDTO();
            dtpDueDate.MinDate = DateTime.Now.Date;
            InitializeForm();
            if (isEditing)
            {
                dtpDueDate.MinDate = TodoItemDTO.DueDate;
                PopulateFormFields();
            }
            
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs())
                return;
            UpdateTodoItemDTO();
            DialogResult = DialogResult.OK;
            Close();
        }
        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

       // ------------------------------------------- HELPER METHODS ------------------------------------------------------

        /// <summary>
        /// Initializes form controls and sets up event handlers.
        /// </summary>
        private void InitializeForm()
        {
            txtSummary.Focus();
            cmbCategory.DataSource = _categoryList;
            cmbCategory.DisplayMember = "Name";
            cmbCategory.ValueMember = "Id";

            var priorities = _priorityList;
            if (priorities != null && priorities.Count >= 3)
            {
                rbHigh.Text = priorities[0].Name;
                rbMedium.Text = priorities[1].Name;
                rbLow.Text = priorities[2].Name;
            }
        }

        /// <summary>
        /// Populates form fields with data from the TodoItemDTO.
        /// </summary>
        private void PopulateFormFields()
        {
            txtSummary.Text = TodoItemDTO.Summary;
            txtDescription.Text = TodoItemDTO.Description;
            dtpDueDate.Value = TodoItemDTO.DueDate;
            chkCompleted.Checked = TodoItemDTO.Completed;

            cmbCategory.SelectedValue = TodoItemDTO.CategoryId;

            switch (TodoItemDTO.PriorityId)
            {
                case 1:
                    rbLow.Checked = true;
                    break;
                case 2:
                    rbMedium.Checked = true;
                    break;
                case 3:
                    rbHigh.Checked = true;
                    break;
            }
        }


        /// <summary>
        /// Validates user inputs before saving.
        /// </summary>
        /// <returns>True if inputs are valid, otherwise false.</returns>
        private bool ValidateInputs()
        {
            if (string.IsNullOrEmpty(txtSummary.Text))
            {
                MessageBox.Show("Summary is a required field.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSummary.Focus();
                return false;
            }

            if (cmbCategory.SelectedItem == null)
            {
                MessageBox.Show("Select a category.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbCategory.Focus();
                return false;
            }

            if (!(rbHigh.Checked || rbMedium.Checked || rbLow.Checked))
            {
                MessageBox.Show("Please set a priority.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                rbHigh.Focus();
                return false;
            }

            return true;
        }

        /// <summary>
        /// Updates the TodoItemDTO with data from form fields.
        /// </summary>
        private void UpdateTodoItemDTO()
        {
            TodoItemDTO.Summary = txtSummary.Text;
            TodoItemDTO.Description = txtDescription.Text;
            TodoItemDTO.DueDate = dtpDueDate.Value.Date;
            TodoItemDTO.Completed = chkCompleted.Checked;
            TodoItemDTO.CategoryId = (int)cmbCategory.SelectedValue;

            if (rbHigh.Checked)
                TodoItemDTO.PriorityId = 3;
            else if (rbMedium.Checked)
                TodoItemDTO.PriorityId = 2;
            else if (rbLow.Checked)
                TodoItemDTO.PriorityId = 1;
        }
    }

}
