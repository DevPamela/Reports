using Reports.Domain.Constants;
using Reports.Domain.Exceptions;
using Reports.Domain.Interfaces;
using Reports.Domain.Models;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Reports.Domain
{
    public static class GoalsServiceExtensions 
    {
        private readonly IGoalService _goalService;
        /// <summary>
        /// Assigns the value of each unsatisfactory Goal.Name received in the request body to the corresponding value of the paragraph to be built.
        /// </summary>
        /// <param name="goal"></param>
        /// <returns></returns>
        /// <exception cref="GoalNotFoundException"></exception>
        public static Goal GetGoalsReachedFromDB(this Goal goal)
        {
          
            var goalName = new GoalsPartiallySatisfactory();
            if (cons.ConstantExiste(typeof(GoalsPartiallySatisfactory), goal.Name))
            {
                goal.Name = goalName;
            }

            //else { throw new GoalNotFoundException("Goal not found."); }
            return goal;

        }

        public static Goal GetGoalsNotReachedFromDB(this Goal goal)
        {
            var cons = new GoalsUnsatisfactory();
            var goalName = cons.GetConst(typeof(GoalsUnsatisfactory), goal.Name);
            if (cons.ConstantExiste(typeof(GoalsUnsatisfactory), goal.Name))
            {
                goal.Name = goalName;
             }

            else { throw new GoalNotFoundException("Goal not found."); }
            return goal;
        }

        public static Goal GetGoalsPartiallyReachedFromDB(this Goal goal)
        {
            var cons = new GoalsPartiallySatisfactory();
            var goalName = cons.GetConst(typeof(GoalsPartiallySatisfactory), goal.Name);
            if (cons.ConstantExiste(typeof(GoalsPartiallySatisfactory), goal.Name))
            {
                goal.Name = goalName;
            }

            else { throw new GoalNotFoundException("Goal not found."); }
            return goal;
        }
    }
}