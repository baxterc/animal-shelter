using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Xunit;

namespace AnimalShelter
{
  public class SpeciesTest : IDisposable
  {
    public SpeciesTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=shelter_test;Integrated Security=SSPI;";
    }
    [Fact]
    public void Test_CategoriesEmptyAtFirst()
    {
      int result = Species.GetAll().Count;
      Assert.Equal(0, result);
    }
    [Fact]
    public void Test_Equal_ReturnsTrueForSameName()
    {
      Species firstSpecies = new Species("Dog");
      Species secondSpecies = new Species("Dog");
      Assert.Equal(firstSpecies, secondSpecies);
    }
    [Fact]
    public void Test_Save_SavesSpeciesToDatabase()
    {
      Species testSpecies = new Species("Dog");
      testSpecies.Save();
      List<Species> result = Species.GetAll();
      List<Species> testList = new List<Species>{testSpecies};
      Assert.Equal(testList, result);
    }
    [Fact]
    public void Test_Save_AssignsIdToSpeciesObject()
    {
      Species testSpecies = new Species("Dog");
      testSpecies.Save();
      Species savedSpecies = Species.GetAll()[0];
      int result = savedSpecies.GetId();
      int testId = testSpecies.GetId();
      Assert.Equal (testId, result);
    }
    [Fact]
    public void Test_Find_FindsSpeciesInDatabase()
    {
      Species testSpecies = new Species("Dog");
      testSpecies.Save();
      Species foundSpecies = Species.Find(testSpecies.GetId());
      Assert.Equal(testSpecies, foundSpecies);
    }
    public void Dispose()
    {
      Animal.DeleteAll();
      Species.DeleteAll();
    }
  }
}
