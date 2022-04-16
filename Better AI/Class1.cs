using Base.AI.Defs;
using Base.Core;
using Base.Defs;
using Base.Entities.Abilities;
using Base.Entities.Effects;
using Base.Entities.Statuses;
using Base.Levels;
using Base.UI;
using Base.Utils.Maths;
using Harmony;
using PhoenixPoint.Common.Core;
using PhoenixPoint.Common.Entities.GameTags;
using PhoenixPoint.Common.Entities.GameTagsTypes;
using PhoenixPoint.Common.UI;
using PhoenixPoint.Geoscape.Entities.DifficultySystem;
using PhoenixPoint.Geoscape.Events.Eventus;
using PhoenixPoint.Tactical;
using PhoenixPoint.Tactical.AI;
using PhoenixPoint.Tactical.AI.Actions;
using PhoenixPoint.Tactical.AI.Considerations;
using PhoenixPoint.Tactical.Entities;
using PhoenixPoint.Tactical.Entities.Abilities;
using PhoenixPoint.Tactical.Entities.Animations;
using PhoenixPoint.Tactical.Entities.DamageKeywords;
using PhoenixPoint.Tactical.Entities.Effects;
using PhoenixPoint.Tactical.Entities.Effects.DamageTypes;
using PhoenixPoint.Tactical.Entities.Equipments;
using PhoenixPoint.Tactical.Entities.Statuses;
using PhoenixPoint.Tactical.Entities.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Better_AI
{
    internal class AIChanges
    {
        public static T CreateDefFromClone<T>(T source, string guid, string name) where T : BaseDef
        {
            DefRepository Repo = GameUtl.GameComponent<DefRepository>();
            try
            {
                if (Repo.GetDef(guid) != null)
                {
                    if (!(Repo.GetDef(guid) is T tmp))
                    {
                        throw new TypeAccessException($"An item with the GUID <{guid}> has already been added to the Repo, but the type <{Repo.GetDef(guid).GetType().Name}> does not match <{typeof(T).Name}>!");
                    }
                    else
                    {
                        if (tmp != null)
                        {
                            return tmp;
                        }
                    }
                }
                T tmp2 = Repo.GetRuntimeDefs<T>(true).FirstOrDefault(rt => rt.Guid.Equals(guid));
                if (tmp2 != null)
                {
                    return tmp2;
                }
                Type type = null;
                string resultName = "";
                if (source != null)
                {
                    int start = source.name.IndexOf('[') + 1;
                    int end = source.name.IndexOf(']');
                    string toReplace = !name.Contains("[") && start > 0 && end > start ? source.name.Substring(start, end - start) : source.name;
                    resultName = source.name.Replace(toReplace, name);
                }
                else
                {
                    type = typeof(T);
                    resultName = name;
                }
                T result = (T)Repo.CreateRuntimeDef(
                    source,
                    type,
                    guid);
                result.name = resultName;
                return result;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
    public static class MyMod
    {
        public static void HomeMod(Func<string, object, object> api = null)
        {
            HarmonyInstance.Create("your.mod.id").PatchAll();
            api?.Invoke("log verbose", "Mod Initialised.");
            DefRepository Repo = GameUtl.GameComponent<DefRepository>();
            SharedData Shared = GameUtl.GameComponent<SharedData>();

            AIActionsTemplateDef soldierAI = Repo.GetAllDefs<AIActionsTemplateDef>().FirstOrDefault(a => a.name.Equals("AIActionsTemplateDef"));
            AIActionsTemplateDef crabmanAI = Repo.GetAllDefs<AIActionsTemplateDef>().FirstOrDefault(a => a.name.Equals("Crabman_AIActionsTemplateDef"));
            AIActionsTemplateDef crabmanTankAI = Repo.GetAllDefs<AIActionsTemplateDef>().FirstOrDefault(a => a.name.Equals("CrabmanTank_AIActionsTemplateDef"));
            AIActionsTemplateDef crabmanBrawlerAI = Repo.GetAllDefs<AIActionsTemplateDef>().FirstOrDefault(a => a.name.Equals("CrabmanBrawler_AIActionsTemplateDef"));
            AIActionsTemplateDef fishmanAI = Repo.GetAllDefs<AIActionsTemplateDef>().FirstOrDefault(a => a.name.Equals("Fishman_AIActionsTemplateDef"));
            AISafePositionConsiderationDef fishmanSafeAI = Repo.GetAllDefs<AISafePositionConsiderationDef>().FirstOrDefault(a => a.name.Equals("Fishman_SafePosition_AIConsiderationDef"));
            AIActionsTemplateDef QueenAI = Repo.GetAllDefs<AIActionsTemplateDef>().FirstOrDefault(a => a.name.Equals("Queen_AIActionsTemplateDef"));
            AIActionsTemplateDef acheronAAI = Repo.GetAllDefs<AIActionsTemplateDef>().FirstOrDefault(a => a.name.Equals("AcheronAggressive_AIActionsTemplateDef"));
            AIActionsTemplateDef acheronDAI = Repo.GetAllDefs<AIActionsTemplateDef>().FirstOrDefault(a => a.name.Equals("AcheronDefensive_AIActionsTemplateDef"));
            AIActionMoveAndAttackDef SirenAcidAI = Repo.GetAllDefs<AIActionMoveAndAttackDef>().FirstOrDefault(a => a.name.Equals("Siren_MoveAndSpitAcid_AIActionDef"));

            AIActionEndCharacterTurnDef endturn = Repo.GetAllDefs<AIActionEndCharacterTurnDef>().FirstOrDefault(a => a.name.Equals("EndCharacterTurn_AIActionDef"));
            AIActionMoveAndAttackDef moveAndShoot = Repo.GetAllDefs<AIActionMoveAndAttackDef>().FirstOrDefault(a => a.name.Equals("MoveAndShoot_AIActionDef"));
            AIActionMoveAndAttackDef moveAndStrike = Repo.GetAllDefs<AIActionMoveAndAttackDef>().FirstOrDefault(a => a.name.Equals("MoveAndStrike_AIActionDef"));
            AIActionDeployShieldDef deployShield = Repo.GetAllDefs<AIActionDeployShieldDef>().FirstOrDefault(a => a.name.Equals("DeployShield_AIActionDef"));
            AIActionDeployShieldDef crabDeployShield = Repo.GetAllDefs<AIActionDeployShieldDef>().FirstOrDefault(a => a.name.Equals("Crabman_DeployShield_AIActionDef"));
            AIActionMoveToPositionDef moveRandom = Repo.GetAllDefs<AIActionMoveToPositionDef>().FirstOrDefault(a => a.name.Equals("MoveToRandomWaypoint_AIActionDef"));
            AIActionMoveToPositionDef moveSafe = Repo.GetAllDefs<AIActionMoveToPositionDef>().FirstOrDefault(a => a.name.Equals("MoveToSafePosition_AIActionDef"));
            AIActionMoveToPositionDef moveNoShield = Repo.GetAllDefs<AIActionMoveToPositionDef>().FirstOrDefault(a => a.name.Equals("Crabman_Advance_Normal_WithoutShield_AIActionDef"));
            AIActionMoveAndEscapeDef flee = Repo.GetAllDefs<AIActionMoveAndEscapeDef>().FirstOrDefault(a => a.name.Equals("Flee_AIActionDef"));
            AIActionOverwatchDef overwatch = Repo.GetAllDefs<AIActionOverwatchDef>().FirstOrDefault(a => a.name.Equals("Overwatch_AIActionDef"));
            AISpreadConsiderationDef sirenAcidSpread = Repo.GetAllDefs<AISpreadConsiderationDef>().FirstOrDefault(a => a.name.Equals("Siren_Spread_AIConsiderationDef"));
            AIAttackPositionConsiderationDef sirenAttackPosition = Repo.GetAllDefs<AIAttackPositionConsiderationDef>().FirstOrDefault(a => a.name.Equals("Siren_AcidSpitAttackPosition_AIConsiderationDef"));
            WeaponDef sirenAcidTorso = Repo.GetAllDefs<WeaponDef>().FirstOrDefault(a => a.name.Equals("Siren_Torso_AcidSpitter_WeaponDef"));
            WeaponDef sirenArmisAcidTorso = Repo.GetAllDefs<WeaponDef>().FirstOrDefault(a => a.name.Equals("Siren_Torso_Orichalcum_WeaponDef"));

            soldierAI.ActionDefs[7].Weight = 2;
            soldierAI.ActionDefs[26].Weight = 350;
            fishmanAI.ActionDefs[4].Weight = 200;
            fishmanAI.ActionDefs[5].Weight = 1000;
            fishmanSafeAI.NoneCoverProtection = 0.5f;
            fishmanSafeAI.VisionScoreWhenVisibleByAllEnemies = 0.1f;
            acheronAAI.ActionDefs[1].Weight = 250;
            QueenAI.ActionDefs[9].Weight = 0.01f;
            SirenAcidAI.Weight = 600;
            /*
            //sirenAcidSpread.MinOptimalRange = 1;
            //sirenAttackPosition.FriendlyHitScoreMultiplier = 1;
            //sirenAttackPosition.IgnoreDamageOnSelf = true;
            //sirenAcidTorso.DamagePayload.Range = 10;
            //sirenAcidTorso.APToUsePerc = 25;
            //sirenArmisAcidTorso.DamagePayload.Range = 10;
            //sirenArmisAcidTorso.APToUsePerc = 25;

            sirenAcidTorso.DamagePayload.DamageKeywords = new List<DamageKeywordPair>()
                {
                sirenAcidTorso.DamagePayload.DamageKeywords[0],
                new DamageKeywordPair{DamageKeywordDef = Shared.SharedDamageKeywords.BlastKeyword, Value = 10 },
                };

            sirenArmisAcidTorso.DamagePayload.DamageKeywords = new List<DamageKeywordPair>()
                {
                sirenArmisAcidTorso.DamagePayload.DamageKeywords[0],
                new DamageKeywordPair{DamageKeywordDef = Shared.SharedDamageKeywords.BlastKeyword, Value = 10 },
                };
            */
            AIActionMoveAndAttackDef mAShoot = AIChanges.CreateDefFromClone(
                    Repo.GetAllDefs<AIActionMoveAndAttackDef>().FirstOrDefault(t => t.name.Equals("MoveAndShoot_AIActionDef")),
                    "3fd2dfd1-3cc0-4c71-b427-22afd020b45d",
                    "BC_MoveAndShoot_AIActionDef");
            AIActionMoveAndAttackDef mAStrike = AIChanges.CreateDefFromClone(
                Repo.GetAllDefs<AIActionMoveAndAttackDef>().FirstOrDefault(a => a.name.Equals("MoveAndStrike_AIActionDef")),
                "78c28fb8-0573-467a-a1c3-94b40673ef47",
                "VC_MoveAndStrike_AIActionDef");


            fishmanAI.ActionDefs[2] = mAShoot;
            fishmanAI.ActionDefs[3] = mAStrike;
            fishmanAI.ActionDefs[2].Weight = 500;
            fishmanAI.ActionDefs[3].Weight = 300;
        }
        public static void MainMod(Func<string, object, object> api)
        {
            HarmonyInstance.Create("your.mod.id").PatchAll();
            api("log verbose", "Mod Initialised.");
            /*
            TacAIActorDef fhAIActor = Repo.GetAllDefs<TacAIActorDef>().FirstOrDefault(a => a.name.Equals("Facehugger_AIActorDef"));
            TacAIActorDef soldierAIActor = Repo.GetAllDefs<TacAIActorDef>().FirstOrDefault(a => a.name.Equals("AIActorDef"));
            TacAIActorDef arthronAIActor = Repo.GetAllDefs<TacAIActorDef>().FirstOrDefault(a => a.name.Equals("Crabman_AIActorDef"));
            TacAIActorDef tritonAIActor = Repo.GetAllDefs<TacAIActorDef>().FirstOrDefault(a => a.name.Equals("Fishman_AIActorDef"));
            TacAIActorDef sirenAIActor = Repo.GetAllDefs<TacAIActorDef>().FirstOrDefault(a => a.name.Equals("Siren_AIActorDef"));
            TacAIActorDef chironAIACtor = Repo.GetAllDefs<TacAIActorDef>().FirstOrDefault(a => a.name.Equals("Chiron_AIActorDef"));
            TacAIActorDef swarmerAIACtor = Repo.GetAllDefs<TacAIActorDef>().FirstOrDefault(a => a.name.Equals("Swarmer_AIActorDef"));
            TacAIActorDef acheronAIACtor = Repo.GetAllDefs<TacAIActorDef>().FirstOrDefault(a => a.name.Equals("Acheron_AIActorDef"));
            TacAIActorDef queenAIACtor = Repo.GetAllDefs<TacAIActorDef>().FirstOrDefault(a => a.name.Equals("Queen_AIActorDef"));

            fhAIActor.AIActorData.IsAlerted = true;
            soldierAIActor.AIActorData.IsAlerted = true;
            arthronAIActor.AIActorData.IsAlerted = true;
            tritonAIActor.AIActorData.IsAlerted = true;
            sirenAIActor.AIActorData.IsAlerted = true;
            chironAIACtor.AIActorData.IsAlerted = true;
            swarmerAIACtor.AIActorData.IsAlerted = true;
            acheronAIACtor.AIActorData.IsAlerted = true;
            queenAIACtor.AIActorData.IsAlerted  = true;
          
            crabmanTankAI.ActionDefs = new AIActionDef[]
            {
                endturn,
                moveAndShoot,
                deployShield,
                moveRandom,
                moveSafe,
            };
            crabmanBrawlerAI.ActionDefs = new AIActionDef[]
            {
                endturn,
                moveAndShoot,
                deployShield,
                moveRandom,
                moveSafe,
            };
            crabmanAI.ActionDefs = new AIActionDef[]
            {
                endturn,
                moveRandom,
                moveAndShoot,
                moveAndStrike,
                crabDeployShield,
                moveNoShield,
                flee,
                overwatch,
            };
            */
        }
    }
}
