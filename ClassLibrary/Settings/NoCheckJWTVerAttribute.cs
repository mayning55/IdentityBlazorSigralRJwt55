namespace ClassLibrary.Settings;

[AttributeUsage(AttributeTargets.Method)]//限定应用范围为方法
public class NoCheckJWTVerAttribute : Attribute
{
}
