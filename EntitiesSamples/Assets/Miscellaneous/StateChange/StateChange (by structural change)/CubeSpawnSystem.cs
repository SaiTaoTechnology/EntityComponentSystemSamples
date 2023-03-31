using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

namespace Miscellaneous.StateChangeStructural
{
    public partial struct CubeSpawnSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Config>();
            state.RequireForUpdate<Execute.StateChangeStructural>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Enabled = false;

            var config = SystemAPI.GetSingleton<Config>();
            state.EntityManager.Instantiate(config.Prefab, (int)(config.Size * config.Size), Allocator.Temp);

            var center = (config.Size - 1) / 2f;
            int i = 0;
            foreach (var trans in
                     SystemAPI.Query<RefRW<LocalTransform>>()
                         .WithAll<Cube>())
            {
                trans.ValueRW.Scale = 1;
                trans.ValueRW.Position.x = (i % config.Size - center) * 1.5f;
                trans.ValueRW.Position.z = (i / config.Size - center) * 1.5f;
                i++;
            }
        }
    }
}