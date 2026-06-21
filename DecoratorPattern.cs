

// Decorator, pattern Mario game example.

// Concrete, IComponent interfact.

using System;

public interface ICharacter {
    string getAbilities();
}

// Concrete classes, for IComponent.

public class Mario : ICharacter {
    public string getAbilities(){
        return "Mario";
    }
}

// IDecorator, on "is - A" relationship, saying Decorator is A Character.

public abstract class CharacterDecorator : ICharacter {
    // this is "has - A" relationship, saying Decorator has A Character.
    protected ICharacter _character;
    
    public CharacterDecorator(ICharacter character){
        _character = character;
    }
    
       public virtual string getAbilities()
    {
        return _character.getAbilities();
    }
}

// Concrete Decorators,

public class HeightUpDecorator : CharacterDecorator {
    public  HeightUpDecorator(ICharacter character) : base(character){}
    
    public override string getAbilities(){
        return _character.getAbilities() + " With Height UP!";
    }
}

// Concrete decorator.

public class GunPowerDecorator : CharacterDecorator {
    public  GunPowerDecorator(ICharacter character) : base(character){}
    
    public override string getAbilities(){
        return _character.getAbilities() + " With Gun Power!";
    }
}


public class Program {
    public static void Main(){
        
        // Create Character.
        
       ICharacter mario = new Mario();

        Console.WriteLine(mario.getAbilities());

        mario = new HeightUpDecorator(mario);
        Console.WriteLine(mario.getAbilities());

        mario = new GunPowerDecorator(mario);
        Console.WriteLine(mario.getAbilities());
    }
}
