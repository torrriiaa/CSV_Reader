using CSV_Reader.Models;

namespace CSV_Reader.Repositories.Interfaces;

public interface IPersonRepository
{
    public Task<IEnumerable<Person>> GetAllPersonsAsync();
    public Task<Person> GetPersonByIdAsync(int id);
    public Task AddPersonsAsync(IEnumerable<Person> persons);
    public Task UpdatePersonAsync(Person person);
    public Task DeletePersonAsync(Person person);
    public Task SaveAsync();
}
