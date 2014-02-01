using System.Collections.Generic;
using КВАНТ.Logical.Engine;

namespace КВАНТ.Logical.ProgrammEngine.array_l_element
{
    public interface IModuleDialogInterface
    {
        ElementData ContentData { set; get; }
        string Id {get;}
         List<ModuleRemot> AvalibleModules { set; get; }
    }
}