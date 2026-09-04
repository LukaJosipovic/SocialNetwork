using Application.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileClient.Helper
{
    public static class EnumHelper
    {
        public static string GetEnumDescription(ActivityCategory category)
        {
            return category switch
            {
                ActivityCategory.FoodAndDrink => "Food & Drink",
                ActivityCategory.SportsAndFitness => "Sports & Fitness",
                ActivityCategory.ArtsAndCulture => "Arts & Culture",
                ActivityCategory.EntertainmentAndNightlife => "Entertainment & Nightlife",
                ActivityCategory.LearningAndHobbies => "Learning & Hobbies",
                ActivityCategory.OutdoorsAndNature => "Outdoors & Nature",
                ActivityCategory.VolunteeringAndCauses => "Volunteering & Causes",
                ActivityCategory.GamesAndActivities => "Games & Activities",
                ActivityCategory.TravelAndExploration => "Travel & Exploration",
                ActivityCategory.ChillAndRelaxation => "Chill & Relaxation",
                ActivityCategory.Other => "Other",
                _ => category.ToString()
            };
        }
    }
}
