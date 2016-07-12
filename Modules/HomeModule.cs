using System;
using System.Collections.Generic;
using Nancy;
using Nancy.ViewEngines.Razor;

namespace AnimalShelter
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ => {
        return View["index.cshtml"];
      };
      Get["/animals"] = _ => {
        List<Animal> AllAnimals = Animal.GetAll();
        return View["animals.cshtml", AllAnimals];
      };
      Get["/animals/new"] = _ => {
        List<Species> allSpecies= Species.GetAll();
        return View["animals_form.cshtml", allSpecies];
      };
      Post["/animals/new"] = _ => {
        Animal newAnimal = new Animal(Request.Form["animal-name"], Request.Form["animal-gender"], new DateTime(Request.Form["year"], Request.Form["month"], Request.Form["day"]), Request.Form["animal-breed"], Request.Form["species-id"]);
        newAnimal.Save();
        List<Animal> AllAnimals = Animal.GetAll();
        return View["animals.cshtml", AllAnimals];
      };
      Get["/species"] = _ => {
        List<Species> allSpecies = Species.GetAll();
        return View ["species.cshtml", allSpecies];
      };
      Get["/species/new"]= _ => {
        return View ["species_form.cshtml"];
      };
      Post["/species/new"]= _ =>{
        Species newSpecies = new Species(Request.Form["species"]);
        newSpecies.Save();
        return View ["species_added.cshtml"];
      };
      Post["/animals/delete"] = _ => {
        Animal.DeleteAll();
        return View ["cleared.cshtml"];
      };
      Get["/species/{id}"]= parameter =>{
        Dicitonary<string, object> model = new Dictionary<string, object>();
        var speciesAnimals = Animals.SpeciesFind(parameter.id)
        return View["species_list.cshtml"]
      }
    }
  }
}
