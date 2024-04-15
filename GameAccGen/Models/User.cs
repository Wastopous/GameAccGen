namespace GameAccGen.Models;

public class User
{
    public int UserID { get; set; }
    public string? Name { get; set; }
    public string LastName { get; set; }
    public string BirthDate { get; set; }
    public string Login { get; set; }
    public int PinCode { get; set; }
}