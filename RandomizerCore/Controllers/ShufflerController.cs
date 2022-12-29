using RandomizerCore.Randomizer;

namespace RandomizerCore.Controllers;

/*
 * Changing the randomizer over to a MVC system since it allows us to have less coupling.
 * Having the controllers be the insertion point for the UI and CLI removes hard dependencies on code and lets us
 * encapsulate things better. It also helps force better code practice and makes adding code features easier.
 *
 * Insertion point for the Shuffler
 */
public class ShufflerController
{
    private Shuffler _shuffler;
    
    public ShufflerController()
    {
        _shuffler = new Shuffler();
    }
}