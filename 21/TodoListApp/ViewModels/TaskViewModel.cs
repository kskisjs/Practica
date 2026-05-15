using System.ComponentModel.DataAnnotations;

namespace TodoListApp.ViewModels
{
    public class TaskViewModel
    {
        [Required(ErrorMessage = "Описание задачи обязательно.")]
        [StringLength(200, ErrorMessage = "Описание не должно превышать 200 символов.")]
        [Display(Name = "Описание")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Укажите срок выполнения.")]
        [DataType(DataType.Date)]
        [Display(Name = "Срок выполнения")]
        public DateTime DueDate { get; set; } = DateTime.Today.AddDays(1);

        [Display(Name = "Выполнена")]
        public bool IsCompleted { get; set; }
    }
}
