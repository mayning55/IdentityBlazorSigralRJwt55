namespace DateClassLibrary.Data
{
    /// <summary>
    /// 两个实体之间存在多对多关系。
    /// </summary>
    public class DepPerson
    {
        public long Id { get; set; }
        public long PersonId { get; set; }
        public long DepartmentId { get; set; }
        public Department? Department { get; set; }
        public Person? Person { get; set; }
    }
}
