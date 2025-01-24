namespace CSV_Reader.Models;

public class Person
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public bool IsMarried { get; set; } 
    public required string Phone {  get; set; }
    public decimal Salary { get; set; }
}
