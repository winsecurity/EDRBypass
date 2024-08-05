using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace add_ace_adminsdholder
{
    class cliparse
    {

        public string getargvalue(string[] args, string argkey)
        {

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].ToLower().Equals(argkey.ToLower()))
                {
                    try
                    {
                        return args[i + 1];

                    }
                    catch (Exception e)
                    {
                        return null;
                    }
                }
            }
            return null;
        }

    }
}
