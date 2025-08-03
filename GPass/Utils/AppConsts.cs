using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GPass.Utils;

class AppConsts
{
    public static readonly string APP_NAME = Assembly.GetEntryAssembly()!.GetName().Name!;
}
