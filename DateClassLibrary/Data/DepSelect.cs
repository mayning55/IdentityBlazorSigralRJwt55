namespace DateClassLibrary.Data;

/// <summary>
/// 用于编辑用户时，可以选择多个部门。这里只实现用户选择部门，同样部门也可以选择用户，但没做。
/// </summary>
public class DepSelect
{
    public long Id { get; set; }
    public string DepartmentName { get; set; }
    public bool IsSelect { get; set; }

}
