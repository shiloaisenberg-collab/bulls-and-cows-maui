using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BullsAndCowsApp.Models
{
    public class GuessResult
    {
        public int bulls { get; set; }
        public int cows { get; set; }
        public string feedback => $"{bulls} bulls - {cows} cows";
        public bool IsWinningGusss
        {
            get { return bulls == 4; }
        }
    }
    
}
