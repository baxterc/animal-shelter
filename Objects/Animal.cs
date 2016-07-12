using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace AnimalShelter
{
  public class Animal
  {
    private int _id;
    private string _name;
    private string _gender;
    private DateTime _dateOfAdmission;
    private string _breed;
    private int _speciesId;

    public Animal(string Name, string Gender, DateTime DateOfAdmission, string Breed, int SpeciesId, int Id = 0)
    {
      _id = Id;
      _name = Name;
      _gender = Gender;
      _dateOfAdmission = DateOfAdmission;
      _breed = Breed;
      _speciesId = SpeciesId;
    }

    public override bool Equals(System.Object otherAnimal)
    {
      if (!(otherAnimal is Animal))
      {
        return false;
      }
      else
      {
        Animal newAnimal = (Animal) otherAnimal;
        bool idEquality = (this.GetId() == newAnimal.GetId());
        bool nameEquality = (this.GetName() == newAnimal.GetName());
        bool genderEquality = (this.GetGender() == newAnimal.GetGender());
        bool dateOfAdmissionEquality = (this.GetDateOfAdmission() == newAnimal.GetDateOfAdmission());
        bool breedEquality = (this.GetBreed() == newAnimal.GetBreed());
        bool speciesEquality = (this.GetSpeciesId() == newAnimal.GetSpeciesId());
        return (idEquality && nameEquality && genderEquality && dateOfAdmissionEquality && breedEquality && speciesEquality);
      }
    }
    public int GetId()
    {
      return _id;
    }
    public string GetName()
    {
      return _name;
    }
    public void SetName(string newName)
    {
      _name = newName;
    }
    public string GetGender()
    {
      return _gender;
    }
    public void SetGender(string newGender)
    {
      _gender = newGender;
    }
    public DateTime GetDateOfAdmission()
    {
      return _dateOfAdmission;
    }
    public void SetDateOfAdmission(DateTime newDateOfAdmission)
    {
      _dateOfAdmission = newDateOfAdmission;
    }
    public string GetBreed()
    {
      return _breed;
    }
    public void SetBreed(string newBreed)
    {
      _breed = newBreed;
    }
    public int GetSpeciesId()
    {
      return _speciesId;
    }
    public void SetSpeciesId(int newSpeciesId)
    {
      _speciesId = newSpeciesId;
    }
    public static List<Animal> GetAll()
    {
      List<Animal> allAnimals = new List<Animal>{};
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM animals;", conn);
      rdr = cmd.ExecuteReader();

    while(rdr.Read())
    {
      int animalId = rdr.GetInt32(0);
      string animalName = rdr.GetString(1);
      string animalGender = rdr.GetString(2);
      DateTime animalDateofAdmission = rdr.GetDateTime(3);
      string animalBreed = rdr.GetString(4);
      int animalSpeciesId = rdr.GetInt32(5);

      Animal newAnimal = new Animal(animalName, animalGender, animalDateofAdmission, animalBreed,animalSpeciesId, animalId);
      allAnimals.Add(newAnimal);
    }
    if (rdr != null)
    {
      rdr.Close();
    }
    if (conn != null)
    {
      conn.Close();
    }
    return allAnimals;
    }
    public void Save()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO animals (name, gender, dateofadmission, breed, speciesid) OUTPUT INSERTED.id VALUES (@AnimalName, @AnimalGender, @AnimalDateOfAdmission, @AnimalBreed, @AnimalSpeciesId);", conn);

      SqlParameter nameParameter = new SqlParameter();
      nameParameter.ParameterName = "@AnimalName";
      nameParameter.Value = this.GetName();

      SqlParameter genderParameter = new SqlParameter();
      genderParameter.ParameterName = "@AnimalGender";
      genderParameter.Value = this.GetGender();

      SqlParameter dateofadmissionParameter = new SqlParameter();
      dateofadmissionParameter.ParameterName ="@AnimalDateOfAdmission";
      dateofadmissionParameter.Value = this.GetDateOfAdmission();

      SqlParameter breedParameter = new SqlParameter();
      breedParameter.ParameterName ="@AnimalBreed";
      breedParameter.Value = this.GetBreed();


      SqlParameter speciesIdParameter = new SqlParameter();
      speciesIdParameter.ParameterName ="@AnimalSpeciesId";
      speciesIdParameter.Value = this.GetSpeciesId();

      cmd.Parameters.Add(nameParameter);
      cmd.Parameters.Add(genderParameter);
      cmd.Parameters.Add(dateofadmissionParameter);
      cmd.Parameters.Add(breedParameter);
      cmd.Parameters.Add(speciesIdParameter);

      rdr = cmd.ExecuteReader();
      while (rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }
    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM animals;", conn);
      cmd.ExecuteNonQuery();
    }
    public static Animal Find(int id)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();
      SqlCommand cmd = new SqlCommand("SELECT * FROM animals WHERE id = @AnimalId;", conn);
      SqlParameter idParameter = new SqlParameter();
      idParameter.ParameterName = "@AnimalId";
      idParameter.Value = id.ToString();
      cmd.Parameters.Add(idParameter);
      rdr = cmd.ExecuteReader();
      int foundAnimalId = 0;
      string foundAnimalName = null;
      string foundAnimalGender = null;
      DateTime foundDateOfAdmission = DateTime.MinValue;
      string foundAnimalBreed = null;
      int foundSpeciesId = 0;
      while (rdr.Read())
      {
        foundAnimalId = rdr.GetInt32(0);
        foundAnimalName = rdr.GetString(1);
        foundAnimalGender = rdr.GetString(2);
        foundDateOfAdmission = rdr.GetDateTime(3);
        foundAnimalBreed = rdr.GetString(4);
        foundSpeciesId = rdr.GetInt32(5);
      }
      Animal foundAnimal = new Animal(foundAnimalName, foundAnimalGender,  foundDateOfAdmission, foundAnimalBreed, foundSpeciesId, foundAnimalId);
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundAnimal;
    }
  }
}
