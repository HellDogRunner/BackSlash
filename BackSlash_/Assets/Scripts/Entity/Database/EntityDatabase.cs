using UnityEngine;
using RedMoonGames.Basics;
using System.Collections.Generic;

namespace Scripts.Entity
{
    [CreateAssetMenu(fileName = "Entity", menuName = "Scriptable Objects/Entity")]
    public class EntityDatabase : EntityScriptableDatabase<DefenseModel, AttackModel, StabilityModel>
    {
        public int GetHealth()
        {
            return Health;
        }
        
        public StabilityModel GetStability()
        {
            var stability = new StabilityModel();
            
            stability.Max = Stability.Max;
            stability.Current = Stability.Current;
            stability.TimeToRecovery = Stability.TimeToRecovery;
            stability.RecoveryInterval = Stability.RecoveryInterval;
            stability.StaggerTime = Stability.StaggerTime;
            stability.StunTime = Stability.StunTime;
            
            return stability;
        }
        
        public List<DefenseModel> GetDefense()
        {
            var defense = new List<DefenseModel>();
            
            foreach (var def in Defense)
            {
                var d = new DefenseModel();
                
                d.Name = def.Name;
                d.SetType(def.Type);
                d.Reduction = def.Reduction;
                d.Accumulation = def.Accumulation;
                d.Current = def.Current;
                d.Immunity = def.Immunity;
            
                defense.Add(d);
            }
            
            return defense;
        }
        
        public List<AttackModel> GetAttacksList()
        {
            var attack = new List<AttackModel>();
            
            foreach (var atc in Attack)
            {
                attack.Add(GetAttack(atc));
            }
        
            return attack;
        }
        
        public AttackModel GetAttack(AttackModel attack)
        {
            var newAttack = new AttackModel();
                
            newAttack.Name = attack.Name;
            newAttack.Type = attack.Type;
            newAttack.Ranged = attack.Ranged;
            newAttack.Damage = attack.Damage;
            newAttack.StabilityDamage = attack.StabilityDamage;
            newAttack.TimeAfter = attack.TimeAfter;
            newAttack.Cooldown = attack.Cooldown;
            newAttack.LastUseTime = attack.LastUseTime;
            newAttack.Effect = attack.Effect;
            newAttack.EffectValue = attack.EffectValue;
            
            return newAttack;
        }
        
        public void SetMaxHealth(int value)
        {
            Health = value;
        }
        
        public void SetStability(uint value)
        {
            //Stability = value;
        }
        
        public AttackModel GetAttackByName(string name)
        {
            return Attack.GetBy(model => model.Name == name);
        }

        [ContextMenu("Clear Fields")]
        public void ClearFields()
        {
            Health = 0;
            Stability.Max = 0;
            Stability.TimeToRecovery = 0;
            Stability.RecoveryInterval = 0;
            Defense.Clear();
            Attack.Clear();
        }

        [ContextMenu("Set Default", false, 0)]
		private void SetDefault()
		{
            Health = 100;
            Stability.Max = 100;
            Stability.TimeToRecovery = 5;
            Stability.RecoveryInterval = 0.2f;
		    FillDefense();
		    //FillAttack();
		}
		
		private void FillDefense()
		{
		    EEntity[] types = {EEntity.Physical, EEntity.Burning, EEntity.Bleeding, EEntity.Frost, EEntity.Shock, EEntity.Poison};
            
            Defense.Clear();
            
            for (int i = 0; i < 6; i++)
		    {
                var model = new DefenseModel();
                model.Name = types[i].ToString();
                model.SetType(types[i]);
                Defense.Add(model);
		    }
		}
		
        // private void FillAttack()
		// {
		// 	AnimatorControllerLayer[] layers = Animator.layers;
		// 	var newAttackNames = new List<string>();

        //     for (int i = 0; i < layers.Length; i++)
        //     {
        //         ChildAnimatorState[] states = layers[i].stateMachine.states;

        //         for (int j = 0; j < states.Length; j++)
        //         {
        //             var state = states[j].state;
                    
        //             if (state.tag == "Attack")
        //             {
        //                 newAttackNames.Add(state.name);

        //                 if (!GetAttackNames().Contains(state.name))
        //                 {
        //                     var attack = new AttackModel();
        //                     attack.Name = state.name;
        //                     Attack.Add(attack); 
        //                 }
        //             }
        //         }
        //     }
            
        //     SortAttack(newAttackNames);
		// }
		
		// private void SortAttack(List<string> newAttacks)
		// {
        //     var attacksForRemove = new List<AttackModel>();
		
        //     foreach (var attack in Attack)
        //     {
        //         if (!newAttacks.Contains(attack.Name))
        //         {
        //             attacksForRemove.Add(attack);
        //         }
        //     }
            
        //     foreach (var attack in attacksForRemove)
        //     {
        //         Attack.Remove(attack);
        //     }
		// }
		
		// private List<string> GetAttackNames()
		// {
        //     List<string> names = new List<string>();
		
        //     foreach (var attack in Attack)
        //     {
        //         names.Add(attack.Name);
        //     }
            
        //     return names;
        // }

		// [CustomEditor(typeof(EntityDatabase))]
		// public class EditorButton : Editor
		// {
		// 	public override void OnInspectorGUI()
		// 	{
		// 		base.OnInspectorGUI();
				
		// 		var _data = (EntityDatabase)target;
				
		// 		GUILayout.Space(10);

		// 		if (GUILayout.Button("Update Attack"))
		// 		{
        //             _data.FillAttack();
        //         }
		// 	} 	
		// }
    }  
}
