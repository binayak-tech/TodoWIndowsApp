using System;

namespace TodoApp
{
    public class TodoItem
    {
        public int Id { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public Priority Priority { get; set; }
        public DateTime DueDate { get; set; }
        public bool Completed { get; set; }
        public string Category { get; set; }
    }

    public enum Priority
    {
        //Low = 1,
        //Medium = 2,
        //High = 3
        Low, Medium, High
    }
}
