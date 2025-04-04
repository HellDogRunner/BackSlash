using UnityEngine;
using RedMoonGames.Basics;

namespace Scripts.Entity
{
    [CreateAssetMenu(fileName = "Entity", menuName = "Scriptable Objects/Entity")]
    public class EntityDatabase : EntityScriptableDatabase<DefenseModel, AttackModel, StabilityModel>
    {
        public EntityDatabase GetData()
        {
            return this;
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
