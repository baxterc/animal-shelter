using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace AnimalShelter
  {
  public class Species
  {
    private int _id;
    private string _name;

    public Species(string Name, int Id = 0)
    {
      _id= Id;
      _name = Name;
    }
    public override bool Equals(System.Object otherSpecies)
    {
      if (!(otherSpecies is Species))
      {
        return false;
      }
      else
      {
        Species newSpecies = (Species) otherSpecies;
        bool idEquality = this.GetId() == newSpecies.GetId();
        bool nameEquality = this.GetName() == newSpecies.GetName();
        return (idEquality && nameEquality);
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
    public static List<Species> GetAll()
    {
      List<Species> allSpecies = new List<Species>{};

      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM species", conn);
      rdr = cmd.ExecuteReader();

      while (rdr.Read())
      {
        int speciesId = rdr.GetInt32(0);
        string speciesName = rdr.GetString(1);
        Species newSpecies = new Species(speciesName, speciesId);
        allSpecies.Add(newSpecies);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return allSpecies;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO species (name) OUTPUT INSERTED.id VALUES (@SpeciesName);", conn);

      SqlParameter nameParameter = new SqlParameter();
      nameParameter.ParameterName = "@SpeciesName";
      nameParameter.Value = this.GetName();
      cmd.Parameters.Add(nameParameter);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }
    }
    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM species;", conn);
      cmd.ExecuteNonQuery();
    }
    public static Species Find(int id)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM species WHERE id = @SpeciesId;", conn);
      SqlParameter speciesIdParameter = new SqlParameter();
      speciesIdParameter.ParameterName = "SpeciesId";
      speciesIdParameter.Value = id.ToString();
      cmd.Parameters.Add(speciesIdParameter);
      rdr = cmd.ExecuteReader();

      int foundSpeciesId = 0;
      string foundSpeciesName = null;

      while(rdr.Read())
      {
        foundSpeciesId = rdr.GetInt32(0);
        foundSpeciesName = rdr.GetString(1);
      }
      Species foundSpecies = new Species(foundSpeciesName, foundSpeciesId);
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundSpecies;
    }
  }
}
