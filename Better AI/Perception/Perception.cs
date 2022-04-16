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

namespace Better_AI.Perception
{
    internal class Perception
    {

        public static void Change_Perception()
        {
            DefRepository defRepository = GameUtl.GameComponent<DefRepository>();
            SharedData Shared = GameUtl.GameComponent<SharedData>();

			BodyPartAspectDef bodyPartAspectDef = defRepository.GetAllDefs<BodyPartAspectDef>().FirstOrDefault((BodyPartAspectDef a) => a.name.Equals("E_BodyPartAspect [SY_Sniper_Helmet_BodyPartDef]"));
			bodyPartAspectDef.Perception = 4f;
			BodyPartAspectDef bodyPartAspectDef2 = defRepository.GetAllDefs<BodyPartAspectDef>().FirstOrDefault((BodyPartAspectDef a) => a.name.Equals("E_BodyPartAspect [AN_Assault_Helmet_BodyPartDef]"));
			bodyPartAspectDef2.Perception = 2f;
			BodyPartAspectDef bodyPartAspectDef3 = defRepository.GetAllDefs<BodyPartAspectDef>().FirstOrDefault((BodyPartAspectDef a) => a.name.Equals("E_BodyPartAspect [AN_Berserker_Helmet_BodyPartDef]"));
			bodyPartAspectDef3.Perception = 5f;
			bodyPartAspectDef3.WillPower = 2f;
			BodyPartAspectDef bodyPartAspectDef4 = defRepository.GetAllDefs<BodyPartAspectDef>().FirstOrDefault((BodyPartAspectDef a) => a.name.Equals("E_BodyPartAspect [AN_Berserker_Helmet_Viking_BodyPartDef]"));
			bodyPartAspectDef4.WillPower = 2f;
			BodyPartAspectDef bodyPartAspectDef5 = defRepository.GetAllDefs<BodyPartAspectDef>().FirstOrDefault((BodyPartAspectDef a) => a.name.Equals("E_BodyPartAspect [AN_Priest_Legs_ItemDef]"));
			bodyPartAspectDef5.Perception = 2f;
			BodyPartAspectDef bodyPartAspectDef6 = defRepository.GetAllDefs<BodyPartAspectDef>().FirstOrDefault((BodyPartAspectDef a) => a.name.Equals("E_BodyPartAspect [AN_Priest_Torso_BodyPartDef]"));
			bodyPartAspectDef6.Perception = 4f;
			BodyPartAspectDef bodyPartAspectDef7 = defRepository.GetAllDefs<BodyPartAspectDef>().FirstOrDefault((BodyPartAspectDef a) => a.name.Equals("E_BodyPartAspect [NJ_Heavy_Helmet_BodyPartDef]"));
			bodyPartAspectDef7.Perception = -2f;
			BodyPartAspectDef bodyPartAspectDef8 = defRepository.GetAllDefs<BodyPartAspectDef>().FirstOrDefault((BodyPartAspectDef a) => a.name.Equals("E_BodyPartAspect [PX_Sniper_Helmet_BodyPartDef]"));
			bodyPartAspectDef8.Perception = 3f;
			BodyPartAspectDef bodyPartAspectDef9 = defRepository.GetAllDefs<BodyPartAspectDef>().FirstOrDefault((BodyPartAspectDef a) => a.name.Equals("E_BodyPartAspect [SY_Shinobi_BIO_Helmet_BodyPartDef]"));
			bodyPartAspectDef9.Perception = 3f;
			BodyPartAspectDef bodyPartAspectDef10 = defRepository.GetAllDefs<BodyPartAspectDef>().FirstOrDefault((BodyPartAspectDef a) => a.name.Equals("E_BodyPartAspect [NJ_Sniper_Helmet_BodyPartDef]"));
			bodyPartAspectDef10.Perception = 4f;
			BodyPartAspectDef bodyPartAspectDef11 = defRepository.GetAllDefs<BodyPartAspectDef>().FirstOrDefault((BodyPartAspectDef a) => a.name.Equals("E_BodyPartAspect [PX_Heavy_Helmet_BodyPartDef]"));
			bodyPartAspectDef11.Perception = 0f;
			BodyPartAspectDef bodyPartAspectDef12 = defRepository.GetAllDefs<BodyPartAspectDef>().FirstOrDefault((BodyPartAspectDef a) => a.name.Equals("E_BodyPartAspect [IN_Heavy_Helmet_BodyPartDef]"));
			bodyPartAspectDef12.Perception = -2f;
			BodyPartAspectDef bodyPartAspectDef13 = defRepository.GetAllDefs<BodyPartAspectDef>().FirstOrDefault((BodyPartAspectDef a) => a.name.Equals("E_BodyPartAspect [AN_Berserker_Watcher_Helmet_BodyPartDef]"));
			bodyPartAspectDef13.Perception = 8f;
			BodyPartAspectDef bodyPartAspectDef14 = defRepository.GetAllDefs<BodyPartAspectDef>().FirstOrDefault((BodyPartAspectDef a) => a.name.Equals("E_BodyPartAspect [SY_Infiltrator_Helmet_BodyPartDef]"));
			bodyPartAspectDef14.Perception = 5f;
			BodyPartAspectDef bodyPartAspectDef15 = defRepository.GetAllDefs<BodyPartAspectDef>().FirstOrDefault((BodyPartAspectDef a) => a.name.Equals("E_BodyPartAspect [AN_Berserker_Watcher_Torso_BodyPartDef]"));
			bodyPartAspectDef15.Perception = 3f;
		}
    }
}

