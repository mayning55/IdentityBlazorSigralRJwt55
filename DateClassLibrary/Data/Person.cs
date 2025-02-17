namespace DateClassLibrary.Data
{
    /// <summary>
    /// Person实体类，通过DepPersons与Department存在多对多关系
    /// </summary>
    public class Person
    {
        public long Id { get; set; }
        public string Number { get; set; }
        public string FirstName { get; set; }
        public ICollection<DepPerson>? DepPersons { get; set; }
    }
}
