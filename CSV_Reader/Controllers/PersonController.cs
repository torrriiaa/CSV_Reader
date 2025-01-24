using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CSV_Reader.Models;
using CSV_Reader.Repositories.Interfaces;
using System.Globalization;

namespace CSV_Reader.Controllers;

public class PersonController : Controller
{
    private readonly IPersonRepository _personRepository;

    public PersonController(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    public async Task<IActionResult> Index()
    {
        var persons = await _personRepository.GetAllPersonsAsync();
        return View(persons);
    }

    public IActionResult Upload()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Файл не завантажено");

        using var stream = new StreamReader(file.OpenReadStream());
        var csvData = new List<Person>();

        while (!stream.EndOfStream)
        {
            var data = await stream.ReadToEndAsync();
            var lines = data.Split('\n');
            for (int i = 0; i < lines.Length - 1; i++)
            {
                var values = lines[i].Split(',', ';');
                var person = new Person
                {
                    Name = values[0],
                    DateOfBirth = DateOnly.Parse(values[1], CultureInfo.InvariantCulture),
                    IsMarried = bool.Parse(values[2]),
                    Phone = values[3],
                    Salary = decimal.Parse(values[4])
                };
                csvData.Add(person);
            }
        }

        await _personRepository.AddPersonsAsync(csvData);
        await _personRepository.SaveAsync();
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var person = await _personRepository.GetPersonByIdAsync(id.Value);
        if (person == null) return NotFound();

        return View(person);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,DateOfBirth,IsMarried,Phone,Salary")] Person person)
    {
        if (id != person.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                await _personRepository.UpdatePersonAsync(person);
                await _personRepository.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if ((await _personRepository.GetPersonByIdAsync(person.Id)) == null)
                    return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(person);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var person = await _personRepository.GetPersonByIdAsync(id.Value);
        if (person == null) return NotFound();

        return View(person);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var person = await _personRepository.GetPersonByIdAsync(id);
        if (person != null)
        {
            await _personRepository.DeletePersonAsync(person);
            await _personRepository.SaveAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}
