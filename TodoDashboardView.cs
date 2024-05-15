using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TodoApp.DTO;
using TodoApp.Entity;
using TodoApp.Repository;
using TodoApp.ViewModel;

namespace TodoApp
{
    /// <summary>
    /// This is the main form that displays the list of todo items.
    /// </summary>
    public partial class TodoDashboardView : Form
    {
        //private readonly DBManagerCodeFirst _dbManager = DBManagerCodeFirst.Instance;
        private readonly TodoDbContext _context;
        private readonly RepositoryFactory _repositoryFactory;
        private readonly TodoItemRepository _todoItemRepository;
        private readonly CategoryRepository _categoryRepository;
        private readonly PriorityRepository _priorityRepository;

        /// <summary>
        /// Gets or sets the list of todo items cached in memory.
        /// </summary>
        private List<TodoItemDTO> _todoItemsCache = new List<TodoItemDTO>();
        public List<Category>  categoryList = new List<Category>();
        public List<Priority> priorityList = new List<Priority>();
        public TodoDashboardView()
        {
            InitializeComponent();
            
            _context = new TodoDbContext();
            _repositoryFactory = new RepositoryFactory(_context);
            _todoItemRepository = _repositoryFactory.CreateTodoItemRepository();
            _categoryRepository = _repositoryFactory.CreateCategoryRepository();
            _priorityRepository = _repositoryFactory.CreatePriorityRepository();

            deactivateEditDeleteButtons();
            InitializeDataGridView();
            LoadTodoItemsFromDatabase();
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            AddOrUpdateView todoForm = new AddOrUpdateView(null,categoryList, priorityList);
            if (todoForm.ShowDialog() == DialogResult.OK)
            {
                
                TodoItemDTO todoItemDTO = todoForm.TodoItemDTO;
                todoItemDTO.Id = _todoItemRepository.InsertItem(todoItemDTO);
                todoItemDTO = SetCategoryAndPriority(todoItemDTO);

                _todoItemsCache.Add(todoItemDTO);
                RefreshDataGridView();
            }
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                TodoItemDTO selectedTodo = dataGridView1.SelectedRows[0].DataBoundItem as TodoItemDTO;
                AddOrUpdateView todoForm = new AddOrUpdateView(selectedTodo, categoryList, priorityList, true);
                if (todoForm.ShowDialog() == DialogResult.OK)
                {
                    TodoItemDTO todoItemDTO = todoForm.TodoItemDTO;
                    todoItemDTO = SetCategoryAndPriority(todoItemDTO);

                    _todoItemRepository.UpdateItem(todoItemDTO);
                    int index = _todoItemsCache.FindIndex(t => t.Id == selectedTodo.Id);
                    if (index != -1)
                    {
                        _todoItemsCache[index] = todoForm.TodoItemDTO;
                    }
                    RefreshDataGridView();
                }
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (!ValidateSelectedRows()) return;

            List<TodoItemDTO> selectedTodos = GetSelectedTodoItems();

            DialogResult result = ShowDeleteConfirmation(selectedTodos.Count, selectedTodos[0].Summary);
            if (result == DialogResult.OK)
            {
                DeleteItemsFromDatabase(selectedTodos);
                DeleteSelectedTodoItems(selectedTodos);
                RefreshDataGridView();
                ClearTodoDetails();
            }
        }

        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0 && dataGridView1.SelectedRows[0].Index != -1)
            {
                TodoItemDTO selectedTodo = dataGridView1.SelectedRows[0].DataBoundItem as TodoItemDTO;
                DisplayTodoDetails(selectedTodo);
            }
            else
            {
                dataGridView1.ClearSelection();
                ClearTodoDetails();
            }
            editButton.Enabled = dataGridView1.SelectedRows.Count == 1;
        }

        // --------------------------------------------------- Helper Methods ------------------------------------------------

        /// <summary>
        /// It initalizes the Datagridview  control and sets appropriate column widths.
        /// </summary>
        private void InitializeDataGridView()
        {
            dataGridView1.Columns[0].Width = (int)(dataGridView1.Width * 0.3);
            for (int i = 1; i < dataGridView1.Columns.Count; i++)
            {
                dataGridView1.Columns[i].Width = (int)(dataGridView1.Width * 0.175);
            }
        }

        /// <summary>
        /// Responsible getting the data from db manager and saving it in cache
        /// The cached todo items data then used as a data source for data grid view
        /// It also responsible for checking if grid is empty or not.
        /// </summary>
        private void LoadTodoItemsFromDatabase()
        {
            _todoItemsCache = _todoItemRepository.GetTodoList();
            categoryList = _categoryRepository.GetAllCategories();
            priorityList = _priorityRepository.GetAllPriorities();

            foreach (var todoItem in _todoItemsCache)
            {
                SetCategoryAndPriority(todoItem);
            }

            dataGridView1.DataSource = _todoItemsCache;
            dataGridView1.Refresh();
            if (_todoItemsCache.Count == 0)
            {
                deactivateEditDeleteButtons();
            }
            else
            {
                activateEditDeleteButtons();
            }
        }

        /// <summary>
        /// Sets category and priority name filed values in the todoItem object
        /// </summary>
        /// <param name="todoItem">TodoItemDTO object</param>
        /// <returns>TodoItemDTO object</returns>
        private TodoItemDTO SetCategoryAndPriority(TodoItemDTO todoItem)
        {
            todoItem.Category = categoryList.FirstOrDefault(c => c.Id == todoItem.CategoryId)?.Name;
            todoItem.Priority = priorityList.FirstOrDefault(p => p.Id == todoItem.PriorityId)?.Name;
            return todoItem;
        }
   

        /// <summary>
        /// This mthod is called when a row in datagrid view is selected,
        /// It displays the related information in the display pannel by setting individual field values.
        /// </summary>
        /// <param name="todoItemDto"> todoItemDto contains all the information related to selected row</param>
        private void DisplayTodoDetails(TodoItemDTO todoItemDto)
        {
            txtSummary.Text = todoItemDto.Summary;
            txtDescription.Text = todoItemDto.Description;
            txtDueDate.Text = todoItemDto.DueDate.Date.ToString("dd/MM/yyyy");
            txtPriority.Text = todoItemDto.Priority;
            txtCategory.Text = todoItemDto.Category;
            if (todoItemDto.Completed) txtCompleted.Text = "✔️";
            else txtCompleted.Text = "✖️";

            ResizeDecriptionBox();
            ResizeSummaryBox();
        }

        /// <summary>
        /// Checks if any row is selected or not, if not then displays a warning message to user.
        /// </summary>
        /// <returns> Returns true if the selected row count is 1 or more else returns false </returns>
        private bool ValidateSelectedRows()
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an item to delete.", "No Item Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Runs a loop and saves the data about selected rows in datagridview into an object
        /// </summary>
        /// <returns>Returns a object containing selected row data of datagrid view<returns>
        private List<TodoItemDTO> GetSelectedTodoItems()
        {
            List<TodoItemDTO> selectedTodos = new List<TodoItemDTO>();
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                selectedTodos.Add(row.DataBoundItem as TodoItemDTO);
            }
            return selectedTodos;
        }

        /// <summary>
        /// Shows Message box for delete button click event.
        /// </summary>
        /// <param name="selectedCount"></param>
        /// <param name="summary"></param>
        /// <returns></returns>
        private DialogResult ShowDeleteConfirmation(int selectedCount, string summary)
        {
            // Truncate the summary if its length is more than 20 characters
            if (summary.Length > 20)
                summary = summary.Substring(0, 20) + "...";
            
            if (selectedCount == 1)
                return MessageBox.Show($"Are you sure you want to delete '{summary}'?", "Confirm Delete", MessageBoxButtons.OKCancel);
            
            return MessageBox.Show($"Are you sure you want to delete {selectedCount} item(s)?", "Confirm Delete", MessageBoxButtons.OKCancel);
        }
        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "completedColumn")
            {
                if ((bool)e.Value)
                    e.Value = "✔️";
                else
                    e.Value = "✖️";

                dataGridView1.Columns[e.ColumnIndex].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
        }

        /// <summary>
        /// Removes selected items from the cache upon deletion
        /// </summary>
        /// <param name="selectedTodos"></param>
        private void DeleteSelectedTodoItems(List<TodoItemDTO> selectedTodos)
        {
            foreach (var todo in selectedTodos)
            {
                _todoItemsCache.Remove(todo);
            }
        }

        /// <summary>
        /// Interacts with the database to delete the selected todo items
        /// </summary>
        /// <param name="selectedTodos"></param>
        private void DeleteItemsFromDatabase(List<TodoItemDTO> selectedTodos)
        {
            if (selectedTodos.Count == 1)
                _todoItemRepository.DeleteItem(selectedTodos[0].Id);
            else 
            { 
                List<int> selectedIds = selectedTodos.Select(todo => todo.Id).ToList();
                string idList = string.Join(",", selectedIds);
                _todoItemRepository.DeleteItems(selectedIds);
            }
        }

        /// <summary>
        /// resizes description box depending on the content length.
        /// </summary>
        private void ResizeDecriptionBox()
        {
            int length = txtDescription.Text.Length;
            int lines = Math.Max((int)Math.Ceiling(length / 32.0), txtDescription.Text.Split('\n').Length) + 1;

            if (length < 32)
            {
                txtDescription.Multiline = false;
                txtDescription.ScrollBars = ScrollBars.None;
                txtDescription.Height = txtDescription.Font.Height;
            }
            else if (lines > 5)
            {
                txtDescription.Multiline = true;
                txtDescription.ScrollBars = ScrollBars.Vertical;
                txtDescription.Height = txtDescription.Font.Height * 5;
            }
            else if (lines <= 5)
            {
                txtDescription.Multiline = true;
                txtDescription.Height = txtDescription.Font.Height * lines;
                txtDescription.ScrollBars = ScrollBars.None;
            }
        }
        /// <summary>
        /// resizes summary box depending on the content length.
        /// </summary>
        private void ResizeSummaryBox()
        {
            int length = txtSummary.Text.Length;
            int lines = (int)Math.Ceiling(length / 30.0);
            if (length > 34 )
            {
                txtSummary.Multiline = true;
                txtSummary.Height = txtDescription.Font.Height * lines;
            }
            else
            {
                txtSummary.Multiline = false;
                txtSummary.ScrollBars = ScrollBars.None;
                txtSummary.Height = txtDescription.Font.Height;
            }
        }

        /// <summary>
        /// Clears field displaying todo item data and set it empty strings.
        /// </summary>
        private void ClearTodoDetails()
        {
            txtSummary.Clear();
            txtDescription.Clear();
            txtDueDate.Text = "";
            txtPriority.Text = "";
            txtCompleted.Text = "";
            txtCategory.Text = "";
            dataGridView1.ClearSelection();
        }

        /// <summary>
        /// Enables edit and delete buttons
        /// </summary>
        public void activateEditDeleteButtons()
        {
            editButton.Enabled = true;
            deleteButton.Enabled = true;
        }
        /// <summary>
        /// Disables the Edit and Delete buttons
        /// </summary>
        public void deactivateEditDeleteButtons()
        {
            editButton.Enabled = false;
            deleteButton.Enabled = false;
        }
        /// <summary>
        /// Refreshes the datagridview to get changes reflected
        /// </summary>
        public void RefreshDataGridView()
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = _todoItemsCache;
            dataGridView1.Refresh();
        }
    }
}
