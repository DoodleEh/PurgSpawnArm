using HarmonyLib;
using System.Reflection.Emit;

namespace PurgSpawnArm
{
	public static class Utils
	{
		//Both used to future proof by not hardcoding the indices of Local variables
		public static void StlocIndex(this CodeInstruction self, out object index)
		{
			index = -1;
			if (self.IsStloc())
			{
				OpCode code = self.opcode;
				switch (code.Name)
				{
					case "stloc.0":
						index = 0;
						break;
					case "stloc.1":
						index = 1;
						break;
					case "stloc.2":
						index = 2;
						break;
					case "stloc.3":
						index = 3;
						break;
					case "stloc":
					case "stloc.s":
						index = self.operand;
						break;
				}
			}
		}

		public static void LdlocIndex(this CodeInstruction self, out object index)
		{
			index = -1;
			if (self.IsLdloc())
			{
				OpCode code = self.opcode;
				switch (code.Name)
				{
					case "ldloc.0":
						index = 0;
						break;
					case "ldloc.1":
						index = 1;
						break;
					case "ldloc.2":
						index = 2;
						break;
					case "ldloc.3":
						index = 3;
						break;
					case "ldloc":
					case "ldloc.s":
						index = self.operand;
						break;
				}
			}
		}
	}
}
