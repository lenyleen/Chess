using System.Collections.Generic;
using Zenject;

namespace GameLogic.Moves
{
    public class MovesInstaller : Installer<MovesInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<PawnMoves>().AsSingle().WithArguments( new List<(int, int)>() {(0, 1)},new List<(int, int)>() {(1, 1), (1, -1)});
            Container.Bind<RookMoves>().AsSingle().WithArguments(new List<(int, int)>() {(1, 0), (-1, 0), (0, 1), (0, -1)});
            Container.Bind<QueenMoves>().AsSingle().WithArguments(null);
            Container.Bind<KingMoves>().AsSingle().WithArguments( new List<(int, int)>() {(-1, 1), (1, -1), (-1, -1), (1, 1), (1, 0), (-1, 0), (0, 1), (0, -1)});
            Container.Bind<KnightMoves>().AsSingle().WithArguments(new List<(int, int)>() {(2, 1), (2, -1), (-2, 1), (-2, -1), (1, 2), (-1, 2), (-1, -2), (1, -2)});
            Container.Bind<BishopMoves>().AsSingle().WithArguments(new List<(int, int)>() {(1, 1), (-1, 1), (-1, -1), (1, -1)});
        }
    }
}