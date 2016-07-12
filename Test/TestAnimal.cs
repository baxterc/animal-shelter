using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Xunit;

namespace AnimalShelter
{
  public class AnimalShelterTest : IDisposable
  {
    public AnimalShelterTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=shelter_test;Integrated Security=SSPI;";
    }
    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
      int result = Animal.GetAll().Count;
      Assert.Equal(0, result);
    }
    [Fact]
    public void Test_Equal_ReturnsTrueIfNamesAreTheSame()
    {
      Animal firstAnimal = new Animal("Fred", "male", new DateTime(2016, 7, 7), "Labrador", 1);
      Animal secondAnimal = new Animal("Fred", "male", new DateTime(2016, 7, 7), "Labrador", 1);
      Assert.Equal(firstAnimal, secondAnimal);
    }
    [Fact]
    public void Test_Save_SavesToDatabase()
    {
      Animal testAnimal = new Animal("Fred", "male", new DateTime(2016, 7, 7), "Labrador", 1);
      testAnimal.Save();
      List<Animal> result = Animal.GetAll();
      List<Animal> testList = new List<Animal>{testAnimal};
      Assert.Equal(testList, result);
    }
    [Fact]
    public void Test_Save_AssignsIdToObjects()
    {
      Animal testAnimal = new Animal("Fred", "male", new DateTime(2016, 7, 7), "Labrador", 1);
      testAnimal.Save();
      Animal savedAnimal = Animal.GetAll()[0];
      int result = savedAnimal.GetId();
      int testId = testAnimal.GetId();
      Assert.Equal(testId, result);
    }
    [Fact]
    public void Test_Find_FindsAnimalInDatabase()
    {
      Animal testAnimal = new Animal("Fred", "male", new DateTime(2016, 7, 7), "Labrador", 1);
      testAnimal.Save();
      Animal foundAnimal = Animal.Find(testAnimal.GetId());
      Console.WriteLine("ID of animal at entry: " + testAnimal.GetId());
      Console.WriteLine("ID of animal in DB: " + foundAnimal.GetId());

      Assert.Equal(testAnimal, foundAnimal);
    }
    public void Dispose()
    {
      Animal.DeleteAll();
    }
  }
}
