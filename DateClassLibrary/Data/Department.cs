using System.ComponentModel.DataAnnotations;

namespace DateClassLibrary.Data
{
    /// <summary>
    /// Deparment实体类，通过DepPersons与Person存在多对多关系
    /// todo将与User也存在多对多关系！！！！
    /// </summary>
    public class Department
    {
        public long Id { get; set; }
        [Display(Name = "Department Name")]
        public string? Name { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-mm-dd}", ApplyFormatInEditMode = true)]
        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow.AddHours(8);
        public ICollection<DepPerson>? DepPersons { get; set; }

    }
}
