using Reports.Domain.Constants;
using Reports.Domain.Exceptions;
using Reports.Domain.Models;
using Reports.Infrastructure;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Reports.Domain
{
    public static class GoalsServiceExtensions 
    {
        /// <summary>
        /// Assigns the value of each unsatisfactory Goal.Name received in the request body to the corresponding value of the paragraph to be built.
        /// </summary>
        /// <param name="goal"></param>
        /// <returns></returns>
        /// <exception cref="GoalNotFoundException"></exception>
        public static Goal GetGoalsReachedFromDB(this Goal goal)
        {
            var cons = new GoalsPartiallySatisfactory();
            var goalName = cons.GetConst(typeof(GoalsPartiallySatisfactory), goal.Name);
            if (cons.ConstanteExiste(typeof(GoalsPartiallySatisfactory), goal.Name))
            {
                goal.Name = goalName;
            }

            else { throw new GoalNotFoundException("Goal not found."); }
            return goal;

        }

        /// <summary>
        /// Assigns the value of each satisfactory Goal.Name received in the request body to the corresponding value of the paragraph to be built.
        /// </summary>
        /// <param name="goal"></param>
        /// <returns></returns>
        public static Goal GetGoalsNotReachedFromDB(this Goal goal)
        {
            var repo = new ReportRepository();
            var goalName = repo.GetItem(goal.Name);

            if (repo.CheckIItemExists(goal.Name))
            {
                goal.Name = goalName;
            }
            else
                return null;

            return goal;      
        }

    }
}

//var cons = new GoalsPartiallySatisfactory();
////var repo = new ReportRepository();
//var goalName = cons.GetConst(typeof(GoalsPartiallySatisfactory), goal.Name))
//           // var goalName = repo.GetItem(goal.Name);

//    //if (repo.CheckIItemExists(goal.Name))
//    //{
//    //    goal.Name = goalName;
//    //}
//    //else
//    //if (cons.ConstanteExiste(typeof(GoalsPartiallySatisfactory), goal.Name))
//    //{
//    //    goal.Name = goalName;
//    //}
//            else { throw new GoalNotFoundException("Goal not found."); }


//return goal;