using System.Collections;
using ColorzCore.Parser.AST;

namespace ColorzCore.Raws
{
    internal interface IRawParam
    {
        string Name { get; }
        int Position { get; } //Length and position in bits
        int Length { get; }
        bool Fits(IParamNode input);

        void Set(BitArray data, IParamNode input);
        /**
         * From Language.raws:
         * 
         * Parameter:
            Each code has 0 or more parameters. Syntax is following:
            ParameterName, Position, Length, flags
            If no Flags are used, last ',' can be left out. The position must
            be greater or equal than zero, or greater or equal than 2 if code
            has an ID. The Sum of Position and Length must be smaller than
            length of the code and the parameters can't use the same bits 
            in code. There also must be white space before the ParameterName.

             pointer
              Means that the parameter will automatically be to/from GBA pointer 
              when assembling/disassembling. You can also specify the priority
              of pointed code, which allows EA to disassemble the pointed code
              in some disassembly modes.

             coordinates, coordinate
              Specifies the amount of coordinates in the parameter. If range of 
              values is specified, the largest value is considered the real amount.
              Unspecified parameters in assembly will default to 0. Default amount
              of coordinates is 1.

             preferredBase
              Preferred number base of parameter upon disassembly. Allowed values 
              are 2, 10 and 16. Default is 16.

             fixed
              Means that parameter is not used as input parameter and instead
              the name of the parameter is intrepretted as number and applied 
              to parameter position. Fixed parameters do not effect code
              identification in assembly.

             signed
              Makes parameter handle values with highest bit as 1 as negative
              numbers.

    */
    }
}
