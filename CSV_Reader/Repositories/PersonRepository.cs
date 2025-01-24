using CSV_Reader.Data;
using CSV_Reader.Models;
using CSV_Reader.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CSV_Reader.Repositories;

public class PersonRepository : IPersonRepository
{
    private readonly CsvPersonDbContext _context;

    public PersonRepository(CsvPersonDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Person>> GetAllPersonsAsync()
    {
        return await _context.Persons.ToListAsync();
    }

    public async Task<Person> GetPersonByIdAsync(int id)
    {
        return await _context.Persons.FindAsync(id);
    }

    public async Task AddPersonsAsync(IEnumerable<Person> persons)
    {
        await _context.Persons.AddRangeAsync(persons);
    }

    public async Task UpdatePersonAsync(Person person)
    {
        _context.Persons.Update(person);
    }

    public async Task DeletePersonAsync(Person person)
    {
        _context.Persons.Remove(person);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}
