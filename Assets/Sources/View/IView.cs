using Entitas;
using Entitas.Generics;

public interface IView
{
    void InitializeView(Contexts contexts, IEntity entity);
}

public interface IView<in TEntity>
{
    void InitializeView(Contexts contexts, TEntity entity);
}

// CONGRATULATIONS! YOU FOUND THE CAT!
//               )\._.,--....,'``.       
// .b--.        /;   _.. \   _\  (`._ ,. 
//`=,-,-'~~~   `----(,_..'--(,_..'`-.;.'