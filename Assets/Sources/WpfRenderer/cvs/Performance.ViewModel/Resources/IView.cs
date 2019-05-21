using Entitas;
using Entitas.MatchLine;
using Performance.Controls;
using Performance.ViewModels;

public interface IViewBehavior
{

}

public interface IView : IViewBehavior
{
    void InitializeView(MainViewModel model, ElementViewModel element, Contexts contexts, IEntity entity);
}

public interface IView<in TEntity> : IViewBehavior
{
    void InitializeView(MainViewModel model, ElementViewModel element, Contexts contexts, TEntity entity);
}

// CONGRATULATIONS! YOU FOUND THE CAT!
//               )\._.,--....,'``.       
// .b--.        /;   _.. \   _\  (`._ ,. 
//`=,-,-'~~~   `----(,_..'--(,_..'`-.;.'