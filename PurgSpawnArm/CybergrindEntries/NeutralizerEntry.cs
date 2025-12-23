using PurgatorioCyberGrind.Systems;
using PurgSpawnArm;

namespace PurgatorioCyberGrind.CybergrindEntries
{
	public class NeutralizerEntry : CustomCyberGrindEntry
	{
		public override bool AddedToTheCybergrind()
		{
			return Plugin.NeutralizerInCybergrind.value;
		}

		public override void SetEntrySettings(out int spawnCost, out int costIncreasePerSpawn, out int spawnWave, out string spawnObjectName)
		{
			spawnCost = 55;
			costIncreasePerSpawn = 0; //Capped at 1 reguardless
			spawnWave = 19;
			spawnObjectName = "Neutralizer";
		}

		public override SpawnTypePosition SetTypePosition()
		{
			return new AfterSpawnTypeEnemy(CybergrindEnemyCatagories.UncommonEnemies.Idol);
		}

		public override int CapNonCommonEnemyAmount(int currentWave, int enemyAmount)
		{
			return 1;
		}

		public override bool? CanBeRadiant(EndlessEnemy target, int currentWave, int enemyAmount)
		{
			return false;
		}

		public override bool UncommonMeleePositionsOnly()
		{
			return true;
		}
	}
}